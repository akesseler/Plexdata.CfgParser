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

using System;

namespace Plexdata.CfgParser.Attributes
{
    /// <summary>
    /// This attribute represents the header of a configuration container.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A top-level configuration class using this property is able to 
    /// generate a configuration header. But this applies only when the 
    /// configuration file will be written.
    /// </para>
    /// <para>
    /// Furthermore, it will be possible to choose either the default 
    /// configuration header or (this is new) to choose a simplified 
    /// configuration header by adjusting the attribute properties 
    /// accordingly.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigHeaderAttribute : Attribute
    {
        #region Construction

        /// <summary>
        /// The default attribute constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes all class properties with its 
        /// default values.
        /// </remarks>
        public ConfigHeaderAttribute()
            : base()
        {
            this.IsExtended = true;
            this.Title = null;
            this.Placeholders = false;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Enables or disables extended header usage.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property allows to enable or to disable a usage of the 
        /// extended header. This attribute can only be used for classes 
        /// that contain config section attributes!
        /// </para>
        /// <para>
        /// </para>
        /// </remarks>
        /// <value>
        /// True to enable extended header usage and false to disable 
        /// extended header usage. By default, the usage of extended 
        /// header is enabled.
        /// </value>
        public Boolean IsExtended { get; set; }

        /// <summary>
        /// Gets or sets the header title.
        /// </summary>
        /// <remarks>
        /// This property allows to get or to set the header title.
        /// </remarks>
        /// <value>
        /// A string representing the header title or null or empty 
        /// to disable header title usage. By default, the title is 
        /// not used.
        /// </value>
        public String Title { get; set; }

        /// <summary>
        /// Enables or disables header placeholders usage.
        /// </summary>
        /// <remarks>
        /// This property allows to enable or to disable a usage of 
        /// the header placeholders.
        /// </remarks>
        /// <value>
        /// True to enable header placeholders usage and false to 
        /// disable this feature. By default, the usage of header 
        /// placeholders is disabled.
        /// </value>
        public Boolean Placeholders { get; set; }

        #endregion
    }
}
