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

namespace Plexdata.CfgParser.Attributes
{
    /// <summary>
    /// This attribute allows to provide an instance of a parser of custom types.
    /// </summary>
    /// <remarks>
    /// For sure, it would be possible to provide any object type. But to be enable a real 
    /// custom type parsing it is necessary to provide class that is derived from interface 
    /// <see cref="Plexdata.CfgParser.Interfaces.ICustomParser{TType}"/>. Otherwise, parsing 
    /// the custom type will not be possible.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomParserAttribute : Attribute
    {
        #region Construction

        /// <summary>
        /// The parameterized attribute constructor.
        /// </summary>
        /// <remarks>
        /// The constructor initializes all class properties with its 
        /// expected values.
        /// </remarks>
        /// <param name="instance">
        /// The type representing the instance of the custom type parser 
        /// to be used. Providing a <c>null</c> value is possible but would 
        /// disable a proper custom type parsing.
        /// </param>
        public CustomParserAttribute(Type instance)
            : base()
        {
            this.Instance = instance;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the instance type of the used custom type parser.
        /// </summary>
        /// <remarks>
        /// This property gets the instance type of the used custom type parser. 
        /// The actual value can be set through attribute construction.
        /// </remarks>
        /// <value>
        /// The type of the instance that represents the parser to be used for 
        /// custom type conversion, or null if not set through construction.
        /// </value>
        public Type Instance { get; private set; }

        #endregion
    }
}
