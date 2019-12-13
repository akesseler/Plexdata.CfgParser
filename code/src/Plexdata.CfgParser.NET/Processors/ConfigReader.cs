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

using Plexdata.CfgParser.Entities;
using Plexdata.CfgParser.Extensions;
using Plexdata.CfgParser.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plexdata.CfgParser.Processors
{
    /// <summary>
    /// The pure static class allows reading a particular configuration file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Configuration files are files that contain a relation of value-data-pairs. Furthermore, 
    /// each of these value-data-pairs is assigned to its parent section. For sure, such files are 
    /// also known as INI files.
    /// </para>
    /// <para>
    /// How file data processing actually works should become a bit more clear with the following 
    /// details. But beforehand note that each of the blank lines will be ignored.
    /// </para>
    /// <list type="bullet">
    /// <item><description>
    /// First of all the whole file content is read line by line.
    /// </description></item>
    /// <item><description>
    /// As next the file header will be extracted, but only if it exists. The result is put into 
    /// the property <see cref="ConfigContent.Header"/>. Note, each comment line at the beginning 
    /// of the source file, which is not broken by a blank line, a section and so forth, is considered 
    /// as part of the file header.
    /// </description></item>
    /// <item><description>
    /// After processing the file header, it is tried to find the first section. A section is a text 
    /// label that is surrounded by square brackets (<c>[</c> and <c>]</c>). This in turn means that 
    /// a section label is any content between the square brackets. Hence, this includes white spaces 
    /// as well. But section labels should neither include any of the value markers nor any of the 
    /// comment markers.
    /// </description></item>
    /// <item><description>
    /// As soon as a section label could be determined, each of the following value-data-pairs will 
    /// be assigned to this section. This assignment takes place until the next section is found or 
    /// the file end is reached.
    /// </description></item>
    /// </list>
    /// <para>
    /// Now it seems to be time to clarify some of the exceptions.
    /// </para>
    /// <list type="bullet">
    /// <item><description>
    /// Each pure comment line that could not be assigned to the file header causes an entry in the 
    /// warnings list, no matter where such a line is placed! 
    /// </description></item>
    /// <item><description>
    /// Each value-data-pair that could not be assigned to a section causes an entry in the warnings 
    /// list! This especially is related to such lines that are put between the file header and the 
    /// first section.
    /// </description></item>
    /// <item><description>
    /// Each item that is neither a pure comment line nor a section nor a value-data-pairs is treated 
    /// as "something else"! Such items are put into the property <see cref="ConfigContent.Others"/>.
    /// </description></item>
    /// </list>
    /// </remarks>
    public static class ConfigReader
    {
        #region Public methods

        /// <summary>
        /// This method tries to read all data from provided file using UTF8 encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ConfigReader.Read(String, Encoding, IList{ConfigWarning})"/> 
        /// with UTF8 encoding and <c>null</c> for the list of warnings.
        /// </remarks>
        /// <param name="filename">
        /// The fully qualified file name to read data from.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file 
        /// data.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided file name is null or empty or only includes white 
        /// spaces.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(String, Encoding, IList{ConfigWarning})"/>
        public static ConfigContent Read(String filename)
        {
            return ConfigReader.Read(filename, Encoding.UTF8, null);
        }

        /// <summary>
        /// This method tries to read all data from provided file using UTF8 encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ConfigReader.Read(String, Encoding, IList{ConfigWarning})"/> 
        /// with UTF8 encoding.
        /// </remarks>
        /// <param name="filename">
        /// The fully qualified file name to read data from.
        /// </param>
        /// <param name="warnings">
        /// A reference to a list that might contain warnings that have occurred during file 
        /// processing.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file 
        /// data.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided file name is null or empty or only includes white 
        /// spaces.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(String, Encoding, IList{ConfigWarning})"/>
        public static ConfigContent Read(String filename, IList<ConfigWarning> warnings)
        {
            return ConfigReader.Read(filename, Encoding.UTF8, warnings);
        }

        /// <summary>
        /// This method tries to read all data from provided file using provided encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ConfigReader.Read(String, Encoding, IList{ConfigWarning})"/> 
        /// with <c>null</c> for the list of warnings.
        /// </remarks>
        /// <param name="filename">
        /// The fully qualified file name to read data from.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used to read all file data.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided file name is null or empty or only includes white spaces.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(String, Encoding, IList{ConfigWarning})"/>
        public static ConfigContent Read(String filename, Encoding encoding)
        {
            return ConfigReader.Read(filename, encoding, null);
        }

        /// <summary>
        /// This method tries to read all data from provided file using provided encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method tries to open a file stream for read and then it calls the method 
        /// <see cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/> using all 
        /// provided parameters.
        /// </remarks>
        /// <param name="filename">
        /// The fully qualified file name to read data from.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used to read all file data.
        /// </param>
        /// <param name="warnings">
        /// A reference to a list that might contain warnings that have occurred during file 
        /// processing.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided file name is null or empty or only includes white spaces.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/>
        public static ConfigContent Read(String filename, Encoding encoding, IList<ConfigWarning> warnings)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("File name must not be null or empty or consists only of white spaces.");
            }

            using (Stream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ConfigReader.Read(stream, encoding, warnings);
            }
        }

        /// <summary>
        /// This method tries to read all data from provided stream using UTF8 encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/> 
        /// with UTF8 encoding and <c>null</c> for the list of warnings.
        /// </remarks>
        /// <param name="stream">
        /// The stream to read data from.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided stream is null.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/>
        public static ConfigContent Read(Stream stream)
        {
            return ConfigReader.Read(stream, Encoding.UTF8, null);
        }

        /// <summary>
        /// This method tries to read all data from provided stream using UTF8 encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/> 
        /// with UTF8 encoding.
        /// </remarks>
        /// <param name="stream">
        /// The stream to read data from.
        /// </param>
        /// <param name="warnings">
        /// A reference to a list that might contain warnings that have occurred during file 
        /// processing.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided stream is null.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/>
        public static ConfigContent Read(Stream stream, IList<ConfigWarning> warnings)
        {
            return ConfigReader.Read(stream, Encoding.UTF8, warnings);
        }

        /// <summary>
        /// This method tries to read all data from provided stream using provided encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/> 
        /// with <c>null</c> for the list of warnings.
        /// </remarks>
        /// <param name="stream">
        /// The stream to read data from.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used to read all stream data.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided stream is null.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(Stream, Encoding, IList{ConfigWarning})"/>
        public static ConfigContent Read(Stream stream, Encoding encoding)
        {
            return ConfigReader.Read(stream, encoding, null);
        }

        /// <summary>
        /// This method tries to read all data from provided stream using provided encoding and presents 
        /// the result in its return value.
        /// </summary>
        /// <remarks>
        /// This method tries to create a text reader for provided stream and then it calls method
        /// <see cref="ConfigReader.Read(TextReader, IList{ConfigWarning})"/> using all provided 
        /// parameters.
        /// </remarks>
        /// <param name="stream">
        /// The stream to read data from.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used to read all stream data.
        /// </param>
        /// <param name="warnings">
        /// A reference to a list that might contain warnings that have occurred during file 
        /// processing.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided stream is null.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(TextReader, IList{ConfigWarning})"/>
        public static ConfigContent Read(Stream stream, Encoding encoding, IList<ConfigWarning> warnings)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "Stream to read from must not be null.");
            }

            using (TextReader reader = new StreamReader(stream, encoding))
            {
                return ConfigReader.Read(reader, warnings);
            }
        }

        /// <summary>
        /// This method tries to read all data from provided text reader and presents the result in 
        /// its return value.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ConfigReader.Read(TextReader, IList{ConfigWarning})"/> with 
        /// <c>null</c> for the list of warnings.
        /// </remarks>
        /// <param name="reader">
        /// The text reader to read all data from.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided text reader is null.
        /// </exception>
        /// <seealso cref="ConfigReader.Read(TextReader, IList{ConfigWarning})"/>
        public static ConfigContent Read(TextReader reader)
        {
            return ConfigReader.Read(reader, null);
        }

        /// <summary>
        /// This method tries to read all data from provided text reader and presents the result in 
        /// its return value.
        /// </summary>
        /// <remarks>
        /// This method initiates the data processing by calling the internal processing method.
        /// </remarks>
        /// <param name="reader">
        /// The text reader to read all data from.
        /// </param>
        /// <param name="warnings">
        /// A reference to a list that might contain warnings that have occurred during file 
        /// processing.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided text reader is null.
        /// </exception>
        /// <seealso cref="ConfigReader.Process(TextReader, IList{ConfigWarning})"/>
        public static ConfigContent Read(TextReader reader, IList<ConfigWarning> warnings)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader), "Text reader must not be null.");
            }

            return ConfigReader.Process(reader, warnings);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method tries to process each single line of provided text reader and converts 
        /// them into instances of <see cref="IConfigItem"/> derived classes accordingly.
        /// </summary>
        /// <remarks>
        /// This method actually reads the whole content and processes each of the available 
        /// lines.
        /// </remarks>
        /// <param name="reader">
        /// The text reader to read all data from.
        /// </param>
        /// <param name="warnings">
        /// A reference to a list that might contain warnings that have occurred during file 
        /// processing.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/> that contains all processed file data.
        /// </returns>
        private static ConfigContent Process(TextReader reader, IList<ConfigWarning> warnings)
        {
            ConfigContent result = new ConfigContent();
            ConfigSection section = null;

            List<String> lines = ConfigReader.ReadLines(reader);

            Int32 offset = ConfigReader.ProcessHeader(lines, result);

            for (Int32 index = offset; index < lines.Count; index++)
            {
                String line = lines[index];

                if (line.IsHollow())
                {
                    continue;
                }

                if (line.IsComment())
                {
                    if (warnings != null)
                    {
                        warnings.Add(new ConfigWarning(index + 1, line, "Misplaced comment"));
                    }

                    continue;
                }

                if (line.IsSection())
                {
                    section = result.Append(ConfigSection.Parse(line));
                    continue;
                }

                if (line.IsValue())
                {
                    if (section == null)
                    {
                        if (warnings != null)
                        {
                            warnings.Add(new ConfigWarning(index + 1, line, "Misplaced value"));
                        }

                        continue;
                    }

                    section.Append(ConfigValue.Parse(line));
                    continue;
                }

                result.Others.Append(line);
            }

            if (reader is StreamReader)
            {
                if ((reader as StreamReader).BaseStream is FileStream)
                {
                    result.Filename = ((reader as StreamReader).BaseStream as FileStream).Name;
                }
            }

            return result;
        }

        /// <summary>
        /// This method tries to process the configuration header.
        /// </summary>
        /// <remarks>
        /// Each leading blank line is skipped. Skipping takes place until the first pure comment 
        /// line has been found. Then each pure comment line is assigned to the header. This is 
        /// repeated until the next non-comment line occurs. 
        /// </remarks>
        /// <param name="lines">
        /// The list of strings representing the whole configuration content.
        /// </param>
        /// <param name="content">
        /// The configuration content where header lines should be assigned to.
        /// </param>
        /// <returns>
        /// Returns the first line behind the header.
        /// </returns>
        private static Int32 ProcessHeader(List<String> lines, ConfigContent content)
        {
            for (Int32 index = 0; index < lines.Count; index++)
            {
                String line = lines[index];

                if (line.IsHollow())
                {
                    if (content.Header.Count > 0)
                    {
                        return index;
                    }

                    continue;
                }

                if (line.IsComment())
                {
                    content.Header.Append(ConfigComment.Parse(line));

                    continue;
                }

                return index;
            }

            return 0;
        }

        /// <summary>
        /// This method reads all lines from provided source.
        /// </summary>
        /// <remarks>
        /// Each of the read lines is taken as it is. No trimming or other manipulation 
        /// takes place. The method does not stop until the end of file has been reached.
        /// </remarks>
        /// <param name="reader">
        /// The source to read all lines from.
        /// </param>
        /// <returns>
        /// Returns the list of read lines.
        /// </returns>
        private static List<String> ReadLines(TextReader reader)
        {
            List<String> lines = new List<String>();
            String line = null;

            do
            {
                line = reader.ReadLine();

                if (line != null)
                {
                    lines.Add(line);
                }
            }
            while (line != null);

            return lines;
        }

        #endregion
    }
}
