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
        /// Gets the starting angle for the input sector.
        /// </summary>
        [Option('s', "start", Required = true, HelpText = "The starting angle for the input sector.")]
        public double Start { get; }

        /// <summary>
        /// Gets the ending angle for the input sector.
        /// </summary>
        [Option('e', "end", Required = true, HelpText = "The ending angle for the input sector.")]
        public double End { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>
        /// <param name="inputFiles">The input files.</param>
        /// <param name="start">The starting angle.</param>
        /// <param name="end">The ending angle.</param>
        public Options(IEnumerable<string> inputFiles, double start, double end)
        {
            this.InputFiles = inputFiles;
            this.Start = start;
            this.End = end;
        }
    }
}
