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
using System.Reflection;

namespace Plexdata.CfgParser.Internals
{
    /// <summary>
    /// This internal class serves as base class for the classes <see cref="SectionDescriptor"/> 
    /// and <see cref="ValueDescriptor"/>.
    /// </summary>
    /// <remarks>
    /// This class is declared as pure abstract and cannot be instantiated.
    /// </remarks>
    /// <typeparam name="TAttribute">
    /// The type of assigned attribute.
    /// </typeparam>
    internal abstract class AttributeDescriptor<TAttribute> where TAttribute : Attribute
    {
        #region Construction

        /// <summary>
        /// The protected constructor just initializes all properties.
        /// </summary>
        /// <remarks>
        /// Only derived classes can call this constructor.
        /// </remarks>
        /// <param name="attribute">
        /// The generic attribute type to be assigned.
        /// </param>
        /// <param name="property">
        /// An instance of class <see cref="PropertyInfo"/> to be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if <paramref name="attribute"/> or 
        /// <paramref name="property"/> is <c>null</c>.
        /// </exception>
        protected AttributeDescriptor(TAttribute attribute, PropertyInfo property)
            : base()
        {
            this.Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the assigned generic attribute type.
        /// </summary>
        /// <remarks>
        /// The generic attribute type is defined by derived classes.
        /// </remarks>
        /// <value>
        /// An instance of type <typeparamref name="TAttribute"/>.
        /// </value>
        public TAttribute Attribute { get; private set; }

        /// <summary>
        /// Gets the assigned instance of class <see cref="PropertyInfo"/>.
        /// </summary>
        /// The property info type is set by derived classes.
        /// <remarks>
        /// The property information are used to determine additional details 
        /// and to be able to set or get the property's value.
        /// </remarks>
        /// <value>
        /// An instance of class <see cref="PropertyInfo"/>.
        /// </value>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Gets the descriptor to be used.
        /// </summary> 
        /// <remarks>
        /// The item descriptor right here is nothing else but the property 
        /// name and might differ in derived classes.
        /// </remarks>
        /// <value>
        /// A string representing the item descriptor.
        /// </value>
        public virtual String Descriptor
        {
            get
            {
                return this.Property.Name;
            }
        }

        /// <summary>
        /// Determines how string comparison should be done.
        /// </summary>
        /// <remarks>
        /// This property currently returns "invariant culture" and might 
        /// differ in derived classes.
        /// </remarks>
        /// <value>
        /// The value of how string comparison should be done.
        /// </value>
        public virtual StringComparison Comparison
        {
            get
            {
                return StringComparison.InvariantCulture;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <remarks>
        /// If <paramref name="other"/> is a string then the <see cref="AttributeDescriptor{TAttribute}.Descriptor"/> 
        /// of this class is compared using assigned currently assigned <see cref="AttributeDescriptor{TAttribute}.Comparison"/>. 
        /// Otherwise the Equals method of the base class is called.
        /// </remarks>
        /// <param name="other">
        /// The object to compare with the current object.
        /// </param>
        /// <returns>
        /// True if the specified object is equal to the current object and false otherwise.
        /// </returns>
        public override Boolean Equals(Object other)
        {
            if (other is String)
            {
                return String.Equals(other as String, this.Descriptor, this.Comparison);
            }
            else
            {
                return base.Equals(other);
            }
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <remarks>
        /// This method actually returns the hash code of <see cref="AttributeDescriptor{TAttribute}.Descriptor"/>.
        /// </remarks>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this.Descriptor.GetHashCode();
        }

        #endregion
    }
}
