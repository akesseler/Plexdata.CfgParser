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

using Plexdata.CfgParser.Attributes;
using System;
using System.Reflection;

namespace Plexdata.CfgParser.Internals
{
    /// <summary>
    /// This internal class represents a specialized attribute descriptor.
    /// </summary>
    /// <remarks>
    /// The class is a specialization for attributes of type <see cref="ConfigValueAttribute"/>.
    /// </remarks>
    internal class ValueDescriptor : AttributeDescriptor<ConfigValueAttribute>
    {
        #region Construction

        /// <summary>
        /// This constructor initializes all properties of this class.
        /// </summary>
        /// <remarks>
        /// Only the base class constructor is called.
        /// </remarks>
        /// <param name="attribute">
        /// An instance of class <see cref="ConfigValueAttribute"/>.
        /// </param>
        /// <param name="property">
        /// An instance of class <see cref="PropertyInfo"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if <paramref name="attribute"/> or 
        /// <paramref name="property"/> is <c>null</c>.
        /// </exception>
        public ValueDescriptor(ConfigValueAttribute attribute, PropertyInfo property)
            : base(attribute, property)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the descriptor to be used.
        /// </summary>
        /// <remarks>
        /// The label of class <see cref="ConfigValueAttribute"/> is returned if 
        /// not null or empty or whitespace. Otherwise the descriptor of the base 
        /// class is returned.
        /// </remarks>
        /// <value>
        /// A string representing the item descriptor.
        /// </value>
        public override String Descriptor
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(this.Attribute.Label))
                {
                    return this.Attribute.Label;
                }
                else
                {
                    return base.Descriptor;
                }
            }
        }

        /// <summary>
        /// Determines how string comparison should be done.
        /// </summary>
        /// <remarks>
        /// The "invariant culture, ignore case" is returned if label of class 
        /// <see cref="ConfigValueAttribute"/> not null or empty or whitespace. 
        /// Otherwise the string comparison of the base class is returned.
        /// </remarks>
        /// <value>
        /// The value of how string comparison should be done.
        /// </value>
        public override StringComparison Comparison
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(this.Attribute.Label))
                {
                    return StringComparison.InvariantCultureIgnoreCase;
                }
                else
                {
                    return base.Comparison;
                }
            }
        }

        #endregion
    }
}
