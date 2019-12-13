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
    /// This attribute represents a property that is used as a configuration value.
    /// </summary>
    /// <remarks>
    /// A configuration value is a property that represents a label-value-combinations.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigValueAttribute : Attribute
    {
        #region Construction

        /// <summary>
        /// The default attribute constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes all class properties with its default values.
        /// </remarks>
        public ConfigValueAttribute()
            : this(null)
        {
        }

        /// <summary>
        /// The attribute constructor that initializes the label property.
        /// </summary>
        /// <remarks>
        /// The constructor just initializes the label property. All other class 
        /// properties are initialized with its default values.
        /// </remarks>
        /// <param name="label">
        /// The label to be used.
        /// </param>
        public ConfigValueAttribute(String label)
            : base()
        {
            this.Label = label ?? String.Empty;
            this.Comment = String.Empty;
            this.Default = null;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets the label of a configuration value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The label of a configuration value represents the configuration value name 
        /// that is used inside a configuration file.
        /// </para>
        /// The name of the assigned property is used as label if this argument is not set.
        /// <para>
        /// </para>
        /// </remarks>
        /// <value>
        /// The label to be used.
        /// </value>
        public String Label { get; set; }

        /// <summary>
        /// Gets and sets the comment of a configuration value.
        /// </summary>
        /// <remarks>
        /// No comment is used if this argument is not set.
        /// </remarks>
        /// <value>
        /// The comment to be used.
        /// </value>
        public String Comment { get; set; }

        /// <summary>
        /// Gets and sets the default value of a configuration value.
        /// </summary>
        /// <remarks>
        /// At the moment this default value in not used.
        /// </remarks>
        /// <value>
        /// The default value to be used.
        /// </value>
        public Object Default { get; set; }

        #endregion
    }
}
