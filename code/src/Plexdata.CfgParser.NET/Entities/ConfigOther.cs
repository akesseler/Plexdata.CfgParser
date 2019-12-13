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

using Plexdata.CfgParser.Interfaces;
using System;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents one single configuration line that is neither a comment 
    /// nor a section nor a value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Other items may occur everywhere inside a configuration file. But they will be put 
    /// in front of the very first section in case of writing them into a configuration file.
    /// </para>
    /// <para>
    /// Additionally note, each other item is treated as it is. This means in detail, nothing 
    /// (except whitespace trimming) will be changed during processing such unassigned data.
    /// </para>
    /// </remarks>
    public class ConfigOther : IConfigEntity, IEquatable<ConfigOther>, IEquatable<String>
    {
        #region Private fields

        /// <summary>
        /// The field that holds the value of this other item.
        /// </summary>
        /// <remarks>
        /// The other value should not be manipulated directly, for example by 
        /// using Reflection.
        /// </remarks>
        private String value;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with their default values. 
        /// An instance created by this constructor is not considered to be valid.
        /// </remarks>
        public ConfigOther()
            : this(null)
        {
        }

        /// <summary>
        /// The constructor of an instance of this class that initializes the value property.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided value. 
        /// </remarks>
        /// <param name="value">
        /// The value to be used.
        /// </param>
        public ConfigOther(String value)
            : base()
        {
            this.Value = value;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if this instance can be considered as valid.
        /// </summary>
        /// <remarks>
        /// An other item is considered as valid as soon as its value is not null, 
        /// not empty and does not only consist of whitespaces.
        /// </remarks>
        /// <value>
        /// True if this instance is valid and false if not.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.Value);
            }
        }

        /// <summary>
        ///  Gets and sets the value of this other item.
        /// </summary>
        /// <remarks>
        /// The set value is converted into an empty string if it is null. 
        /// Additionally, the value string is trimmed.
        /// </remarks>
        /// <value>
        /// The value to be used.
        /// </value>
        public String Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = (value ?? String.Empty).Trim();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method compares provided string with the value of this instance 
        /// of class <see cref="ConfigOther"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The string to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided string is equal to the value of this 
        /// other item. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigOther.Equals(ConfigOther)"/>
        public Boolean Equals(String other)
        {
            return String.Equals(this.Value, other, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// This method compares provided instance with the value of this instance 
        /// of class <see cref="ConfigOther"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The instance to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided instance is not null and its value is equal 
        /// to the value of this other item. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigOther.Equals(String)"/>
        public Boolean Equals(ConfigOther other)
        {
            if (other != null)
            {
                return this.Equals(other.Value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method converts currently used data into its output format and returns it.
        /// </summary>
        /// <remarks>
        /// This method is especially needed to write current value data into a configuration 
        /// file.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set value data in output format.
        /// </returns>
        public String ToOutput()
        {
            return this.Value;
        }

        /// <summary>
        /// This method returns a string representation of current value data that might 
        /// be used for debugging purposes.
        /// </summary>
        /// <remarks>
        /// The method is not intended to be used for output.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set value data.
        /// </returns>
        public override String ToString()
        {
            return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Value)}='{this.Value}'";
        }

        #endregion
    }
}
