//
//  CircleSector.cs
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

namespace ColourRotater
{
    /// <summary>
    /// Represents a sector in a circle.
    /// </summary>
    public class CircleSector
    {
        /// <summary>
        /// Gets the starting angle of the sector.
        /// </summary>
        public double Start { get; }

        /// <summary>
        /// Gets the ending angle of the sector.
        /// </summary>
        public double End { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleSector"/> class.
        /// </summary>
        /// <param name="start">The starting angle of the sector.</param>
        /// <param name="end">The ending angle of the sector.</param>
        public CircleSector(double start, double end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Determines whether the given angle is inside the sector.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>true if the angle is inside the sector; otherwise, false.</returns>
        public bool IsInsideSector(double angle)
        {
            return angle >= this.Start && angle <= this.End;
        }

        /// <summary>
        /// Calculates the alpha of the given angle inside the sector; that is, the linear distance between 0 and 1 from
        /// the starting angle to the given angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The linear distance.</returns>
        public double AlphaOf(double angle)
        {
            var totalDist = Math.Abs(this.Start - this.End);
            var angleDist = Math.Abs(this.Start - angle);

            return angleDist / totalDist;
        }

        /// <summary>
        /// Calculates the value at the given linear distance inside the sector.
        /// </summary>
        /// <param name="alpha">The linear distance.</param>
        /// <returns>The angle.</returns>
        public double LinearInterpolate(double alpha)
        {
            var totalDist = Math.Abs(this.Start - this.End);
            return this.Start + (totalDist * alpha);
        }
    }
}
