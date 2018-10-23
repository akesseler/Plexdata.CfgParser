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
    [TestOf(nameof(ConfigHeader))]
    public class ConfigHeaderTests
    {
        [Test]
        [TestCase(0, "")]
        [TestCase(1, "# comment-0")]
        [TestCase(2, "# comment-0# comment-1")]
        [TestCase(3, "# comment-0# comment-1# comment-2")]
        public void Append_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigHeader instance = new ConfigHeader();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"comment-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigComment comment in instance.Comments)
            {
                actual += comment.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, "")]
        [TestCase(1, "# comment-0")]
        [TestCase(2, "# comment-1# comment-0")]
        [TestCase(3, "# comment-2# comment-1# comment-0")]
        public void Prepend_VariousItems_ResultIsExpected(Int32 count, String expected)
        {
            ConfigHeader instance = new ConfigHeader();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Prepend($"comment-{index}");
            }

            String actual = String.Empty;

            foreach (ConfigComment comment in instance.Comments)
            {
                actual += comment.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(5, 3, "# comment-0# comment-1# comment-2# comment-insert# comment-3# comment-4")]
        [TestCase(5, 10, "# comment-0# comment-1# comment-2# comment-3# comment-4# comment-insert")]
        [TestCase(5, -10, "# comment-insert# comment-0# comment-1# comment-2# comment-3# comment-4")]
        public void Insert_VariousItems_ResultIsExpected(Int32 count, Int32 insert, String expected)
        {
            ConfigHeader instance = new ConfigHeader();

            for (Int32 index = 0; index < count; index++)
            {
                instance.Append($"comment-{index}");
            }

            instance.Insert(insert, "comment-insert");

            String actual = String.Empty;

            foreach (ConfigComment comment in instance.Comments)
            {
                actual += comment.ToOutput();
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Insert_CommentInvalid_ThrowsArgumentNullException()
        {
            ConfigHeader instance = new ConfigHeader();
            ConfigComment comment = null;
            Assert.Throws<ArgumentNullException>(() => { instance.Insert(0, comment); });
        }

        [Test]
        public void Remove_CommentTextNotFound_ResultIsNull()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-0");
            instance[1] = new ConfigComment("comment-1");
            instance[2] = new ConfigComment("comment-2");
            instance[3] = new ConfigComment("comment-3");
            instance[4] = new ConfigComment("comment-4");

            Assert.IsNull(instance.Remove("CommentTextNotFound"));
        }

        [Test]
        [TestCase("comment-2")]
        [TestCase("COMMENT-2")]
        [TestCase("Comment-2")]
        public void Remove_CommentTextFound_ResultIsNotNull(String value)
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-0");
            instance[1] = new ConfigComment("comment-1");
            instance[2] = new ConfigComment("comment-2");
            instance[3] = new ConfigComment("comment-3");
            instance[4] = new ConfigComment("comment-4");

            Assert.IsNotNull(instance.Remove(value));
        }

        [Test]
        public void Remove_CommentIndexNotFound_ResultIsNull()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-0");
            instance[1] = new ConfigComment("comment-1");
            instance[2] = new ConfigComment("comment-2");
            instance[3] = new ConfigComment("comment-3");
            instance[4] = new ConfigComment("comment-4");

            Assert.IsNull(instance.Remove(5));
        }

        [Test]
        public void Remove_CommentIndexFound_ResultIsNotNull()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-0");
            instance[1] = new ConfigComment("comment-1");
            instance[2] = new ConfigComment("comment-2");
            instance[3] = new ConfigComment("comment-3");
            instance[4] = new ConfigComment("comment-4");

            Assert.IsNotNull(instance.Remove(2));
        }

        [Test]
        public void Find_CommentTextNotFound_ResultIsNull()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-0");
            instance[1] = new ConfigComment("comment-1");
            instance[2] = new ConfigComment("comment-2");
            instance[3] = new ConfigComment("comment-3");
            instance[4] = new ConfigComment("comment-4");

            Assert.IsNull(instance.Find("CommentTextNotFound"));
        }

        [Test]
        [TestCase("comment-2")]
        [TestCase("COMMENT-2")]
        [TestCase("Comment-2")]
        public void Find_CommentTextFound_ResultIsNotNull(String value)
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-0");
            instance[1] = new ConfigComment("comment-1");
            instance[2] = new ConfigComment("comment-2");
            instance[3] = new ConfigComment("comment-3");
            instance[4] = new ConfigComment("comment-4");

            Assert.IsNotNull(instance.Find(value));
        }

        [Test]
        public void ToOutput_FiveComments_ResultIsAsExpected()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-0");
            instance[1] = new ConfigComment("comment-1");
            instance[2] = new ConfigComment("comment-2");
            instance[3] = new ConfigComment("comment-3");
            instance[4] = new ConfigComment("comment-4");

            String actual = String.Join(String.Empty, instance.ToOutput());

            Assert.AreEqual("# comment-0# comment-1# comment-2# comment-3# comment-4", actual);
        }

        [Test]
        public void IndexArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigHeader instance = new ConfigHeader();

            Assert.Throws<ArgumentNullException>(() => instance[0] = null);
        }

        [Test]
        public void IndexArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-1");
            instance[1] = new ConfigComment("comment-2");
            instance[2] = new ConfigComment("comment-3");

            ConfigComment comment = new ConfigComment("appended-comment");

            instance[1000] = comment;

            Assert.AreSame(comment, instance[3]);
        }

        [Test]
        public void IndexArrayAccessor_PrependValidValue_ResultIsValuePrepended()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-1");
            instance[1] = new ConfigComment("comment-2");
            instance[2] = new ConfigComment("comment-3");

            ConfigComment comment = new ConfigComment("prepended-comment");

            instance[-1000] = comment;

            Assert.AreSame(comment, instance[0]);
        }

        [Test]
        public void IndexArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-1");
            instance[1] = new ConfigComment("comment-2");
            instance[2] = new ConfigComment("comment-3");

            ConfigComment comment = new ConfigComment("replaced-comment");

            instance[1] = comment;

            Assert.AreSame(comment, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_SetInvalidValue_ThrowsArgumentNullException()
        {
            ConfigHeader instance = new ConfigHeader();

            Assert.Throws<ArgumentNullException>(() => instance["comment"] = null);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_GetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigHeader instance = new ConfigHeader();
            ConfigComment comment;
            Assert.Throws<ArgumentException>(() => comment = instance[key]);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void KeyArrayAccessor_SetValueWithInvalidKey_ThrowsArgumentException(String key)
        {
            ConfigHeader instance = new ConfigHeader();

            Assert.Throws<ArgumentException>(() => instance[key] = new ConfigComment());
        }

        [Test]
        public void KeyArrayAccessor_AppendValidValue_ResultIsValueAppended()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-1");
            instance[1] = new ConfigComment("comment-2");
            instance[2] = new ConfigComment("comment-3");

            ConfigComment comment = new ConfigComment("appended-comment");

            instance["comment-not-known"] = comment;

            Assert.AreSame(comment, instance[3]);
        }

        [Test]
        public void KeyArrayAccessor_AppendEmptyValue_ResultIsValueAppendedWithKey()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-1");
            instance[1] = new ConfigComment("comment-2");
            instance[2] = new ConfigComment("comment-3");

            ConfigComment comment = new ConfigComment();

            instance["comment-42"] = comment;

            Assert.AreSame(comment, instance[3]);
            Assert.AreEqual("comment-42", instance[3].Text);
            Assert.AreEqual("comment-42", comment.Text);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceValidValue_ResultIsValueReplaced()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-1");
            instance[1] = new ConfigComment("comment-2");
            instance[2] = new ConfigComment("comment-3");

            ConfigComment comment = new ConfigComment("replaced-comment");

            instance["comment-2"] = comment;

            Assert.AreSame(comment, instance[1]);
        }

        [Test]
        public void KeyArrayAccessor_ReplaceEmptyValue_ResultIsValueReplacedWithKey()
        {
            ConfigHeader instance = new ConfigHeader();

            instance[0] = new ConfigComment("comment-1");
            instance[1] = new ConfigComment("comment-2");
            instance[2] = new ConfigComment("comment-3");

            ConfigComment comment = new ConfigComment();

            instance["comment-2"] = comment;

            Assert.AreSame(comment, instance[1]);
            Assert.AreEqual("comment-2", instance[1].Text);
            Assert.AreEqual("comment-2", comment.Text);
        }
    }
}
