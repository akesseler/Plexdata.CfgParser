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
using Plexdata.CfgParser.Settings;
using System;

namespace Plexdata.CfgParser.Tests.Entities
{
    [TestFixture]
    [TestOf(nameof(ConfigSection))]
    public class ConfigSectionTests
    {
        private ConfigSettingsBase lastConfigSettings = null;

        [SetUp]
        public void Setup()
        {
            this.lastConfigSettings = ConfigSettings.Settings;
        }

        [TearDown]
        public void Cleanup()
        {
            ConfigSettings.Settings = this.lastConfigSettings;
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ConfigSection_ConstructionTitleInvalid_ThrowsArgumentException(String title)
        {
            Assert.Throws<ArgumentException>(() => { new ConfigSection(title); });
        }

        [Test]
        [TestCase("TitleNoSpaces", "TitleNoSpaces")]
        [TestCase("Title Inner Spaces", "Title Inner Spaces")]
        [TestCase("   TitleOuterSpaces  ", "TitleOuterSpaces")]
        [TestCase("  Title Inner + Outer Spaces  ", "Title Inner + Outer Spaces")]
        public void ConfigSection_ConstructionTitleValid_ResultIsExpected(String title, String expected)
        {
            ConfigSection instance = new ConfigSection(title);
            Assert.AreEqual(expected, instance.Title);
        }

        [Test]
        public void ConfigSection_ConstructionCommentValid_CommentIsNotNull()
        {
            ConfigSection instance = new ConfigSection("title", "comment");
            Assert.IsNotNull(instance.Comment);
        }

        [Test]
        [TestCase("[title]", true)]
        [TestCase("[  title  ]", true)]
        [TestCase("[ ]", false)]
        public void TryParse_BufferAsIs_ResultIsExpected(String buffer, Boolean expected)
        {
            Assert.AreEqual(expected, ConfigSection.TryParse(buffer, out ConfigSection instance));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Parse_BufferInvalid_ThrowsArgumentException(String buffer)
        {
            Assert.Throws<ArgumentException>(() => { ConfigSection.Parse(buffer); });
        }

        [Test]
        [TestCase("]")]
        [TestCase("  ]")]
        [TestCase("title")]
        [TestCase("  title  ")]
        [TestCase("title]")]
        [TestCase("  title]")]
        [TestCase("title  ]")]
        [TestCase("  title  ]")]
        [TestCase("[")]
        [TestCase("  [")]
        [TestCase("[title")]
        [TestCase("  [title")]
        [TestCase("[  title")]
        [TestCase("  [  title")]
        public void Parse_BufferInvalid_ThrowsFormatException(String buffer)
        {
            Assert.Throws<FormatException>(() => { ConfigSection.Parse(buffer); });
        }

        [Test]
        [TestCase("[title]", "[title]")]
        [TestCase("  [title]  ", "[title]")]
        [TestCase("[  title  ]", "[title]")]
        [TestCase("  [  title  ]  ", "[title]")]
        [TestCase("[title]#comment", "[title] # comment")]
        [TestCase("[title];comment", "[title] ; comment")]
        [TestCase("[title]    #    comment", "[title] # comment")]
        [TestCase("[title]    ;    comment", "[title] ; comment")]
        [TestCase("   [title]    #comment", "[title] # comment")]
        [TestCase("   [title]    ;comment", "[title] ; comment")]
        [TestCase("   [title]        #    comment", "[title] # comment")]
        [TestCase("   [title]        ;    comment", "[title] ; comment")]
        [TestCase("[  title  ]#comment", "[title] # comment")]
        [TestCase("[  title  ];comment", "[title] ; comment")]
        [TestCase("[  title  ]    #    comment", "[title] # comment")]
        [TestCase("[  title  ]    ;    comment", "[title] ; comment")]
        [TestCase("   [  title  ]   #comment", "[title] # comment")]
        [TestCase("   [  title  ]   ;comment", "[title] ; comment")]
        [TestCase("   [  title  ]       #    comment", "[title] # comment")]
        [TestCase("   [  title  ]       ;    comment", "[title] ; comment")]
        public void Parse_BufferValid_ResultIsExpected(String buffer, String expected)
        {
            ConfigSection instance = ConfigSection.Parse(buffer);
            String actual = String.Join(String.Empty, instance.ToOutput());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, "")]
        [TestCase(1, "value-0 = ")]
        [TestCase(2, "value-0 = value-1 = ")]
        [TestCase(3, "value-0 = value-1 = value-2 = ")]
        public void Append_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigSection instance = new ConfigSection("title");

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"value-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigValue value in instance.Values)
            {
                actual += value.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, "")]
        [TestCase(1, "value-0 = ")]
        [TestCase(2, "value-1 = value-0 = ")]
        [TestCase(3, "value-2 = value-1 = value-0 = ")]
        public void Prepend_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigSection instance = new ConfigSection("title");

