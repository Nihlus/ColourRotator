//
//  RotationProfile.cs
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

using System.Collections;
using System.Collections.Generic;

namespace ColourRotator
{
    /// <summary>
    /// Represents a colour rotation profile; that is, a set of target colours and names.
    /// </summary>
    public class RotationProfile : IReadOnlyList<TargetColour>
    {
        /// <summary>
        /// Gets the target colours in the rotation profile.
        /// </summary>
        public IReadOnlyList<TargetColour> TargetColours { get; }

        /// <inheritdoc />
        public IEnumerator<TargetColour> GetEnumerator() => this.TargetColours.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public int Count => this.TargetColours.Count;

        /// <inheritdoc />
        public TargetColour this[int index] => this.TargetColours[index];

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationProfile"/> class.
        /// </summary>
        /// <param name="targetColours">The target colours.</param>
        public RotationProfile(IReadOnlyList<TargetColour> targetColours)
        {
            this.TargetColours = targetColours;
        }
    }
}
