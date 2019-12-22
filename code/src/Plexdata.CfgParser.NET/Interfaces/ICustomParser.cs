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
using System.Globalization;

namespace Plexdata.CfgParser.Interfaces
{
    /// <summary>
    /// An interface representing a user-defined custom type parser.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface provides all functionality needed to perform a parsing of 
    /// custom types. An implementation of this interface is a user task but is 
    /// used while parsing a configuration.
    /// </para>
    /// A class derived from this interface is applied as type argument of attribute 
    /// <see cref="Plexdata.CfgParser.Attributes.CustomParserAttribute"/>. Be aware, 
    /// the interface implementation must provide a default (parameterless) constructor!
    /// <para>
    /// </para>
    /// </remarks>
    /// <typeparam name="TType">
    /// </typeparam>
    /// <seealso cref="Plexdata.CfgParser.Processors.ConfigParser{TInstance}"/>
    /// <seealso cref="Plexdata.CfgParser.Attributes.CustomParserAttribute"/>.
    public interface ICustomParser<TType>
    {
        /// <summary>
        /// Parses provided <paramref name="value"/> and returns an instance 
        /// of resulting custom type of <typeparamref name="TType"/>. <i>From 
        /// string into type for reading</i>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method parses provided <paramref name="value"/> and returns 
        /// an instance of resulting custom type of <typeparamref name="TType"/>.
        /// </para>
        /// <para>
        /// To clarify the usage, this method is called by the configuration parser 
        /// <see cref="Plexdata.CfgParser.Processors.ConfigParser{TInstance}"/> as 
        /// soon as the reading of an external configuration takes place. In other 
        /// words, reading of a string value from a configuration and converting it 
        /// into the type of a particular configuration property.
        /// </para>
        /// </remarks>
        /// <param name="label">
        /// The `label` of the configuration value. This can be either the property name 
        /// or the <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute.Label"/> 
        /// of attribute <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute"/>.
        /// </param>
        /// <param name="value">
        /// The `value` of the configuration property for which loading is currently in 
        /// progress.
        /// </param>
        /// <param name="fallback">
        /// The `fallback` of a configuration value just represents the value of property 
        /// <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute.Default"/> of 
        /// attribute <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute"/>.
        /// </param>
        /// <param name="culture">
        /// The `culture` to be used for value conversion.
        /// </param>
        /// <returns>
        /// An instance of the resulting user-defined type that will be assigned to the 
        /// corresponding property. A value of <c>null</c> might be returned, or if wanted, 
        /// the result of an instance representing the `fallback` value.
        /// </returns>
        TType Parse(String label, String value, Object fallback, CultureInfo culture);

        /// <summary>
        /// Parses provided <paramref name="value"/> and returns a string representing 
        /// the value´s content. <i>From type into string for writing</i>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method parses provided <paramref name="value"/> and returns a string 
        /// representing the value´s content.
        /// </para>
        /// <para>
        /// To clarify the usage, this method is called by the configuration parser 
        /// <see cref="Plexdata.CfgParser.Processors.ConfigParser{TInstance}"/> as 
        /// soon as the writing of an external configuration takes place. In other 
        /// words, converting the type of a particular configuration property and 
        /// writing it into a configuration as string.
        /// </para>
        /// </remarks>
        /// <param name="label">
        /// The `label` of the configuration value. This can be either the property name 
        /// or the <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute.Label"/> 
        /// of attribute <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute"/>.
        /// </param>
        /// <param name="value">
        /// The `value` of the configuration property for which saving is currently in 
        /// progress.
        /// </param>
        /// <param name="fallback">
        /// The `fallback` of a configuration value just represents the value of property 
        /// <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute.Default"/> of 
        /// attribute <see cref="Plexdata.CfgParser.Attributes.ConfigValueAttribute"/>.
        /// </param>
        /// <param name="culture">
        /// The `culture` to be used for value conversion.
        /// </param>
        /// <returns>
        /// A string representing the content of the corresponding property.
        /// </returns>
        String Parse(String label, TType value, Object fallback, CultureInfo culture);
    }
}
