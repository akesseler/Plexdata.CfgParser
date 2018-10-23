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
    /// This interface represents the contract to be fulfilled by each configuration entity.
    /// </summary>
    /// <remarks>
    /// This interface is used for configuration entities that have to provide output functionality.
    /// </remarks>
    /// <seealso cref="IConfigItem"/>
    public interface IConfigEntity : IConfigItem
    {
        /// <summary>
        /// Allows to convert an entity's data into its output format.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to generate a proper output strings.
        /// </remarks>
        /// <returns>
        /// A string representing an entity's output format.
        /// </returns>
        String ToOutput();
    }
}
