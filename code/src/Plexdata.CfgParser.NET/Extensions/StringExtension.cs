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
using System;
using System.Linq;

namespace Plexdata.CfgParser.Extensions
{
    /// <summary>
    /// This internal class provides extension methods for various string related operations.
    /// </summary>
    /// <remarks>
    /// This extension class is intentionally made as internal because of it provides nothing 
    /// that can be used from the outside world.
    /// </remarks>
    internal static class StringExtension
    {
        #region Public static methods

        /// <summary>
        /// This method determines if given 'buffer' represents a hollow configuration item.
        /// </summary>
        /// <remarks>
        /// A hollow line just represents an empty configuration line. A empty configuration 
        /// is everything that is really empty or consists only of whitespaces.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be verified.
        /// </param>
        /// <returns>
        /// True if given 'buffer' represents a hollow configuration item and false otherwise.
        /// </returns>
        public static Boolean IsHollow(this String buffer)
        {
            return String.IsNullOrWhiteSpace(buffer);
        }

        /// <summary>
        /// This method determines if given 'buffer' represents a comment configuration item.
        /// </summary>
        /// <remarks>
        /// A comment line is determined by starting with one of the supported comment markers, 
        /// no matter if provided buffer also includes leading whitespaces.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be verified.
        /// </param>
        /// <returns>
        /// True if given 'buffer' represents a comment configuration item and false otherwise.
        /// </returns>
        public static Boolean IsComment(this String buffer)
        {
            if (String.IsNullOrWhiteSpace(buffer))
            {
                return false;
            }

            buffer = buffer.TrimStart();

            return ConfigDefines.CommentMarkers.Contains(buffer[0]);
        }

        /// <summary>
        /// This method determines if given 'buffer' represents a section configuration item.
        /// </summary>
        /// <remarks>
        /// A section is determined by it is enclosed in the section prefix and suffix no matter 
        /// if provided buffer includes leading whitespaces or contains whitespaces between prefix 
        /// and suffix.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be verified.
        /// </param>
        /// <returns>
        /// True if given 'buffer' represents a section configuration item and false otherwise.
        /// </returns>
        public static Boolean IsSection(this String buffer)
        {
            buffer = buffer?.TrimStart();

            if (String.IsNullOrEmpty(buffer))
            {
                return false;
            }

            if (buffer[0] != ConfigDefines.SectionPrefix)
            {
                return false;
            }

            for (Int32 index = 1; index < buffer.Length; index++)
            {
                if (buffer[index] == ConfigDefines.SectionSuffix)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This method determines if given 'buffer' represents a value configuration item.
        /// </summary>
        /// <remarks>
        /// As value is considered everything that starts with a label which is followed by one of 
        /// the supported value markers, no matter if provided buffer includes leading whitespaces. 
        /// Anything else is not considered as a value.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be verified.
        /// </param>
        /// <returns>
        /// True if given 'buffer' represents a value configuration item and false otherwise.
        /// </returns>
        public static Boolean IsValue(this String buffer)
        {
            if (String.IsNullOrWhiteSpace(buffer))
            {
                return false;
            }

            for (Int32 index = 0; index < buffer.Length; index++)
            {
                if (ConfigDefines.StringMarker == buffer[index])
                {
                    return false;
                }

                if (ConfigDefines.CommentMarkers.Contains(buffer[index]))
                {
                    return false;
                }

                if (ConfigDefines.ValueMarkers.Contains(buffer[index]))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
