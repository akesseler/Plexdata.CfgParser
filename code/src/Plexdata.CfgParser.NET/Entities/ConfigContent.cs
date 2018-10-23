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
using Plexdata.CfgParser.Processors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents the content of a configuration file.
    /// </summary>
    /// <remarks>
    /// This class can be used for both, reading the content of a configuration file as well 
    /// as for creating or updating and writing an own content into a configuration file.
    /// </remarks>
    public class ConfigContent : IConfigArray<ConfigSection>
    {
        #region Private fields

        /// <summary>
        /// The field that holds the list of all sections assigned to this content.
        /// </summary>
        /// <remarks>
        /// The list of configuration sections should not be manipulated directly, for 
        /// example by using Reflection.
        /// </remarks>
        private readonly List<ConfigSection> sections;

        /// <summary>
        /// The field that holds the currently used configuration header.
        /// </summary>
        /// <remarks>
        /// The configuration header should not be manipulated directly, for example by 
        /// using Reflection.
        /// </remarks>
        private ConfigHeader header;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with their default values. 
        /// An instance created by this constructor is not considered to be valid.
        /// </remarks>
        public ConfigContent()
        {
            this.header = new ConfigHeader();
            this.Others = new ConfigOthers();
            this.Filename = String.Empty;
            this.sections = new List<ConfigSection>();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the configuration section at the specified index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to set a configuration 
        /// section.
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
        /// The zero-based index of the configuration section to get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigSection"/>.
        /// </value>
        /// <returns>
        /// The configuration section at the specified index or <c>null</c> if provided 
        /// index is out of range.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        public ConfigSection this[Int32 index]
        {
            get
            {
                if (index >= 0 && index < this.sections.Count)
                {
                    return this.sections[index];
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

                if (index >= 0 && index < this.sections.Count)
                {
                    this.sections[index] = value;
                }
                else
                {
                    this.Insert(index, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the configuration section for the specified key.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to get or set a 
        /// configuration section.
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
        /// The key (which is a section's title) of the configuration section to 
        /// get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigSection"/>.
        /// </value>
        /// <returns>
        /// The configuration section for specified key or <c>null</c> if provided 
        /// key could not be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when trying to get or set a value using an 
        /// invalid key.
        /// </exception>
        public ConfigSection this[String key]
        {
            get
            {
                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Section key must not be null or empty.", nameof(key));
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
                    throw new ArgumentException("Section key must not be null or empty.", nameof(key));
                }

                if (String.IsNullOrWhiteSpace(value.Title))
                {
                    value.Title = key;
                }

                for (Int32 index = 0; index < this.sections.Count; index++)
                {
                    if (this.sections[index].Equals(key))
                    {
                        this.sections[index] = value;
                        return;
                    }
                }

                this.Append(value);
            }
        }

        /// <summary>
        /// The filename that has been used during reading and/or writing of a configuration 
        /// file.
        /// </summary>
        /// <remarks>
        /// The filename property is mainly managed by the classes <see cref="ConfigReader"/> and 
        /// <see cref="ConfigWriter"/> and might be therefore invalid under various circumstances.
        /// </remarks>
        /// <value>
        /// The filename of the configuration file.
        /// </value>
        public String Filename { get; internal set; }

        /// <summary>
        /// Returns the number of currently available configuration sections.
        /// </summary>
        /// <remarks>
        /// The property just returns the number of currently available section items.
        /// </remarks>
        /// <value>
        /// The number of available configuration sections.
        /// </value>
        public Int32 Count
        {
            get
            {
                return this.sections.Count;
            }
        }

        /// <summary>
        /// Determines if this instance can be considered as valid.
        /// </summary>
        /// <remarks>
        /// A configuration content is considered as valid as soon as it contains 
        /// one section.
        /// </remarks>
        /// <value>
        /// True if this instance is valid and false if not.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return this.sections.Any();
            }
        }

        /// <summary>
        /// Gets or sets the configuration header.
        /// </summary>
        /// <remarks>
        /// The currently used configuration header is replaced by en empty header 
        /// if provided value is <c>null</c>.
        /// </remarks>
        /// <value>
        /// The configuration header.
        /// </value>
        public ConfigHeader Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value ?? new ConfigHeader();
            }
        }

        /// <summary>
        /// Gets the configuration others.
        /// </summary>
        /// <remarks>
        /// This property may contain some items after reading a configuration file. In such 
        /// a case it means that at least one line inside the configuration file is neither 
        /// a header comment nor a section nor a value-data-pair.
        /// </remarks>
        /// <value>
        /// The configuration others.
        /// </value>
        public ConfigOthers Others { get; private set; }

        /// <summary>
        /// Gets an enumerable list of all assigned configuration sections.
        /// </summary>
        /// <remarks>
        /// This property returns an enumerable list of all currently assigned configuration 
        /// sections belonging to this configuration content.
        /// </remarks>
        /// <value>
        /// An enumerable list of all configuration sections.
        /// </value>
        public IEnumerable<ConfigSection> Sections
        {
            get
            {
                foreach (ConfigSection section in this.sections)
                {
                    yield return section;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method removes all currently assigned configuration sections.
        /// </summary>
        /// <remarks>
        /// This method just removes all available configuration sections.
        /// </remarks>
        public void Clear()
        {
            this.sections.Clear();
        }

        /// <summary>
        /// This method tries to find a configuration section for a provided 
        /// section title.
        /// </summary>
        /// <remarks>
        /// Searching for configuration sections takes place using invariant culture 
        /// as well as ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The configuration section title to search for.
        /// </param>
        /// <returns>
        /// The found configuration section instance or <c>null</c> if not found.
        /// </returns>
        /// <seealso cref="ConfigSection.Equals(String)"/>
        public ConfigSection Find(String value)
        {
            for (Int32 index = 0; index < this.sections.Count; index++)
            {
                if (this.sections[index].Equals(value))
                {
                    return this.sections[index];
                }
            }

            return null;
        }

        /// <summary>
        /// This method appends a new instance of class <see cref="ConfigSection"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new section item and appends it to the list of sections.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial configuration section title.
        /// </param>
        /// <returns>
        /// The newly appended instance of class <see cref="ConfigSection"/>.
        /// </returns>
        /// <seealso cref="ConfigContent.Append(ConfigSection)"/>
        public ConfigSection Append(String value)
        {
            return this.Append(new ConfigSection(value));
        }

        /// <summary>
        /// This method appends the provided instance of class <see cref="ConfigSection"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method appends provided section to the list of sections.
        /// </remarks>
        /// <param name="entity">
        /// The configuration section to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigSection"/>.
        /// </returns>
        /// <seealso cref="ConfigContent.Insert(Int32, ConfigSection)"/>
        public ConfigSection Append(ConfigSection entity)
        {
            return this.Insert(Int32.MaxValue, entity);
        }

        /// <summary>
        /// This method tries to insert a new instance of class <see cref="ConfigSection"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The new instance is prepended or appended accordingly if provided index is 
        /// out of range.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new configuration section instance.
        /// </param>
        /// <param name="value">
        /// The string to be used as initial configuration section title.
        /// </param>
        /// <returns>
        /// The newly inserted instance of class <see cref="ConfigSection"/>.
        /// </returns>
        /// <seealso cref="ConfigContent.Insert(Int32, ConfigSection)"/>
        public ConfigSection Insert(Int32 index, String value)
        {
            return this.Insert(index, new ConfigSection(value));
        }

        /// <summary>
        /// This method tries to insert the provided instance of class <see cref="ConfigSection"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The provided configuration section is prepended if the index is less than zero and 
        /// it is appended if the index is greater or equal to current value count.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new configuration section instance.
        /// </param>
        /// <param name="entity">
        /// The configuration section to be inserted.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigSection"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided instance of class <see cref="ConfigSection"/> 
        /// is <c>null</c>.
        /// </exception>
        /// <seealso cref="ConfigContent.Insert(Int32, String)"/>
        public ConfigSection Insert(Int32 index, ConfigSection entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (index < 0)
            {
                index = 0;
            }
            else if (index >= this.sections.Count)
            {
                index = this.sections.Count;
            }

            this.sections.Insert(index, entity);

            return entity;
        }

        /// <summary>
        /// This method prepends a new instance of class <see cref="ConfigSection"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new section item and prepends it to the list of sections.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial configuration section title.
        /// </param>
        /// <returns>
        /// The newly prepended instance of class <see cref="ConfigSection"/>.
        /// </returns>
        /// <seealso cref="ConfigContent.Prepend(ConfigSection)"/>
        public ConfigSection Prepend(String value)
        {
            return this.Prepend(new ConfigSection(value));
        }

        /// <summary>
        /// This method prepends the provided instance of class <see cref="ConfigSection"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method prepends provided section to the list of sections.
        /// </remarks>
        /// <param name="entity">
        /// The configuration section to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigSection"/>.
        /// </returns>
        /// <seealso cref="ConfigContent.Insert(Int32, ConfigSection)"/>
        public ConfigSection Prepend(ConfigSection entity)
        {
            return this.Insert(Int32.MinValue, entity);
        }

        /// <summary>
        /// This method tries to find a configuration section and removes it from the section
        /// list. Further, the removed configuration section is returned.
        /// </summary>
        /// <remarks>
        /// Searching for configuration sections takes place using invariant culture as well as 
        /// ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The configuration section title to be removed.
        /// </param>
        /// <returns>
        /// The instance of the removed configuration section, or <c>null</c> if not found.
        /// </returns>
        /// <seealso cref="ConfigContent.Remove(Int32)"/>
        public ConfigSection Remove(String value)
        {
            for (Int32 index = 0; index < this.sections.Count; index++)
            {
                if (this.sections[index].Equals(value))
                {
                    return this.Remove(index);
                }
            }

            return null;
        }

        /// <summary>
        /// This method tries to remove a configuration section from the section list. Further, 
        /// the removed configuration section is returned.
        /// </summary>
        /// <remarks>
        /// Removing items for an invalid index will not cause an exception.
        /// </remarks>
        /// <param name="index">
        /// The index where to remove the configuration section.
        /// </param>
        /// <returns>
        /// The instance of the removed configuration section, or <c>null</c> if index was out 
        /// of range.
        /// </returns>
        /// <seealso cref="ConfigContent.Remove(String)"/>
        public ConfigSection Remove(Int32 index)
        {
            ConfigSection result = null;

            if (0 <= index && index < this.sections.Count)
            {
                result = this.sections[index];
                this.sections.RemoveAt(index);
            }

            return result;
        }

        /// <summary>
        /// This method converts currently used data into its output format and returns it.
        /// </summary>
        /// <remarks>
        /// This method is especially needed to write current section data into a configuration 
        /// file.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set content data in output format.
        /// </returns>
        public IEnumerable<String> ToOutput()
        {
            foreach (String output in this.Header.ToOutput())
            {
                yield return output;
            }

            if (this.Others.IsValid)
            {
                foreach (String output in this.Others.ToOutput())
                {
                    yield return output;
                }
            }

            if (this.IsValid)
            {
                foreach (ConfigSection section in this.Sections)
                {
                    foreach (String output in section.ToOutput())
                    {
                        yield return output;
                    }
                }
            }
        }

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
            return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Header)}={this.Header.IsValid}, {nameof(this.Others)}={this.Others.IsValid}, {nameof(this.Sections)}={this.Count}";
        }

        #endregion
    }
}
