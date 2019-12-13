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
    /// This attribute represents a property that is used as a configuration section.
    /// </summary>
    /// <remarks>
    /// A configuration section is a class that contains pairs of label-value-combinations.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigSectionAttribute : Attribute
    {
        #region Construction

        /// <summary>
        /// The default attribute constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes all class properties with its default values.
        /// </remarks>
        public ConfigSectionAttribute()
          : this(null)
        {
        }

        /// <summary>
        /// The attribute constructor that initializes the title property.
        /// </summary>
        /// <remarks>
        /// The constructor just initializes the title property. All other class 
        /// properties are initialized with its default values.
        /// </remarks>
        /// <param name="title">
        /// The title to be used.
        /// </param>
        public ConfigSectionAttribute(String title)
            : base()
        {
            this.Title = title ?? String.Empty;
            this.Comment = String.Empty;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets the title of a configuration section.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The title of a configuration section represents the configuration section name 
        /// that is used inside a configuration file.
        /// </para>
        /// The name of the assigned property is used as title if this argument is not set.
        /// <para>
        /// </para>
        /// </remarks>
        /// <value>
        /// The title to be used.
        /// </value>
        public String Title { get; set; }

        /// <summary>
        /// Gets and sets the comment of a configuration section.
        /// </summary>
        /// <remarks>
        /// No comment is used if this argument is not set.
        /// </remarks>
        /// <value>
        /// The comment to be used.
        /// </value>
        public String Comment { get; set; }

        #endregion
    }
}
