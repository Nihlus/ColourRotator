//
//  AngleHelpers.cs
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

namespace ColourRotator
{
    /// <summary>
    /// Holds various angle helper functions.
    /// </summary>
    public static class AngleHelpers
    {
        /// <summary>
        /// Normalizes an angle, mapping it to a value between 0 and 360.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The normalized angle.</returns>
        public static double NormalizeAngle(double angle)
        {
            var normalizedAngle = angle % 360.0;
            if (normalizedAngle < 0)
            {
                normalizedAngle += 360.0;
            }

            return normalizedAngle;
        }

        /// <summary>
        /// Calculates the unsigned shortest-path distance between two angles.
        /// </summary>
        /// <param name="a">The first angle.</param>
        /// <param name="b">The second angle.</param>
        /// <returns>The signed distance.</returns>
        public static double Distance(double a, double b)
        {
            return NormalizeAngle(((b - a + 180.0) % 360) - 180.0);
        }
    }
}
