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

using Plexdata.CfgParser.Constants;
using Plexdata.CfgParser.Entities;
using System;

namespace Plexdata.CfgParser.Settings
{
    /// <summary>
    /// This pure static class allows managing the settings of how to handle configuration 
    /// processing.
    /// </summary>
    /// <remarks>
    /// This pure static class serves as central point where to set and where to get global 
    /// configuration settings.
    /// </remarks>
    public static class ConfigSettings
    {
        #region Public static fields

        /// <summary>
        /// The default settings to be used if no other settings have been specified.
        /// </summary>
        /// <remarks>
        /// The used default settings is an instance of class <see cref="ConfigSettingsMixed"/> 
        /// which actually uses the Unix-style comment marker and the Windows-style value marker. 
        /// The feature of replacing white spaces is off by default.
        /// </remarks>
        public static readonly ConfigSettingsBase DefaultConfigSettings = new ConfigSettingsMixed();

        #endregion

        #region Private static fields

        /// <summary>
        /// The instance of currently used configuration settings.
        /// </summary>
        /// <remarks>
        /// The default value is set to <see cref="ConfigSettings.DefaultConfigSettings"/>.
        /// </remarks>
        private static ConfigSettingsBase currentSettings = ConfigSettings.DefaultConfigSettings;

        #endregion

        #region Construction

        /// <summary>
        /// The static constructor initializes all static fields of the <see cref="ConfigSettings"/> 
        /// class.
        /// </summary>
        /// <remarks>
        /// The field of current settings is initialized with <see cref="ConfigSettings.DefaultConfigSettings"/>.
        /// </remarks>
        static ConfigSettings() { }

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets and sets the configuration settings that are currently used.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="ConfigSettings.DefaultConfigSettings"/> are used if provided 
        /// <c>value</c> is set to <c>null</c>.
        /// </para>
        /// <para>
        /// This value becomes relevant when writing a configuration file.
        /// </para>
        /// </remarks>
        /// <value>
        /// An instance of a class derived from class <see cref="ConfigSettingsBase"/>.
        /// </value>
        public static ConfigSettingsBase Settings
        {
            get
            {
                return ConfigSettings.currentSettings;
            }
            set
            {
                if (value == null)
                {
                    value = ConfigSettings.DefaultConfigSettings;
                }

                ConfigSettings.currentSettings = value;
            }
        }

        /// <summary>
        /// Returns the default value marker.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of <see cref="ConfigSettingsBase.DefaultValueMarker"/> of property 
        /// <see cref="ConfigSettings.Settings"/> is actually returned.
        /// </para>
        /// <para>
        /// This value becomes relevant when writing a configuration file.
        /// </para>
        /// </remarks>
        /// <value>
        /// The character representing the default value marker.
        /// </value>
        public static Char DefaultValueMarker
        {
            get
            {
                return ConfigSettings.currentSettings.DefaultValueMarker;
            }
        }

