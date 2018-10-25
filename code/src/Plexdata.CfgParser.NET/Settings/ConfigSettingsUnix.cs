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

using Plexdata.CfgParser.Constants;

namespace Plexdata.CfgParser.Settings
{
    /// <summary>
    /// This class represent a set of configuration settings that uses Unix configuration 
    /// file style.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Unix configuration file style actually means that comments use the hash character (#) 
    /// and the colon character (:) is used to separate value labels from their value data.
    /// See example for more details.
    /// </para>
    /// <para>
    /// Keep in mind, such a configuration file style is only important when writing respectively 
    /// saving a configuration file.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// Below find an example of how a configuration file would look like if this style is used.
    /// </para>
    /// <code>
    /// # comment line 1
    /// # comment line 2
    /// 
    /// [section-1]
    /// label-11 : value-11
    /// label-12 : value-12 # comment at value 1.2
    /// 
    /// [section-2]
    /// label-21 : value-21
    /// label-22 : value-22 # comment at value 2.2
    /// </code>
    /// </example>
    /// <seealso cref="ConfigDefines.ValueMarkers"/>
    /// <seealso cref="ConfigDefines.CommentMarkers"/>
    public sealed class ConfigSettingsUnix : ConfigSettingsBase
    {
        #region Construction

        /// <summary>
        /// The default constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// The value marker ':' and the comment marker '#' is used by default.
        /// </remarks>
        public ConfigSettingsUnix()
            : base(ConfigDefines.ValueMarkers[0], ConfigDefines.CommentMarkers[0])
        {
        }

        #endregion
    }
}
