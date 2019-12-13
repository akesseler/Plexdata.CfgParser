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
using Plexdata.CfgParser.Extensions;
using Plexdata.CfgParser.Settings;
using System;

namespace Plexdata.CfgParser.Tests.Extensions
{
    [TestFixture]
    [TestOf(nameof(ConfigExtension))]
    public class ConfigExtensionTests
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
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("[]", "")]
        [TestCase(" ", "")]
        [TestCase("[ ]", "")]
        [TestCase("contains inner white spaces", "contains inner white spaces")]
        [TestCase("[contains inner white spaces]", "contains inner white spaces")]
        [TestCase("   contains leading and inner white spaces  ", "contains leading and inner white spaces")]
        [TestCase("[   contains leading and inner white spaces  ]", "contains leading and inner white spaces")]
        public void FixupTitle_ConfigSettingsMixedIsUsed_ResultIsAsExpected(String input, String expected)
        {
            ConfigSettings.Settings = new ConfigSettingsMixed();
            Assert.AreEqual(expected, input.FixupTitle());
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("contains inner white spaces", "contains inner white spaces")]
        [TestCase("   contains leading and inner white spaces  ", "contains leading and inner white spaces")]
        public void FixupLabel_ConfigSettingsMixedIsUsed_ResultIsAsExpected(String input, String expected)
        {
            ConfigSettings.Settings = new ConfigSettingsMixed();
            Assert.AreEqual(expected, input.FixupLabel());
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("contains inner white spaces", "contains inner white spaces")]
        [TestCase("   contains leading and inner white spaces  ", "contains leading and inner white spaces")]
        [TestCase("comment marker # included", "\"comment marker # included\"")]
        [TestCase("   comment marker # included  ", "\"comment marker # included\"")]
        [TestCase("comment marker ; included", "\"comment marker ; included\"")]
        [TestCase("  comment marker ; included  ", "\"comment marker ; included\"")]
        [TestCase("value marker : included", "\"value marker : included\"")]
        [TestCase("  value marker : included  ", "\"value marker : included\"")]
        [TestCase("value marker = included", "\"value marker = included\"")]
        [TestCase("  value marker = included  ", "\"value marker = included\"")]
        public void FixupValue_VariousValues_ResultIsAsExpected(String input, String expected)
        {
            ConfigSettings.Settings = new ConfigSettingsMixed();
            Assert.AreEqual(expected, input.FixupValue());
        }

        [Test]
        [TestCase('@', " = ")]
        [TestCase(':', ": ")]
        [TestCase('=', " = ")]
        public void FixupMarker_ConfigSettingsMixedIsUsed_ResultIsAsExpected(Char input, String expected)
        {
            ConfigSettings.Settings = new ConfigSettingsMixed();
            Assert.AreEqual(expected, input.FixupMarker());
        }

        [Test]
        [TestCase('@', " = ")]
        [TestCase(':', ": ")]
        [TestCase('=', " = ")]
        public void FixupMarker_ConfigSettingsWindowsIsUsed_ResultIsAsExpected(Char input, String expected)
        {
            ConfigSettings.Settings = new ConfigSettingsWindows();
            Assert.AreEqual(expected, input.FixupMarker());
        }

        [Test]
        [TestCase('@', ": ")]
        [TestCase(':', ": ")]
        [TestCase('=', " = ")]
        public void FixupMarker_ConfigSettingsUnixIsUsed_ResultIsAsExpected(Char input, String expected)
        {
            ConfigSettings.Settings = new ConfigSettingsUnix();
            Assert.AreEqual(expected, input.FixupMarker());
        }
    }
}
