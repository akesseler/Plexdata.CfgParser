/*
 * MIT License
 * 
 * Copyright (c) 2018 plexdata.de
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

using Plexdata.CfgParser.Settings;
using System;

namespace Plexdata.CfgParser.Constants
{
    /// <summary>
    /// This class contains all supported constant values.
    /// </summary>
    /// <remarks>
    /// This pure static class just serves as a set of constant values 
    /// used everywhere in the library.
    /// </remarks>
    public static class ConfigDefines
    {
        #region Public static fields

        /// <summary>
        /// The open prefix for a configuration section.
        /// </summary>
        /// <remarks>
        /// The section prefix is set to character <c>[</c>.
        /// </remarks>
        public static readonly Char SectionPrefix = '[';

        /// <summary>
        /// The close suffix for a configuration section.
        /// </summary>
        /// <remarks>
        /// The section suffix is set to character <c>]</c>.
        /// </remarks>
        public static readonly Char SectionSuffix = ']';

        /// <summary>
        /// The list of supported value markers.
        /// </summary>
        /// <remarks>
        /// The first marker in the list represents Unix-style value marker 
        /// (which is <c>:</c>) and the second one represents Windows-style 
        /// value marker (which is <c>=</c>) .
        /// </remarks>
        /// <seealso cref="ConfigSettings"/>
        public static readonly Char[] ValueMarkers = new Char[] { ':', '=' };

        /// <summary>
        /// The list of supported comment markers.
        /// </summary>
        /// <remarks>
        /// The first marker in the list represents Unix-style comment marker 
        /// (which is <c>#</c>) and the second one represents Windows-style 
        /// comment marker (which is <c>;</c>) .
        /// </remarks>
        /// <seealso cref="ConfigSettings"/>
        public static readonly Char[] CommentMarkers = new Char[] { '#', ';' };

        /// <summary>
        /// The start and end character that surrounds string data.
        /// </summary>
        /// <remarks>
        /// The string marker is set to character <c>"</c>.
        /// </remarks>
        public static readonly Char StringMarker = '"';

        /// <summary>
        /// The placeholder for the file name.
        /// </summary>
        /// <remarks>
        /// The file name placeholder is set to <c>{file-name-placeholder}</c>. It is used 
        /// when the output is generated. Each comment that includes this placeholder will 
        /// contain the name of the output file.
        /// </remarks>
        public static readonly String FileNamePlaceholder = "{file-name-placeholder}";

        /// <summary>
        /// The placeholder for the file date.
        /// </summary>
        /// <remarks>
        /// The file date placeholder is set to <c>{file-date-placeholder}</c>. It is used 
        /// when the output is generated. Each comment that includes this placeholder will 
        /// contain the timestamp in ISO format of the file's creation date.
        /// </remarks>
        public static readonly String FileDatePlaceholder = "{file-date-placeholder}";

        #endregion

        #region Construction

        /// <summary>
        /// The static constructor initializes all static fields of the <see cref="ConfigDefines"/> 
        /// class.
        /// </summary>
        /// <remarks>
        /// Nothing else but the static field initialization is done.
        /// </remarks>
        static ConfigDefines() { }

        #endregion
    }
}

