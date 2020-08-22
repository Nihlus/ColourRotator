//
//  TargetColour.cs
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

using System.Diagnostics;

namespace ColourRotator
{
    /// <summary>
    /// Represents a single target colour.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public class TargetColour
    {
        /// <summary>
        /// Gets the display value for the debugger.
        /// </summary>
        private string DebuggerDisplay => $"{this.Name}: {this.Hue:F1}";

        /// <summary>
        /// Gets the name of the target colour.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the hue in HSV space of the target colour.
        /// </summary>
        public double Hue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetColour"/> class.
        /// </summary>
        /// <param name="name">The name of the colour.</param>
        /// <param name="hue">The colour hue in HSV space.</param>
        public TargetColour(string name, double hue)
        {
            this.Name = name;
            this.Hue = hue;
        }

        /// <summary>
        /// Deconstructs a target colour.
        /// </summary>
        /// <param name="name">The name of the colour.</param>
        /// <param name="hue">The colour hue in HSV space.</param>
        public void Deconstruct(out string name, out double hue)
        {
            name = this.Name;
            hue = this.Hue;
        }
    }
}
