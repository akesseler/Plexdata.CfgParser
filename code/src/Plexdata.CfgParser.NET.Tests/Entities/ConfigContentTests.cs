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
using Plexdata.CfgParser.Entities;
using System;

namespace Plexdata.CfgParser.Tests.Entities
{
    [TestFixture]
    [TestOf(nameof(ConfigContent))]
    public class ConfigContentTests
    {
        [Test]
        [TestCase(0, "")]
        [TestCase(1, "[section-0]")]
        [TestCase(2, "[section-0][section-1]")]
        [TestCase(3, "[section-0][section-1][section-2]")]
        public void Append_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigContent instance = new ConfigContent();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"section-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigSection section in instance.Sections)
            {
                actual += String.Join(String.Empty, section.ToOutput());
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, "")]
        [TestCase(1, "[section-0]")]
        [TestCase(2, "[section-1][section-0]")]
        [TestCase(3, "[section-2][section-1][section-0]")]
        public void Prepend_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigContent instance = new ConfigContent();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Prepend($"section-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigSection section in instance.Sections)
            {
                actual += String.Join(String.Empty, section.ToOutput());
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(5, 3, "[section-0][section-1][section-2][section-insert][section-3][section-4]")]
        [TestCase(5, 10, "[section-0][section-1][section-2][section-3][section-4][section-insert]")]
        [TestCase(5, -10, "[section-insert][section-0][section-1][section-2][section-3][section-4]")]
        public void Insert_VariousItems_ResultIsExpected(Int32 count, Int32 insert, String expected)
        {
            ConfigContent instance = new ConfigContent();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"section-{index}");
            }

            instance.Insert(insert, "section-insert");

            String actual = String.Empty;

            foreach (ConfigSection section in instance.Sections)
            {
                actual += String.Join(String.Empty, section.ToOutput());
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Insert_SectionInvalid_ThrowsArgumentNullException()
        {
            ConfigContent instance = new ConfigContent();
            ConfigSection section = null;
            Assert.Throws<ArgumentNullException>(() => { instance.Insert(0, section); });
        }

        [Test]
        public void Remove_SectionTextNotFound_ResultIsNull()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-0");
            instance[1] = new ConfigSection("section-1");
            instance[2] = new ConfigSection("section-2");
            instance[3] = new ConfigSection("section-3");
            instance[4] = new ConfigSection("section-4");

            Assert.IsNull(instance.Remove("SectionTitleNotFound"));
        }

        [Test]
        [TestCase("section-2")]
        [TestCase("SECTION-2")]
        [TestCase("Section-2")]
        public void Remove_SectionTextFound_ResultIsNotNull(String value)
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-0");
            instance[1] = new ConfigSection("section-1");
            instance[2] = new ConfigSection("section-2");
            instance[3] = new ConfigSection("section-3");
            instance[4] = new ConfigSection("section-4");

