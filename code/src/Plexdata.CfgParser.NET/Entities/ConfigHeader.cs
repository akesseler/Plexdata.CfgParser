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
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.CfgParser.Entities
{
    /// <summary>
    /// This class represents the header of a configuration file.
    /// </summary>
    /// <remarks>
    /// The configuration file header consists of a series of interrelated comments 
    /// which should not be interrupted by a blank line. Also important is that a 
    /// configuration file header should be at the top of a configuration file.
    /// </remarks>
    public class ConfigHeader : IConfigArray<ConfigComment>
    {
        #region Private fields

        /// <summary>
        /// The field that holds the list of all comments assigned to this header.
        /// </summary>
        /// <remarks>
        /// The list of configuration comments should not be manipulated directly, for 
        /// example by using Reflection.
        /// </remarks>
        private readonly List<ConfigComment> comments;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor of an instance of this class.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with their default values. 
        /// An instance created by this constructor is not considered to be valid.
        /// </remarks>
        public ConfigHeader()
        {
            this.comments = new List<ConfigComment>();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the header comment at the specified index.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to set a header comment.
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
        /// The zero-based index of the header comment to get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigComment"/>.
        /// </value>
        /// <returns>
        /// The header comment at the specified index or <c>null</c> if provided 
        /// index is out of range.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        public ConfigComment this[Int32 index]
        {
            get
            {
                if (index >= 0 && index < this.comments.Count)
                {
                    return this.comments[index];
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

                if (index >= 0 && index < this.comments.Count)
                {
                    this.comments[index] = value;
                }
                else
                {
                    this.Insert(index, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the header comment for the specified key.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following rules might be considered when trying to get or set a 
        /// header comment.
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
        /// The key (which is a comment's text) of the header comment to 
        /// get or set.
        /// </param>
        /// <value>
        /// An instance of class <see cref="ConfigComment"/>.
        /// </value>
        /// <returns>
        /// The header comment for specified key or <c>null</c> if provided 
        /// key could not be found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when trying to set a value of <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown when trying to get or set a value using an 
        /// invalid key.
        /// </exception>
        public ConfigComment this[String key]
        {
            get
            {
                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Comment key must not be null or empty.", nameof(key));
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
                    throw new ArgumentException("Comment key must not be null or empty.", nameof(key));
                }

                if (String.IsNullOrWhiteSpace(value.Text))
                {
                    value.Text = key;
                }

                for (Int32 index = 0; index < this.comments.Count; index++)
                {
                    if (this.comments[index].Equals(key))
                    {
                        this.comments[index] = value;
                        return;
                    }
                }

                this.Append(value);
            }
        }

        /// <summary>
        /// Returns the number of currently available header comments.
        /// </summary>
        /// <remarks>
        /// The property just returns the number of currently available comment items.
        /// </remarks>
        /// <value>
        /// The number of available header comments.
        /// </value>
        public Int32 Count
        {
            get
            {
                return this.comments.Count;
            }
        }

        /// <summary>
        /// Determines if this instance can be considered as valid.
        /// </summary>
        /// <remarks>
        /// A configuration header is considered as valid as soon as it contains 
        /// one comment.
        /// </remarks>
        /// <value>
        /// True if this instance is valid and false if not.
        /// </value>
        public Boolean IsValid
        {
            get
            {
                return this.comments.Any();
            }
        }

        /// <summary>
        /// Gets an enumerable list of all assigned header comments.
        /// </summary>
        /// <remarks>
        /// This property returns an enumerable list of all currently assigned configuration 
        /// comments belonging to this header.
        /// </remarks>
        /// <value>
        /// An enumerable list of all header comments.
        /// </value>
        public IEnumerable<ConfigComment> Comments
        {
            get
            {
                foreach (ConfigComment comment in this.comments)
                {
                    yield return comment;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method removes all currently assigned header comments.
        /// </summary>
        /// <remarks>
        /// This method just removes all available configuration comments.
        /// </remarks>
        public void Clear()
        {
            this.comments.Clear();
        }

        /// <summary>
        /// This method tries to find a header comment for a provided 
        /// header comment text.
        /// </summary>
        /// <remarks>
        /// Searching for header comments takes place using invariant culture 
        /// as well as ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The header comment text to search for.
        /// </param>
        /// <returns>
        /// The found header comment instance or <c>null</c> if not found.
        /// </returns>
        /// <seealso cref="ConfigComment.Equals(String)"/>
        public ConfigComment Find(String value)
        {
            for (Int32 index = 0; index < this.comments.Count; index++)
            {
                if (this.comments[index].Equals(value))
                {
                    return this.comments[index];
                }
            }

            return null;
        }

        /// <summary>
        /// This method appends a new instance of class <see cref="ConfigComment"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new comment item and appends it to the list of comments.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial comment text.
        /// </param>
        /// <returns>
        /// The newly appended instance of class <see cref="ConfigComment"/>.
        /// </returns>
        /// <seealso cref="ConfigHeader.Append(ConfigComment)"/>
        public ConfigComment Append(String value)
        {
            return this.Append(new ConfigComment(value));
        }

        /// <summary>
        /// This method appends the provided instance of class <see cref="ConfigComment"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method appends provided comment to the list of comments.
        /// </remarks>
        /// <param name="entity">
        /// The header comment to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigComment"/>.
        /// </returns>
        /// <seealso cref="ConfigHeader.Insert(Int32, ConfigComment)"/>
        public ConfigComment Append(ConfigComment entity)
        {
            return this.Insert(Int32.MaxValue, entity);
        }

        /// <summary>
        /// This method tries to insert a new instance of class <see cref="ConfigComment"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The new instance is prepended or appended accordingly if provided index is 
        /// out of range.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new header comment instance.
        /// </param>
        /// <param name="value">
        /// The string to be used as initial comment text.
        /// </param>
        /// <returns>
        /// The newly inserted instance of class <see cref="ConfigComment"/>.
        /// </returns>
        /// <seealso cref="ConfigHeader.Insert(Int32, ConfigComment)"/>
        public ConfigComment Insert(Int32 index, String value)
        {
            return this.Insert(index, new ConfigComment(value));
        }

        /// <summary>
        /// This method tries to insert the provided instance of class <see cref="ConfigComment"/> 
        /// at provieded index and returns it.
        /// </summary>
        /// <remarks>
        /// The provided header comment is prepended if the index is less than zero and 
        /// it is appended if the index is greater or equal to current value count.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new header comment instance.
        /// </param>
        /// <param name="entity">
        /// The header comment to be inserted.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigComment"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided instance of class <see cref="ConfigComment"/> 
        /// is <c>null</c>.
        /// </exception>
        /// <seealso cref="ConfigHeader.Insert(Int32, String)"/>
        public ConfigComment Insert(Int32 index, ConfigComment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (index < 0)
            {
                index = 0;
            }
            else if (index >= this.comments.Count)
            {
                index = this.comments.Count;
            }

            this.comments.Insert(index, entity);

            return entity;
        }

        /// <summary>
        /// This method prepends a new instance of class <see cref="ConfigComment"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method creates a new comment item and prepends it to the list of comments.
        /// </remarks>
        /// <param name="value">
        /// The string to be used as initial comment text.
        /// </param>
        /// <returns>
        /// The newly prepended instance of class <see cref="ConfigComment"/>.
        /// </returns>
        /// <seealso cref="ConfigHeader.Prepend(ConfigComment)"/>
        public ConfigComment Prepend(String value)
        {
            return this.Prepend(new ConfigComment(value));
        }

        /// <summary>
        /// This method prepends the provided instance of class <see cref="ConfigComment"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// This method prepends provided comment to the list of comments.
        /// </remarks>
        /// <param name="entity">
        /// The header comment to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of class <see cref="ConfigComment"/>.
        /// </returns>
        /// <seealso cref="ConfigHeader.Insert(Int32, ConfigComment)"/>
        public ConfigComment Prepend(ConfigComment entity)
        {
            return this.Insert(Int32.MinValue, entity);
        }

        /// <summary>
        /// This method tries to find a header comment and removes it from the comment 
        /// list. Further, the removed header comment is returned.
        /// </summary>
        /// <remarks>
        /// Searching for header comments takes place using invariant culture as well as 
        /// ignoring upper and lower cases. 
        /// </remarks>
        /// <param name="value">
        /// The header comment text to be removed.
        /// </param>
        /// <returns>
        /// The instance of the removed header comment, or <c>null</c> if not found.
        /// </returns>
        /// <seealso cref="ConfigHeader.Remove(Int32)"/>
        public ConfigComment Remove(String value)
        {
            for (Int32 index = 0; index < this.comments.Count; index++)
            {
                if (this.comments[index].Equals(value))
                {
                    return this.Remove(index);
                }
            }

            return null;
        }

        /// <summary>
        /// This method tries to remove a header comment from the comment list. Further, 
        /// the removed header comment is returned.
        /// </summary>
        /// <remarks>
        /// Removing items for an invalid index will not cause an exception.
        /// </remarks>
        /// <param name="index">
        /// The index where to remove the header comment.
        /// </param>
        /// <returns>
        /// The instance of the removed header comment, or <c>null</c> if index was out 
        /// of range.
        /// </returns>
        /// <seealso cref="ConfigHeader.Remove(String)"/>
        public ConfigComment Remove(Int32 index)
        {
            ConfigComment result = null;

            if (0 <= index && index < this.comments.Count)
            {
                result = this.comments[index];
                this.comments.RemoveAt(index);
            }

            return result;
        }

        /// <summary>
        /// This method converts currently used data into its output format and returns it.
        /// </summary>
        /// <remarks>
        /// This method is especially needed to write current header data into a configuration 
        /// file.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set header data in output format.
        /// </returns>
        public IEnumerable<String> ToOutput()
        {
            if (this.comments.Any())
            {
                foreach (ConfigComment comment in this.comments)
                {
                    yield return comment.ToOutput();
                }

                yield return String.Empty;
            }
        }

        /// <summary>
        /// This method returns a string representation of current header data that might 
        /// be used for debugging purposes.
        /// </summary>
        /// <remarks>
        /// The method is not intended to be used for output.
        /// </remarks>
        /// <returns>
        /// The string representation of currently set header data.
        /// </returns>
        public override String ToString()
        {
            return $"{this.GetType().Name}: {nameof(this.IsValid)}={this.IsValid}, {nameof(this.Comments)}={this.Count}";
        }

        #endregion
    }
}
