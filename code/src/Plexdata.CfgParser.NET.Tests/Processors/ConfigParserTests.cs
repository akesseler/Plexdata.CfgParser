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
using System.Globalization;
using System.Linq;

namespace Plexdata.CfgParser.Tests.Processors
{
    [TestFixture]
    [TestOf(nameof(ConfigParser<DummyContentBase>))]
    public class ConfigParserTests
    {
        #region Test classes.

        private class DummyContentBase { }

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

        private class InstanceWithWrongSection
        {
            [ConfigSection]
            public String Section1 { get; set; }
        }

        private class InstanceOneSectionNoValue
        {
            [ConfigSection]
            public SectionNoValue Section1 { get; set; }
        }

        private class InstanceOneSectionOneValue
        {
            [ConfigSection]
            public SectionOneValue Section1 { get; set; }
        }

        private class SmokeTestSectionOne
        {
            public SmokeTestSectionOne() { this.Value13 = true; }
            [ConfigValue("value-11", Comment = "comment value 11", Default = "value-11-default")]
            public String Value11 { get; set; }
            [ConfigValue("value-12", Comment = "comment value 12", Default = 42F)]
            public Single? Value12 { get; set; }
            [ConfigValue]
            public Boolean Value13 { get; set; }
            [ConfigValue]
            public Boolean? Value14 { get; set; }
            [ConfigValue(Default = "yea, a default string")]
            public String Value15 { get; set; }
        }

        private class SmokeTestSectionTwo
        {
            internal enum SectionTwoEnum { foo1, foo2, foo3 }
            [ConfigValue(Comment = "comment value 21")]
            public String Value21 { get; set; }
            [ConfigValue(Comment = "comment value 22", Default = SectionTwoEnum.foo3)]
            public SectionTwoEnum Value22 { get; set; }
            [ConfigValue(Comment = "comment value 23", Default = SectionTwoEnum.foo3)]
            public SectionTwoEnum? Value23 { get; set; }
        }

        private class FinalSmokeTestClass
        {
            public FinalSmokeTestClass() { this.Section1 = new SmokeTestSectionOne(); this.Section2 = new SmokeTestSectionTwo(); }
            [ConfigSection("section-1", Comment = "comment section-1")]
            public SmokeTestSectionOne Section1 { get; set; }
            [ConfigSection]
            public SmokeTestSectionTwo Section2 { get; set; }
        }

        #endregion

        #region From content into instance.

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

        [Test]
        public void Parse_InstanceWithoutSection_ResultIsEmpty()
        {
            DummyContentBase instance = new DummyContentBase();
            ConfigContent actual = ConfigParser<DummyContentBase>.Parse(instance);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Sections, Is.Not.Null);
            Assert.That(actual.Others, Is.Not.Null);
            Assert.That(actual.Header, Is.Not.Null);
            Assert.That(actual.Sections.Any(), Is.False);
            Assert.That(actual.Others.IsValid, Is.False);
            Assert.That(actual.Header.IsValid, Is.False);
        }

