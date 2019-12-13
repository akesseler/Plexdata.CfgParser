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
using Plexdata.CfgParser.Internals;
using Plexdata.CfgParser.Tests.Internals.Helpers;
using System;
using System.Reflection;

namespace Plexdata.CfgParser.Tests.Internals
{
    [TestFixture]
    [TestOf(nameof(AttributeDescriptor<Attribute>))]
    public class AttributeDescriptorTests
    {
        private class DummyAttribute : Attribute
        {
        }

        private class DummyDescriptor : AttributeDescriptor<DummyAttribute>
        {
            public DummyDescriptor(DummyAttribute attribute, PropertyInfo property)
                : base(attribute, property)
            {
            }
        }

        [Test]
        public void Construction_InvalidAttribute_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyDescriptor(null, new DummyProperty("hello")), Throws.ArgumentNullException);
        }

        [Test]
        public void Construction_InvalidProperty_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyDescriptor(new DummyAttribute(), null), Throws.ArgumentNullException);
        }

        [Test]
        [TestCase("FunnyName", "FunnyName", true)]
        [TestCase("FunnyName", "funnyname", false)]
        [TestCase("FunnyName", "FUNNYNAME", false)]
        public void Equals_SourceIsString_ResultIsExpected(String value, String compare, Boolean expected)
        {
            DummyDescriptor instance = new DummyDescriptor(new DummyAttribute(), new DummyProperty(value));

            Assert.That(instance.Equals(compare), Is.EqualTo(expected));
        }

        [Test]
        public void Equals_SourceIsOtherObject_ResultIsExpected()
        {
            DummyDescriptor instance = new DummyDescriptor(new DummyAttribute(), new DummyProperty("hello"));
            DummyDescriptor other = new DummyDescriptor(new DummyAttribute(), new DummyProperty("hello"));
            Assert.That(instance.Equals(other), Is.False);
        }

        [Test]
        public void Equals_SourceIsSameObject_ResultIsExpected()
        {
            DummyDescriptor instance = new DummyDescriptor(new DummyAttribute(), new DummyProperty("hello"));
            Assert.That(instance.Equals(instance), Is.True);
        }

        [Test]
        [TestCase("FunnyName")]
        [TestCase("funnyname")]
        [TestCase("FUNNYNAME")]
        public void GetHashCode_SourceIsString_ResultIsExpected(String value)
        {
            DummyDescriptor instance = new DummyDescriptor(new DummyAttribute(), new DummyProperty(value));
            Assert.That(instance.GetHashCode(), Is.EqualTo(value.GetHashCode()));
        }

        [Test]
        public void Comparison_SourceIsInvariantCulture_ResultIsInvariantCulture()
        {
            DummyDescriptor instance = new DummyDescriptor(new DummyAttribute(), new DummyProperty("hello"));
            Assert.That(instance.Comparison, Is.EqualTo(StringComparison.InvariantCulture));
        }
    }
}
