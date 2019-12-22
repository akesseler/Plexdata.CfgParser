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

using NUnit.Framework;
using Plexdata.CfgParser.Exceptions;
using System;

namespace Plexdata.CfgParser.Tests.Exceptions
{
    [TestFixture]
    [TestOf(nameof(CustomParserException))]
    public class CustomParserExceptionTests
    {
        [Test]
        public void CustomParserException_LabelValueMessageAreNull_PropertiesAreEmpty()
        {
            CustomParserException actual = new CustomParserException((String)null, (String)null, (String)null);

            Assert.That(actual.Label, Is.Empty);
            Assert.That(actual.Value, Is.Empty);
            Assert.That(actual.Message, Is.Not.Empty);
            Assert.That(actual.InnerException, Is.Null);
        }

        [Test]
        public void CustomParserException_LabelValueExceptionAreNull_PropertiesAreEmpty()
        {
            CustomParserException actual = new CustomParserException((String)null, (String)null, (Exception)null);

            Assert.That(actual.Label, Is.Empty);
            Assert.That(actual.Value, Is.Empty);
            Assert.That(actual.Message, Is.Not.Empty);
            Assert.That(actual.InnerException, Is.Null);
        }

        [Test]
        public void CustomParserException_LabelValueMessageExceptionAreNull_PropertiesAreEmpty()
        {
            CustomParserException actual = new CustomParserException((String)null, (String)null, (String)null, (Exception)null);

            Assert.That(actual.Label, Is.Empty);
            Assert.That(actual.Value, Is.Empty);
            Assert.That(actual.Message, Is.Not.Empty);
            Assert.That(actual.InnerException, Is.Null);
        }

        [Test]
        public void CustomParserException_ExceptionIsNotNull_MessageIsExceptionMessage()
        {
            CustomParserException actual = new CustomParserException((String)null, (String)null, new Exception("exception message"));

            Assert.That(actual.Message, Is.EqualTo("exception message"));
        }

        [Test]
        public void CustomParserException_LabelValueMessageExceptionAreValid_PropertiesAreAsExpected()
        {
            CustomParserException actual = new CustomParserException("label", "value", "message", new Exception("exception"));

            Assert.That(actual.Label, Is.EqualTo("label"));
            Assert.That(actual.Value, Is.EqualTo("value"));
            Assert.That(actual.Message, Is.EqualTo("message"));
            Assert.That(actual.InnerException.Message, Is.EqualTo("exception"));
        }
    }
}
