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

namespace Plexdata.CfgParser.Exceptions
{
    /// <summary>
    /// An exception that might be thrown in conjunction with custom type conversion.
    /// </summary>
    /// <remarks>
    /// This exception might be thrown in cases when a custom type conversion fails.
    /// </remarks>
    public class CustomParserException : Exception
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of this `Exception` class with a 
        /// <paramref name="label"/>, a <paramref name="value"/> and a 
        /// specified error <paramref name="message"/>.
        /// </summary>
        /// <remarks>
        ///  This constructor initializes a new instance of this `Exception` 
        ///  class with a <paramref name="label"/>, a <paramref name="value"/> 
        ///  and a specified error <paramref name="message"/>.
        /// </remarks>
        /// <param name="label">
        /// The <see cref="CustomParserException.Label"/> of a particular 
        /// configuration value that has caused this exception.
        /// </param>
        /// <param name="value">
        /// The <see cref="CustomParserException.Value"/> of a particular 
        /// configuration value that has caused this exception. The actual 
        /// value might be empty.
        /// </param>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <seealso cref="CustomParserException.CustomParserException(String, String, String, Exception)"/>
        public CustomParserException(String label, String value, String message)
            : this(label, value, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of this `Exception` class with a 
        /// <paramref name="label"/>, a <paramref name="value"/> and a 
        /// specified inner <paramref name="exception"/>.
        /// </summary>
        /// <remarks>
        ///  This constructor initializes a new instance of this `Exception` 
        ///  class with a <paramref name="label"/>, a <paramref name="value"/> 
        ///  and a specified inner <paramref name="exception"/>.
        /// </remarks>
        /// <param name="label">
        /// The <see cref="CustomParserException.Label"/> of a particular 
        /// configuration value that has caused this exception.
        /// </param>
        /// <param name="value">
        /// The <see cref="CustomParserException.Value"/> of a particular 
        /// configuration value that has caused this exception.
        /// The actual value might be empty.
        /// </param>
        /// <param name="exception">
        /// The exception that is the cause of the current exception, or a 
        /// <c>null</c> reference if no inner exception is specified. The 
        /// property <see cref="Exception.Message"/> is initialized from 
        /// this argument, but only if it is not <c>null</c>.
        /// </param>
        /// <seealso cref="CustomParserException.CustomParserException(String, String, String, Exception)"/>
        public CustomParserException(String label, String value, Exception exception)
            : this(label, value, exception?.Message, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of this `Exception` class with a 
        /// <paramref name="label"/>, a <paramref name="value"/>, an error 
        /// <paramref name="message"/> and an inner <paramref name="exception"/>.
        /// </summary>
        /// <remarks>
        /// This constructor initializes a new instance of this `Exception` class 
        /// with a <paramref name="label"/>, a <paramref name="value"/>, an error 
        /// <paramref name="message"/> and an inner <paramref name="exception"/>.
        /// </remarks>
        /// <param name="label">
        /// The <see cref="CustomParserException.Label"/> of a particular 
        /// configuration value that has caused this exception.
        /// </param>
        /// <param name="value">
        /// The <see cref="CustomParserException.Value"/> of a particular 
        /// configuration value that has caused this exception.
        /// The actual value might be empty.
        /// </param>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="exception">
        /// The exception that is the cause of the current exception, or a 
        /// <c>null</c> reference if no inner exception is specified.
        /// </param>
        public CustomParserException(String label, String value, String message, Exception exception)
            : base(message, exception)
        {
            this.Label = label ?? String.Empty;
            this.Value = value ?? String.Empty;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the label of a configuration value that has caused this 
        /// exception.
        /// </summary>
        /// <remarks>
        /// This property gets the label of a configuration value that has 
        /// caused this exception.
        /// </remarks>
        /// <value>
        /// The label of a configuration value that has caused this exception 
        /// or an empty string.
        /// </value>
        public String Label { get; private set; }

        /// <summary>
        /// Gets the value of a configuration value that has caused this 
        /// exception.
        /// </summary>
        /// <remarks>
        /// This property gets the value of a configuration value that has 
        /// caused this exception.
        /// </remarks>
        /// <value>
        /// The value of a configuration value that has caused this exception 
        /// or an empty string.
        /// </value>
        public String Value { get; private set; }

        #endregion
    }
}