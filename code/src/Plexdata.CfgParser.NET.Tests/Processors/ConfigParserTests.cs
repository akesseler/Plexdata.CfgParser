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
using Plexdata.CfgParser.Attributes;
using Plexdata.CfgParser.Entities;
using Plexdata.CfgParser.Processors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexdata.CfgParser.Tests.Processors
{
    [TestFixture]
    [TestOf(nameof(ConfigParser<DummyContentBase>))]
    public class ConfigParserTests
    {
        private class DummyContentBase { }

        #region From content into instance.

        private class ContentWithoutPublicConstructor
        {
            internal ContentWithoutPublicConstructor() : base() { }
        }

        private class ContentConstructorThrows
        {
            public ContentConstructorThrows() : base()
            {
                throw new NotSupportedException();
            }
        }

        private class SectionWithoutPublicConstructor
        {
            internal SectionWithoutPublicConstructor() : base() { }
        }

        private class ContentWithSectionWithoutPublicConstructor
        {
            [ConfigSection]
            public SectionWithoutPublicConstructor Section1 { get; set; }
        }

        private class SectionConstructorThrows
        {
            public SectionConstructorThrows() : base()
            {
                throw new NotSupportedException();
            }
        }

        private class ContentWithSectionConstructorThrows
        {
            [ConfigSection]
            public SectionConstructorThrows Section1 { get; set; }
        }

        private class SectionNoValue
        {
        }

        private class ContentOneSectionNoValue
        {
            [ConfigSection]
            public SectionNoValue Section1 { get; set; }
        }

        private class SectionOneValue
        {
            [ConfigValue]
            public String Value1 { get; set; }
        }

        private class ContentOneSectionOneValue
        {
            [ConfigSection]
            public SectionOneValue Section1 { get; set; }
        }

        private class SectionOneUnsupportedValue
        {
            [ConfigValue]
            public Object Value1 { get; set; }
        }

        private class ContentSectionOneUnsupportedValue
        {
            [ConfigSection]
            public SectionOneUnsupportedValue Section1 { get; set; }
        }

        [Test]
        public void Parse_ConfigContentIsNull_ThrowsArgumentNullException()
        {
            ConfigContent content = null;
            Assert.That(() => ConfigParser<DummyContentBase>.Parse(content), Throws.ArgumentNullException);
        }

        [Test]
        public void Parse_ContentCultureInfoIsNull_ThrowsArgumentNullException()
        {
            ConfigContent content = new ConfigContent();
            CultureInfo culture = null;
            Assert.That(() => ConfigParser<DummyContentBase>.Parse(content, culture), Throws.ArgumentNullException);
        }

        [Test]
        public void Parse_ContentWithoutPublicConstructor_ThrowsInvalidOperationException()
        {
            ConfigContent content = new ConfigContent();
            Assert.That(() => ConfigParser<ContentWithoutPublicConstructor>.Parse(content), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Parse_ContentConstructorThrows_ThrowsInvalidOperationException()
        {
            ConfigContent content = new ConfigContent();
            Assert.That(() => ConfigParser<ContentConstructorThrows>.Parse(content), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Parse_SectionWithoutPublicConstructor_ThrowsInvalidOperationException()
        {
            ConfigContent content = new ConfigContent();
            content.Append("Section1");
            Assert.That(() => ConfigParser<ContentWithSectionWithoutPublicConstructor>.Parse(content), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Parse_SectionConstructorThrows_ThrowsInvalidOperationException()
        {
            ConfigContent content = new ConfigContent();
            content.Append("Section1");
            Assert.That(() => ConfigParser<ContentWithSectionConstructorThrows>.Parse(content), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Parse_ContentIsEmpty_ResultIsExceptionNotThrown()
        {
            ConfigContent content = new ConfigContent();
            Assert.That(() => ConfigParser<ContentWithSectionWithoutPublicConstructor>.Parse(content), Throws.Nothing);
        }

        [Test]
        public void Parse_ContentOneSectionNoValue_ResultAndSectionNotNull()
        {
            ConfigContent content = new ConfigContent();
            content.Append("Section1");
            ContentOneSectionNoValue actual = ConfigParser<ContentOneSectionNoValue>.Parse(content);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Section1, Is.Not.Null);
        }
        
        [Test]
        public void Parse_ContentOneSectionOneValueNotSet_ResultAndSectionNotNullValueIsNull()
        {
            ConfigContent content = new ConfigContent();
            content.Append("Section1");
            ContentOneSectionOneValue actual = ConfigParser<ContentOneSectionOneValue>.Parse(content);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Section1, Is.Not.Null);
            Assert.That(actual.Section1.Value1, Is.Null);
        }

        [Test]
        public void Parse_ContentOneSectionOneValueSet_ValueIsEqualAsExpected()
        {
            ConfigContent content = new ConfigContent();
            content.Append("Section1").Append("Value1").Value = "fancy-value";
            ContentOneSectionOneValue actual = ConfigParser<ContentOneSectionOneValue>.Parse(content);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Section1, Is.Not.Null);
            Assert.That(actual.Section1.Value1, Is.EqualTo("fancy-value"));
        }

        [Test]
        public void Parse_ContentSectionOneUnsupportedValue_ResultAndSectionNotNullValueIsNull()
        {
            ConfigContent content = new ConfigContent();
            content.Append("Section1").Append("Value1").Value = "fancy-value";
            ContentSectionOneUnsupportedValue actual = ConfigParser<ContentSectionOneUnsupportedValue>.Parse(content);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Section1, Is.Not.Null);
            Assert.That(actual.Section1.Value1, Is.Null);
        }

        #endregion

        #region From instance into content.

        [Test]
        public void Parse_InstanceIsNull_ThrowsArgumentNullException()
        {
            DummyContentBase instance = null;
            Assert.That(() => ConfigParser<DummyContentBase>.Parse(instance), Throws.ArgumentNullException);
        }

        [Test]
        public void Parse_InstanceCultureInfoIsNull_ThrowsArgumentNullException()
        {
            DummyContentBase instance = new DummyContentBase();
            CultureInfo culture = null;
            Assert.That(() => ConfigParser<DummyContentBase>.Parse(instance, culture), Throws.ArgumentNullException);
        }





        // TODO: More tests...

        #endregion

        [Test]
        public void Parse_ConfigContentIsEmpty_ResultIsInstanceOf()
        {
            ConfigContent content = new ConfigContent();
            Assert.That(ConfigParser<DummyContentBase>.Parse(content), Is.InstanceOf<DummyContentBase>());
        }





    }
}
