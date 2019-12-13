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

using System;

namespace Plexdata.CfgParser.Settings
{
    /// <summary>
    /// This class represents the base class for all pre-configured configuration 
    /// settings classes.
    /// </summary>
    /// <remarks>
    /// Users may derive their own classes and configure them according to their 
    /// needs.
    /// </remarks>
    public abstract class ConfigSettingsBase
    {
        #region Construction

        /// <summary>
        /// The constructor that configures value and comment markers to be 
        /// used with an instance of a class derived from this base class.
        /// </summary>
        /// <remarks>
        /// Derived classes call this constructor and provide their default 
        /// markers.
        /// </remarks>
        /// <param name="defaultValueMarker">
        /// The value marker to be used as default value marker.
        /// </param>
        /// <param name="defaultCommentMarker">
        /// The comment marker to be used as default comment marker.
        /// </param>
        protected ConfigSettingsBase(Char defaultValueMarker, Char defaultCommentMarker)
            : base()
        {
            this.DefaultValueMarker = defaultValueMarker;
            this.DefaultCommentMarker = defaultCommentMarker;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Returns the default value marker to be used.
        /// </summary>
        /// <remarks>
        /// The default value marker is set by derived classes.
        /// </remarks>
        /// <value>
        /// The character representing the default value marker.
        /// </value>
        public Char DefaultValueMarker { get; protected set; }

        /// <summary>
        /// Returns the default comment marker to be used.
        /// </summary>
        /// <remarks>
        /// The default comment marker is set by derived classes.
        /// </remarks>
        /// <value>
        /// The character representing the default comment marker.
        /// </value>
        public Char DefaultCommentMarker { get; protected set; }

        #endregion
    }
}
