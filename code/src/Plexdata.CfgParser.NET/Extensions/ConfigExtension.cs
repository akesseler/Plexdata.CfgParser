/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Plexdata.CfgParser.Constants;
using Plexdata.CfgParser.Settings;
using System;
using System.Linq;

namespace Plexdata.CfgParser.Extensions
{
    /// <summary>
    /// This internal class provides extension methods for various configuration item related operations.
    /// </summary>
    /// <remarks>
    /// This extension class is intentionally made as internal because of it provides nothing 
    /// that can be used from the outside world.
    /// </remarks>
    internal static class ConfigExtension
    {
        #region Public static methods

        /// <summary>
        /// This method is responsible to prepare section titles for output.
        /// </summary>
        /// <remarks>
        /// This method removes every section prefix as well as every section suffix. 
        /// Thereafter, the resulting title is trimmed.
        /// </remarks>
        /// <param name="title">
        /// The section title to be fixed up.
        /// </param>
        /// <returns>
        /// The fixed up section title. An empty string is returned if provided title 
        /// is <c>null</c>.
        /// </returns>
        public static String FixupTitle(this String title)
        {
            if (title == null)
            {
                return String.Empty;
            }

            return title.Replace(ConfigDefines.SectionPrefix.ToString(), String.Empty).Replace(ConfigDefines.SectionSuffix.ToString(), String.Empty).Trim();
        }

        /// <summary>
        /// This method is responsible to prepare value labels for output.
        /// </summary>
        /// <remarks>
        /// This method just trims provided value label.
        /// </remarks>
        /// <param name="label">
        /// The value label to be fixed up.
        /// </param>
        /// <returns>
        /// The fixed up value label. An empty string is returned if provided label 
        /// is <c>null</c>.
        /// </returns>
        public static String FixupLabel(this String label)
        {
            if (label == null)
            {
                return String.Empty;
            }

            return label.Trim();
        }

        /// <summary>
        /// This method is responsible to prepare value data for output.
        /// </summary>
        /// <remarks>
        /// First of all provided value is trimmed. As next it is checked whether the provided 
        /// value includes one of the supported markers for comments and values. If so, then the 
        /// result will be surrounded by string markers. Otherwise, the value is returned without 
        /// any string marker.
        /// </remarks>
        /// <param name="value">
        /// The value data to be fixed up.
        /// </param>
        /// <returns>
        /// The fixed up value data. An empty string is returned if provided value 
        /// is <c>null</c>.
        /// </returns>
        public static String FixupValue(this String value)
        {
            if (value == null)
            {
                return String.Empty;
            }

            value = value.Trim();

            if (value.IndexOfAny(ConfigDefines.CommentMarkers) >= 0 || value.IndexOfAny(ConfigDefines.ValueMarkers) >= 0)
            {
                value = $"{ConfigDefines.StringMarker}{value}{ConfigDefines.StringMarker}";
            }

            return value;
        }

        /// <summary>
        /// This method is responsible to prepare value markers for output.
        /// </summary>
        /// <remarks>
        /// As first it is checked whether provided marker is one of the supported value 
        /// markers. If not, then the provided marker is replaced by current default value 
        /// marker. Thereafter, the marker is extended by spaces. How many spaces are used 
        /// and where the spaces are put depends on current configuration file style.
        /// </remarks>
        /// <param name="marker">
        /// The value marker to be fixed up.
        /// </param>
        /// <returns>
        /// The fixed up value marker.
        /// </returns>
        public static String FixupMarker(this Char marker)
        {
            if (!ConfigDefines.ValueMarkers.Contains(marker))
            {
                marker = ConfigSettings.DefaultValueMarker;
            }

            if (marker == ConfigDefines.ValueMarkers[0])
            {
                return $"{marker} ";
            }

            if (marker == ConfigDefines.ValueMarkers[1])
            {
                return $" {marker} ";
            }

            return marker.ToString();
        }

        #endregion
    }
}
