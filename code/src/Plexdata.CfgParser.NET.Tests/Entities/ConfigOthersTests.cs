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
    [TestOf(nameof(ConfigOthers))]
    public class ConfigOthersTests
    {
        [Test]
        [TestCase(0, "")]
        [TestCase(1, "other-0")]
        [TestCase(2, "other-0other-1")]
        [TestCase(3, "other-0other-1other-2")]
        public void Append_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigOthers instance = new ConfigOthers();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"other-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigOther other in instance.Items)
            {
                actual += other.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, "")]
        [TestCase(1, "other-0")]
        [TestCase(2, "other-1other-0")]
        [TestCase(3, "other-2other-1other-0")]
        public void Prepend_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigOthers instance = new ConfigOthers();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Prepend($"other-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigOther other in instance.Items)
            {
                actual += other.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(5, 3, "other-0other-1other-2other-insertother-3other-4")]
        [TestCase(5, 10, "other-0other-1other-2other-3other-4other-insert")]
        [TestCase(5, -10, "other-insertother-0other-1other-2other-3other-4")]
        public void Insert_VariousItems_ResultIsExpected(Int32 count, Int32 insert, String expected)
        {
            ConfigOthers instance = new ConfigOthers();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"other-{index}");
            }

            instance.Insert(insert, "other-insert");

            String actual = String.Empty;

            foreach (ConfigOther other in instance.Items)
            {
                actual += other.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Insert_OtherInvalid_ThrowsArgumentNullException()
        {
            ConfigOthers instance = new ConfigOthers();
            ConfigOther other = null;
            Assert.Throws<ArgumentNullException>(() => { instance.Insert(0, other); });
        }

        [Test]
        public void Remove_OtherTextNotFound_ResultIsNull()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-0");
            instance[1] = new ConfigOther("other-1");
            instance[2] = new ConfigOther("other-2");
            instance[3] = new ConfigOther("other-3");
            instance[4] = new ConfigOther("other-4");

            Assert.IsNull(instance.Remove("OtherTextNotFound"));
        }

        [Test]
        [TestCase("other-2")]
        [TestCase("OTHER-2")]
        [TestCase("Other-2")]
        public void Remove_OtherTextFound_ResultIsNotNull(String value)
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-0");
            instance[1] = new ConfigOther("other-1");
            instance[2] = new ConfigOther("other-2");
            instance[3] = new ConfigOther("other-3");
            instance[4] = new ConfigOther("other-4");

            Assert.IsNotNull(instance.Remove(value));
        }

        [Test]
        public void Remove_OtherIndexNotFound_ResultIsNull()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-0");
            instance[1] = new ConfigOther("other-1");
            instance[2] = new ConfigOther("other-2");
            instance[3] = new ConfigOther("other-3");
            instance[4] = new ConfigOther("other-4");

            Assert.IsNull(instance.Remove(5));
        }

        [Test]
        public void Remove_OtherIndexFound_ResultIsNotNull()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-0");
            instance[1] = new ConfigOther("other-1");
            instance[2] = new ConfigOther("other-2");
            instance[3] = new ConfigOther("other-3");
            instance[4] = new ConfigOther("other-4");

            Assert.IsNotNull(instance.Remove(2));
        }

        [Test]
        public void Find_OtherTextNotFound_ResultIsNull()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-0");
            instance[1] = new ConfigOther("other-1");
            instance[2] = new ConfigOther("other-2");
            instance[3] = new ConfigOther("other-3");
            instance[4] = new ConfigOther("other-4");

            Assert.IsNull(instance.Find("OtherTextNotFound"));
        }

        [Test]
        [TestCase("other-2")]
        [TestCase("OTHER-2")]
        [TestCase("Other-2")]
        public void Find_OtherTextFound_ResultIsNotNull(String value)
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-0");
            instance[1] = new ConfigOther("other-1");
            instance[2] = new ConfigOther("other-2");
            instance[3] = new ConfigOther("other-3");
            instance[4] = new ConfigOther("other-4");

            Assert.IsNotNull(instance.Find(value));
        }

        [Test]
        public void ToOutput_FiveOthers_ResultIsAsExpected()
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
        public void ArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigOthers instance = new ConfigOthers();

            Assert.Throws<ArgumentNullException>(() => instance[0] = null);
        }

        [Test]
        public void ArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther("appended-other");

            instance[1000] = other;

            Assert.AreSame(other, instance[instance.Count - 1]);
        }

        [Test]
        public void ArrayAccessor_PrependValidValue_ResultIsValuePrepended()
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
        public void ArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigOthers instance = new ConfigOthers();

            instance[0] = new ConfigOther("other-1");
            instance[1] = new ConfigOther("other-2");
            instance[2] = new ConfigOther("other-3");

            ConfigOther other = new ConfigOther("replaced-other");

            instance[1] = other;

            Assert.AreSame(other, instance[1]);
        }
    }
}
