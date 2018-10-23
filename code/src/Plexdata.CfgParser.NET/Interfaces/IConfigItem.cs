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

using System;

namespace Plexdata.CfgParser.Interfaces
{
    /// <summary>
    /// This interface represents the contract to be fulfilled by each configuration item.
    /// </summary>
    /// <remarks>
    /// This interface is used for configuration entities that have to provide validation functionality.
    /// </remarks>
    public interface IConfigItem
    {
        /// <summary>
        /// Allows to determine whether a configuration item is valid or not.
        /// </summary>
        /// <remarks>
        /// In derived classes this property enables to determine the valid state of a 
        /// configuration item.
        /// </remarks>
        /// <value>
        /// True indicates that a configuration item can be considered as valid 
        /// and false if not.
        /// </value>
        Boolean IsValid { get; }
    }
}
