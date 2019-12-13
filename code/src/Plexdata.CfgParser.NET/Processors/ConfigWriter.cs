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
using System.IO;
using System.Text;

namespace Plexdata.CfgParser.Processors
{
    /// <summary>
    /// The pure static class allows writing a particular configuration file.
    /// </summary>
    /// <remarks>
    /// Writing a configuration takes place by calling method <c>ToOutput()</c> of each instance 
    /// that are assigned to provided instance of class <see cref="ConfigContent"/>.
    /// </remarks>
    public static class ConfigWriter
    {
        #region Public methods

        /// <summary>
        /// This method saves the <paramref name="content"/> into file referenced by 
        /// <paramref name="filename"/>.
        /// </summary>
        /// <remarks>
        /// The file is written using UTF8 encoding. Be aware, the file to write must not 
        /// exist. Otherwise and exception is thrown.
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="filename">
        /// The fully qualified target filename.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown either if the filename is null, empty or whitespaces 
        /// or if overwrite mode is disabled and the target file already exist.
        /// </exception>
        /// <seealso cref="ConfigWriter.Write(ConfigContent, String, Encoding, Boolean)"/>
        public static void Write(ConfigContent content, String filename)
        {
            ConfigWriter.Write(content, filename, Encoding.UTF8, false);
        }

        /// <summary>
        /// This method saves the <paramref name="content"/> into file referenced by 
        /// <paramref name="filename"/> using <paramref name="overwrite"/> mode.
        /// </summary>
        /// <remarks>
        /// The file is written using UTF8 encoding. An already existing file can be 
        /// overwritten by enabling the <paramref name="overwrite"/> mode.
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="filename">
        /// The fully qualified target filename.
        /// </param>
        /// <param name="overwrite">
        /// True to enable overwrite mode and false to disable it.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown either if the filename is null, empty or whitespaces 
        /// or if overwrite mode is disabled and the target file already exist.
        /// </exception>
        /// <seealso cref="ConfigWriter.Write(ConfigContent, String, Encoding, Boolean)"/>
        public static void Write(ConfigContent content, String filename, Boolean overwrite)
        {
            ConfigWriter.Write(content, filename, Encoding.UTF8, overwrite);
        }

        /// <summary>
        /// This method saves the <paramref name="content"/> into file referenced by 
        /// <paramref name="filename"/> using the <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// Be aware, the file to write must not exist. Otherwise and exception is thrown.
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="filename">
        /// The fully qualified target filename.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown either if the filename is null, empty or whitespaces 
        /// or if overwrite mode is disabled and the target file already exist.
        /// </exception>
        /// <seealso cref="ConfigWriter.Write(ConfigContent, String, Encoding, Boolean)"/>
        public static void Write(ConfigContent content, String filename, Encoding encoding)
        {
            ConfigWriter.Write(content, filename, encoding, false);
        }

        /// <summary>
        /// This method saves the <paramref name="content"/> into file referenced by 
        /// <paramref name="filename"/> using the <paramref name="encoding"/> as well 
        /// as the <paramref name="overwrite"/> mode.
        /// </summary>
        /// <remarks>
        /// An existing file can be overwritten by enabling the <paramref name="overwrite"/> 
        /// mode.
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="filename">
        /// The fully qualified target filename.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used.
        /// </param>
        /// <param name="overwrite">
        /// True to enable overwrite mode and false to disable it.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown either if the filename is null, empty or whitespaces 
        /// or if overwrite mode is disabled and the target file already exist.
        /// </exception>
        /// <seealso cref="ConfigWriter.Write(ConfigContent, Stream, Encoding)"/>
        public static void Write(ConfigContent content, String filename, Encoding encoding, Boolean overwrite)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("File name must not be null or empty or consists only of white spaces.");
            }

            if (!overwrite && File.Exists(filename))
            {
                throw new ArgumentException("Unable to write into an existing file with disabled overwrite mode.");
            }

            content.Filename = filename;

            using (Stream stream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                ConfigWriter.Write(content, stream, encoding);
            }
        }

        /// <summary>
        /// This method saves the <paramref name="content"/> into the referenced 
        /// <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// The content is written using UTF8 encoding. 
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="stream">
        /// The stream to write into.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when provided stream is <c>null</c>.
        /// </exception>
        /// <seealso cref="Write(ConfigContent, Stream, Encoding)"/>
        public static void Write(ConfigContent content, Stream stream)
        {
            ConfigWriter.Write(content, stream, Encoding.UTF8);
        }

        /// <summary>
        /// This method saves the <paramref name="content"/> into the referenced 
        /// <paramref name="stream"/> using <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// This method uses an instance of <see cref="TextWriter"/> with provided 
        /// <paramref name="encoding"/> to write the actual output.
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="stream">
        /// The stream to write into.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown when provided stream is <c>null</c>.
        /// </exception>
        /// <seealso cref="Write(ConfigContent, TextWriter)"/>
        public static void Write(ConfigContent content, Stream stream, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "Stream to write into must not be null.");
            }

            using (TextWriter writer = new StreamWriter(stream, encoding))
            {
                ConfigWriter.Write(content, writer);
            }
        }

        /// <summary>
        /// This method saves the <paramref name="content"/> into the referenced 
        /// <paramref name="writer"/>.
        /// </summary>
        /// <remarks>
        /// The content is written after all parameters could be confirmed as valid.
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="writer">
        /// The instance of class <see cref="TextWriter"/> to perform all writing operations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if the  <paramref name="content"/> is <c>null</c> 
        /// or the <paramref name="writer"/> is <c>null</c>.
        /// </exception>
        public static void Write(ConfigContent content, TextWriter writer)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "The content to write must not be null.");
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer), "Text writer must not be null.");
            }

            ConfigWriter.Process(content, writer);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method save provided <paramref name="content"/> using the 
        /// <paramref name="writer"/>.
        /// </summary>
        /// <remarks>
        /// Beside writing the actual data, this method also replaces all placeholders 
        /// in the header by its real data.
        /// </remarks>
        /// <param name="content">
        /// The content to save.
        /// </param>
        /// <param name="writer">
        /// The instance of class <see cref="TextWriter"/> to perform all writing operations.
        /// </param>
        private static void Process(ConfigContent content, TextWriter writer)
        {
            String filename = "unused";
            String filedate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!String.IsNullOrWhiteSpace(content.Filename))
            {
                filename = Path.GetFileName(content.Filename);
            }

            foreach (String output in content.ToOutput())
            {
                writer.WriteLine(output.Replace(ConfigDefines.FileNamePlaceholder, filename).Replace(ConfigDefines.FileDatePlaceholder, filedate));
            }

            writer.Flush();
        }

        #endregion
    }
}
