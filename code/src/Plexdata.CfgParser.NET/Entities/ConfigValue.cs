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
using Plexdata.CfgParser.Extensions;
using Plexdata.CfgParser.Interfaces;
using Plexdata.CfgParser.Settings;
using System;
using System.Linq;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents one single configuration value item. 
    /// </summary>
    /// <remarks>
    /// Each configuration value item is built as 'label' 'marker' 'value' and 'comment', 
    /// whereby 'value' and 'comment' are treated as optional parts.
    /// </remarks>
    public class ConfigValue : IConfigEntity, IEquatable<ConfigValue>, IEquatable<String>
    {
        #region Private fields

        /// <summary>
        /// The field that holds the label of this configuration value.
        /// </summary>
        /// <remarks>
        /// The value label should not be manipulated directly, for example by 
        /// using Reflection.
        /// </remarks>
        private String label;

        /// <summary>
        /// The field that holds the value of this configuration value.
        /// </summary>
        /// <remarks>
        /// The value data should not be manipulated directly, for example by 
        /// using Reflection.
        /// </remarks>
        private String value;

        /// <summary>
        /// The field that holds the marker of this configuration value.
        /// </summary>
        /// <remarks>
        /// The value marker should not be manipulated directly, for example by 
        /// using Reflection.
        /// </remarks>
        private Char marker;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with their default values. 
        /// An instance created by this constructor is not considered to be valid.
        /// </remarks>
        public ConfigValue()
            : base()
        {
            this.label = String.Empty;
            this.value = String.Empty;
            this.marker = ConfigSettings.DefaultValueMarker;
            this.Comment = null;
        }

        /// <summary>
        /// The constructor of an instance of this class that initializes the label property.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided label. All 
        /// other properties are initialized with their default values.
        /// </remarks>
        /// <param name="label">
        /// The label to be used.
        /// </param>
        public ConfigValue(String label)
            : this(label, null)
        {
        }

        /// <summary>
        /// The constructor of an instance of this class that initializes the label property 
        /// as well as the value property.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided label and value. 
        /// All other properties are initialized with their default values.
        /// </remarks>
        /// <param name="label">
        /// The label to be used.
        /// </param>
        /// <param name="value">
        /// The value to be used.
        /// </param>
        public ConfigValue(String label, String value)
            : this(label, value, null)
        {
        }

        /// <summary>
        /// The constructor of an instance of this class that initializes the label property, 
        /// the value property as well as the comment property.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided label, value 
        /// as well as comment. The marker is initialized with its default value.
        /// </remarks>
        /// <param name="label">
        /// The label to be used.
        /// </param>
        /// <param name="value">
        /// The value to be used.
        /// </param>
        /// <param name="comment">
        /// The comment to be used.
        /// </param>
        public ConfigValue(String label, String value, String comment)
            : base()
        {
            this.Label = label;
            this.Value = value;
            this.Marker = ConfigSettings.DefaultValueMarker;
            this.Comment = null;

            if (!String.IsNullOrWhiteSpace(comment))
            {
                this.Comment = new ConfigComment(comment);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if this instance can be considered as valid.
        /// </summary>
        /// <remarks>
        /// A configuration value is considered as valid as soon as its label 
        /// is not null, not empty and does not only consist of whitespaces.
        /// </remarks>
        /// <value>
        /// True if this instance is valid and false if not.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return !String.IsNullOrWhiteSpace(this.label);
            }
        }

        /// <summary>
        /// Gets and sets the label of this configuration value.
        /// </summary>
        /// <remarks>
        /// The label of a configuration value represents the part to left of the 
        /// configuration value marker. It is recommended that a label should not 
        /// include whitespaces.
        /// </remarks>
        /// <value>
        /// The label to be used.
        /// </value>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if the label is null, or empty, or consists 
        /// of whitespaces only.
        /// </exception>
        public String Label
        {
            get
            {
                return this.label;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Value label must not be null or empty.", nameof(this.Label));
                }

                this.label = value.Trim();
            }
        }

        /// <summary>
        ///  Gets and sets the value of this configuration value.
        /// </summary>
        /// <remarks>
        /// The set value is converted into an empty string if it is null. Additionally, 
        /// the value string is trimmed.
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

        /// <summary>
        /// Gets and sets the marker of this configuration value.
        /// </summary>
        /// <remarks>
        /// The marker is used to separate the label from its value.
        /// </remarks>
        /// <value>
        /// The marker to be used.
        /// </value>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when the set marker is not one of the supported 
        /// value markers. See <see cref="ConfigDefines.ValueMarkers"/> for more 
        /// information about supported value markers.
        /// </exception>
        public Char Marker
        {
            get
            {
                return this.marker;
            }
            set
            {
                if (!ConfigDefines.ValueMarkers.Contains(value))
                {
                    throw new ArgumentException($"Marker must be one of these characters: {String.Join(", ", ConfigDefines.ValueMarkers)}.", nameof(this.Marker));
                }

                this.marker = value;
            }
        }

        /// <summary>
        /// Gets and sets the comment of this configuration value.
        /// </summary>
        /// <remarks>
        /// The comment of a configuration value can be <c>null</c> which means that 
        /// comment is optional.
        /// </remarks>
        /// <value>
        /// The comment that is assigned to this configuration value.
        /// </value>
        /// <seealso cref="ConfigComment"/>
        public ConfigComment Comment { get; set; }

        #endregion

        #region Public static methods

        /// <summary>
        /// This method parses the provided <paramref name="buffer"/> and tries to 
        /// create an instance of class <see cref="ConfigValue"/> from it.
        /// </summary>
        /// <remarks>
        /// Parsing of the provided buffer is the way of how to create an instance of 
        /// this class from a single line of a configuration file.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be parsed.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigValue"/> if parsing was successful.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if the buffer is null, or empty, or consists only 
        /// of whitespaces.
        /// </exception>
        /// <exception cref="FormatException">
        /// This exception is thrown in every case of a parsing issue. Such issues 
        /// could for example be an invalid value label.
        /// </exception>
        /// <seealso cref="ConfigValue.TryParse(String, out ConfigValue)"/>
        public static ConfigValue Parse(String buffer)
        {
            if (String.IsNullOrWhiteSpace(buffer))
            {
                throw new ArgumentException("Value cannot be null, empty or whitespace.");
            }

            buffer = buffer?.Trim();

            String label = String.Empty;
            String marker = String.Empty;
            String value = String.Empty;
            String comment = String.Empty;
            Boolean moving = false;

            for (Int32 index = 0; index < buffer.Length; index++)
            {
                Char affected = buffer[index];

                if (label.Length == 0)
                {
                    if (ConfigDefines.CommentMarkers.Contains(affected))
                    {
                        throw new FormatException("Comments must be placed at the end of a value.");
                    }

                    if (affected == ConfigDefines.StringMarker)
                    {
                        throw new FormatException("Value label must not contain string markers.");
                    }
                }

                if (affected == ConfigDefines.StringMarker)
                {
                    moving = !moving;
                }

                if (!moving && ConfigDefines.ValueMarkers.Contains(affected))
                {
                    label = buffer.Substring(0, index);
                    marker = buffer.Substring(index, 1);
                    continue;
                }

                if (!moving && ConfigDefines.CommentMarkers.Contains(affected))
                {
                    comment = buffer.Substring(index + 0);
                    break;
                }

                if (moving || label.Length != 0)
                {
                    value += affected;
                }
            }

            label = label.Trim();
            marker = marker.Trim();
            value = value.Trim().Replace(ConfigDefines.StringMarker.ToString(), String.Empty);

            if (String.IsNullOrEmpty(label))
            {
                throw new FormatException($"Value label could not be determined.");
            }

            return new ConfigValue
            {
                Label = label,
                Marker = marker[0],
                Value = value,
                Comment = ConfigComment.Parse(comment)
            };
        }

        /// <summary>
        /// This method tries parsing the provided <paramref name="buffer"/> and creates 
        /// an instance of class <see cref="ConfigValue"/> if successful.
        /// </summary>
        /// <remarks>
        /// This method is an alternative of method <see cref="ConfigValue.Parse(String)"/> 
        /// and does not throw any exception.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be parsed.
        /// </param>
        /// <param name="result">
        /// An instance of class <see cref="ConfigValue"/> if successful.
        /// </param>
        /// <returns>
        /// True is returned if buffer parsing was successful and false otherwise.
        /// </returns>
        public static Boolean TryParse(String buffer, out ConfigValue result)
        {
            try
            {
                result = ConfigValue.Parse(buffer);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method compares provided string with the label of this instance 
        /// of class <see cref="ConfigValue"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The string to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided string is equal to the label of this 
        /// configuration value. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigValue.Equals(ConfigValue)"/>
        public Boolean Equals(String other)
        {
            return String.Equals(this.Label, other, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// This method compares provided instance with the label of this instance 
        /// of class <see cref="ConfigValue"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The instance to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided instance is not null and its label is equal 
        /// to the label of this configuration value. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigValue.Equals(String)"/>
        public Boolean Equals(ConfigValue other)
        {
            if (other != null)
            {
                return this.Equals(other.Label);
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
            if (this.Comment != null && this.Comment.IsValid)
            {
                String space = String.IsNullOrWhiteSpace(this.Value) ? String.Empty : " ";
                return $"{this.Label.FixupLabel()}{this.Marker.FixupMarker()}{this.Value.FixupValue()}{space}{this.Comment.ToOutput()}";
            }
            else
            {
                return $"{this.Label.FixupLabel()}{this.Marker.FixupMarker()}{this.Value.FixupValue()}";
            }
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
            if (this.Comment != null)
            {
                return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Label)}='{this.Label}', {nameof(this.Value)}='{this.Value}', {nameof(this.Comment)}={this.Comment.IsValid}";
            }
            else
            {
                return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Label)}='{this.Label}', {nameof(this.Value)}='{this.Value}', {nameof(this.Comment)}={false}";
            }
        }

        #endregion
    }
}
