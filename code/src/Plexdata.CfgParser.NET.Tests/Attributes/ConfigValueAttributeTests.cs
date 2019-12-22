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
using System;

namespace Plexdata.CfgParser.Tests.Attributes
{
    [TestFixture]
    [TestOf(nameof(ConfigValueAttribute))]
    public class ConfigValueAttributeTests
    {
        [Test]
        public void ConfigValueAttribute_DefaultConstruction_PropertiesWithDefaultSettings()
        {
            ConfigValueAttribute actual = new ConfigValueAttribute();

            Assert.That(actual.Label, Is.Empty);
            Assert.That(actual.Comment, Is.Empty);
            Assert.That(actual.Default, Is.Null);
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase("label", "label")]
        public void ConfigValueAttribute_ConstructionWithLabel_PropertiesWithExpectedSettings(String label, String expected)
        {
            ConfigValueAttribute actual = new ConfigValueAttribute(label);

            Assert.That(actual.Label, Is.EqualTo(expected));
            Assert.That(actual.Comment, Is.Empty);
            Assert.That(actual.Default, Is.Null);
        }
    }
}
