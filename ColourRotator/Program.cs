﻿//
//  Program.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ColourRotator.Json;
using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;

namespace ColourRotator
{
    /// <summary>
    /// The main class of the program.
    /// </summary>
    public class Program
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new RotationProfileConverter(),
                new TargetColourConverter()
            }
        };

        private static Options _options = null!;

        /// <summary>
        /// The main entrypoint of the program.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <returns>The return code of the program.</returns>
        public static async Task<int> Main(string[] args)
        {
            var didOptionsFailToParse = false;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => _options = o)
                .WithNotParsed(e => didOptionsFailToParse = true);

            if (didOptionsFailToParse || !await VerifyOptionsAsync())
            {
                return 1;
            }

            RotationProfile? rotationProfile = null;
            if (!(_options.ProfilePath is null))
            {
                var realPath = Path.GetFullPath(_options.ProfilePath);
                await using var file = File.OpenRead(realPath);
                try
                {
                    rotationProfile = await JsonSerializer.DeserializeAsync<RotationProfile>(file, SerializerOptions);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    await LogError("Failed to parse custom colour profile.");

                    if (_options.Verbose)
                    {
                        await LogError(ex.ToString());
                    }

                    return 1;
                }
            }

            // Fall back to the default bundled profile
            if (rotationProfile is null)
            {
                await using var defaultProfileResource = Assembly
                    .GetExecutingAssembly()
                    .GetManifestResourceStream("ColourRotator.Json.default-profile.json");

                if (defaultProfileResource is null)
                {
                    await LogError("No default rotation profile is available.");
                    return 1;
                }

                rotationProfile = await JsonSerializer.DeserializeAsync<RotationProfile>
                (
                    defaultProfileResource,
                    SerializerOptions
                );
            }

            foreach (var inputFile in _options.InputFiles)
            {
                var realPath = Path.GetFullPath(inputFile);
                var outputPath =
                    Directory.GetCurrentDirectory() ??
                    Path.GetDirectoryName(realPath) ??
                    throw new InvalidOperationException();

                var inputFileName = Path.GetFileNameWithoutExtension(realPath);

                using var image = Image.Load<RgbaVector>(realPath);
                foreach (var (colour, hue) in rotationProfile)
                {
                    if (_options.Verbose)
                    {
                        await Console.Out.WriteLineAsync($"Rotating to {colour}...");
                    }

                    var rotated = RotateColours
                    (
                        image,
                        new CircleSector
                        (
                            _options.Hue - (_options.InWindowSize / 2),
                            _options.Hue + (_options.InWindowSize / 2)
                        ),
                        new CircleSector
                        (
                            hue - (_options.OutWindowSize / 2),
                            hue + (_options.OutWindowSize / 2)
                        )
                    );

                    var outputFile = Path.Combine(outputPath, $"{inputFileName}-{colour}.png");
                    await rotated.SaveAsPngAsync(outputFile);
                }
            }

            return 0;
        }

        private static Image<RgbaVector> RotateColours(Image<RgbaVector> image, CircleSector from, CircleSector to)
        {
            var converter = new ColorSpaceConverter();

            var clone = image.Clone();
            for (var x = 0; x < clone.Height; ++x)
            {
                var row = clone.GetPixelRowSpan(x);
                for (var y = 0; y < row.Length; ++y)
                {
                    var currentPixelValue = row[y];

                    var hsv = converter.ToHsv(new Rgb(currentPixelValue.R, currentPixelValue.G, currentPixelValue.B));

                    if (!from.IsInsideSector(hsv.H))
                    {
                        continue;
                    }

                    var alpha = from.AlphaOf(hsv.H);
                    var newHue = to.LinearInterpolate(alpha);

                    var newRgb = converter.ToRgb
                    (
                        new Hsv
                        (
                            (float)newHue,
                            Math.Clamp((float)(hsv.S + _options.SaturationModifier), 0.0f, 1.0f),
                            Math.Clamp((float)(hsv.V + _options.ValueModifier), 0.0f, 1.0f)
                        )
                    );

                    row[y] = new RgbaVector(newRgb.R, newRgb.G, newRgb.B, currentPixelValue.A);
                }
            }

            return clone;
        }

        private static async Task<bool> VerifyOptionsAsync()
        {
            if (_options.SaturationModifier < -1.0 || _options.SaturationModifier > 1.0)
            {
                await LogError("Saturation modifier out of range.");
                return false;
            }

            if (_options.ValueModifier < -1.0 || _options.ValueModifier > 1.0)
            {
                await LogError("Value modifier out of range.");
                return false;
            }

            foreach (var inputFile in _options.InputFiles)
            {
                var realInputPath = Path.GetFullPath(inputFile);
                if (File.Exists(realInputPath))
                {
                    continue;
                }

                await LogError($"File not found: {realInputPath}");
                return false;
            }

            if (_options.ProfilePath is null)
            {
                return true;
            }

            var realProfilePath = Path.GetFullPath(_options.ProfilePath);
            if (File.Exists(realProfilePath))
            {
                return true;
            }

            await LogError("Rotation profile not found.");
            return false;
        }

        private static async Task LogError(string errorMessage)
        {
            var originalColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            await Console.Error.WriteLineAsync(errorMessage);

            Console.ForegroundColor = originalColour;
        }
    }
}
