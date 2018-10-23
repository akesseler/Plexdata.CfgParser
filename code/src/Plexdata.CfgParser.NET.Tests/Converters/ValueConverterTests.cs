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

using NUnit.Framework;
using Plexdata.CfgParser.Converters;
using System;
using System.Globalization;

namespace Plexdata.CfgParser.Tests.Converters
{
    [TestFixture]
    [TestOf(nameof(ValueConverter))]
    public class ValueConverterTests
    {
        private enum DummyEnum
        {
            Dummy0,
            Dummy1,
            Dummy2,
            Dummy3,
            Dummy4,
        }

        [Test]
        [TestCase(typeof(String))]
        [TestCase(typeof(Char))]
        [TestCase(typeof(Char?))]
        [TestCase(typeof(Boolean))]
        [TestCase(typeof(Boolean?))]
        [TestCase(typeof(SByte))]
        [TestCase(typeof(SByte?))]
        [TestCase(typeof(Byte))]
        [TestCase(typeof(Byte?))]
        [TestCase(typeof(Int16))]
        [TestCase(typeof(Int16?))]
        [TestCase(typeof(UInt16))]
        [TestCase(typeof(UInt16?))]
        [TestCase(typeof(Int32))]
        [TestCase(typeof(Int32?))]
        [TestCase(typeof(UInt32))]
        [TestCase(typeof(UInt32?))]
        [TestCase(typeof(Int64))]
        [TestCase(typeof(Int64?))]
        [TestCase(typeof(UInt64))]
        [TestCase(typeof(UInt64?))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTime?))]
        [TestCase(typeof(Decimal))]
        [TestCase(typeof(Decimal?))]
        [TestCase(typeof(Double))]
        [TestCase(typeof(Double?))]
        [TestCase(typeof(Single))]
        [TestCase(typeof(Single?))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(Guid?))]
        [TestCase(typeof(DummyEnum))]
        [TestCase(typeof(DummyEnum?))]
        public void IsSupportedType_TypeValidation_ResultIsTrue(Type type)
        {
            Assert.That(() => ValueConverter.IsSupportedType(type), Is.True);
        }

        [Test]
        public void Convert_TypeIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => ValueConverter.Convert(null, null, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Convert_CultureInfoIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => ValueConverter.Convert(null, typeof(Object), null), Throws.ArgumentNullException);
        }

        [Test]
        public void Convert_UnsupportedType_ThrowsNotSupportedException()
        {
            Assert.That(() => ValueConverter.Convert(null, typeof(Object)), Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        [TestCase(typeof(Char), null)]
        [TestCase(typeof(Char), "")]
        [TestCase(typeof(Boolean), null)]
        [TestCase(typeof(Boolean), "")]
        [TestCase(typeof(Boolean), " ")]
        [TestCase(typeof(SByte), null)]
        [TestCase(typeof(SByte), "")]
        [TestCase(typeof(SByte), " ")]
        [TestCase(typeof(Byte), null)]
        [TestCase(typeof(Byte), "")]
        [TestCase(typeof(Byte), " ")]
        [TestCase(typeof(Int16), null)]
        [TestCase(typeof(Int16), "")]
        [TestCase(typeof(Int16), " ")]
        [TestCase(typeof(UInt16), null)]
        [TestCase(typeof(UInt16), "")]
        [TestCase(typeof(UInt16), " ")]
        [TestCase(typeof(Int32), null)]
        [TestCase(typeof(Int32), "")]
        [TestCase(typeof(Int32), " ")]
        [TestCase(typeof(UInt32), null)]
        [TestCase(typeof(UInt32), "")]
        [TestCase(typeof(UInt32), " ")]
        [TestCase(typeof(Int64), null)]
        [TestCase(typeof(Int64), "")]
        [TestCase(typeof(Int64), " ")]
        [TestCase(typeof(UInt64), null)]
        [TestCase(typeof(UInt64), "")]
        [TestCase(typeof(UInt64), " ")]
        [TestCase(typeof(DateTime), null)]
        [TestCase(typeof(DateTime), "")]
        [TestCase(typeof(DateTime), " ")]
        [TestCase(typeof(Decimal), null)]
        [TestCase(typeof(Decimal), "")]
        [TestCase(typeof(Decimal), " ")]
        [TestCase(typeof(Double), null)]
        [TestCase(typeof(Double), "")]
        [TestCase(typeof(Double), " ")]
        [TestCase(typeof(Single), null)]
        [TestCase(typeof(Single), "")]
        [TestCase(typeof(Single), " ")]
        [TestCase(typeof(Guid), null)]
        [TestCase(typeof(Guid), "")]
        [TestCase(typeof(Guid), " ")]
        [TestCase(typeof(DummyEnum), null)]
        [TestCase(typeof(DummyEnum), "")]
        [TestCase(typeof(DummyEnum), " ")]
        [TestCase(typeof(DummyEnum), "DummyEnumValueNotFound")]
        public void Convert_InvalidNonNullableValue_InnerThrowsArgumentException(Type type, String value)
        {
            Assert.That(() => ValueConverter.Convert(value, type), Throws.InnerException.TypeOf<ArgumentException>());
        }

        [Test]
        [TestCase(typeof(Char?), null)]
        [TestCase(typeof(Char?), "")]
        [TestCase(typeof(Boolean?), null)]
        [TestCase(typeof(Boolean?), "")]
        [TestCase(typeof(Boolean?), " ")]
        [TestCase(typeof(SByte?), null)]
        [TestCase(typeof(SByte?), "")]
        [TestCase(typeof(SByte?), " ")]
        [TestCase(typeof(Byte?), null)]
        [TestCase(typeof(Byte?), "")]
        [TestCase(typeof(Byte?), " ")]
        [TestCase(typeof(Int16?), null)]
        [TestCase(typeof(Int16?), "")]
        [TestCase(typeof(Int16?), " ")]
        [TestCase(typeof(UInt16?), null)]
        [TestCase(typeof(UInt16?), "")]
        [TestCase(typeof(UInt16?), " ")]
        [TestCase(typeof(Int32?), null)]
        [TestCase(typeof(Int32?), "")]
        [TestCase(typeof(Int32?), " ")]
        [TestCase(typeof(UInt32?), null)]
        [TestCase(typeof(UInt32?), "")]
        [TestCase(typeof(UInt32?), " ")]
        [TestCase(typeof(Int64?), null)]
        [TestCase(typeof(Int64?), "")]
        [TestCase(typeof(Int64?), " ")]
        [TestCase(typeof(UInt64?), null)]
        [TestCase(typeof(UInt64?), "")]
        [TestCase(typeof(UInt64?), " ")]
        [TestCase(typeof(DateTime?), null)]
        [TestCase(typeof(DateTime?), "")]
        [TestCase(typeof(DateTime?), " ")]
        [TestCase(typeof(Decimal?), null)]
        [TestCase(typeof(Decimal?), "")]
        [TestCase(typeof(Decimal?), " ")]
        [TestCase(typeof(Double?), null)]
        [TestCase(typeof(Double?), "")]
        [TestCase(typeof(Double?), " ")]
        [TestCase(typeof(Single?), null)]
        [TestCase(typeof(Single?), "")]
        [TestCase(typeof(Single?), " ")]
        [TestCase(typeof(Guid?), null)]
        [TestCase(typeof(Guid?), "")]
        [TestCase(typeof(Guid?), " ")]
        [TestCase(typeof(DummyEnum?), null)]
        [TestCase(typeof(DummyEnum?), "")]
        [TestCase(typeof(DummyEnum?), " ")]
        [TestCase(typeof(DummyEnum?), "DummyEnumValueNotFound")]
        public void Convert_InvalidNullableValue_ResultIsNull(Type type, String value)
        {
            Assert.That(ValueConverter.Convert(value, type), Is.Null);
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("Hello", "Hello")]
        [TestCase("  Hello  ", "  Hello  ")]
        public void Convert_ValueIsString_ResultIsExpected(String value, Object expected)
        {
            Assert.That(ValueConverter.Convert(value, typeof(String)), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, " ", ' ')]
        [TestCase(true, " ", ' ')]
        [TestCase(false, "H", 'H')]
        [TestCase(true, "H", 'H')]
        [TestCase(false, "Hello", 'H')]
        [TestCase(true, "Hello", 'H')]
        public void Convert_ValueIsChar_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Char?) : typeof(Char);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        public void Convert_InvalidBooleanValue_InnerThrowsNotSupportedException()
        {
            Assert.That(() => ValueConverter.Convert("invalid", typeof(Boolean), CultureInfo.GetCultureInfo("en-US")), Throws.InnerException.TypeOf<NotSupportedException>());
        }

        [Test]
        [TestCase(false, "true", true)]
        [TestCase(true, "true", true)]
        [TestCase(false, "True", true)]
        [TestCase(true, "True", true)]
        [TestCase(false, "TRUE", true)]
        [TestCase(true, "TRUE", true)]
        [TestCase(false, " true ", true)]
        [TestCase(true, " true ", true)]
        [TestCase(false, " True ", true)]
        [TestCase(true, " True ", true)]
        [TestCase(false, " TRUE ", true)]
        [TestCase(true, " TRUE ", true)]
        [TestCase(false, "1", true)]
        [TestCase(true, "1", true)]
        [TestCase(false, " 1 ", true)]
        [TestCase(true, " 1 ", true)]
        [TestCase(false, "yes", true)]
        [TestCase(true, "yes", true)]
        [TestCase(false, "Yes", true)]
        [TestCase(true, "Yes", true)]
        [TestCase(false, "YES", true)]
        [TestCase(true, "YES", true)]
        [TestCase(false, " yes ", true)]
        [TestCase(true, " yes ", true)]
        [TestCase(false, " Yes ", true)]
        [TestCase(true, " Yes ", true)]
        [TestCase(false, " YES ", true)]
        [TestCase(true, " YES ", true)]
        [TestCase(false, "on", true)]
        [TestCase(true, "on", true)]
        [TestCase(false, "On", true)]
        [TestCase(true, "On", true)]
        [TestCase(false, "ON", true)]
        [TestCase(true, "ON", true)]
        [TestCase(false, " on ", true)]
        [TestCase(true, " on ", true)]
        [TestCase(false, " On ", true)]
        [TestCase(true, " On ", true)]
        [TestCase(false, " ON ", true)]
        [TestCase(true, " ON ", true)]
        [TestCase(false, "yea", true)]
        [TestCase(true, "yea", true)]
        [TestCase(false, "Yea", true)]
        [TestCase(true, "Yea", true)]
        [TestCase(false, "YEA", true)]
        [TestCase(true, "YEA", true)]
        [TestCase(false, " yea ", true)]
        [TestCase(true, " yea ", true)]
        [TestCase(false, " Yea ", true)]
        [TestCase(true, " Yea ", true)]
        [TestCase(false, " YEA ", true)]
        [TestCase(true, " YEA ", true)]
        [TestCase(false, "false", false)]
        [TestCase(true, "false", false)]
        [TestCase(false, "False", false)]
        [TestCase(true, "False", false)]
        [TestCase(false, "FALSE", false)]
        [TestCase(true, "FALSE", false)]
        [TestCase(false, " false ", false)]
        [TestCase(true, " false ", false)]
        [TestCase(false, " False ", false)]
        [TestCase(true, " False ", false)]
        [TestCase(false, " FALSE ", false)]
        [TestCase(true, " FALSE ", false)]
        [TestCase(false, "0", false)]
        [TestCase(true, "0", false)]
        [TestCase(false, " 0 ", false)]
        [TestCase(true, " 0 ", false)]
        [TestCase(false, "no", false)]
        [TestCase(true, "no", false)]
        [TestCase(false, "No", false)]
        [TestCase(true, "No", false)]
        [TestCase(false, "NO", false)]
        [TestCase(true, "NO", false)]
        [TestCase(false, " no ", false)]
        [TestCase(true, " no ", false)]
        [TestCase(false, " No ", false)]
        [TestCase(true, " No ", false)]
        [TestCase(false, " NO ", false)]
        [TestCase(true, " NO ", false)]
        [TestCase(false, "off", false)]
        [TestCase(true, "off", false)]
        [TestCase(false, "Off", false)]
        [TestCase(true, "Off", false)]
        [TestCase(false, "OFF", false)]
        [TestCase(true, "OFF", false)]
        [TestCase(false, " off ", false)]
        [TestCase(true, " off ", false)]
        [TestCase(false, " Off ", false)]
        [TestCase(true, " Off ", false)]
        [TestCase(false, " OFF ", false)]
        [TestCase(true, " OFF ", false)]
        [TestCase(false, "nay", false)]
        [TestCase(true, "nay", false)]
        [TestCase(false, "Nay", false)]
        [TestCase(true, "Nay", false)]
        [TestCase(false, "NAY", false)]
        [TestCase(true, "NAY", false)]
        [TestCase(false, " nay ", false)]
        [TestCase(true, " nay ", false)]
        [TestCase(false, " Nay ", false)]
        [TestCase(true, " Nay ", false)]
        [TestCase(false, " NAY ", false)]
        [TestCase(true, " NAY ", false)]
        public void Convert_ValueIsBoolean_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Boolean?) : typeof(Boolean);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "-128", SByte.MinValue)]
        [TestCase(true, "-128", SByte.MinValue)]
        [TestCase(false, " -128 ", SByte.MinValue)]
        [TestCase(true, " -128 ", SByte.MinValue)]
        [TestCase(false, "127", SByte.MaxValue)]
        [TestCase(true, "127", SByte.MaxValue)]
        [TestCase(false, " 127 ", SByte.MaxValue)]
        [TestCase(true, " 127 ", SByte.MaxValue)]
        public void Convert_ValueIsSByte_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(SByte?) : typeof(SByte);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "0", Byte.MinValue)]
        [TestCase(true, "0", Byte.MinValue)]
        [TestCase(false, " 0 ", Byte.MinValue)]
        [TestCase(true, " 0 ", Byte.MinValue)]
        [TestCase(false, "255", Byte.MaxValue)]
        [TestCase(true, "255", Byte.MaxValue)]
        [TestCase(false, " 255 ", Byte.MaxValue)]
        [TestCase(true, " 255 ", Byte.MaxValue)]
        public void Convert_ValueIsByte_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Byte?) : typeof(Byte);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "-32768", Int16.MinValue)]
        [TestCase(true, "-32768", Int16.MinValue)]
        [TestCase(false, "-32,768", Int16.MinValue)]
        [TestCase(true, "-32,768", Int16.MinValue)]
        [TestCase(false, " -32768 ", Int16.MinValue)]
        [TestCase(true, " -32768 ", Int16.MinValue)]
        [TestCase(false, " -32,768 ", Int16.MinValue)]
        [TestCase(true, " -32,768 ", Int16.MinValue)]
        [TestCase(false, "32767", Int16.MaxValue)]
        [TestCase(true, "32767", Int16.MaxValue)]
        [TestCase(false, "32,767", Int16.MaxValue)]
        [TestCase(true, "32,767", Int16.MaxValue)]
        [TestCase(false, " 32767 ", Int16.MaxValue)]
        [TestCase(true, " 32767 ", Int16.MaxValue)]
        [TestCase(false, " 32,767 ", Int16.MaxValue)]
        [TestCase(true, " 32,767 ", Int16.MaxValue)]
        public void Convert_ValueIsInt16_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Int16?) : typeof(Int16);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "0", UInt16.MinValue)]
        [TestCase(true, "0", UInt16.MinValue)]
        [TestCase(false, " 0 ", UInt16.MinValue)]
        [TestCase(true, " 0 ", UInt16.MinValue)]
        [TestCase(false, "65535", UInt16.MaxValue)]
        [TestCase(true, "65535", UInt16.MaxValue)]
        [TestCase(false, "65,535", UInt16.MaxValue)]
        [TestCase(true, "65,535", UInt16.MaxValue)]
        [TestCase(false, " 65535 ", UInt16.MaxValue)]
        [TestCase(true, " 65535 ", UInt16.MaxValue)]
        [TestCase(false, " 65,535 ", UInt16.MaxValue)]
        [TestCase(true, " 65,535 ", UInt16.MaxValue)]
        public void Convert_ValueIsUInt16_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(UInt16?) : typeof(UInt16);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "-2147483648", Int32.MinValue)]
        [TestCase(true, "-2147483648", Int32.MinValue)]
        [TestCase(false, "-2,147,483,648", Int32.MinValue)]
        [TestCase(true, "-2,147,483,648", Int32.MinValue)]
        [TestCase(false, " -2147483648 ", Int32.MinValue)]
        [TestCase(true, " -2147483648 ", Int32.MinValue)]
        [TestCase(false, " -2,147,483,648 ", Int32.MinValue)]
        [TestCase(true, " -2,147,483,648 ", Int32.MinValue)]
        [TestCase(false, "2147483647", Int32.MaxValue)]
        [TestCase(true, "2147483647", Int32.MaxValue)]
        [TestCase(false, "2,147,483,647", Int32.MaxValue)]
        [TestCase(true, "2,147,483,647", Int32.MaxValue)]
        [TestCase(false, " 2147483647 ", Int32.MaxValue)]
        [TestCase(true, " 2147483647 ", Int32.MaxValue)]
        [TestCase(false, " 2,147,483,647 ", Int32.MaxValue)]
        [TestCase(true, " 2,147,483,647 ", Int32.MaxValue)]
        public void Convert_ValueIsInt32_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Int32?) : typeof(Int32);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "0", UInt32.MinValue)]
        [TestCase(true, "0", UInt32.MinValue)]
        [TestCase(false, " 0 ", UInt32.MinValue)]
        [TestCase(true, " 0 ", UInt32.MinValue)]
        [TestCase(false, "4294967295", UInt32.MaxValue)]
        [TestCase(true, "4294967295", UInt32.MaxValue)]
        [TestCase(false, "4,294,967,295", UInt32.MaxValue)]
        [TestCase(true, "4,294,967,295", UInt32.MaxValue)]
        [TestCase(false, " 4294967295 ", UInt32.MaxValue)]
        [TestCase(true, " 4294967295 ", UInt32.MaxValue)]
        [TestCase(false, " 4,294,967,295 ", UInt32.MaxValue)]
        [TestCase(true, " 4,294,967,295 ", UInt32.MaxValue)]
        public void Convert_ValueIsUInt32_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(UInt32?) : typeof(UInt32);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "-9223372036854775808", Int64.MinValue)]
        [TestCase(true, "-9223372036854775808", Int64.MinValue)]
        [TestCase(false, "-9,223,372,036,854,775,808", Int64.MinValue)]
        [TestCase(true, "-9,223,372,036,854,775,808", Int64.MinValue)]
        [TestCase(false, " -9223372036854775808 ", Int64.MinValue)]
        [TestCase(true, " -9223372036854775808 ", Int64.MinValue)]
        [TestCase(false, " -9,223,372,036,854,775,808 ", Int64.MinValue)]
        [TestCase(true, " -9,223,372,036,854,775,808 ", Int64.MinValue)]
        [TestCase(false, "9223372036854775807", Int64.MaxValue)]
        [TestCase(true, "9223372036854775807", Int64.MaxValue)]
        [TestCase(false, "9,223,372,036,854,775,807", Int64.MaxValue)]
        [TestCase(true, "9,223,372,036,854,775,807", Int64.MaxValue)]
        [TestCase(false, " 9223372036854775807 ", Int64.MaxValue)]
        [TestCase(true, " 9223372036854775807 ", Int64.MaxValue)]
        [TestCase(false, " 9,223,372,036,854,775,807 ", Int64.MaxValue)]
        [TestCase(true, " 9,223,372,036,854,775,807 ", Int64.MaxValue)]
        public void Convert_ValueIsInt64_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Int64?) : typeof(Int64);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "0", UInt64.MinValue)]
        [TestCase(true, "0", UInt64.MinValue)]
        [TestCase(false, " 0 ", UInt64.MinValue)]
        [TestCase(true, " 0 ", UInt64.MinValue)]
        [TestCase(false, "18446744073709551615", UInt64.MaxValue)]
        [TestCase(true, "18446744073709551615", UInt64.MaxValue)]
        [TestCase(false, "18,446,744,073,709,551,615", UInt64.MaxValue)]
        [TestCase(true, "18,446,744,073,709,551,615", UInt64.MaxValue)]
        [TestCase(false, " 18446744073709551615 ", UInt64.MaxValue)]
        [TestCase(true, " 18446744073709551615 ", UInt64.MaxValue)]
        [TestCase(false, " 18,446,744,073,709,551,615 ", UInt64.MaxValue)]
        [TestCase(true, " 18,446,744,073,709,551,615 ", UInt64.MaxValue)]
        public void Convert_ValueIsUInt64_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(UInt64?) : typeof(UInt64);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "29.10.1967 23:05:42", "29.10.1967 23:05:42")]
        [TestCase(true, "29.10.1967 23:05:42", "29.10.1967 23:05:42")]
        [TestCase(false, " 29.10.1967 23:05:42 ", "29.10.1967 23:05:42")]
        [TestCase(true, " 29.10.1967 23:05:42 ", "29.10.1967 23:05:42")]
        [TestCase(false, "29.10.1967     23:05:42", "29.10.1967 23:05:42")]
        [TestCase(true, "29.10.1967     23:05:42", "29.10.1967 23:05:42")]
        [TestCase(false, " 29.10.1967     23:05:42 ", "29.10.1967 23:05:42")]
        [TestCase(true, "  29.10.1967     23:05:42 ", "29.10.1967 23:05:42")]
        public void Convert_ValueIsDateTime_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(DateTime?) : typeof(DateTime);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("de-DE")), Is.EqualTo(DateTime.Parse(expected.ToString())));
        }

        [Test]
        [TestCase(false, "-79228162514264337593543950335", "-79228162514264337593543950335")]
        [TestCase(true, "-79228162514264337593543950335", "-79228162514264337593543950335")]
        [TestCase(false, "-79,228,162,514,264,337,593,543,950,335", "-79228162514264337593543950335")]
        [TestCase(true, "-79,228,162,514,264,337,593,543,950,335", "-79228162514264337593543950335")]
        [TestCase(false, " -79228162514264337593543950335 ", "-79228162514264337593543950335")]
        [TestCase(true, " -79228162514264337593543950335 ", "-79228162514264337593543950335")]
        [TestCase(false, " -79,228,162,514,264,337,593,543,950,335 ", "-79228162514264337593543950335")]
        [TestCase(true, " -79,228,162,514,264,337,593,543,950,335 ", "-79228162514264337593543950335")]
        [TestCase(false, "79228162514264337593543950335", "79228162514264337593543950335")]
        [TestCase(true, "79228162514264337593543950335", "79228162514264337593543950335")]
        [TestCase(false, "79,228,162,514,264,337,593,543,950,335", "79228162514264337593543950335")]
        [TestCase(true, "79,228,162,514,264,337,593,543,950,335", "79228162514264337593543950335")]
        [TestCase(false, " 79228162514264337593543950335 ", "79228162514264337593543950335")]
        [TestCase(true, " 79228162514264337593543950335 ", "79228162514264337593543950335")]
        [TestCase(false, " 79,228,162,514,264,337,593,543,950,335 ", "79228162514264337593543950335")]
        [TestCase(true, " 79,228,162,514,264,337,593,543,950,335 ", "79228162514264337593543950335")]
        public void Convert_ValueIsDecimal_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Decimal?) : typeof(Decimal);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")).ToString(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "-1234567890.0987654321", -1234567890.0987654321D)]
        [TestCase(true, "-1234567890.0987654321", -1234567890.0987654321D)]
        [TestCase(false, "-1,234,567,890.0987654321", -1234567890.0987654321D)]
        [TestCase(true, "-1,234,567,890.0987654321", -1234567890.0987654321D)]
        [TestCase(false, " -1234567890.0987654321 ", -1234567890.0987654321D)]
        [TestCase(true, " -1234567890.0987654321 ", -1234567890.0987654321D)]
        [TestCase(false, " -1,234,567,890.0987654321 ", -1234567890.0987654321D)]
        [TestCase(true, " -1,234,567,890.0987654321 ", -1234567890.0987654321D)]
        [TestCase(false, "1234567890.0987654321", 1234567890.0987654321D)]
        [TestCase(true, "1234567890.0987654321", 1234567890.0987654321D)]
        [TestCase(false, "1,234,567,890.0987654321", 1234567890.0987654321D)]
        [TestCase(true, "1,234,567,890.0987654321", 1234567890.0987654321D)]
        [TestCase(false, " 1234567890.0987654321 ", 1234567890.0987654321D)]
        [TestCase(true, " 1234567890.0987654321 ", 1234567890.0987654321D)]
        [TestCase(false, " 1,234,567,890.0987654321 ", 1234567890.0987654321D)]
        [TestCase(true, " 1,234,567,890.0987654321 ", 1234567890.0987654321D)]
        public void Convert_ValueIsDouble_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Double?) : typeof(Double);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "-1234567890.0987654321", -1234567890.0987654321F)]
        [TestCase(true, "-1234567890.0987654321", -1234567890.0987654321F)]
        [TestCase(false, "-1,234,567,890.0987654321", -1234567890.0987654321F)]
        [TestCase(true, "-1,234,567,890.0987654321", -1234567890.0987654321F)]
        [TestCase(false, " -1234567890.0987654321 ", -1234567890.0987654321F)]
        [TestCase(true, " -1234567890.0987654321 ", -1234567890.0987654321F)]
        [TestCase(false, " -1,234,567,890.0987654321 ", -1234567890.0987654321F)]
        [TestCase(true, " -1,234,567,890.0987654321 ", -1234567890.0987654321F)]
        [TestCase(false, "1234567890.0987654321", 1234567890.0987654321F)]
        [TestCase(true, "1234567890.0987654321", 1234567890.0987654321F)]
        [TestCase(false, "1,234,567,890.0987654321", 1234567890.0987654321F)]
        [TestCase(true, "1,234,567,890.0987654321", 1234567890.0987654321F)]
        [TestCase(false, " 1234567890.0987654321 ", 1234567890.0987654321F)]
        [TestCase(true, " 1234567890.0987654321 ", 1234567890.0987654321F)]
        [TestCase(false, " 1,234,567,890.0987654321 ", 1234567890.0987654321F)]
        [TestCase(true, " 1,234,567,890.0987654321 ", 1234567890.0987654321F)]
        public void Convert_ValueIsSingle_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Single?) : typeof(Single);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "0123456789abcdef0123456789abcdef", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, "0123456789abcdef0123456789abcdef", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(false, " 0123456789abcdef0123456789abcdef ", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, " 0123456789abcdef0123456789abcdef ", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(false, "01234567-89ab-cdef-0123-456789abcdef", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, "01234567-89ab-cdef-0123-456789abcdef", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(false, " 01234567-89ab-cdef-0123-456789abcdef ", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, " 01234567-89ab-cdef-0123-456789abcdef ", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(false, "{01234567-89ab-cdef-0123-456789abcdef}", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, "{01234567-89ab-cdef-0123-456789abcdef}", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(false, " {01234567-89ab-cdef-0123-456789abcdef} ", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, " {01234567-89ab-cdef-0123-456789abcdef} ", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(false, "(01234567-89ab-cdef-0123-456789abcdef)", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, "(01234567-89ab-cdef-0123-456789abcdef)", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(false, " (01234567-89ab-cdef-0123-456789abcdef) ", "01234567-89ab-cdef-0123-456789abcdef")]
        [TestCase(true, " (01234567-89ab-cdef-0123-456789abcdef) ", "01234567-89ab-cdef-0123-456789abcdef")]
        public void Convert_ValueIsGuid_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(Guid?) : typeof(Guid);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")).ToString(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, "Dummy2", DummyEnum.Dummy2)]
        [TestCase(true, "Dummy2", DummyEnum.Dummy2)]
        [TestCase(false, "dummy2", DummyEnum.Dummy2)]
        [TestCase(true, "dummy2", DummyEnum.Dummy2)]
        [TestCase(false, "DUMMY2", DummyEnum.Dummy2)]
        [TestCase(true, "DUMMY2", DummyEnum.Dummy2)]
        public void Convert_ValueIsEnum_ResultIsExpected(Boolean nullable, String value, Object expected)
        {
            Type type = nullable ? typeof(DummyEnum?) : typeof(DummyEnum);
            Assert.That(ValueConverter.Convert(value, type, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo(expected));
        }
    }
}