            Assert.IsNotNull(instance.Remove(value));
        }

        [Test]
        public void Remove_SectionIndexNotFound_ResultIsNull()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-0");
            instance[1] = new ConfigSection("section-1");
            instance[2] = new ConfigSection("section-2");
            instance[3] = new ConfigSection("section-3");
            instance[4] = new ConfigSection("section-4");

            Assert.IsNull(instance.Remove(5));
        }

        [Test]
        public void Remove_SectionIndexFound_ResultIsNotNull()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-0");
            instance[1] = new ConfigSection("section-1");
            instance[2] = new ConfigSection("section-2");
            instance[3] = new ConfigSection("section-3");
            instance[4] = new ConfigSection("section-4");

            Assert.IsNotNull(instance.Remove(2));
        }

        [Test]
        public void Find_SectionTextNotFound_ResultIsNull()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-0");
            instance[1] = new ConfigSection("section-1");
            instance[2] = new ConfigSection("section-2");
            instance[3] = new ConfigSection("section-3");
            instance[4] = new ConfigSection("section-4");

            Assert.IsNull(instance.Find("SectionTitleNotFound"));
        }

        [Test]
        [TestCase("section-2")]
        [TestCase("SECTION-2")]
        [TestCase("Section-2")]
        public void Find_SectionTextFound_ResultIsNotNull(String value)
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-0");
            instance[1] = new ConfigSection("section-1");
            instance[2] = new ConfigSection("section-2");
            instance[3] = new ConfigSection("section-3");
            instance[4] = new ConfigSection("section-4");

            Assert.IsNotNull(instance.Find(value));
        }

        [Test]
        public void ToOutput_FiveSections_ResultIsAsExpected()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-0");
            instance[1] = new ConfigSection("section-1");
            instance[2] = new ConfigSection("section-2");
            instance[3] = new ConfigSection("section-3");
            instance[4] = new ConfigSection("section-4");

            String actual = String.Join(String.Empty, instance.ToOutput());

            Assert.AreEqual("[section-0][section-1][section-2][section-3][section-4]", actual);
        }

        [Test]
        public void IndexArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigContent instance = new ConfigContent();

            Assert.Throws<ArgumentNullException>(() => instance[0] = null);
        }

        [Test]
        public void IndexArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-1");
            instance[1] = new ConfigSection("section-2");
            instance[2] = new ConfigSection("section-3");

            ConfigSection section = new ConfigSection("appended-section");

            instance[1000] = section;

            Assert.AreSame(section, instance[3]);
        }

        [Test]
        public void IndexArrayAccessor_PrependValidValue_ResultIsValuePrepended()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-1");
            instance[1] = new ConfigSection("section-2");
            instance[2] = new ConfigSection("section-3");

            ConfigSection section = new ConfigSection("prepended-section");

            instance[-1000] = section;

            Assert.AreSame(section, instance[0]);
        }

        [Test]
        public void IndexArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-1");
            instance[1] = new ConfigSection("section-2");
            instance[2] = new ConfigSection("section-3");

            ConfigSection section = new ConfigSection("replaced-section");

            instance[1] = section;

            Assert.AreSame(section, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigContent instance = new ConfigContent();

            Assert.Throws<ArgumentNullException>(() => instance["section"] = null);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_GetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigContent instance = new ConfigContent();
            ConfigSection section;
            Assert.Throws<ArgumentException>(() => section = instance[key]);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_SetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigContent instance = new ConfigContent();

            Assert.Throws<ArgumentException>(() => instance[key] = new ConfigSection());
        }

        [Test]
        public void KeyArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-1");
            instance[1] = new ConfigSection("section-2");
            instance[2] = new ConfigSection("section-3");

            ConfigSection section = new ConfigSection("appended-section");

            instance["section-not-known"] = section;

            Assert.AreSame(section, instance[3]);
        }

        [Test]
        public void KeyArrayAccessor_AppendEmptyValue_ResultIsValueAppendedWithKey()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-1");
            instance[1] = new ConfigSection("section-2");
            instance[2] = new ConfigSection("section-3");

            ConfigSection section = new ConfigSection();

            instance["section-42"] = section;

            Assert.AreSame(section, instance[3]);
            Assert.AreEqual("section-42", instance[3].Title);
            Assert.AreEqual("section-42", section.Title);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-1");
            instance[1] = new ConfigSection("section-2");
            instance[2] = new ConfigSection("section-3");

            ConfigSection section = new ConfigSection("replaced-section");

            instance["section-2"] = section;

            Assert.AreSame(section, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceEmptyValue_ResultIsValueReplacedWithKey()
        {
            ConfigContent instance = new ConfigContent();

            instance[0] = new ConfigSection("section-1");
            instance[1] = new ConfigSection("section-2");
            instance[2] = new ConfigSection("section-3");

            ConfigSection section = new ConfigSection();

            instance["section-2"] = section;

            Assert.AreSame(section, instance[1]);
            Assert.AreEqual("section-2", instance[1].Title);
            Assert.AreEqual("section-2", section.Title);
        }
    }
}
