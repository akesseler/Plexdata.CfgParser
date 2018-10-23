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
using System.Collections.Generic;

namespace Plexdata.CfgParser.Interfaces
{
    /// <summary>
    /// This interface represents the contract to be fulfilled by each configuration item 
    /// that includes enumerable child items.
    /// </summary>
    /// <remarks>
    /// This interface is used for configuration items that have to provide a list of items.
    /// </remarks>
    /// <typeparam name="TType">
    /// The generic type of the child items.
    /// </typeparam>
    public interface IConfigArray<TType> : IConfigItem where TType : IConfigItem
    {
        /// <summary>
        /// Gets or sets the item value at the specified index.
        /// </summary>
        /// <remarks>
        /// In derived classes this array accessor gets or sets an instance of 
        /// affected class.
        /// </remarks>
        /// <param name="index">
        /// The zero-based index of the item value to get or set.
        /// </param>
        /// <value>
        /// An instance of class <typeparamref name="TType"/>.
        /// </value>
        /// <returns>
        /// The item value at the specified index.
        /// </returns>
        TType this[Int32 index] { get; set; }

        /// <summary>
        /// Gets or sets the item value for the specified key.
        /// </summary>
        /// <remarks>
        /// In derived classes this array accessor gets or sets an instance of affected 
        /// class.
        /// </remarks>
        /// <param name="key">
        /// The key of the item value to get or set.
        /// </param>
        /// <value>
        /// An instance of class <typeparamref name="TType"/>.
        /// </value>
        /// <returns>
        /// The item value for the specified key.
        /// </returns>
        TType this[String key] { get; set; }

        /// <summary>
        /// Determines the number of available child items.
        /// </summary>
        /// <remarks>
        /// In derived classes this property returns the number of available child items.
        /// </remarks>
        /// <value>
        /// The number of available child items.
        /// </value>
        Int32 Count { get; }

        /// <summary>
        /// This method removes all currently assigned item values.
        /// </summary>
        /// <remarks>
        /// In derived classes this method just removes all available item values.
        /// </remarks>
        void Clear();

        /// <summary>
        /// This method tries to find an item for a provided value string.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to search for an item for a 
        /// provided value string.
        /// </remarks>
        /// <param name="value">
        /// The value string to search for.
        /// </param>
        /// <returns>
        /// The found item instance.
        /// </returns>
        TType Find(String value);

        /// <summary>
        /// This method appends a new instance of an item and returns it.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to append new item instances.
        /// </remarks>
        /// <param name="value">
        /// The initial descriptor of the item to be appended.
        /// </param>
        /// <returns>
        /// The newly appended item instance.
        /// </returns>
        TType Append(String value);

        /// <summary>
        /// This method appends provided instance of an item and returns it.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to append provided item instances.
        /// </remarks>
        /// <param name="entity">
        /// The instance of an item to be appended.
        /// </param>
        /// <returns>
        /// The provided instance of the appended item.
        /// </returns>
        TType Append(TType entity);

        /// <summary>
        /// This method inserts a new instance of an item and returns it.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to insert new item instances.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the new item.
        /// </param>
        /// <param name="value">
        /// The initial descriptor of the item to be appended.
        /// </param>
        /// <returns>
        /// The newly inserted item instance.
        /// </returns>
        TType Insert(Int32 index, String value);

        /// <summary>
        /// This method inserts provided instance of an item and returns it.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to insert provided item instances.
        /// </remarks>
        /// <param name="index">
        /// The index where to insert the provided item.
        /// </param>
        /// <param name="entity">
        /// The instance of an item to be inserted.
        /// </param>
        /// <returns>
        /// The provided instance of the inserted item.
        /// </returns>
        TType Insert(Int32 index, TType entity);

        /// <summary>
        /// This method prepends a new instance of an item and returns it.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to prepend new item instances.
        /// </remarks>
        /// <param name="value">
        /// The initial descriptor of the item to be prepended.
        /// </param>
        /// <returns>
        /// The newly prepended item instance.
        /// </returns>
        TType Prepend(String value);

        /// <summary>
        /// This method prepends provided instance of an item and returns it.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to prepend provided item instances.
        /// </remarks>
        /// <param name="entity">
        /// The instance of an item to be prepended.
        /// </param>
        /// <returns>
        /// The provided instance of the prepended item.
        /// </returns>
        TType Prepend(TType entity);

        /// <summary>
        /// This method tries to find an item and removes it from the child list. 
        /// Further, the removed item is returned.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to remove fitting child items.
        /// </remarks>
        /// <param name="value">
        /// The descriptor of the item to be removed.
        /// </param>
        /// <returns>
        /// The instance of the removed item.
        /// </returns>
        TType Remove(String value);

        /// <summary>
        /// This method tries to remove an item from the child list. Further, the 
        /// removed item is returned.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to remove child items by index.
        /// </remarks>
        /// <param name="index">
        /// The index where to remove the item.
        /// </param>
        /// <returns>
        /// The instance of the removed item.
        /// </returns>
        TType Remove(Int32 index);

        /// <summary>
        /// Allows to convert an entity's data into its enumerable output format.
        /// </summary>
        /// <remarks>
        /// In derived classes this method is responsible to generate a list of proper 
        /// output strings.
        /// </remarks>
        /// <returns>
        /// An enumerable list of strings representing an entity's output format.
        /// </returns>
        IEnumerable<String> ToOutput();
    }
}
