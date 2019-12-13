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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Plexdata.CfgParser.Converters
{
    /// <summary>
    /// This class converts strings into typed values.
    /// </summary>
    /// <remarks>
    /// Main task of this class is to convert string values into their typebased values.
    /// </remarks>
    public static class ValueConverter
    {
        #region Private fields

        /// <summary>
        /// The field that holds the list of all supported true values.
        /// </summary>
        /// <remarks>
        /// The list of supported true values contains "true", "1", "yes", "on", "yea".
        /// </remarks>
        private static readonly String[] yeahs = new String[] { "true", "1", "yes", "on", "yea" };

        /// <summary>
        /// The field that holds the list of all supported false values.
        /// </summary>
        /// <remarks>
        /// The list of supported false values contains "false", "0", "no", "off", "nay".
        /// </remarks>
        private static readonly String[] nopes = new String[] { "false", "0", "no", "off", "nay" };

        /// <summary>
        /// The field that holds the list of all supported types that can be converted.
        /// </summary>
        /// <remarks>
        /// The list of is actually a dictionary that assigns each type to its internal 
        /// convert method. 
        /// </remarks>
        private static readonly Dictionary<Type, Delegate> types = new Dictionary<Type, Delegate>
        {
            [typeof(String)] = new Func<String, CultureInfo, Object>(ValueConverter.ToStringSimple),
            [typeof(Version)] = new Func<String, CultureInfo, Object>(ValueConverter.ToVersionSimple),
            [typeof(IPAddress)] = new Func<String, CultureInfo, Object>(ValueConverter.ToIPAddressSimple),
            [typeof(Char)] = new Func<String, CultureInfo, Object>(ValueConverter.ToCharStandard),
            [typeof(Char?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToCharNullable),
            [typeof(Boolean)] = new Func<String, CultureInfo, Object>(ValueConverter.ToBooleanStandard),
            [typeof(Boolean?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToBooleanNullable),
            [typeof(SByte)] = new Func<String, CultureInfo, Object>(ValueConverter.ToSByteStandard),
            [typeof(SByte?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToSByteNullable),
            [typeof(Byte)] = new Func<String, CultureInfo, Object>(ValueConverter.ToByteStandard),
            [typeof(Byte?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToByteNullable),
            [typeof(Int16)] = new Func<String, CultureInfo, Object>(ValueConverter.ToInt16Standard),
            [typeof(Int16?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToInt16Nullable),
            [typeof(UInt16)] = new Func<String, CultureInfo, Object>(ValueConverter.ToUInt16Standard),
            [typeof(UInt16?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToUInt16Nullable),
            [typeof(Int32)] = new Func<String, CultureInfo, Object>(ValueConverter.ToInt32Standard),
            [typeof(Int32?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToInt32Nullable),
            [typeof(UInt32)] = new Func<String, CultureInfo, Object>(ValueConverter.ToUInt32Standard),
            [typeof(UInt32?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToUInt32Nullable),
            [typeof(Int64)] = new Func<String, CultureInfo, Object>(ValueConverter.ToInt64Standard),
            [typeof(Int64?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToInt64Nullable),
            [typeof(UInt64)] = new Func<String, CultureInfo, Object>(ValueConverter.ToUInt64Standard),
            [typeof(UInt64?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToUInt64Nullable),
            [typeof(DateTime)] = new Func<String, CultureInfo, Object>(ValueConverter.ToDateTimeStandard),
            [typeof(DateTime?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToDateTimeNullable),
            [typeof(Decimal)] = new Func<String, CultureInfo, Object>(ValueConverter.ToDecimalStandard),
            [typeof(Decimal?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToDecimalNullable),
            [typeof(Double)] = new Func<String, CultureInfo, Object>(ValueConverter.ToDoubleStandard),
            [typeof(Double?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToDoubleNullable),
            [typeof(Single)] = new Func<String, CultureInfo, Object>(ValueConverter.ToSingleStandard),
            [typeof(Single?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToSingleNullable),
            [typeof(Guid)] = new Func<String, CultureInfo, Object>(ValueConverter.ToGuidStandard),
            [typeof(Guid?)] = new Func<String, CultureInfo, Object>(ValueConverter.ToGuidNullable),
            [typeof(Enum)] = new Func<String, Type, CultureInfo, Object>(ValueConverter.ToEnumDefault),
        };

        #endregion

        #region Construction

        /// <summary>
        /// The static constructor initializes all static fields of the <see cref="ValueConverter"/> 
        /// class.
        /// </summary>
        /// <remarks>
        /// Nothing else but the static field initialization is done.
        /// </remarks>
        static ValueConverter()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the list of types supported by this converter.
        /// </summary>
        /// <remarks>
        /// At the moment this list of supported types includes String, Version, IP-Address, Char, Boolean, 
        /// SByte, Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, DateTime, Decimal, Double, Single and 
        /// Guid as well as the nullable version of each of these types. Enumerations and their nullable 
        /// types are also supported.
        /// </remarks>
        /// <value>
        /// An enumerable list that contains all supported types.
        /// </value>
        public static IEnumerable<Type> SupportedTypes
        {
            get
            {
                return ValueConverter.types.Keys;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Check if a particular type is supported by this converter.
        /// </summary>
        /// <remarks>
        /// The method just check if provided type is included in the list of supported types.
        /// </remarks>
        /// <param name="type">
        /// The type to be checked.
        /// </param>
        /// <returns>
        /// True if the <paramref name="type"/> is supported and false if not 
        /// or <paramref name="type"/> is <c>null</c>.
        /// </returns>
        /// <seealso cref="ValueConverter.SupportedTypes"/>
        public static Boolean IsSupportedType(Type type)
        {
            return ValueConverter.IsEnumType(type) || ValueConverter.IsElseType(type);
        }

        /// <summary>
        /// The method converts provided value into requested type.
        /// </summary>
        /// <remarks>
        /// Current UI culture is used to convert provided value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="type">
        /// The type to convert the value into.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <seealso cref="ValueConverter.Convert(String, Type, CultureInfo)"/>
        public static Object Convert(String value, Type type)
        {
            return ValueConverter.Convert(value, type, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// The method converts provided value into requested type using provided culture.
        /// </summary>
        /// <remarks>
        /// A conversion may fail under various circumstances. Therefore, it is recommended to 
        /// surround each call by a <c>try ... catch</c> block.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="type">
        /// The type to convert the value into.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided type or culture is <c>null</c>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if provided type is unsupported.
        /// </exception>
        /// <seealso cref="ValueConverter.Convert(String, Type)"/>
        public static Object Convert(String value, Type type, CultureInfo culture)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            if (ValueConverter.IsEnumType(type))
            {
                return ValueConverter.types[typeof(Enum)].DynamicInvoke(value, type, culture);
            }

            if (ValueConverter.IsElseType(type))
            {
                return ValueConverter.types[type].DynamicInvoke(value, culture);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// This method tries to convert provided value into requested type.
        /// </summary>
        /// <remarks>
        /// A conversion may fail under various circumstances. In such a case false is returned.
        /// Additionally, current UI culture is used to convert provided value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="type">
        /// The type to convert the value into.
        /// </param>
        /// <param name="result">
        /// The object of the converted value.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false if not.
        /// </returns>
        /// <seealso cref="ValueConverter.TryConvert(String, Type, CultureInfo, out Object, out Exception)"/>
        public static Boolean TryConvert(String value, Type type, out Object result)
        {
            return ValueConverter.TryConvert(value, type, CultureInfo.CurrentUICulture, out result, out Exception error);
        }

        /// <summary>
        /// This method tries to convert provided value into requested type.
        /// </summary>
        /// <remarks>
        /// A conversion may fail under various circumstances. In such a case false is returned.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="type">
        /// The type to convert the value into.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <param name="result">
        /// The object of the converted value.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false if not.
        /// </returns>
        /// <seealso cref="ValueConverter.TryConvert(String, Type, CultureInfo, out Object, out Exception)"/>
        public static Boolean TryConvert(String value, Type type, CultureInfo culture, out Object result)
        {
            return ValueConverter.TryConvert(value, type, culture, out result, out Exception error);
        }

        /// <summary>
        /// This method tries to convert provided value into requested type.
        /// </summary>
        /// <remarks>
        /// A conversion may fail under various circumstances. In such a case false is returned.
        /// Additionally, current UI culture is used to convert provided value.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="type">
        /// The type to convert the value into.
        /// </param>
        /// <param name="result">
        /// The object of the converted value.
        /// </param>
        /// <param name="error">
        /// An exception object to receive conversion errors.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false if not.
        /// </returns>
        /// <seealso cref="ValueConverter.TryConvert(String, Type, CultureInfo, out Object, out Exception)"/>
        public static Boolean TryConvert(String value, Type type, out Object result, out Exception error)
        {
            return ValueConverter.TryConvert(value, type, CultureInfo.CurrentUICulture, out result, out error);
        }

        /// <summary>
        /// This method tries to convert provided value into requested type.
        /// </summary>
        /// <remarks>
        /// A conversion may fail under various circumstances. In such a case false is returned.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="type">
        /// The type to convert the value into.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <param name="result">
        /// The object of the converted value.
        /// </param>
        /// <param name="error">
        /// An exception object to receive conversion errors.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false if not.
        /// </returns>
        /// <seealso cref="ValueConverter.Convert(String, Type, CultureInfo)"/>
        public static Boolean TryConvert(String value, Type type, CultureInfo culture, out Object result, out Exception error)
        {
            error = null;
            result = null;
            try
            {
                result = ValueConverter.Convert(value, type, culture);
                return true;
            }
            catch (Exception exception)
            {
                error = exception;

                if (exception is TargetInvocationException)
                {
                    if (exception.InnerException != null)
                    {
                        error = exception.InnerException;
                    }
                }

                return false;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Determines if provided type is an enumeration type.
        /// </summary>
        /// <remarks>
        /// The base type of provided type is used to determine the real type. 
        /// In case of a nullable type the underlying type is used.
        /// </remarks>
        /// <param name="type">
        /// The type to be checked.
        /// </param>
        /// <returns>
        /// True if the <paramref name="type"/> is an enumeration type and false 
        /// if not or <paramref name="type"/> is <c>null</c>.
        /// </returns>
        /// <seealso cref="ValueConverter.IsElseType(Type)"/>
        private static Boolean IsEnumType(Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
            }

            return type != null && type.BaseType != null && type.BaseType == typeof(Enum);
        }

        /// <summary>
        /// Determines if provided type is any other of the supported types.
        /// </summary>
        /// <remarks>
        /// Do not try to test for enumeration types with this method.
        /// </remarks>
        /// <param name="type">
        /// The type to be checked.
        /// </param>
        /// <returns>
        /// True if the <paramref name="type"/> is supported and false if not 
        /// or <paramref name="type"/> is <c>null</c>.
        /// </returns>
        /// <seealso cref="ValueConverter.IsEnumType(Type)"/>
        private static Boolean IsElseType(Type type)
        {
            return type != null && ValueConverter.types.ContainsKey(type);
        }

        /// <summary>
        /// This internal method converts provided value into its string representation.
        /// </summary>
        /// <remarks>
        /// An empty string is returned if provided value is <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToStringSimple(String value, CultureInfo culture)
        {
            return value ?? String.Empty;
        }

        /// <summary>
        /// This internal method converts provided value into its version representation.
        /// </summary>
        /// <remarks>
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> 
        /// or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToVersionSimple(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return Version.Parse(value);
        }

        /// <summary>
        /// This internal method converts provided value into its IP address representation.
        /// </summary>
        /// <remarks>
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> 
        /// or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToIPAddressSimple(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return IPAddress.Parse(value);
        }

        /// <summary>
        /// This internal method converts provided value into its character representation.
        /// </summary>
        /// <remarks>
        /// This method takes the first letter of provided value and converts it into its 
        /// character representation.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToCharStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Char.Parse(value.Substring(0, 1));
        }

        /// <summary>
        /// This internal method converts provided value into its nullable character representation.
        /// </summary>
        /// <remarks>
        /// This method takes the first letter of provided value and converts it into its 
        /// character representation. A <c>null</c> object is returned in case of provided 
        /// value is <c>null</c> or empty.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToCharNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            return ValueConverter.ToCharStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its boolean representation.
        /// </summary>
        /// <remarks>
        /// This method tries to interpret provided value as boolean. For this purpose the lists 
        /// of supported true and false values is used. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if provided value could not be interpreted as a boolean 
        /// type.
        /// </exception>
        /// <seealso cref="ValueConverter.yeahs"/>
        /// <seealso cref="ValueConverter.nopes"/>
        private static Object ToBooleanStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            value = value.Trim().ToLower();

            if (ValueConverter.yeahs.Any(x => x == value))
            {
                return true;
            }

            if (ValueConverter.nopes.Any(x => x == value))
            {
                return false;
            }

            throw new NotSupportedException($"Value of \"{value}\" is not supported for boolean data types.");
        }

        /// <summary>
        /// This internal method converts provided value into its nullable boolean representation.
        /// </summary>
        /// <remarks>
        /// This method tries to interpret provided value as boolean. For this purpose the lists 
        /// of supported true and false values is used. A <c>null</c> object is returned in case of 
        /// provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if provided value could not be interpreted as a boolean type.
        /// </exception>
        /// <seealso cref="ValueConverter.yeahs"/>
        /// <seealso cref="ValueConverter.nopes"/>
        private static Object ToBooleanNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToBooleanStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its signed byte representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToSByteStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return SByte.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable signed byte representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToSByteNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToSByteStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its unsigned byte representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToByteStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Byte.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable unsigned byte representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToByteNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToByteStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its signed short representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToInt16Standard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Int16.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable signed short representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToInt16Nullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToInt16Standard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its unsigned short representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToUInt16Standard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return UInt16.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable unsigned short representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToUInt16Nullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToUInt16Standard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its signed integer representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToInt32Standard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Int32.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable signed integer representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToInt32Nullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToInt32Standard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its unsigned integer representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToUInt32Standard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return UInt32.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable unsigned integer representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToUInt32Nullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToUInt32Standard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its signed long representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format. 
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToInt64Standard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Int64.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable signed long representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToInt64Nullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToInt64Standard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its unsigned long representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToUInt64Standard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return UInt64.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable unsigned long representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToUInt64Nullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToUInt64Standard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its date and time representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it by using the culture's date time format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToDateTimeStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return DateTime.Parse(value, culture.DateTimeFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable date and time representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it by using the culture's date time format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToDateTimeNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToDateTimeStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its decimal representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToDecimalStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Decimal.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable decimal representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToDecimalNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToDecimalStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its double representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToDoubleStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Double.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable double representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToDoubleNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToDoubleStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its float representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToSingleStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Single.Parse(value, NumberStyles.Any, culture.NumberFormat);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable float representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from any number format.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToSingleNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToSingleStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its GUID representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from a GUID's typical formats.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions form underlying type converter are also possible!
        /// </exception>
        private static Object ToGuidStandard(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(nameof(value));
            }

            return Guid.Parse(value);
        }

        /// <summary>
        /// This internal method converts provided value into its nullable GUID representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it from a GUID's typical formats.
        /// A <c>null</c> object is returned in case of provided value is <c>null</c> or empty or whitespace.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        private static Object ToGuidNullable(String value, CultureInfo culture)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return ValueConverter.ToGuidStandard(value, culture);
        }

        /// <summary>
        /// This internal method converts provided value into its enumeration or into its nullable 
        /// enumeration representation.
        /// </summary>
        /// <remarks>
        /// This method parses provided value and tries to convert it into a value of requested 
        /// enumeration type.
        /// </remarks>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="type">
        /// The type of requested enumeration type.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The object of the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value is null or empty or whitespace or is not 
        /// part of referenced enumeration type.
        /// </exception>
        /// <seealso cref="ValueConverter.ToEnumValue(String, Type, CultureInfo)"/>
        private static Object ToEnumDefault(String value, Type type, CultureInfo culture)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                return ValueConverter.ToEnumValue(value, Nullable.GetUnderlyingType(type), culture);
            }
            else
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value));
                }

                Object result = ValueConverter.ToEnumValue(value, type, culture);

                if (result == null)
                {
                    throw new ArgumentException($"The value {value} could not be resolved.");
                }

                return result;
            }
        }

        /// <summary>
        /// This internal method tries to find corresponding enumeration value.
        /// </summary>
        /// <remarks>
        /// The method iterates through all values of referenced enumeration type and check 
        /// if provided value is included. The string comparision is done by using invariant 
        /// culture and ignoring upper and lower cases.
        /// </remarks>
        /// <param name="value">
        /// The type of requested enumeration type.
        /// </param>
        /// <param name="type">
        /// The type of requested enumeration type.
        /// </param>
        /// <param name="culture">
        /// The culture to be used to convert the value.
        /// </param>
        /// <returns>
        /// The enumeration value is returned if found. Otherwise <c>null</c> is returned.
        /// </returns>
        private static Object ToEnumValue(String value, Type type, CultureInfo culture)
        {
            foreach (Object current in Enum.GetValues(type))
            {
                if (current != null && String.Equals(current.ToString(), value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return current;
                }
            }

            return null;
        }

        #endregion
    }
}