        [Test]
        public void Parse_InstanceWithWrongSectionType_ResultHasEmptySection()
        {
            InstanceWithWrongSection instance = new InstanceWithWrongSection();
            ConfigContent actual = ConfigParser<InstanceWithWrongSection>.Parse(instance);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Sections, Is.Not.Null);
            Assert.That(actual.Others, Is.Not.Null);
            Assert.That(actual.Header, Is.Not.Null);
            Assert.That(actual.Others.IsValid, Is.False);
            Assert.That(actual.Header.IsValid, Is.False);
            Assert.That(actual.Sections.Count(), Is.EqualTo(1));
            Assert.That(actual.Sections.First().Values.Any(), Is.False);
        }

        [Test]
        public void Parse_InstanceOneSectionNoValue_ResultHasEmptySection()
        {
            InstanceOneSectionNoValue instance = new InstanceOneSectionNoValue();
            ConfigContent actual = ConfigParser<InstanceOneSectionNoValue>.Parse(instance);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Sections, Is.Not.Null);
            Assert.That(actual.Others, Is.Not.Null);
            Assert.That(actual.Header, Is.Not.Null);
            Assert.That(actual.Others.IsValid, Is.False);
            Assert.That(actual.Header.IsValid, Is.False);
            Assert.That(actual.Sections.Count(), Is.EqualTo(1));
            Assert.That(actual.Sections.First().Values.Any(), Is.False);
        }

        [Test]
        public void Parse_InstanceOneSectionOneValueButSectionIsNull_ResultHasEmptySection()
        {
            InstanceOneSectionOneValue instance = new InstanceOneSectionOneValue();
            ConfigContent actual = ConfigParser<InstanceOneSectionOneValue>.Parse(instance);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Sections, Is.Not.Null);
            Assert.That(actual.Others, Is.Not.Null);
            Assert.That(actual.Header, Is.Not.Null);
            Assert.That(actual.Others.IsValid, Is.False);
            Assert.That(actual.Header.IsValid, Is.False);
            Assert.That(actual.Sections.Count(), Is.EqualTo(1));
            Assert.That(actual.Sections.First().Values.Any(), Is.False);
        }

        [Test]
        [TestCase(null)]
        [TestCase("value-data")]
        public void Parse_InstanceOneSectionOneValue_ResultHasOneSectionOneValue(String value)
        {
            InstanceOneSectionOneValue instance = new InstanceOneSectionOneValue();
            instance.Section1 = new SectionOneValue();
            instance.Section1.Value1 = value;
            ConfigContent actual = ConfigParser<InstanceOneSectionOneValue>.Parse(instance);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Sections, Is.Not.Null);
            Assert.That(actual.Others, Is.Not.Null);
            Assert.That(actual.Header, Is.Not.Null);
            Assert.That(actual.Others.IsValid, Is.False);
            Assert.That(actual.Header.IsValid, Is.False);
            Assert.That(actual.Sections.Count(), Is.EqualTo(1));
            Assert.That(actual.Sections.First().Values.Count(), Is.EqualTo(1));
        }

        #endregion

        #region Final smoke tests.

        [Test]
        public void Parse_FinalSmokeTestForContent_ResultIsAsExpected()
        {
            String expected =
                "[section-1] # comment section-1" +
                "value-11 = value-11-default # comment value 11" +
                "value-12 = 42 # comment value 12" +
                "Value13 = True" +
                "Value14 = Value15 = yea, a default string" +
                "[Section2]" +
                "Value21 = # comment value 21" +
                "Value22 = foo1 # comment value 22" +
                "Value23 = foo3 # comment value 23";

            FinalSmokeTestClass instance = new FinalSmokeTestClass();
            ConfigContent actual = ConfigParser<FinalSmokeTestClass>.Parse(instance);
            Assert.That(String.Join(String.Empty, actual.ToOutput()), Is.EqualTo(expected));
        }

        [Test]
        public void Parse_FinalSmokeTestForInstance_ResultIsAsExpected()
        {
            ConfigContent config = new ConfigContent();
            ConfigSection section = config.Append(new ConfigSection("section-1", "comment section-1"));
            section.Append(new ConfigValue("value-11", "value-11-data", "comment value 11"));
            section.Append(new ConfigValue("value-12", "42", "comment value 12"));
            section.Append(new ConfigValue("Value13", "True", ""));
            section.Append(new ConfigValue("Value14", "", ""));
            section.Append(new ConfigValue("Value15", "yea, a default string", ""));
            section = config.Append(new ConfigSection("Section2", ""));
            section.Append(new ConfigValue("Value21", "", "comment value 21"));
            section.Append(new ConfigValue("Value22", "foo1", "comment value 22"));
            section.Append(new ConfigValue("Value23", "foo3", "comment value 23"));

            FinalSmokeTestClass instance = ConfigParser<FinalSmokeTestClass>.Parse(config);

            Assert.That(instance.Section1, Is.Not.Null);
            Assert.That(instance.Section1.Value11, Is.EqualTo("value-11-data"));
            Assert.That(instance.Section1.Value12, Is.EqualTo(42F));
            Assert.That(instance.Section1.Value13, Is.EqualTo(true));
            Assert.That(instance.Section1.Value14, Is.Null);
            Assert.That(instance.Section1.Value15, Is.EqualTo("yea, a default string"));
            Assert.That(instance.Section2, Is.Not.Null);
            Assert.That(instance.Section2.Value21, Is.EqualTo(""));
            Assert.That(instance.Section2.Value22, Is.EqualTo(SmokeTestSectionTwo.SectionTwoEnum.foo1));
            Assert.That(instance.Section2.Value23, Is.EqualTo(SmokeTestSectionTwo.SectionTwoEnum.foo3));
        }

        #endregion
    }
}
