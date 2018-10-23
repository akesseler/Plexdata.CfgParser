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
    [TestOf(nameof(ConfigComment))]
    public class ConfigCommentTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ConfigComment_ConstructionTextInvalid_ResultIsInvalid(String text)
        {
            ConfigComment instance = new ConfigComment(text);
            Assert.IsFalse(instance.IsValid);
        }

        [Test]
        [TestCase("text")]
        [TestCase("  text  ")]
        public void ConfigComment_ConstructionTextValid_ResultIsValid(String text)
        {
            ConfigComment instance = new ConfigComment(text);
            Assert.IsTrue(instance.IsValid);
        }

        [Test]
        [TestCase("text", "text")]
        [TestCase("  text  ", "text")]
        [TestCase("text  ", "text")]
        [TestCase("  text", "text")]
        public void ConfigComment_ConstructionTextValid_ResultIsExpected(String text, String expected)
        {
            ConfigComment instance = new ConfigComment(text);
            Assert.AreEqual(expected, instance.Text);
        }

        [Test]
        public void SetGetMarker_MarkerUnsupported_ThrowsArgumentException()
        {
            ConfigComment instance = new ConfigComment("text");
            Assert.Throws<ArgumentException>(() => { instance.Marker = 'b'; });
        }

        [Test]
        [TestCase('#', '#')]
        [TestCase(';', ';')]
        public void SetGetMarker_MarkerSupported_ResultIsExpected(Char marker, Char expected)
        {
            ConfigComment instance = new ConfigComment("text");
            instance.Marker = marker;
            Assert.AreEqual(expected, instance.Marker);
        }

        [Test]
        [TestCase("# text", true)]
        [TestCase("; text", true)]
        [TestCase("@ text", false)]
        public void TryParse_BufferAsIs_ResultIsExpected(String buffer, Boolean expected)
        {
            Assert.AreEqual(expected, ConfigComment.TryParse(buffer, out ConfigComment instance));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Parse_BufferInvalid_ResultIsNull(String buffer)
        {
            Assert.IsNull(ConfigComment.Parse(buffer));
        }

        [Test]
        [TestCase("@")]
        [TestCase("  @")]
        [TestCase("@text")]
        [TestCase("@  text")]
        [TestCase("  @text")]
        [TestCase("  @  text")]
        public void Parse_MarkerInvalid_ThrowsFormatException(String buffer)
        {
            Assert.Throws<FormatException>(() => { ConfigComment.Parse(buffer); });
        }

        [Test]
        [TestCase("#", "# ")]
        [TestCase("  #", "# ")]
        [TestCase("#text", "# text")]
        [TestCase("#  text", "# text")]
        [TestCase("  #text", "# text")]
        [TestCase("  #  text", "# text")]
        [TestCase(";", "; ")]
        [TestCase("  ;", "; ")]
        [TestCase(";text", "; text")]
        [TestCase(";  text", "; text")]
        [TestCase("  ;text", "; text")]
        [TestCase("  ;  text", "; text")]
        public void Parse_BufferValid_ResultIsExpected(String buffer, String expected)
        {
            ConfigComment instance = ConfigComment.Parse(buffer);
            Assert.AreEqual(expected, instance.ToOutput());
        }
    }
}