            for (Int32 index = 0; index < count; index++)
            {
                instance.Prepend($"value-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigValue value in instance.Values)
            {
                actual += value.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(5, 3, "value-0 = value-1 = value-2 = value-insert = value-3 = value-4 = ")]
        [TestCase(5, 10, "value-0 = value-1 = value-2 = value-3 = value-4 = value-insert = ")]
        [TestCase(5, -10, "value-insert = value-0 = value-1 = value-2 = value-3 = value-4 = ")]
        public void Insert_VariousItems_ResultIsExpected(Int32 count, Int32 insert, String expected)
        {
            ConfigSection instance = new ConfigSection("title");

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"value-{index}");
            }

            instance.Insert(insert, "value-insert");

            String actual = String.Empty;

            foreach (ConfigValue value in instance.Values)
            {
                actual += value.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Insert_ValueInvalid_ThrowsArgumentNullException()
        {
            ConfigSection instance = new ConfigSection("title");
            ConfigValue value = null;
            Assert.Throws<ArgumentNullException>(() => { instance.Insert(0, value); });
        }

        [Test]
        public void Remove_ValueLabelNotFound_ResultIsNull()
        {
            ConfigSection instance = new ConfigSection("title");

            instance[0] = new ConfigValue("value-0");
            instance[1] = new ConfigValue("value-1");
            instance[2] = new ConfigValue("value-2");
            instance[3] = new ConfigValue("value-3");
            instance[4] = new ConfigValue("value-4");

            Assert.IsNull(instance.Remove("ValueLabelNotFound"));
        }

        [Test]
        [TestCase("value-2")]
        [TestCase("VALUE-2")]
        [TestCase("Value-2")]
        public void Remove_ValueLabelFound_ResultIsNotNull(String value)
        {
            ConfigSection instance = new ConfigSection("title");

            instance[0] = new ConfigValue("value-0");
            instance[1] = new ConfigValue("value-1");
            instance[2] = new ConfigValue("value-2");
            instance[3] = new ConfigValue("value-3");
            instance[4] = new ConfigValue("value-4");

            Assert.IsNotNull(instance.Remove(value));
        }

        [Test]
        public void Remove_ValueIndexNotFound_ResultIsNull()
        {
            ConfigSection instance = new ConfigSection("title");

            instance[0] = new ConfigValue("value-0");
            instance[1] = new ConfigValue("value-1");
            instance[2] = new ConfigValue("value-2");
            instance[3] = new ConfigValue("value-3");
            instance[4] = new ConfigValue("value-4");

            Assert.IsNull(instance.Remove(5));
        }

        [Test]
        public void Remove_ValueIndexFound_ResultIsNotNull()
        {
            ConfigSection instance = new ConfigSection("title");

            instance[0] = new ConfigValue("value-0");
            instance[1] = new ConfigValue("value-1");
            instance[2] = new ConfigValue("value-2");
            instance[3] = new ConfigValue("value-3");
            instance[4] = new ConfigValue("value-4");

            Assert.IsNotNull(instance.Remove(2));
        }

        [Test]
        public void Find_ValueLabelNotFound_ResultIsNull()
        {
            ConfigSection instance = new ConfigSection("title");

            instance[0] = new ConfigValue("value-0");
            instance[1] = new ConfigValue("value-1");
            instance[2] = new ConfigValue("value-2");
            instance[3] = new ConfigValue("value-3");
            instance[4] = new ConfigValue("value-4");

            Assert.IsNull(instance.Find("ValueLabelNotFound"));
        }

        [Test]
        [TestCase("value-2")]
        [TestCase("VALUE-2")]
        [TestCase("Value-2")]
        public void Find_ValueLabelFound_ResultIsEqual(String value)
        {
            ConfigSection instance = new ConfigSection("title");

            instance[0] = new ConfigValue("value-0");
            instance[1] = new ConfigValue("value-1");
            instance[2] = new ConfigValue("value-2");
            instance[3] = new ConfigValue("value-3");
            instance[4] = new ConfigValue("value-4");

            Assert.IsNotNull(instance.Find(value));
        }

        [Test]
        public void IndexArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigSection instance = new ConfigSection();

            Assert.Throws<ArgumentNullException>(() => instance[0] = null);
        }

        [Test]
        public void IndexArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigSection instance = new ConfigSection();

            instance[0] = new ConfigValue("value-1");
            instance[1] = new ConfigValue("value-2");
            instance[2] = new ConfigValue("value-3");

            ConfigValue value = new ConfigValue("appended-value");

            instance[1000] = value;

            Assert.AreSame(value, instance[3]);
        }