        /// <summary>
        /// Returns the default comment marker.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of <see cref="ConfigSettingsBase.DefaultCommentMarker"/> of property 
        /// <see cref="ConfigSettings.Settings"/> is actually returned.
        /// </para>
        /// <para>
        /// This value becomes relevant when writing a configuration file.
        /// </para>
        /// </remarks>
        /// <value>
        /// The character representing the default comment marker.
        /// </value>
        public static Char DefaultCommentMarker
        {
            get
            {
                return ConfigSettings.currentSettings.DefaultCommentMarker;
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// This method creates the default header to be used in a configuration output.
        /// </summary>
        /// <remarks>
        /// The default header that is created by this method includes the set of rules 
        /// to be followed that are shown to the users.
        /// </remarks>
        /// <returns>
        /// An instance of class <see cref="ConfigHeader"/> containing a list of comments 
        /// representing the header content.
        /// </returns>
        /// <seealso cref="ConfigSettings.CreateDefaultHeader(String, Boolean)"/>
        public static ConfigHeader CreateDefaultHeader()
        {
            return ConfigSettings.CreateDefaultHeader(null);
        }

        /// <summary>
        /// This method creates the default header to be used in a configuration output. 
        /// Further, the created header includes an additional title.
        /// </summary>
        /// <remarks>
        /// The default header that is created by this method includes the set of rules 
        /// to be followed as well as the title that are shown to the users.
        /// </remarks>
        /// <param name="title">
        /// The title to be used.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigHeader"/> containing a list of comments 
        /// representing the header content.
        /// </returns>
        /// <seealso cref="ConfigSettings.CreateDefaultHeader(String, Boolean)"/>
        public static ConfigHeader CreateDefaultHeader(String title)
        {
            return ConfigSettings.CreateDefaultHeader(title, false);
        }

        /// <summary>
        /// This method creates the default header to be used in a configuration output. 
        /// Further, the created header includes additional lines with placeholders for 
        /// the file name and the file's creation date.
        /// </summary>
        /// <remarks>
        /// The default header that is created by this method includes the set of rules 
        /// to be followed as well as some placeholders that are shown to the users.
        /// </remarks>
        /// <param name="placeholders">
        /// True to enable a usage of placeholders and false to disable it.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigHeader"/> containing a list of comments 
        /// representing the header content.
        /// </returns>
        /// <seealso cref="ConfigSettings.CreateDefaultHeader(String, Boolean)"/>
        public static ConfigHeader CreateDefaultHeader(Boolean placeholders)
        {
            return ConfigSettings.CreateDefaultHeader(null, placeholders);
        }

        /// <summary>
        /// This method creates the default header to be used in a configuration output. 
        /// Further, the created header includes an additional title as well as additional 
        /// lines with placeholders for the file name and the file's creation date.
        /// </summary>
        /// <remarks>
        /// Please be aware, no additional comment line is created if provided title is null, 
        /// or empty, or contains whitespaces only. Also keep in mind, no additional comment 
        /// lines are created with a disabled placeholder creation.
        /// </remarks>
        /// <param name="title">
        /// The title to be used.
        /// </param>
        /// <param name="placeholders">
        /// True to enable a usage of placeholders and false to disable it.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigHeader"/> containing a list of comments 
        /// representing the header content.
        /// </returns>
        public static ConfigHeader CreateDefaultHeader(String title, Boolean placeholders)
        {
            ConfigHeader header = new ConfigHeader();

            Char c1 = ConfigDefines.CommentMarkers[0];
            Char c2 = ConfigDefines.CommentMarkers[1];

            Char s1 = ConfigDefines.SectionPrefix;
            Char s2 = ConfigDefines.SectionSuffix;

            Char v1 = ConfigDefines.ValueMarkers[0];
            Char v2 = ConfigDefines.ValueMarkers[1];

            header.Append($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            if (!String.IsNullOrWhiteSpace(title))
            {
                header.Append($"{title.Trim()}");
            }

            if (placeholders)
            {
                header.Append($"File name: {ConfigDefines.FileNamePlaceholder}");
                header.Append($"File date: {ConfigDefines.FileDatePlaceholder}");
            }

            if (header.Count > 1)
            {
                header.Append($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }

            header.Append($"Header rules:");
            header.Append($"- Each header line must start with a comment marker.");
            header.Append($"- Each of the header lines must be a pure comment line.");
            header.Append($"- Each header comment line must be in front on any other content.");
            header.Append($"Comment Rules:");
            header.Append($"- Comments can be tagged by character '{c1}' or by character '{c2}'.");
            header.Append($"- Comments can be placed in a single line but only as header type.");
            header.Append($"- Comments can be placed at the end of line of each section.");
            header.Append($"- Comments can be placed at the end of line of each value-data-pair.");
            header.Append($"Section Rules:");
            header.Append($"- Sections are enclosed in '{s1}' and '{s2}'.");
            header.Append($"- Section names should not include white spaces.");
            header.Append($"Value Rules:");
            header.Append($"- Values can have an empty data part.");
            header.Append($"- Value names should not include white spaces.");
            header.Append($"- Values without a section are treated as 'others'.");
            header.Append($"- Values are built as pair of 'name{v1}data' or of 'name{v2}data'.");
            header.Append($"- Value data that use '{c1}', '{c2}', '{s1}', '{s2}', '{v1}' or '{v2}' must be enclosed by '{ConfigDefines.StringMarker}'.");

            header.Append($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            return header;
        }

        #endregion
    }
}
