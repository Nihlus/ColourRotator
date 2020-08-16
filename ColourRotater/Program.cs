//
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
using System.Collections.Generic;
using System.IO;
using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;

namespace ColourRotater
{
    /// <summary>
    /// The main class of the program.
    /// </summary>
    public class Program
    {
        private static Options? _options;

        /// <summary>
        /// The main entrypoint of the program.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <returns>The return code of the program.</returns>
        public static int Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => _options = o);

            if (_options is null)
            {
                return 1;
            }

            foreach (var inputFile in _options.InputFiles)
            {
                var realPath = Path.GetFullPath(inputFile);
                if (File.Exists(realPath))
                {
                    continue;
                }

                Console.Error.WriteLine($"File not found: {realPath}");
                return 1;
            }

            var baseAngles = new Dictionary<string, int>
            {
                { "yellow", 60 },
                { "orange", 45 },
                { "red", 0 },
                { "magenta", 300 },
                { "purple", 270 },
                { "blue", 225 },
                { "cyan", 195 },
                { "green", 112 },
                { "blue-green", 164 },
            };

            var angleOffset = 45;

            foreach (var inputFile in _options.InputFiles)
            {
                var realPath = Path.GetFullPath(inputFile);
                var outputPath = Path.GetDirectoryName(realPath) ?? Directory.GetCurrentDirectory();
                var inputFileName = Path.GetFileNameWithoutExtension(realPath);

                using var image = Image.Load<RgbaVector>(realPath);
                foreach (var (colour, angle) in baseAngles)
                {
                    var start = NormalizeAngle(angle - angleOffset);
                    var end = NormalizeAngle(angle + angleOffset);

                    var rotated = RotateColours
                    (
                        image,
                        new CircleSector(_options.Start, _options.End),
                        new CircleSector(start, end)
                    );

                    var outputFile = Path.Combine(outputPath, $"{inputFileName}-{colour}.png");
                    rotated.SaveAsPng(outputFile);
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

                    var newRgb = converter.ToRgb(new Hsv((float)newHue, hsv.S, hsv.V));

                    row[y] = new RgbaVector(newRgb.R, newRgb.G, newRgb.B, currentPixelValue.A);
                }
            }

            return clone;
        }

        private static double NormalizeAngle(double angle)
        {
            var normalizedAngle = angle % 360.0;
            if (normalizedAngle < 0)
            {
                normalizedAngle += 360.0;
            }

            return normalizedAngle;
        }
    }
}
