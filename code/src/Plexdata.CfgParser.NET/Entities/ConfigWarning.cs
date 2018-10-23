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

using Plexdata.CfgParser.Interfaces;
using System;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents a configuration content warning.
    /// </summary>
    /// <remarks>
    /// A warning is caused for example by comments that do not belong to the header. 
    /// Another reason for a warning is for example a label-data-pair which does not 
    /// belong to a section. Such misplaced configuration items a managed within an 
    /// instance of this class.
    /// </remarks>
    public class ConfigWarning : IConfigItem
    {
        #region Construction

        /// <summary>
        /// The parameterized constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided line, value 
        /// as well as message.
        /// </remarks>
        /// <param name="line">
        /// The line number which caused the warning.
        /// </param>
        /// <param name="value">
        /// The line content which caused the warning.
        /// </param>
        /// <param name="message">
        /// An additional message that describes the reason of the warning.
        /// </param>
        public ConfigWarning(Int32 line, String value, String message)
            : base()
        {
            this.Line = line;
            this.Value = value ?? String.Empty;
            this.Message = message ?? String.Empty;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if this instance can be considered as valid.
        /// </summary>
        /// <remarks>
        /// At the moment this property always returns true.
        /// </remarks>
        /// <value>
        /// True if this instance is valid and false if not.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the line number which caused the warning.
        /// </summary>
        /// <remarks>
        /// This line number might be used to quickly find the problematically 
        /// configuration line.
        /// </remarks>
        /// <value>
        /// The one-based line index.
        /// </value>
        public Int32 Line { get; private set; }

        /// <summary>
        /// Gets the line content which caused the warning.
        /// </summary>
        /// <remarks>
        /// The value is the same as it occurs within the configuration file.
        /// </remarks>
        /// <value>
        /// The line content.
        /// </value>
        public String Value { get; private set; }

        /// <summary>
        /// Gets the additional message that describes the reason of the 
        /// warning.
        /// </summary>
        /// <remarks>
        /// The message might be useful to have a hint on what the problem has caused.
        /// </remarks>
        /// <value>
        /// The additional message.
        /// </value>
        public String Message { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// This method returns a string representation of current content data that might 
        /// be used for debugging purposes.
        /// </summary>
        /// <remarks>
        /// The method is not intended to be used for output.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set content data.
        /// </returns>
        public override String ToString()
        {
            return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Line)}='{this.Line}', {nameof(this.Message)}='{this.Message}', {nameof(this.Value)}='{this.Value}'";
        }

        #endregion
    }
}
