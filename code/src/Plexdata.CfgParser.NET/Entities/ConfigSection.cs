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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents one single configuration section item. 
    /// </summary>
    /// <remarks>
    /// Each configuration section item is built as '[section-name]' and 'comment', 
    /// whereby the 'comment' is treated as optional part. 
    /// </remarks>
    public class ConfigSection : IConfigArray<ConfigValue>, IEquatable<ConfigSection>, IEquatable<String>
    {
        #region Private fields

        /// <summary>
        /// The field that holds the title of this configuration section.
        /// </summary>
        /// <remarks>
        /// The section title should not be manipulated directly, for example by 
        /// using Reflection.
        /// </remarks>
        private String title;

        /// <summary>
        /// The field that holds the list of all configuration values assigned 
        /// to this configuration section.
        /// </summary>
        /// <remarks>
        /// The list of configuration values should not be manipulated directly, for 
        /// example by using Reflection.
        /// </remarks>
        private readonly List<ConfigValue> values;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with their default values. 
        /// An instance created by this constructor is not considered to be valid.
        /// </remarks>
        public ConfigSection()
            : base()
        {
            this.title = String.Empty;
            this.Comment = null;
            this.values = new List<ConfigValue>();
        }

        /// <summary>
        /// The constructor of an instance of this class that initializes the title property.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided title. All 
        /// other properties are initialized with their default values.
        /// </remarks>
        /// <param name="title">
        /// The title to be used.
        /// </param>
        public ConfigSection(String title)
            : this(title, null)
        {
        }

        /// <summary>
        /// The constructor of an instance of this class that initializes the title property 
        /// as well as the comment property.
        /// </summary>
        /// <remarks>
        /// This constructor initializes an instance of this class using provided label and 
        /// comment.
        /// </remarks>
        /// <param name="title">
        /// The title to be used.
        /// </param>
        /// <param name="comment">
        /// The comment to be used.
        /// </param>
        public ConfigSection(String title, String comment)
            : base()
        {
            this.Title = title;
            this.Comment = null;
            this.values = new List<ConfigValue>();

            if (!String.IsNullOrWhiteSpace(comment))
            {
                this.Comment = new ConfigComment(comment);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the configuration value at the specified index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to set a configuration 
        /// value.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// An exception of type <see cref="ArgumentNullException"/> is thrown if value 
        /// is <c>null</c>.
        /// </description></item>
        /// <item><description>
        /// The value is prepended or appended accordingly if provided index is out of 
        /// range.
        /// </description></item>
        /// <item><description>
        /// The value is replaced if provided index is in-range.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <param name="index">
        /// The zero-based index of the configuration value to get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigValue"/>.
        /// </value>
        /// <returns>
        /// The configuration value at the specified index or <c>null</c> if provided 
        /// index is out of range.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        public ConfigValue this[Int32 index]
        {
            get
            {
                if (index >= 0 && index < this.values.Count)
                {
                    return this.values[index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (index >= 0 && index < this.values.Count)
                {
                    this.values[index] = value;
                }
                else
                {
                    this.Insert(index, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the configuration value for the specified key.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to get or set a 
        /// configuration value.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// An exception of type <see cref="ArgumentNullException"/> is thrown if 
        /// value is <c>null</c>.
        /// </description></item>
        /// <item><description>
        /// An exception of type <see cref="ArgumentException"/> is thrown if key 
        /// is null, or empty, or includes whitespaces only.
        /// </description></item>
        /// <item><description>
        /// The value is appended if provided key could not be found.
        /// </description></item>
        /// <item><description>
        /// The value is replaced if provided key could be found.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <param name="key">
        /// The key (which is a value's label) of the configuration value to 
        /// get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigValue"/>.
        /// </value>
        /// <returns>
        /// The configuration value for specified key or <c>null</c> if provided 
        /// key could not be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when trying to get or set a value using an 
        /// invalid key.
        /// </exception>
        public ConfigValue this[String key]
        {
            get
            {
                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Value key must not be null or empty.", nameof(key));
                }

                return this.Find(key);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Value key must not be null or empty.", nameof(key));
                }

                if (String.IsNullOrWhiteSpace(value.Label))
                {
                    value.Label = key;
                }

                for (Int32 index = 0; index < this.values.Count; index++)
                {
                    if (this.values[index].Equals(key))
                    {
                        this.values[index] = value;
                        return;
                    }
                }

                this.Append(value);
            }
        }

        /// <summary>
        /// Returns the number of currently available configuration values.
        /// </summary>
        /// <remarks>
        /// The property just returns the number of currently available value items.
        /// </remarks>
        /// <value>
        /// The number of available configuration values.
        /// </value>
        public Int32 Count
        {
            get
            {
                return this.values.Count;
            }
        }

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
        /// Gets and sets the used section title.
        /// </summary>
        /// <remarks>
        /// The provided title value is trimmed.
        /// </remarks>
        /// <value>
        /// The section title to be used.
        /// </value>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when trying to set a value that is null 
        /// or empty or only consists of whitespaces.
        /// </exception>
        public String Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Section title must not be null or empty.", nameof(this.Title));
                }

                this.title = value.Trim();
            }
        }

        /// <summary>
        /// Gets and sets the comment of this configuration section.
        /// </summary>
        /// <remarks>
        /// The comment of a configuration section can be <c>null</c> which means that 
        /// comment is optional.
        /// </remarks>
        /// <value>
        /// The comment that is assigned to this configuration section.
        /// </value>
        /// <seealso cref="ConfigComment"/>
        public ConfigComment Comment { get; set; }

        /// <summary>
        /// Gets an enumerable list of all assigned configuration values.
        /// </summary>
        /// <remarks>
        /// This property returns an enumerable list of all currently assigned configuration 
        /// values belonging to this section.
        /// </remarks>
        /// <value>
        /// An enumerable list of all configuration values.
        /// </value>
        public IEnumerable<ConfigValue> Values
        {
            get
            {
                foreach (ConfigValue value in this.values)
                {
                    yield return value;
                }
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// This method parses the provided <paramref name="buffer"/> and tries to 
        /// create an instance of class <see cref="ConfigSection"/> from it.
        /// </summary>
        /// <remarks>
        /// Parsing of the provided buffer is the way of how to create an instance of 
        /// this class from a single line of a configuration file.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be parsed.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigSection"/> if parsing was successful.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if the buffer is null, or empty, or consists only 
        /// of whitespaces.
        /// </exception>
        /// <exception cref="FormatException">
        /// This exception is thrown in every case of a parsing issue. Such issues 
        /// could for example be an invalid section title.
        /// </exception>
        /// <seealso cref="ConfigSection.TryParse(String, out ConfigSection)"/>
        public static ConfigSection Parse(String buffer)
        {
            if (String.IsNullOrWhiteSpace(buffer))
            {
                throw new ArgumentException("Section cannot be null, empty or whitespace.");
            }

            buffer = buffer?.Trim();

            if (buffer[0] != ConfigDefines.SectionPrefix)
            {
                throw new FormatException($"Buffer \"{buffer}\" could not be confirmed as section.");
            }

            String section = String.Empty;
            String comment = String.Empty;

            for (Int32 index = 1; index < buffer.Length; index++)
            {
                if (buffer[index] == ConfigDefines.SectionSuffix)
                {
                    section = buffer.Substring(1, index - 1);
                    comment = buffer.Substring(index + 1);
                    break;
                }
            }

            section = section.Trim();

            if (String.IsNullOrEmpty(section))
            {
                throw new FormatException($"Section title could not be determined.");
            }

            return new ConfigSection
            {
                Title = section,
                Comment = ConfigComment.Parse(comment)
            };
        }

        /// <summary>
        /// This method tries parsing the provided <paramref name="buffer"/> and creates 
        /// an instance of class <see cref="ConfigSection"/> if successful.
        /// </summary>
        /// <remarks>
        /// This method is an alternative of method <see cref="ConfigSection.Parse(String)"/> 
        /// and does not throw any exception.
        /// </remarks>
        /// <param name="buffer">
        /// The buffer to be parsed.
        /// </param>
        /// <param name="result">
        /// An instance of class <see cref="ConfigSection"/> if successful.
        /// </param>
        /// <returns>
        /// True is returned if buffer parsing was successful and false otherwise.
        /// </returns>
        public static Boolean TryParse(String buffer, out ConfigSection result)
        {
            try
            {
                result = ConfigSection.Parse(buffer);
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
        /// This method removes all currently assigned configuration values.
        /// </summary>
        /// <remarks>
        /// This method just removes all available configuration values.
        /// </remarks>
        public void Clear()
        {
            this.values.Clear();
        }

        /// <summary>
        /// This method tries to find a configuration value for a provided 
        /// configuration value label.
        /// </summary>
        /// <remarks>
        /// Searching for configuration values takes place using invariant culture 
        /// as well as ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The configuration value label to search for.
        /// </param>
        /// <returns>
        /// The found configuration value instance or <c>null</c> if not found.
        /// </returns>
        /// <seealso cref="ConfigValue.Equals(String)"/>
        public ConfigValue Find(String value)
        {
            for (Int32 index = 0; index < this.values.Count; index++)
            {
                if (this.values[index].Equals(value))
                {
                    return this.values[index];
                }
            }

            return null;
        }

        /// <summary>
        /// This method appends a new instance of class <see cref="ConfigValue"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new value item and appends it to the list of values.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial value label.
        /// </param>
        /// <returns>
        /// The newly appended instance of class <see cref="ConfigValue"/>.
        /// </returns>
        /// <seealso cref="ConfigSection.Append(ConfigValue)"/>
        public ConfigValue Append(String value)
        {
            return this.Append(new ConfigValue(value));
        }

        /// <summary>
        /// This method appends the provided instance of class <see cref="ConfigValue"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method appends provided value to the list of values.
        /// </remarks>
        /// <param name="entity">
        /// The configuration value to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigValue"/>.
        /// </returns>
        /// <seealso cref="ConfigSection.Insert(Int32, ConfigValue)"/>
        public ConfigValue Append(ConfigValue entity)
        {
            return this.Insert(Int32.MaxValue, entity);
        }

        /// <summary>
        /// This method tries to insert a new instance of class <see cref="ConfigValue"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The new instance is prepended or appended accordingly if provided index is 
        /// out of range.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new configuration value instance.
        /// </param>
        /// <param name="value">
        /// The string to be used as initial value label.
        /// </param>
        /// <returns>
        /// The newly inserted instance of class <see cref="ConfigValue"/>.
        /// </returns>
        /// <seealso cref="ConfigSection.Insert(Int32, ConfigValue)"/>
        public ConfigValue Insert(Int32 index, String value)
        {
            return this.Insert(index, new ConfigValue(value));
        }

        /// <summary>
        /// This method tries to insert the provided instance of class <see cref="ConfigValue"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The provided configuration value is prepended if the index is less than zero 
        /// and it is appended if the index is greater or equal to current value count.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new configuration value instance.
        /// </param>
        /// <param name="entity">
        /// The configuration value to be inserted.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided instance of class <see cref="ConfigValue"/> 
        /// is <c>null</c>.
        /// </exception>
        /// <seealso cref="ConfigSection.Insert(Int32, String)"/>
        public ConfigValue Insert(Int32 index, ConfigValue entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (index < 0)
            {
                index = 0;
            }
            else if (index >= this.values.Count)
            {
                index = this.values.Count;
            }

            this.values.Insert(index, entity);

            return entity;
        }

        /// <summary>
        /// This method prepends a new instance of class <see cref="ConfigValue"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new vlaue item and prepends it to the list of values.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial value label.
        /// </param>
        /// <returns>
        /// The newly prepended instance of class <see cref="ConfigValue"/>.
        /// </returns>
        /// <seealso cref="ConfigSection.Prepend(ConfigValue)"/>
        public ConfigValue Prepend(String value)
        {
            return this.Prepend(new ConfigValue(value));
        }

        /// <summary>
        /// This method prepends the provided instance of class <see cref="ConfigValue"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method prepends provided value to the list of values.
        /// </remarks>
        /// <param name="entity">
        /// The configuration value to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigValue"/>.
        /// </returns>
        /// <seealso cref="ConfigSection.Insert(Int32, ConfigValue)"/>
        public ConfigValue Prepend(ConfigValue entity)
        {
            return this.Insert(Int32.MinValue, entity);
        }

        /// <summary>
        /// This method tries to find a configuration value and removes it from the 
        /// value list. Further, the removed configuration value is returned.
        /// </summary>
        /// <remarks>
        /// Searching for configuration values takes place using invariant culture 
        /// as well as ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The configuration value label to be removed.
        /// </param>
        /// <returns>
        /// The instance of the removed configuration value, or <c>null</c> if not 
        /// found.
        /// </returns>
        /// <seealso cref="ConfigSection.Remove(Int32)"/>
        public ConfigValue Remove(String value)
        {
            for (Int32 index = 0; index < this.values.Count; index++)
            {
                if (this.values[index].Equals(value))
                {
                    return this.Remove(index);
                }
            }

            return null;
        }

        /// <summary>
        /// This method tries to remove a configuration value from the value list. 
        /// Further, the removed configuration value is returned.
        /// </summary>
        /// <remarks>
        /// Removing items for an invalid index will not cause an exception.
        /// </remarks>
        /// <param name="index">
        /// The index where to remove the configuration value.
        /// </param>
        /// <returns>
        /// The instance of the removed configuration value, or <c>null</c> if index 
        /// was out of range.
        /// </returns>
        /// <seealso cref="ConfigSection.Remove(String)"/>
        public ConfigValue Remove(Int32 index)
        {
            ConfigValue result = null;

            if (0 <= index && index < this.values.Count)
            {
                result = this.values[index];
                this.values.RemoveAt(index);
            }

            return result;
        }

        /// <summary>
        /// This method compares provided string with the title of this instance 
        /// of class <see cref="ConfigSection"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The string to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided string is equal to the title of this 
        /// configuration section. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigSection.Equals(ConfigSection)"/>
        public Boolean Equals(String other)
        {
            return String.Equals(this.Title, other, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// This method compares provided instance with the title of this instance 
        /// of class <see cref="ConfigSection"/>.
        /// </summary>
        /// <remarks>
        /// The string comparison takes place using invariant culture and ignoring 
        /// upper and lower cases.
        /// </remarks>
        /// <param name="other">
        /// The instance to be compared.
        /// </param>
        /// <returns>
        /// True is returned if provided instance is not null and its title is equal 
        /// to the title of this configuration section. False is returned otherwise.
        /// </returns>
        /// <seealso cref="ConfigSection.Equals(String)"/>
        public Boolean Equals(ConfigSection other)
        {
            if (other != null)
            {
                return this.Equals(other.Title);
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
        /// This method is especially needed to write current section data into a configuration 
        /// file.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set section data in output format.
        /// </returns>
        public IEnumerable<String> ToOutput()
        {
            if (this.Comment != null && this.Comment.IsValid)
            {
                yield return $"{ConfigDefines.SectionPrefix}{this.Title.FixupTitle()}{ConfigDefines.SectionSuffix} {this.Comment.ToOutput()}";
            }
            else
            {
                yield return $"{ConfigDefines.SectionPrefix}{this.Title.FixupTitle()}{ConfigDefines.SectionSuffix}";
            }

            if (this.values.Any())
            {
                foreach (ConfigValue value in this.values)
                {
                    yield return value.ToOutput();
                }
            }

            yield return String.Empty;
        }

        /// <summary>
        /// This method returns a string representation of current section data that might 
        /// be used for debugging purposes.
        /// </summary>
        /// <remarks>
        /// The method is not intended to be used for output.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set section data.
        /// </returns>
        public override String ToString()
        {
            if (this.Comment != null)
            {
                return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Title)}='{this.Title}', {nameof(this.Values)}={this.Count}, {nameof(this.Comment)}={this.Comment.IsValid}";
            }
            else
            {
                return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Title)}='{this.Title}', {nameof(this.Values)}={this.Count}, {nameof(this.Comment)}={false}";
            }
        }

        #endregion
    }
}
