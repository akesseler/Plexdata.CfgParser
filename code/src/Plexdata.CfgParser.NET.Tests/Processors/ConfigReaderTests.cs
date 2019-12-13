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
using Plexdata.CfgParser.Processors;
using System;
using System.Collections.Generic;
using System.IO;

namespace Plexdata.CfgParser.Tests.Processors
{
    [TestFixture]
    [TestOf(nameof(ConfigReader))]
    public class ConfigReaderTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Read_InvlidFilename_ThrowsArgumentException(String filename)
        {
            Assert.Throws<ArgumentException>(() => { ConfigReader.Read(filename); });
        }

        [Test]
        public void Read_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => { ConfigReader.Read("file does not exist"); });
        }

        [Test]
        public void Read_StreamIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => { ConfigReader.Read((Stream)null); });
        }

        [Test]
        public void Read_TextReaderIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => { ConfigReader.Read((TextReader)null); });
        }

        [Test]
        public void Read_ContentIsEmpty_ResultIsEmpty()
        {
            using (TextReader reader = new StreamReader(this.GetTestContent(new String[0])))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(0, actual.Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsThreeHeaderLines_ResultHeaderCountIsThree()
        {
            String[] lines = new String[]
            {
                "# Header line 1",
                "# Header line 2",
                "# Header line 3",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(0, actual.Count);
                Assert.AreEqual(3, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsSixBlankAndThreeHeaderLines_ResultHeaderCountIsThree()
        {
            String[] lines = new String[]
            {
                String.Empty,
                String.Empty,
                String.Empty,
                "# Header line 1",
                "# Header line 2",
                "# Header line 3",
                String.Empty,
                String.Empty,
                String.Empty,
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(0, actual.Count);
                Assert.AreEqual(3, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsOneEmptySection_ResulIsSectionCountIsOne()
        {
            String[] lines = new String[]
            {
                "[section-1]",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(0, actual[0].Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsOneSectionOneValue_ResulIsSectionCountOneValueCountOne()
        {
            String[] lines = new String[]
            {
                "[section-1]",
                "value-1=data-1",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(1, actual[0].Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsOneSectionTwoValues_ResulIsSectionCountOneValueCountTwo()
        {
            String[] lines = new String[]
            {
                "[section-1]",
                "value-1=data-1",
                "value-2=data-2",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(2, actual[0].Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsTwoEmptySection_ResulIsSectionCountTwo()
        {
            String[] lines = new String[]
            {
                "[section-1]",
                "[section-2]",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(2, actual.Count);
                Assert.AreEqual(0, actual[0].Count);
                Assert.AreEqual(0, actual[1].Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsTwoSectionOneValue_ResulIsSectionCountTwoValueCountOne()
        {
            String[] lines = new String[]
            {
                "[section-1]",
                "value-11=data-11",
                "[section-2]",
                "value-21=data-21",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(2, actual.Count);
                Assert.AreEqual(1, actual[0].Count);
                Assert.AreEqual(1, actual[1].Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsTwoSectionTwoValues_ResulIsSectionCountTwoValueCountTwo()
        {
            String[] lines = new String[]
            {
                "[section-1]",
                "value-11=data-11",
                "value-12=data-12",
                "[section-2]",
                "value-21=data-21",
                "value-22=data-22",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(2, actual.Count);
                Assert.AreEqual(2, actual[0].Count);
                Assert.AreEqual(2, actual[1].Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsOneEmptySectionWithComment_ResulIsSectionCountIsOnePlusComment()
        {
            String[] lines = new String[]
            {
                "[section-1]#comment-section-1",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(0, actual[0].Count);
                Assert.IsTrue(actual[0].Comment.IsValid);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsTwoEmptySectionOneWithComment_ResulIsSectionCountIsTwoAndOneComment()
        {
            String[] lines = new String[]
            {
                "[section-1]",
                "[section-2]#comment-section-2",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(2, actual.Count);
                Assert.AreEqual(0, actual[0].Count);
                Assert.IsNull(actual[0].Comment);
                Assert.AreEqual(0, actual[1].Count);
                Assert.IsNotNull(actual[1].Comment);
                Assert.IsTrue(actual[1].Comment.IsValid);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsOneSectionOneValueWithComment_ResulIsSectionCountIsOneValueCountOnePlusComment()
        {
            String[] lines = new String[]
            {
                "[section-1]",
                "value-1=data-1#comment-value-1",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(1, actual.Count);
                Assert.IsNull(actual[0].Comment);
                Assert.AreEqual(1, actual[0].Count);
                Assert.IsNotNull(actual[0][0].Comment);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(0, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsOneOther_ResulIsOtherCountOne()
        {
            String[] lines = new String[]
            {
                "something-else",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(0, actual.Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(1, actual.Others.Count);
            }
        }

        [Test]
        public void Read_ContentIsTwoOthers_ResulIsOthersCountTwo()
        {
            String[] lines = new String[]
            {
                "something-else-1",
                "something-else-2",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                ConfigContent actual = ConfigReader.Read(reader);

                Assert.AreEqual(0, actual.Count);
                Assert.AreEqual(0, actual.Header.Count);
                Assert.AreEqual(2, actual.Others.Count);
            }
        }

        [Test]
        public void Read_FullTestNoWarnings_ResulIsWarningsCountIsZero()
        {
            String[] lines = new String[]
            {
                "",
                "",
                "",
                "  # header-1",
                "  # header-2         ",
                "  # header-3",
                "",
                "other-1",
                "",
                "        [section-1]    # section-comment-1      ",
                "value-11=data-11",
                "value-12=data-12#value-comment-12            ",
                "",
                "other-2         ",
                "         ",
                "[section-2]         ",
                "",
                "other-4",
                "",
                "value-21=data-21                 # value-comment-21",
                "value-22=data-22            ",
                "",
                "",
                "    other-4",
                "",
                "",
                "",
                "",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                List<ConfigWarning> warnings = new List<ConfigWarning>();
                ConfigContent actual = ConfigReader.Read(reader, warnings);

                Assert.AreEqual(2, actual.Count);
                Assert.IsNotNull(actual[0].Comment);
                Assert.AreEqual(2, actual[0].Count);
                Assert.IsNull(actual[0][0].Comment);
                Assert.IsNotNull(actual[0][1].Comment);
                Assert.IsNull(actual[1].Comment);
                Assert.AreEqual(2, actual[1].Count);
                Assert.IsNotNull(actual[1][0].Comment);
                Assert.IsNull(actual[1][1].Comment);
                Assert.AreEqual(3, actual.Header.Count);
                Assert.AreEqual(4, actual.Others.Count);
                Assert.AreEqual(0, warnings.Count);
            }
        }

        [Test]
        public void Read_FullTestWithWarnings_ResulIsWarningsCountIsFour()
        {
            String[] lines = new String[]
            {
                "",
                "",
                "",
                "  # header-1",
                "  # header-2         ",
                "  # header-3",
                "",
                "# misplaced-comment-1",
                "",
                "      misplaced-value-1=misplaced-value-data-1",
                "",
                "other-1",
                "",
                "        [section-1]    # section-comment-1      ",
                "value-11=data-11",
                "value-12=data-12#value-comment-12            ",
                "",
                "         # misplaced-comment-2",
                "",
                "other-2         ",
                "         ",
                "[section-2]         ",
                "",
                "other-4",
                "",
                "value-21=data-21                 # value-comment-21",
                "value-22=data-22            ",
                "",
                "",
                "    other-4",
                "",
                "",
                "# misplaced-comment-3",
                "",
                "",
            };

            using (TextReader reader = new StreamReader(this.GetTestContent(lines)))
            {
                List<ConfigWarning> warnings = new List<ConfigWarning>();
                ConfigContent actual = ConfigReader.Read(reader, warnings);

                Assert.AreEqual(2, actual.Count);
                Assert.IsNotNull(actual[0].Comment);
                Assert.AreEqual(2, actual[0].Count);
                Assert.IsNull(actual[0][0].Comment);
                Assert.IsNotNull(actual[0][1].Comment);
                Assert.IsNull(actual[1].Comment);
                Assert.AreEqual(2, actual[1].Count);
                Assert.IsNotNull(actual[1][0].Comment);
                Assert.IsNull(actual[1][1].Comment);
                Assert.AreEqual(3, actual.Header.Count);
                Assert.AreEqual(4, actual.Others.Count);
                Assert.AreEqual(4, warnings.Count);
            }
        }

        private MemoryStream GetTestContent(IEnumerable<String> lines)
        {
            using (MemoryStream stream = new MemoryStream())
            using (TextWriter writer = new StreamWriter(stream))
            {
                foreach (String line in lines)
                {
                    writer.WriteLine(line);
                }

                writer.Flush();
                stream.Position = 0;

                return new MemoryStream(stream.ToArray());
            }
        }
    }
}
