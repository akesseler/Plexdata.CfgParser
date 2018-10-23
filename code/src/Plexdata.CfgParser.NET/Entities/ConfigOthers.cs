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
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents the others of a configuration file.
    /// </summary>
    /// <remarks>
    /// With others are meant all configuration file items that cannot be assigned 
    /// to the header or to a section or as a value.
    /// </remarks>
    public class ConfigOthers : IConfigArray<ConfigOther>
    {
        #region Private fields

        /// <summary>
        /// The field that holds the list of all others assigned to this others.
        /// </summary>
        /// <remarks>
        /// The list of configuration others should not be manipulated directly, for 
        /// example by using Reflection.
        /// </remarks>
        private readonly List<ConfigOther> others;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with their default values. 
        /// An instance created by this constructor is not considered to be valid.
        /// </remarks>
        public ConfigOthers()
        {
            this.others = new List<ConfigOther>();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the other item at the specified index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to set an other item.
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
        /// The zero-based index of the other item to get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigOther"/>.
        /// </value>
        /// <returns>
        /// The other item at the specified index or <c>null</c> if provided 
        /// index is out of range.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        public ConfigOther this[Int32 index]
        {
            get
            {
                if (index >= 0 && index < this.others.Count)
                {
                    return this.others[index];
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

                if (index >= 0 && index < this.others.Count)
                {
                    this.others[index] = value;
                }
                else
                {
                    this.Insert(index, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the other item for the specified key.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to get or set an 
        /// other item.
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
        /// The key (which is a other item's value) of the other item to 
        /// get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigOther"/>.
        /// </value>
        /// <returns>
        /// The other item for specified key or <c>null</c> if provided 
        /// key could not be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when trying to get or set a value using an 
        /// invalid key.
        /// </exception>
        public ConfigOther this[String key]
        {
            get
            {
                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Other key must not be null or empty.", nameof(key));
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
                    throw new ArgumentException("Other key must not be null or empty.", nameof(key));
                }

                if (String.IsNullOrWhiteSpace(value.Value))
                {
                    value.Value = key;
                }

                for (Int32 index = 0; index < this.others.Count; index++)
                {
                    if (this.others[index].Equals(key))
                    {
                        this.others[index] = value;
                        return;
                    }
                }

                this.Append(value);
            }
        }

        /// <summary>
        /// Returns the number of currently available other items.
        /// </summary>
        /// <remarks>
        /// The property just returns the number of currently available other items.
        /// </remarks>
        /// <value>
        /// The number of available other items.
        /// </value>
        public Int32 Count
        {
            get
            {
                return this.others.Count;
            }
        }

        /// <summary>
        /// Determines if this instance can be considered as valid.
        /// </summary>
        /// <remarks>
        /// An others instance is considered as valid as soon as it contains 
        /// one other item.
        /// </remarks>
        /// <value>
        /// True if this instance is valid and false if not.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return this.others.Any();
            }
        }

        /// <summary>
        /// Gets an enumerable list of all assigned other items.
        /// </summary>
        /// <remarks>
        /// This property returns an enumerable list of all found other items that 
        /// could not be assigned to somewhere else.
        /// </remarks>
        /// <value>
        /// An enumerable list of all other items.
        /// </value>
        public IEnumerable<ConfigOther> Items
        {
            get
            {
                foreach (ConfigOther other in this.others)
                {
                    yield return other;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method removes all currently assigned other items.
        /// </summary>
        /// <remarks>
        /// This method just removes all available other items.
        /// </remarks>
        public void Clear()
        {
            this.others.Clear();
        }

        /// <summary>
        /// This method tries to find an other item for a provided value.
        /// </summary>
        /// <remarks>
        /// Searching for other items takes place using invariant culture 
        /// as well as ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The other item value to search for.
        /// </param>
        /// <returns>
        /// The found other item instance or <c>null</c> if not found.
        /// </returns>
        /// <seealso cref="ConfigOther.Equals(String)"/>
        public ConfigOther Find(String value)
        {
            for (Int32 index = 0; index < this.others.Count; index++)
            {
                if (this.others[index].Equals(value))
                {
                    return this.others[index];
                }
            }

            return null;
        }

        /// <summary>
        /// This method appends a new instance of class <see cref="ConfigOther"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new other item and appends it to the list of others.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial other item value.
        /// </param>
        /// <returns>
        /// The newly appended instance of class <see cref="ConfigOther"/>.
        /// </returns>
        /// <seealso cref="ConfigOthers.Append(ConfigOther)"/>
        public ConfigOther Append(String value)
        {
            return this.Append(new ConfigOther(value));
        }

        /// <summary>
        /// This method appends the provided instance of class <see cref="ConfigOther"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method appends provided other item to the list of others.
        /// </remarks>
        /// <param name="entity">
        /// The other item to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigOther"/>.
        /// </returns>
        /// <seealso cref="ConfigOthers.Insert(Int32, ConfigOther)"/>
        public ConfigOther Append(ConfigOther entity)
        {
            return this.Insert(Int32.MaxValue, entity);
        }

        /// <summary>
        /// This method tries to insert a new instance of class <see cref="ConfigOther"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The new instance is prepended or appended accordingly if provided index is 
        /// out of range.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new other item instance.
        /// </param>
        /// <param name="value">
        /// The string to be used as initial other item value.
        /// </param>
        /// <returns>
        /// The newly inserted instance of class <see cref="ConfigOther"/>.
        /// </returns>
        /// <seealso cref="ConfigOthers.Insert(Int32, ConfigOther)"/>
        public ConfigOther Insert(Int32 index, String value)
        {
            return this.Insert(index, new ConfigOther(value));
        }

        /// <summary>
        /// This method tries to insert the provided instance of class <see cref="ConfigOther"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The provided other item is prepended if the index is less than zero and 
        /// it is appended if the index is greater or equal to current value count.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new other item instance.
        /// </param>
        /// <param name="entity">
        /// The other item to be inserted.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigOther"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided instance of class <see cref="ConfigOther"/> 
        /// is <c>null</c>.
        /// </exception>
        /// <seealso cref="ConfigOthers.Insert(Int32, String)"/>
        public ConfigOther Insert(Int32 index, ConfigOther entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (index < 0)
            {
                index = 0;
            }
            else if (index >= this.others.Count)
            {
                index = this.others.Count;
            }

            this.others.Insert(index, entity);

            return entity;
        }

        /// <summary>
        /// This method prepends a new instance of class <see cref="ConfigOther"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new other item and prepends it to the list of others.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial other item value.
        /// </param>
        /// <returns>
        /// The newly prepended instance of class <see cref="ConfigOther"/>.
        /// </returns>
        /// <seealso cref="ConfigOthers.Prepend(ConfigOther)"/>
        public ConfigOther Prepend(String value)
        {
            return this.Prepend(new ConfigOther(value));
        }

        /// <summary>
        /// This method prepends the provided instance of class <see cref="ConfigOther"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method prepends provided other item to the list of others.
        /// </remarks>
        /// <param name="entity">
        /// The other item to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigOther"/>.
        /// </returns>
        /// <seealso cref="ConfigOthers.Insert(Int32, ConfigOther)"/>
        public ConfigOther Prepend(ConfigOther entity)
        {
            return this.Insert(Int32.MinValue, entity);
        }

        /// <summary>
        /// This method tries to find an other item and removes it from the others
        /// list. Further, the removed other item is returned.
        /// </summary>
        /// <remarks>
        /// Searching for other items takes place using invariant culture as well as 
        /// ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The other item text to be removed.
        /// </param>
        /// <returns>
        /// The instance of the removed other item, or <c>null</c> if not found.
        /// </returns>
        /// <seealso cref="ConfigOthers.Remove(Int32)"/>
        public ConfigOther Remove(String value)
        {
            for (Int32 index = 0; index < this.others.Count; index++)
            {
                if (this.others[index].Equals(value))
                {
                    return this.Remove(index);
                }
            }

            return null;
        }

        /// <summary>
        /// This method tries to remove an other item from the others list. Further, 
        /// the removed other item is returned.
        /// </summary>
        /// <remarks>
        /// Removing items for an invalid index will not cause an exception.
        /// </remarks>
        /// <param name="index">
        /// The index where to remove the other item.
        /// </param>
        /// <returns>
        /// The instance of the removed other item, or <c>null</c> if index was out 
        /// of range.
        /// </returns>
        /// <seealso cref="ConfigOthers.Remove(String)"/>
        public ConfigOther Remove(Int32 index)
        {
            ConfigOther result = null;

            if (0 <= index && index < this.others.Count)
            {
                result = this.others[index];
                this.others.RemoveAt(index);
            }

            return result;
        }

        /// <summary>
        /// This method converts currently used data into its output format and returns it.
        /// </summary>
        /// <remarks>
        /// This method is especially needed to write current other item data into a configuration 
        /// file.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set other item data in output format.
        /// </returns>
        public IEnumerable<String> ToOutput()
        {
            if (this.others.Any())
            {
                foreach (ConfigOther other in this.others)
                {
                    yield return other.ToOutput();
                }

                yield return String.Empty;
            }
        }

        /// <summary>
        /// This method returns a string representation of current other item data that might 
        /// be used for debugging purposes.
        /// </summary>
        /// <remarks>
        /// The method is not intended to be used for output.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set other item data.
        /// </returns>
        public override String ToString()
        {
            return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Items)}={this.Count}";
        }

        #endregion
    }
}
