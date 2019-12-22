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
using Plexdata.CfgParser.Entities;
using Plexdata.CfgParser.Exceptions;
using Plexdata.CfgParser.Interfaces;
using Plexdata.CfgParser.Processors;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

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

        [ConfigHeader]
        private class ConfigHeaderTestClassPlain { }

        [ConfigHeader(IsExtended = true)]
        private class ConfigHeaderTestClassDefaultTrue { }

        [ConfigHeader(IsExtended = false)]
        private class ConfigHeaderTestClassDefaultFalse { }

        [ConfigHeader(IsExtended = true, Title = "test title", Placeholders = false)]
        private class ConfigHeaderTestClassDefaultTrueWithTitleWithoutPlaceholders { }

        [ConfigHeader(IsExtended = false, Title = "test title", Placeholders = false)]
        private class ConfigHeaderTestClassDefaultFalseWithTitleWithoutPlaceholders { }

        [ConfigHeader(IsExtended = true, Title = null, Placeholders = true)]
        private class ConfigHeaderTestClassDefaultTrueWithoutTitleWithPlaceholders { }

        [ConfigHeader(IsExtended = false, Title = null, Placeholders = true)]
        private class ConfigHeaderTestClassDefaultFalseWithoutTitleWithPlaceholders { }

        [ConfigHeader(IsExtended = true, Title = "test title", Placeholders = true)]
        private class ConfigHeaderTestClassDefaultTrueWithTitleWithPlaceholders { }

        [ConfigHeader(IsExtended = false, Title = "test title", Placeholders = true)]
        private class ConfigHeaderTestClassDefaultFalseWithTitleWithPlaceholders { }

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

        #region Config header tests.

        [Test]
        public void Parse_ConfigHeaderTestClassPlain_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassPlain>.Parse(new ConfigHeaderTestClassPlain());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(20));
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultTrue_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultTrue>.Parse(new ConfigHeaderTestClassDefaultTrue());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(20));
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultFalse_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultFalse>.Parse(new ConfigHeaderTestClassDefaultFalse());

            Assert.That(actual.Header.IsValid, Is.False);
            Assert.That(actual.Header.Comments.Count(), Is.Zero);
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultTrueWithTitleWithoutPlaceholders_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultTrueWithTitleWithoutPlaceholders>.Parse(new ConfigHeaderTestClassDefaultTrueWithTitleWithoutPlaceholders());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(22));
            Assert.That(actual.Header.Comments.ToArray()[1].Text, Is.EqualTo("test title"));
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultFalseWithTitleWithoutPlaceholders_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultFalseWithTitleWithoutPlaceholders>.Parse(new ConfigHeaderTestClassDefaultFalseWithTitleWithoutPlaceholders());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(3));
            Assert.That(actual.Header.Comments.ToArray()[1].Text, Is.EqualTo("test title"));
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultTrueWithoutTitleWithPlaceholders_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultTrueWithoutTitleWithPlaceholders>.Parse(new ConfigHeaderTestClassDefaultTrueWithoutTitleWithPlaceholders());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(23));
            Assert.That(actual.Header.Comments.ToArray()[1].Text.StartsWith("File name: "), Is.True);
            Assert.That(actual.Header.Comments.ToArray()[2].Text.StartsWith("File date: "), Is.True);
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultFalseWithoutTitleWithPlaceholders_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultFalseWithoutTitleWithPlaceholders>.Parse(new ConfigHeaderTestClassDefaultFalseWithoutTitleWithPlaceholders());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(4));
            Assert.That(actual.Header.Comments.ToArray()[1].Text.StartsWith("File name: "), Is.True);
            Assert.That(actual.Header.Comments.ToArray()[2].Text.StartsWith("File date: "), Is.True);
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultTrueWithTitleWithPlaceholders_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultTrueWithTitleWithPlaceholders>.Parse(new ConfigHeaderTestClassDefaultTrueWithTitleWithPlaceholders());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(24));
            Assert.That(actual.Header.Comments.ToArray()[1].Text, Is.EqualTo("test title"));
            Assert.That(actual.Header.Comments.ToArray()[2].Text.StartsWith("File name: "), Is.True);
            Assert.That(actual.Header.Comments.ToArray()[3].Text.StartsWith("File date: "), Is.True);
        }

        [Test]
        public void Parse_ConfigHeaderTestClassDefaultFalseWithTitleWithPlaceholders_ResultAsExpected()
        {
            ConfigContent actual = ConfigParser<ConfigHeaderTestClassDefaultFalseWithTitleWithPlaceholders>.Parse(new ConfigHeaderTestClassDefaultFalseWithTitleWithPlaceholders());

            Assert.That(actual.Header.IsValid, Is.True);
            Assert.That(actual.Header.Comments.Count(), Is.EqualTo(5));
            Assert.That(actual.Header.Comments.ToArray()[1].Text, Is.EqualTo("test title"));
            Assert.That(actual.Header.Comments.ToArray()[2].Text.StartsWith("File name: "), Is.True);
            Assert.That(actual.Header.Comments.ToArray()[3].Text.StartsWith("File date: "), Is.True);
        }

        #endregion

        #region Custom parser tests.

        public class TestCustomType
        {
            public TestCustomType()
            {
                this.Value1 = 111;
                this.Value2 = 222;
            }

            public Int32 Value1 { get; set; }
            public Int32 Value2 { get; set; }

            public override String ToString()
            {
                return "CustomParserTypeToString";
            }
        }

        private static readonly String CustomParserTestContent = $"[Section1]{Environment.NewLine}Complex1=23,42{Environment.NewLine}";

        #region CustomParserSectionNoParser

        private class CustomParserConfigNoParser
        {
            [ConfigSection]
            public CustomParserSectionNoParser Section1 { get; set; }
        }

        private class CustomParserSectionNoParser
        {
            [ConfigValue]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserConfigNoParser_ConstructConverterReturnsNull_ProertyIsNull()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                CustomParserConfigNoParser actual = ConfigParser<CustomParserConfigNoParser>.Parse(ConfigReader.Read(stream));

                Assert.That(actual.Section1.Complex1, Is.Null);
            }
        }

        #endregion

        #region CustomParserSectionNullParser

        private class CustomParserConfigNullParser
        {
            [ConfigSection]
            public CustomParserSectionNullParser Section1 { get; set; }
        }

        private class CustomParserSectionNullParser
        {
            [ConfigValue]
            [CustomParser(null)]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionNullParser_ConstructConverterReturnsNull_ProertyIsNull()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                CustomParserConfigNullParser actual = ConfigParser<CustomParserConfigNullParser>.Parse(ConfigReader.Read(stream));

                Assert.That(actual.Section1.Complex1, Is.Null);
            }
        }

        #endregion

        #region CustomParserSectionWrongParser

        private class CustomParserConfigWrongParser
        {
            [ConfigSection]
            public CustomParserSectionWrongParser Section1 { get; set; }
        }

        private class CustomParserSectionWrongParser
        {
            [ConfigValue]
            [CustomParser(typeof(String))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionWrongParser_ConstructConverterReturnsNull_ProertyIsNull()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                CustomParserConfigWrongParser actual = ConfigParser<CustomParserConfigWrongParser>.Parse(ConfigReader.Read(stream));

                Assert.That(actual.Section1.Complex1, Is.Null);
            }
        }

        #endregion

        #region CustomParserSectionWrongInterface

        private interface IWrongInterface { }

        private class WrongInterface : IWrongInterface { }

        private class CustomParserConfigWrongInterface
        {
            [ConfigSection]
            public CustomParserSectionWrongInterface Section1 { get; set; }
        }

        private class CustomParserSectionWrongInterface
        {
            [ConfigValue]
            [CustomParser(typeof(WrongInterface))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionWrongInterface_ConstructConverterReturnsNull_ProertyIsNull()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                CustomParserConfigWrongInterface actual = ConfigParser<CustomParserConfigWrongInterface>.Parse(ConfigReader.Read(stream));

                Assert.That(actual.Section1.Complex1, Is.Null);
            }
        }

        #endregion

        #region CustomParserSectionWrongPropertyType

        private class WrongPropertyType : ICustomParser<DateTime>
        {
            public DateTime Parse(String label, String value, Object fallback, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public String Parse(String label, DateTime value, Object fallback, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private class CustomParserConfigWrongPropertyType
        {
            [ConfigSection]
            public CustomParserSectionWrongPropertyType Section1 { get; set; }
        }

        private class CustomParserSectionWrongPropertyType
        {
            [ConfigValue]
            [CustomParser(typeof(WrongPropertyType))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionWrongPropertyType_ConstructConverterReturnsNull_ProertyIsNull()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                CustomParserConfigWrongPropertyType actual = ConfigParser<CustomParserConfigWrongPropertyType>.Parse(ConfigReader.Read(stream));

                Assert.That(actual.Section1.Complex1, Is.Null);
            }
        }

        #endregion

        #region CustomParserSectionInterfaceWithNoDefaultConstructor

        private class InterfaceWithNoDefaultConstructor : ICustomParser<TestCustomType>
        {
            private InterfaceWithNoDefaultConstructor()
            {
            }

            public TestCustomType Parse(String label, String value, Object fallback, CultureInfo culture)
            {
                return new TestCustomType();
            }

            public String Parse(String label, TestCustomType value, Object fallback, CultureInfo culture)
            {
                return value.ToString();
            }
        }

        private class CustomParserConfigInterfaceWithNoDefaultConstructor
        {
            [ConfigSection]
            public CustomParserSectionInterfaceWithNoDefaultConstructor Section1 { get; set; }
        }

        private class CustomParserSectionInterfaceWithNoDefaultConstructor
        {
            [ConfigValue]
            [CustomParser(typeof(InterfaceWithNoDefaultConstructor))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionInterfaceWithNoDefaultConstructor_ConstructConverterReturnsNull_ProertyIsNull()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                CustomParserConfigInterfaceWithNoDefaultConstructor actual = ConfigParser<CustomParserConfigInterfaceWithNoDefaultConstructor>.Parse(ConfigReader.Read(stream));

                Assert.That(actual.Section1.Complex1, Is.Null);
            }
        }

        #endregion

        #region CustomParserSectionInterfaceWithDefaultConstructorThrows

        private class InterfaceWithDefaultConstructorThrows : ICustomParser<TestCustomType>
        {
            public InterfaceWithDefaultConstructorThrows()
            {
                throw new InvalidOperationException("This constructor should never be called!");
            }

            public TestCustomType Parse(String label, String value, Object fallback, CultureInfo culture)
            {
                return new TestCustomType();
            }

            public String Parse(String label, TestCustomType value, Object fallback, CultureInfo culture)
            {
                return value.ToString();
            }
        }

        private class CustomParserConfigInterfaceWithDefaultConstructorThrows
        {
            [ConfigSection]
            public CustomParserSectionInterfaceWithDefaultConstructorThrows Section1 { get; set; }
        }

        private class CustomParserSectionInterfaceWithDefaultConstructorThrows
        {
            [ConfigValue]
            [CustomParser(typeof(InterfaceWithDefaultConstructorThrows))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionInterfaceWithDefaultConstructorThrows_ConstructConverterReturnsNull_ProertyIsNull()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                CustomParserConfigInterfaceWithDefaultConstructorThrows actual = ConfigParser<CustomParserConfigInterfaceWithDefaultConstructorThrows>.Parse(ConfigReader.Read(stream));

                Assert.That(actual.Section1.Complex1, Is.Null);
            }
        }

        #endregion

        #region CustomParserSectionFakeInterface

        private class FakeInterfaceWithCallCounter : ICustomParser<TestCustomType>
        {
            // Keep in mind, it was impossible to have a mock for the interface implementation. 
            // Therefore use of this workaround to verify the number of calls.

            public static Int32 ParseFromCalls = 0;

            public static Int32 ParseIntoCalls = 0;

            public TestCustomType Parse(String label, String value, Object fallback, CultureInfo culture)
            {
                FakeInterfaceWithCallCounter.ParseFromCalls += 1;
                return new TestCustomType();
            }

            public String Parse(String label, TestCustomType value, Object fallback, CultureInfo culture)
            {
                FakeInterfaceWithCallCounter.ParseIntoCalls += 1;
                return value.ToString();
            }
        }

        private class CustomParserConfigFakeInterface
        {
            [ConfigSection]
            public CustomParserSectionFakeInterface Section1 { get; set; }
        }

        private class CustomParserSectionFakeInterface
        {
            [ConfigValue]
            [CustomParser(typeof(FakeInterfaceWithCallCounter))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionFakeInterface_ConstructConverterReturnsNotNull_ParseFromWasCalledOnce()
        {
            FakeInterfaceWithCallCounter.ParseFromCalls = 0;
            FakeInterfaceWithCallCounter.ParseIntoCalls = 0;

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                ConfigParser<CustomParserConfigFakeInterface>.Parse(ConfigReader.Read(stream));

                Assert.That(FakeInterfaceWithCallCounter.ParseFromCalls, Is.EqualTo(1));
                Assert.That(FakeInterfaceWithCallCounter.ParseIntoCalls, Is.EqualTo(0));
            }
        }

        [Test]
        public void CustomParserSectionFakeInterface_ConstructConverterReturnsNotNull_ParseIntoWasCalledOnce()
        {
            FakeInterfaceWithCallCounter.ParseFromCalls = 0;
            FakeInterfaceWithCallCounter.ParseIntoCalls = 0;

            CustomParserConfigFakeInterface instance = new CustomParserConfigFakeInterface() { Section1 = new CustomParserSectionFakeInterface() { Complex1 = new TestCustomType() } };

            ConfigParser<CustomParserConfigFakeInterface>.Parse(instance);

            Assert.That(FakeInterfaceWithCallCounter.ParseFromCalls, Is.EqualTo(0));
            Assert.That(FakeInterfaceWithCallCounter.ParseIntoCalls, Is.EqualTo(1));
        }

        #endregion

        #region CustomParserSectionCustomTypeThrowsAnyException

        private class CustomTypeThrowsAnyException : ICustomParser<TestCustomType>
        {
            public TestCustomType Parse(String label, String value, Object fallback, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public String Parse(String label, TestCustomType value, Object fallback, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private class CustomParserConfigCustomTypeThrowsAnyException
        {
            [ConfigSection]
            public CustomParserSectionCustomTypeThrowsAnyException Section1 { get; set; }
        }

        private class CustomParserSectionCustomTypeThrowsAnyException
        {
            [ConfigValue]
            [CustomParser(typeof(CustomTypeThrowsAnyException))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionCustomTypeThrowsAnyException_ParseFromThrowsAnyException_ThrowsCustomParserException()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                Assert.That(() => ConfigParser<CustomParserConfigCustomTypeThrowsAnyException>.Parse(ConfigReader.Read(stream)), Throws.InstanceOf<CustomParserException>());
            }
        }

        [Test]
        public void CustomParserSectionCustomTypeThrowsAnyException_ParseIntoThrowsAnyException_ThrowsCustomParserException()
        {
            CustomParserConfigCustomTypeThrowsAnyException instance = new CustomParserConfigCustomTypeThrowsAnyException() { Section1 = new CustomParserSectionCustomTypeThrowsAnyException() { Complex1 = new TestCustomType() } };

            Assert.That(() => ConfigParser<CustomParserConfigCustomTypeThrowsAnyException>.Parse(instance), Throws.InstanceOf<CustomParserException>());
        }

        #endregion

        #region CustomParserSectionCustomTypeThrowsCustomParserException

        private class CustomTypeThrowsCustomParserException : ICustomParser<TestCustomType>
        {
            public TestCustomType Parse(String label, String value, Object fallback, CultureInfo culture)
            {
                throw new CustomParserException(label, value, "parse from exception");
            }

            public String Parse(String label, TestCustomType value, Object fallback, CultureInfo culture)
            {
                throw new CustomParserException(label, null, "parse into exception");
            }
        }

        private class CustomParserConfigCustomTypeThrowsCustomParserException
        {
            [ConfigSection]
            public CustomParserSectionCustomTypeThrowsCustomParserException Section1 { get; set; }
        }

        private class CustomParserSectionCustomTypeThrowsCustomParserException
        {
            [ConfigValue]
            [CustomParser(typeof(CustomTypeThrowsCustomParserException))]
            public TestCustomType Complex1 { get; set; }
        }

        [Test]
        public void CustomParserSectionCustomTypeThrowsCustomParserException_ParseFromThrowsCustomParserException_ThrowsCustomParserException()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigParserTests.CustomParserTestContent)))
            {
                Assert.That(() => ConfigParser<CustomParserConfigCustomTypeThrowsCustomParserException>.Parse(ConfigReader.Read(stream)), Throws.InstanceOf<CustomParserException>());
            }
        }

        [Test]
        public void CustomParserSectionCustomTypeThrowsCustomParserException_ParseIntoThrowsCustomParserException_ThrowsCustomParserException()
        {
            CustomParserConfigCustomTypeThrowsCustomParserException instance = new CustomParserConfigCustomTypeThrowsCustomParserException() { Section1 = new CustomParserSectionCustomTypeThrowsCustomParserException() { Complex1 = new TestCustomType() } };

            Assert.That(() => ConfigParser<CustomParserConfigCustomTypeThrowsCustomParserException>.Parse(instance), Throws.InstanceOf<CustomParserException>());
        }

        #endregion

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