        [Test]
        public void IndexArrayAccessor_PrependValidValue_ResultIsValuePrepended()
        {
            ConfigSection instance = new ConfigSection();

            instance[0] = new ConfigValue("value-1");
            instance[1] = new ConfigValue("value-2");
            instance[2] = new ConfigValue("value-3");

            ConfigValue value = new ConfigValue("prepended-value");

            instance[-1000] = value;

            Assert.AreSame(value, instance[0]);
        }

        [Test]
        public void IndexArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigSection instance = new ConfigSection();

            instance[0] = new ConfigValue("value-1");
            instance[1] = new ConfigValue("value-2");
            instance[2] = new ConfigValue("value-3");

            ConfigValue value = new ConfigValue("replaced-value");

            instance[1] = value;

            Assert.AreSame(value, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigSection instance = new ConfigSection();

            Assert.Throws<ArgumentNullException>(() => instance["value"] = null);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_GetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigSection instance = new ConfigSection();
            ConfigValue value;
            Assert.Throws<ArgumentException>(() => value = instance[key]);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_SetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigSection instance = new ConfigSection();

            Assert.Throws<ArgumentException>(() => instance[key] = new ConfigValue());
        }

        [Test]
        public void KeyArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigSection instance = new ConfigSection();

            instance[0] = new ConfigValue("value-1");
            instance[1] = new ConfigValue("value-2");
            instance[2] = new ConfigValue("value-3");

            ConfigValue value = new ConfigValue("appended-value");

            instance["value-not-known"] = value;

            Assert.AreSame(value, instance[3]);
        }

        [Test]
        public void KeyArrayAccessor_AppendEmptyValue_ResultIsValueAppendedWithKey()
        {
            ConfigSection instance = new ConfigSection();

            instance[0] = new ConfigValue("value-1");
            instance[1] = new ConfigValue("value-2");
            instance[2] = new ConfigValue("value-3");

            ConfigValue value = new ConfigValue();

            instance["value-42"] = value;

            Assert.AreSame(value, instance[3]);
            Assert.AreEqual("value-42", instance[3].Label);
            Assert.AreEqual("value-42", value.Label);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigSection instance = new ConfigSection();

            instance[0] = new ConfigValue("value-1");
            instance[1] = new ConfigValue("value-2");
            instance[2] = new ConfigValue("value-3");

            ConfigValue value = new ConfigValue("replaced-value");

            instance["value-2"] = value;

            Assert.AreSame(value, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceEmptyValue_ResultIsValueReplacedWithKey()
        {
            ConfigSection instance = new ConfigSection();

            instance[0] = new ConfigValue("value-1");
            instance[1] = new ConfigValue("value-2");
            instance[2] = new ConfigValue("value-3");

            ConfigValue value = new ConfigValue();

            instance["value-2"] = value;

            Assert.AreSame(value, instance[1]);
            Assert.AreEqual("value-2", instance[1].Label);
            Assert.AreEqual("value-2", value.Label);
        }
    }
}
