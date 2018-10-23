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
using Plexdata.CfgParser.Internals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plexdata.CfgParser.Tests.Internals
{
    [TestFixture]
    [TestOf(nameof(DescriptorParser<DummyContentBase>))]
    public class DescriptorParserTests
    {
        private class DummyContentBase { }

        private class NoSectionValues { }

        private class OneSectionValue
        {
            [ConfigValue]
            public String Value1 { get; set; }
        }

        private class TwoSectionValues
        {
            [ConfigValue]
            public String Value1 { get; set; }
            [ConfigValue]
            public String Value2 { get; set; }
        }

        private class TwoSectionValuesOnePrivateSetter
        {
            [ConfigValue]
            public String Value1 { get; private set; }
            [ConfigValue]
            public String Value2 { get; set; }
        }

        private class TwoSectionValuesOneProtectedGetter
        {
            [ConfigValue]
            public String Value1 { protected get; set; }
            [ConfigValue]
            public String Value2 { get; set; }
        }

        private class TwoSectionValuesNotTagged
        {
            public String Value1 { get; set; }
            public String Value2 { get; set; }
        }

        [Test]
        public void ParseSections_NoSectionsNoValues_ResultIsEmpty()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentBase>.ParseSections().ToList();
            Assert.That(result.Any(), Is.False);
        }

        private class DummyContentOneSectionNoValues
        {
            [ConfigSection]
            public NoSectionValues SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionNoValues_ResultIsOneSectionNoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionNoValues>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Any(), Is.False);
        }

        private class DummyContentOneSectionOneValue
        {
            [ConfigSection]
            public OneSectionValue SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionOneValue_ResultIsOneSectionOneValue()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionOneValue>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Count, Is.EqualTo(1));
        }

        private class DummyContentOneSectionTwoValues
        {
            [ConfigSection]
            public TwoSectionValues SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionTwoValues_ResultIsOneSectionTwoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionTwoValues>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Count, Is.EqualTo(2));
        }

        private class DummyContentTwoSectionsNoValues
        {
            [ConfigSection]
            public NoSectionValues SectionOne { get; set; }
            [ConfigSection]
            public NoSectionValues SectionTwo { get; set; }
        }

        [Test]
        public void ParseSections_TwoSectionsNoValues_ResultIsTwoSectionsNoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentTwoSectionsNoValues>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Values.Any(), Is.False);
            Assert.That(result[1].Values.Any(), Is.False);
        }

        private class DummyContentTwoSectionsOneValue
        {
            [ConfigSection]
            public OneSectionValue SectionOne { get; set; }
            [ConfigSection]
            public OneSectionValue SectionTwo { get; set; }
        }

        [Test]
        public void ParseSections_TwoSectionsOneValue_ResultIsTwoSectionsOneValue()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentTwoSectionsOneValue>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Values.Count, Is.EqualTo(1));
            Assert.That(result[1].Values.Count, Is.EqualTo(1));
        }

        private class DummyContentTwoSectionsTwoValues
        {
            [ConfigSection]
            public TwoSectionValues SectionOne { get; set; }
            [ConfigSection]
            public TwoSectionValues SectionTwo { get; set; }
        }

        [Test]
        public void ParseSections_TwoSectionsTwoValues_ResultIsTwoSectionsTwoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentTwoSectionsTwoValues>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Values.Count, Is.EqualTo(2));
            Assert.That(result[1].Values.Count, Is.EqualTo(2));
        }

        private class DummyContentTwoSectionsOnePrivateSetterNoValues
        {
            [ConfigSection]
            public NoSectionValues SectionOne { get; set; }
            [ConfigSection]
            public NoSectionValues SectionTwo { get; private set; }
        }

        [Test]
        public void ParseSections_TwoSectionsOnePrivateSetterNoValues_ResultIsOneSectionNoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentTwoSectionsOnePrivateSetterNoValues>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Any(), Is.False);
        }

        private class DummyContentTwoSectionsOneProtectedGetterNoValues
        {
            [ConfigSection]
            public NoSectionValues SectionOne { get; set; }
            [ConfigSection]
            public NoSectionValues SectionTwo { protected get; set; }
        }

        [Test]
        public void ParseSections_TwoSectionsOneProtectedGetterNoValues_ResultIsOneSectionNoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentTwoSectionsOneProtectedGetterNoValues>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Any(), Is.False);
        }

        private class DummyContentOneSectionTwoValuesOnePrivateSetter
        {
            [ConfigSection]
            public TwoSectionValuesOnePrivateSetter SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionTwoValuesOnePrivateSetter_ResultIsOneSectionOneValue()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionTwoValuesOnePrivateSetter>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Count, Is.EqualTo(1));
        }

        private class DummyContentOneSectionTwoValuesOneProtectedGetter
        {
            [ConfigSection]
            public TwoSectionValuesOneProtectedGetter SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionTwoValuesOneProtectedGetter_ResultIsOneSectionOneValue()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionTwoValuesOneProtectedGetter>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Count, Is.EqualTo(1));
        }

        private class DummyContentOneSectionTwoValuesBothNotTagged
        {
            public TwoSectionValuesNotTagged SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionTwoValuesBothNotTagged_ResultIsEmpty()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionTwoValuesBothNotTagged>.ParseSections().ToList();
            Assert.That(result.Any(), Is.False);
        }

        private class DummyContentOneSectionTwoValuesNotTagged
        {
            [ConfigSection]
            public TwoSectionValuesNotTagged SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionTwoValuesNotTagged_ResultIsOneSectionNoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionTwoValuesNotTagged>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Any(), Is.False);
        }

        private class DummyContentNoSectionButTwoValues
        {
            [ConfigValue]
            public String Value1 { get; set; }
            [ConfigValue]
            public String Value2 { get; set; }
        }

        [Test]
        public void ParseSections_NoSectionButTwoValues_ResultIsEmpty()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentNoSectionButTwoValues>.ParseSections().ToList();
            Assert.That(result.Any(), Is.False);
        }

        private class DummyContentOneSectionWithRecursion
        {
            [ConfigSection]
            public DummyContentOneSectionWithRecursion SectionOne { get; set; }
        }

        [Test]
        public void ParseSections_OneSectionWithRecursion_ResultIsOneSectionNoValues()
        {
            List<SectionDescriptor> result = DescriptorParser<DummyContentOneSectionWithRecursion>.ParseSections().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Values.Any(), Is.False);
        }
    }
}
