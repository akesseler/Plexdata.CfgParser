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
using Plexdata.CfgParser.Constants;
using Plexdata.CfgParser.Entities;
using Plexdata.CfgParser.Processors;
using System;
using System.IO;
using System.Text;

namespace Plexdata.CfgParser.Tests.Processors
{
    [TestFixture]
    [TestOf(nameof(ConfigWriter))]
    public class ConfigWriterTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Save_InvalidFilename_ThrowsArgumentException(String filename)
        {
            Assert.Throws<ArgumentException>(() => { ConfigWriter.Write(null, filename); });
        }

        [Test]
        public void Save_FileExistOverwriteOff_ThrowsArgumentException()
        {
            String filename = Path.GetTempFileName();
            try
            {
                Assert.Throws<ArgumentException>(() => { ConfigWriter.Write(null, filename, false); });
            }
            finally
            {
                if (File.Exists(filename)) { File.Delete(filename); }
            }
        }

        [Test]
        public void Save_InvalidStream_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { ConfigWriter.Write(null, (Stream)null); });
        }

        [Test]
        public void Save_InvalidWriterValidContent_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { ConfigWriter.Write(new ConfigContent(), (TextWriter)null); });
        }

        [Test]
        public void Save_ValidWriterInvalidContent_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { ConfigWriter.Write(null, new StreamWriter(new MemoryStream())); });
        }

        [Test]
        public void Save_HeaderWithActualFilename_ResultContainsFilename()
        {
            String filename = Path.GetTempFileName();
            try
            {
                ConfigContent content = new ConfigContent();
                content.Header.Append($"Filename: {ConfigDefines.FileNamePlaceholder}");

                ConfigWriter.Write(content, filename, true);

                String actual = File.ReadAllText(filename);

                Assert.IsTrue(actual.Contains(Path.GetFileName(filename)));
            }
            finally
            {
                if (File.Exists(filename)) { File.Delete(filename); }
            }
        }

        [Test]
        public void Save_HeaderWithUnusedFilename_ResultContainsUnused()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            ConfigContent content = new ConfigContent();

            content.Header.Append($"Filename: {ConfigDefines.FileNamePlaceholder}");

            ConfigWriter.Write(content, writer);

            String actual = Encoding.UTF8.GetString(stream.ToArray());

            Assert.IsTrue(actual.Contains("unused"));
        }

        [Test]
        public void Save_HeaderWithTimestamp_ResultContainsTimestamp()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            ConfigContent content = new ConfigContent();

            content.Header.Append($"Timestamp: {ConfigDefines.FileDatePlaceholder}");

            ConfigWriter.Write(content, writer);

            String actual = Encoding.UTF8.GetString(stream.ToArray());

            Assert.IsTrue(actual.Contains(DateTime.Now.ToString("yyyy-MM-dd HH:mm:")));
        }

        [Test]
        public void Save_OneEmptySection_ResultContainsOneEmptySection()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            ConfigContent content = new ConfigContent();

            content.Append("section-1");

            ConfigWriter.Write(content, writer);

            String actual = Encoding.UTF8.GetString(stream.ToArray());

            Assert.IsTrue(actual.Contains("[section-1]"));
        }

        [Test]
        public void Save_OneSectionOneValue_ResultContainsOneSectionOneValue()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            ConfigContent content = new ConfigContent();

            ConfigSection section = content.Append("section-1");
            section.Append(new ConfigValue("label-1", "data-1"));

            ConfigWriter.Write(content, writer);

            String actual = Encoding.UTF8.GetString(stream.ToArray());

            Assert.IsTrue(actual.Contains("[section-1]"));
            Assert.IsTrue(actual.Contains("label-1 = data-1"));
        }

        [Test]
        public void Save_FullTest_ResultIsAsExpected()
        {
            String expected =
                $"# header-1\r\n" +
                $"# header-2\r\n" +
                $"\r\n" +
                $"other-1\r\n" +
                $"other-2\r\n" +
                $"other-3\r\n" +
                $"\r\n" +
                $"[section-1] # section-1-comment\r\n" +
                $"label-11 = data-11 # label-11-comment\r\n" +
                $"label-12 = data-12 # label-12-comment\r\n" +
                $"label-13 = data-13 # label-13-comment\r\n" +
                $"\r\n" +
                $"[section-2] # section-2-comment\r\n" +
                $"label-21 = data-21 # label-21-comment\r\n" +
                $"label-22 = data-22 # label-22-comment\r\n" +
                $"\r\n";

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            ConfigContent content = new ConfigContent();
            ConfigSection section = null;

            content.Header.Append("header-1");
            content.Header.Append("header-2");
            content.Others.Append("other-1");
            content.Others.Append("other-2");
            content.Others.Append("other-3");

            section = content.Append(new ConfigSection("section-1", "section-1-comment"));
            section.Append(new ConfigValue("label-11", "data-11", "label-11-comment"));
            section.Append(new ConfigValue("label-12", "data-12", "label-12-comment"));
            section.Append(new ConfigValue("label-13", "data-13", "label-13-comment"));

            section = content.Append(new ConfigSection("section-2", "section-2-comment"));
            section.Append(new ConfigValue("label-21", "data-21", "label-21-comment"));
            section.Append(new ConfigValue("label-22", "data-22", "label-22-comment"));

            ConfigWriter.Write(content, writer);

            String actual = Encoding.UTF8.GetString(stream.ToArray());

            Assert.AreEqual(expected, actual);
        }
    }
}
