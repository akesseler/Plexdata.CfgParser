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

namespace Plexdata.CfgParser.Tests.Settings
{
    [TestFixture]
    [TestOf(nameof(ConfigSettings))]
    public class ConfigSettingsTests
    {
        [Test]
        public void ConfigSettings_DefaultConfigSettingsValidation_ResultIsInstanceOfConfigSettingsMixed()
        {
            Assert.IsInstanceOf<ConfigSettingsMixed>(ConfigSettings.DefaultConfigSettings);
        }

        [Test]
        public void Settings_DefaultConfigSettingsValidation_ResultIsInstanceOfConfigSettingsMixed()
        {
            ConfigSettings.Settings = ConfigSettings.DefaultConfigSettings;
            Assert.IsInstanceOf<ConfigSettingsMixed>(ConfigSettings.Settings);
        }

        [Test]
        public void Settings_SettingsIsNull_ResultIsInstanceOfConfigSettingsMixed()
        {
            ConfigSettings.Settings = null;
            Assert.IsInstanceOf<ConfigSettingsMixed>(ConfigSettings.Settings);
        }

        [Test]
        public void Settings_ConfigSettingsTestClassValidation_ResultIsInstanceOfConfigSettingsTestClass()
        {
            ConfigSettings.Settings = new ConfigSettingsTestClass('@', 'µ');
            Assert.IsInstanceOf<ConfigSettingsTestClass>(ConfigSettings.Settings);
        }

        [Test]
        public void CreateDefaultHeader_WithoutTitleWithoutPlaceholders_ResultIs()
        {
            String expected =
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                $"# Header rules:" +
                $"# - Each header line must start with a comment marker." +
                $"# - Each of the header lines must be a pure comment line." +
                $"# - Each header comment line must be in front on any other content." +
                $"# Comment Rules:" +
                $"# - Comments can be tagged by character '#' or by character ';'." +
                $"# - Comments can be placed in a single line but only as header type." +
                $"# - Comments can be placed at the end of line of each section." +
                $"# - Comments can be placed at the end of line of each value-data-pair." +
                $"# Section Rules:" +
                $"# - Sections are enclosed in '[' and ']'." +
                $"# - Section names should not include white spaces." +
                $"# Value Rules:" +
                $"# - Values can have an empty data part." +
                $"# - Value names should not include white spaces." +
                $"# - Values without a section are treated as 'others'." +
                $"# - Values are built as pair of 'name:data' or of 'name=data'." +
                $"# - Value data that use '#', ';', '[', ']', ':' or '=' must be enclosed by '\"'." +
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

            ConfigHeader header = ConfigSettings.CreateDefaultHeader();

            Assert.AreEqual(expected, String.Join(String.Empty, header.ToOutput()));
        }

        [Test]
        public void CreateDefaultHeader_WithTitleWithoutPlaceholders_ResultIs()
        {
            String expected =
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                $"# My funny title" +
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                $"# Header rules:" +
                $"# - Each header line must start with a comment marker." +
                $"# - Each of the header lines must be a pure comment line." +
                $"# - Each header comment line must be in front on any other content." +
                $"# Comment Rules:" +
                $"# - Comments can be tagged by character '#' or by character ';'." +
                $"# - Comments can be placed in a single line but only as header type." +
                $"# - Comments can be placed at the end of line of each section." +
                $"# - Comments can be placed at the end of line of each value-data-pair." +
                $"# Section Rules:" +
                $"# - Sections are enclosed in '[' and ']'." +
                $"# - Section names should not include white spaces." +
                $"# Value Rules:" +
                $"# - Values can have an empty data part." +
                $"# - Value names should not include white spaces." +
                $"# - Values without a section are treated as 'others'." +
                $"# - Values are built as pair of 'name:data' or of 'name=data'." +
                $"# - Value data that use '#', ';', '[', ']', ':' or '=' must be enclosed by '\"'." +
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

            ConfigHeader header = ConfigSettings.CreateDefaultHeader("My funny title");

            Assert.AreEqual(expected, String.Join(String.Empty, header.ToOutput()));
        }

        [Test]
        public void CreateDefaultHeader_WithoutTitleWithPlaceholders_ResultIs()
        {
            String expected =
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                $"# File name: {{file-name-placeholder}}" +
                $"# File date: {{file-date-placeholder}}" +
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                $"# Header rules:" +
                $"# - Each header line must start with a comment marker." +
                $"# - Each of the header lines must be a pure comment line." +
                $"# - Each header comment line must be in front on any other content." +
                $"# Comment Rules:" +
                $"# - Comments can be tagged by character '#' or by character ';'." +
                $"# - Comments can be placed in a single line but only as header type." +
                $"# - Comments can be placed at the end of line of each section." +
                $"# - Comments can be placed at the end of line of each value-data-pair." +
                $"# Section Rules:" +
                $"# - Sections are enclosed in '[' and ']'." +
                $"# - Section names should not include white spaces." +
                $"# Value Rules:" +
                $"# - Values can have an empty data part." +
                $"# - Value names should not include white spaces." +
                $"# - Values without a section are treated as 'others'." +
                $"# - Values are built as pair of 'name:data' or of 'name=data'." +
                $"# - Value data that use '#', ';', '[', ']', ':' or '=' must be enclosed by '\"'." +
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

            ConfigHeader header = ConfigSettings.CreateDefaultHeader(true);

            Assert.AreEqual(expected, String.Join(String.Empty, header.ToOutput()));
        }

        [Test]
        public void CreateDefaultHeader_WithTitleWithPlaceholders_ResultIs()
        {
            String expected =
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                $"# My funny title" +
                $"# File name: {{file-name-placeholder}}" +
                $"# File date: {{file-date-placeholder}}" +
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                $"# Header rules:" +
                $"# - Each header line must start with a comment marker." +
                $"# - Each of the header lines must be a pure comment line." +
                $"# - Each header comment line must be in front on any other content." +
                $"# Comment Rules:" +
                $"# - Comments can be tagged by character '#' or by character ';'." +
                $"# - Comments can be placed in a single line but only as header type." +
                $"# - Comments can be placed at the end of line of each section." +
                $"# - Comments can be placed at the end of line of each value-data-pair." +
                $"# Section Rules:" +
                $"# - Sections are enclosed in '[' and ']'." +
                $"# - Section names should not include white spaces." +
                $"# Value Rules:" +
                $"# - Values can have an empty data part." +
                $"# - Value names should not include white spaces." +
                $"# - Values without a section are treated as 'others'." +
                $"# - Values are built as pair of 'name:data' or of 'name=data'." +
                $"# - Value data that use '#', ';', '[', ']', ':' or '=' must be enclosed by '\"'." +
                $"# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

            ConfigHeader header = ConfigSettings.CreateDefaultHeader("My funny title", true);

            Assert.AreEqual(expected, String.Join(String.Empty, header.ToOutput()));
        }
    }
}
