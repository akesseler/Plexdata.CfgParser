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
using Plexdata.CfgParser.Attributes;
using Plexdata.CfgParser.Internals;
using Plexdata.CfgParser.Tests.Internals.Helpers;
using System;

namespace Plexdata.CfgParser.Tests.Internals
{
    [TestFixture]
    [TestOf(nameof(ValueDescriptor))]
    public class ValueDescriptorTests
    {
        [Test]
        public void Construction_InvalidAttribute_ThrowsArgumentNullException()
        {
            Assert.That(() => new ValueDescriptor(null, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Construction_InvalidProperty_ThrowsArgumentNullException()
        {
            Assert.That(() => new ValueDescriptor(new ConfigValueAttribute(), null), Throws.ArgumentNullException);
        }

        [Test]
        [TestCase(null, "FunnyProperty", "FunnyProperty")]
        [TestCase("", "FunnyProperty", "FunnyProperty")]
        [TestCase(" ", "FunnyProperty", "FunnyProperty")]
        [TestCase("Funny-Attribute", "FunnyProperty", "Funny-Attribute")]
        [TestCase("funny-attribute", "FunnyProperty", "funny-attribute")]
        [TestCase("FUNNY-ATTRIBUTE", "FunnyProperty", "FUNNY-ATTRIBUTE")]
        [TestCase(" Funny-Attribute ", "FunnyProperty", " Funny-Attribute ")]
        [TestCase(" funny-attribute ", "FunnyProperty", " funny-attribute ")]
        [TestCase(" FUNNY-ATTRIBUTE ", "FunnyProperty", " FUNNY-ATTRIBUTE ")]
        public void Descriptor_SourceIsString_ResultIsEqualToExpected(String attribute, String property, String expected)
        {
            ValueDescriptor instance = new ValueDescriptor(new ConfigValueAttribute(attribute), new DummyProperty(property));
            Assert.That(instance.Descriptor, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(null, "FunnyProperty", "FunnyProperty", true)]
        [TestCase("", "FunnyProperty", "FunnyProperty", true)]
        [TestCase(" ", "FunnyProperty", "FunnyProperty", true)]
        [TestCase("Funny-Attribute", "FunnyProperty", "Funny-Attribute", true)]
        [TestCase("funny-attribute", "FunnyProperty", "Funny-Attribute", true)]
        [TestCase("FUNNY-ATTRIBUTE", "FunnyProperty", "Funny-Attribute", true)]
        [TestCase(null, "FunnyProperty", "NoFunnyProperty", false)]
        [TestCase("", "FunnyProperty", "NoFunnyProperty", false)]
        [TestCase(" ", "FunnyProperty", "NoFunnyProperty", false)]
        [TestCase("Funny-Attribute", "FunnyProperty", "No-Funny-Attribute", false)]
        [TestCase("funny-attribute", "FunnyProperty", "No-Funny-Attribute", false)]
        [TestCase("FUNNY-ATTRIBUTE", "FunnyProperty", "No-Funny-Attribute", false)]
        public void Equals_SourceIsString_ResultIsExpected(String attribute, String property, String compare, Boolean expected)
        {
            ValueDescriptor instance = new ValueDescriptor(new ConfigValueAttribute(attribute), new DummyProperty(property));
            Assert.That(instance.Equals(compare), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(null, "FunnyProperty", StringComparison.InvariantCulture)]
        [TestCase("FunnyAttribute", "FunnyProperty", StringComparison.InvariantCultureIgnoreCase)]
        public void Comparison_SourceIsInvariantCulture_ResultIsInvariantCulture(String attribute, String property, StringComparison expected)
        {
            ValueDescriptor instance = new ValueDescriptor(new ConfigValueAttribute(attribute), new DummyProperty(property));
            Assert.That(instance.Comparison, Is.EqualTo(expected));
        }
    }
}