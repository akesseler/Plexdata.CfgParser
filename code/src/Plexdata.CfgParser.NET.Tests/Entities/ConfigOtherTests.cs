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
using Plexdata.CfgParser.Entities;
using System;

namespace Plexdata.CfgParser.Tests.Entities
{
    [TestFixture]
    [TestOf(nameof(ConfigOther))]
    public class ConfigOtherTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ConfigOther_ConstructionValueInvalid_ResultIsInvalid(String value)
        {
            ConfigOther instance = new ConfigOther(value);
            Assert.IsFalse(instance.IsValid);
        }

        [Test]
        [TestCase("value")]
        [TestCase("  value  ")]
        public void ConfigOther_ConstructionValueValid_ResultIsValid(String value)
        {
            ConfigOther instance = new ConfigOther(value);
            Assert.IsTrue(instance.IsValid);
        }

        [Test]
        [TestCase("value", "value")]
        [TestCase("  value  ", "value")]
        [TestCase("value  ", "value")]
        [TestCase("  value", "value")]
        public void ConfigOther_ConstructionValueValid_ResultIsExpected(String value, String expected)
        {
            ConfigOther instance = new ConfigOther(value);
            Assert.AreEqual(expected, instance.Value);
        }

        [Test]
        public void ToOutput_FiveSections_ResultIsAsExpected()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-0");
            instance[1] = new ConfigOther("other-1");
            instance[2] = new ConfigOther("other-2");
            instance[3] = new ConfigOther("other-3");
            instance[4] = new ConfigOther("other-4");

            String actual = String.Join(String.Empty, instance.ToOutput());

            Assert.AreEqual("other-0other-1other-2other-3other-4", actual);
        }

        [Test]
        public void IndexArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigOthers instance = new ConfigOthers();

            Assert.Throws<ArgumentNullException>(() => instance[0] = null);
        }

        [Test]
        public void IndexArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther("appended-other");

            instance[1000] = other;

            Assert.AreSame(other, instance[3]);
        }

        [Test]
        public void IndexArrayAccessor_PrependValidValue_ResultIsValuePrepended()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther("prepended-other");

            instance[-1000] = other;

            Assert.AreSame(other, instance[0]);
        }

        [Test]
        public void IndexArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther("replaced-other");

            instance[1] = other;

            Assert.AreSame(other, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigOthers instance = new ConfigOthers();

            Assert.Throws<ArgumentNullException>(() => instance["other"] = null);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_GetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigOthers instance = new ConfigOthers();
            ConfigOther other;
            Assert.Throws<ArgumentException>(() => other = instance[key]);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_SetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigOthers instance = new ConfigOthers();

            Assert.Throws<ArgumentException>(() => instance[key] = new ConfigOther());
        }

        [Test]
        public void KeyArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther("appended-other");

            instance["other-not-known"] = other;

            Assert.AreSame(other, instance[3]);
        }

        [Test]
        public void KeyArrayAccessor_AppendEmptyValue_ResultIsValueAppendedWithKey()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther();

            instance["other-42"] = other;

            Assert.AreSame(other, instance[3]);
            Assert.AreEqual("other-42", instance[3].Value);
            Assert.AreEqual("other-42", other.Value);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther("replaced-other");

            instance["other-2"] = other;

            Assert.AreSame(other, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceEmptyValue_ResultIsValueReplacedWithKey()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther();

            instance["other-2"] = other;

            Assert.AreSame(other, instance[1]);
            Assert.AreEqual("other-2", instance[1].Value);
            Assert.AreEqual("other-2", other.Value);
        }
    }
}
