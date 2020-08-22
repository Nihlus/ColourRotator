//
//  TargetColourConverter.cs
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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ColourRotator.Json
{
    /// <summary>
    /// Converts a <see cref="TargetColour"/> to and from JSON.
    /// </summary>
    public class TargetColourConverter : JsonConverter<TargetColour>
    {
        /// <inheritdoc/>
        public override TargetColour Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (!reader.Read())
            {
                throw new JsonException();
            }

            string? name = null;
            double? hue = null;

            while (true)
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    if (!reader.IsFinalBlock && !reader.Read())
                    {
                        throw new JsonException();
                    }

                    break;
                }

                switch (reader.GetString())
                {
                    case "name":
                    {
                        if (!reader.Read())
                        {
                            throw new JsonException();
                        }

                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException();
                        }

                        if (!(name is null))
                        {
                            throw new JsonException();
                        }

                        name = reader.GetString();

                        if (!reader.Read())
                        {
                            throw new JsonException();
                        }

                        break;
                    }
                    case "hue":
                    {
                        if (!reader.Read())
                        {
                            throw new JsonException();
                        }

                        if (reader.TokenType != JsonTokenType.Number)
                        {
                            throw new JsonException();
                        }

                        if (!(hue is null))
                        {
                            throw new JsonException();
                        }

                        if (!reader.TryGetDouble(out var readHue))
                        {
                            throw new JsonException();
                        }

                        if (readHue < 0.0 || readHue >= 360.0)
                        {
                            throw new JsonException();
                        }

                        hue = readHue;

                        if (!reader.Read())
                        {
                            throw new JsonException();
                        }

                        break;
                    }
                    default:
                    {
                        throw new JsonException();
                    }
                }
            }

            if (name is null)
            {
                throw new JsonException();
            }

            if (hue is null)
            {
                throw new JsonException();
            }

            return new TargetColour(name, hue.Value);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TargetColour value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("name", value.Name);
            writer.WriteNumber("hue", value.Hue);
            writer.WriteEndObject();
        }
    }
}
