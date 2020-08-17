//
//  Options.cs
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

using System.Collections.Generic;
using CommandLine;

namespace ColourRotater
{
    /// <summary>
    /// Represents the command-line options to the program.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets the input files that are to be rotated.
        /// </summary>
        [Option('i', "input-files", Required = true, HelpText = "The input files that are to be rotated.")]
        public IEnumerable<string> InputFiles { get; }

        /// <summary>
        /// Gets the hue of the dominant colour in the image.
        /// </summary>
        [Option('h', "hue", Required = true, HelpText = "The hue of the dominant colour in the image.")]
        public double Hue { get; }

        /// <summary>
        /// Gets the size of the input sector window.
        /// </summary>
        [Option('s', "in-size", HelpText = "The size of the input sector window.", Default = 90)]
        public double InWindowSize { get; }

        /// <summary>
        /// Gets the size of the output sector window.
        /// </summary>
        [Option('o', "out-size", HelpText = "The size of the output sector window.", Default = 90)]
        public double OutWindowSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>
        /// <param name="inputFiles">The input files.</param>
        /// <param name="hue">The hue of the dominant colour in the image.</param>
        /// <param name="inWindowSize">The size of the input sector window.</param>
        /// <param name="outWindowSize">The size of the output sector window.</param>
        public Options(IEnumerable<string> inputFiles, double hue, double inWindowSize, double outWindowSize)
        {
            this.InputFiles = inputFiles;
            this.Hue = hue;
            this.InWindowSize = inWindowSize;
            this.OutWindowSize = outWindowSize;
        }
    }
}
