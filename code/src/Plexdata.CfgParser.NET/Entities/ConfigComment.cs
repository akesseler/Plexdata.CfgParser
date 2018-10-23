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
using Plexdata.CfgParser.Interfaces;
using Plexdata.CfgParser.Settings;
using System;
using System.Linq;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents one single configuration comment item. 
    /// </summary>
    /// <remarks>
    /// Each configuration comment item consists of a textual description and is built 
    /// by prefixing it by one of the supported comment separators.
    /// </remarks>
    public class ConfigComment : IConfigEntity, IEquatable<ConfigComment>, IEquatable<String>
    {
        #region Private fields

        /// <summary>
        /// The field that holds the text of this configuration comment.
        /// </summary>
        /// <remarks>
        /// The comment text should not be manipulated directly, for example by 
        /// using Reflection.
        /// </remarks>
        private String text;

        /// <summary>
        /// The field that holds the marker of this configuration comment.
        /// </summary>
        /// <remarks>
        /// The comment marker should not be manipulated directly, for example by 
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
        public ConfigComment()
            : this(null)
        {
        }

        /// <summary>
        /// The constructor of an instance of this class that initializes the text property.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided text. The 
        /// marker is initialized with its default value.
        /// </remarks>
        /// <param name="text">
        /// The text to be used.
        /// </param>
        public ConfigComment(String text)
            : base()
        {
            this.Text = text;
            this.Marker = ConfigSettings.DefaultCommentMarker;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if this instance can be considered as valid.
        /// </summary>
        /// <remarks>
        /// A configuration comment is considered as valid as soon as its text
        /// is not null, not empty and does not only consist of whitespaces.
        /// </remarks>
        /// <value>
        /// True if this instance is valid and false if not.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return !String.IsNullOrEmpty(this.Text);
            }
        }

        /// <summary>
        /// Gets and sets the text of this configuration comment.
        /// </summary>
        /// <remarks>
        /// The set value is converted into an empty string if it is null. Additionally, 
        /// the value string is trimmed.
        /// </remarks>
        /// <value>
        /// The text to be used.
        /// </value>
        public String Text
        {
            get
            {
                return this.text;
            }
            set
            {
                value = value?.Trim();
                this.text = value ?? String.Empty;
            }
        }

        /// <summary>
        /// Gets and sets the marker of this configuration comment.
        /// </summary>
        /// <remarks>
        /// The marker is used to define that a configuration line represents a comment.
        /// </remarks>
        /// <value>
        /// The marker to be used.
        /// </value>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when the set marker is not one of the supported 
        /// comment markers. See <see cref="ConfigDefines.CommentMarkers"/> for more 
        /// information about supported comment markers.
        /// </exception>
        public Char Marker
        {
            get
            {
                return this.marker;
            }
            set
            {
                if (!ConfigDefines.CommentMarkers.Contains(value))
                {
                    throw new ArgumentException($"Marker must be one of these characters: {String.Join(", ", ConfigDefines.CommentMarkers)}.", nameof(this.Marker));
                }

                this.marker = value;
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// This method parses the provided <paramref name="buffer"/> and tries to 
        /// create an instance of class <see cref="ConfigComment"/> from it.
        /// </summary>
        /// <remarks>
        /// Parsing of the provided buffer is the way of how to create an instance of 
        /// this class from a single line of a configuration file.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be parsed.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigComment"/> if parsing was successful. A 
        /// value of <c>null</c> is returned if the buffer is null, or empty, or consists 
        /// only of whitespaces.
        /// </returns>
        /// <exception cref="FormatException">
        /// This exception is thrown in every case of a parsing issue. Such issues 
        /// could for example be an invalid comment marker.
        /// </exception>
        /// <seealso cref="ConfigComment.TryParse(String, out ConfigComment)"/>
        public static ConfigComment Parse(String buffer)
        {
            buffer = buffer?.Trim();

            if (String.IsNullOrEmpty(buffer))
            {
                return null;
            }

            if (!ConfigDefines.CommentMarkers.Contains(buffer[0]))
            {
                throw new FormatException($"Buffer \"{buffer}\" could not be confirmed as comment.");
            }

            return new ConfigComment
            {
                Text = buffer.Substring(1),
                Marker = buffer[0]
            };
        }

        /// <summary>
        /// This method tries parsing the provided <paramref name="buffer"/> and creates 
        /// an instance of class <see cref="ConfigComment"/> if successful.
        /// </summary>
        /// <remarks>
        /// This method is an alternative of method <see cref="ConfigComment.Parse(String)"/> 
        /// and does not throw any exception.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be parsed.
        /// </param>
        /// <param name="result">
        /// An instance of class <see cref="ConfigComment"/> if successful.
        /// </param>
        /// <returns>
        /// True is returned if buffer parsing was successful and false otherwise.
        /// </returns>
        public static Boolean TryParse(String buffer, out ConfigComment result)
        {
            try
            {
                result = ConfigComment.Parse(buffer);
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
        /// This method compares provided string with the text of this instance 
        /// of class <see cref="ConfigComment"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The string to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided string is equal to the text of this 
        /// configuration comment. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigComment.Equals(ConfigComment)"/>
        public Boolean Equals(String other)
        {
            return String.Equals(this.Text, other, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// This method compares provided instance with the text of this instance 
        /// of class <see cref="ConfigComment"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The instance to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided instance is not null and its text is equal 
        /// to the label of this configuration comment. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigComment.Equals(String)"/>
        public Boolean Equals(ConfigComment other)
        {
            if (other != null)
            {
                return this.Equals(other.Text);
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
            return $"{this.Marker} {this.Text}";
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
            return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Text)}='{this.Text}'";
        }

        #endregion
    }
}
