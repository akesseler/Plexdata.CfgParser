﻿/*
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

using Plexdata.CfgParser.Converters;
using Plexdata.CfgParser.Entities;
using Plexdata.CfgParser.Internals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Plexdata.CfgParser.Processors
{
    /// <summary>
    /// This class assigns values of a configuration file to its corresponding properties.
    /// </summary>
    /// <remarks>
    /// This class tries to assign the values of a configuration file to its corresponding 
    /// properties of an instance of type <typeparamref name="TInstance"/> and vice versa.
    /// </remarks>
    /// <typeparam name="TInstance">
    /// The type to be parsed by this class.
    /// </typeparam>
    public static class ConfigParser<TInstance> where TInstance : class
    {
        // TODO: Finalize documentation.

        #region Public methods

        /// <summary>
        /// This method creates a new instance of type <typeparamref name="TInstance"/> 
        /// and assigns the values of provided configuration to its properties.
        /// </summary>
        /// <remarks>
        /// The parsing takes place by using current UI culture.
        /// </remarks>
        /// <param name="content">
        /// The configuration content from which to assign sections and values.
        /// </param>
        /// <returns>
        /// An instance of type <typeparamref name="TInstance"/>.
        /// </returns>
        /// <seealso cref="ConfigParser{TInstance}.Parse(ConfigContent, CultureInfo)"/>
        public static TInstance Parse(ConfigContent content)
        {
            return ConfigParser<TInstance>.Parse(content, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// This method creates a new instance of type <typeparamref name="TInstance"/> 
        /// and assigns the values of provided configuration to its properties using provided 
        /// culture.
        /// </summary>
        /// <remarks>
        /// It is strictly recommended to surround a call to this method by a <c>try ... catch</c> 
        /// block.
        /// </remarks>
        /// <param name="content">
        /// The configuration content from which to assign sections and values.
        /// </param>
        /// <param name="culture">
        /// The culture to be used for value type conversion.
        /// </param>
        /// <returns>
        /// An instance of type <typeparamref name="TInstance"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if <paramref name="content"/> or <paramref name="culture"/> 
        /// is <c>null</c>.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception might be possible.
        /// </exception>
        public static TInstance Parse(ConfigContent content, CultureInfo culture)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "Configuration content must not be null.");
            }

            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture), "Provided culture must not be null.");
            }

            TInstance result = ConfigParser<TInstance>.ConstructInstance();

            foreach (SectionDescriptor current in DescriptorParser<TInstance>.ParseSections())
            {
                ConfigSection section = ConfigParser<TInstance>.TryFindConfigSection(current, content);

                if (section == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Section \"{current.Descriptor}\" not found.");
                    continue;
                }

                ConfigParser<TInstance>.ParseSection(result, current, section, culture);
            }

            return result;
        }

        /// <summary>
        /// This method creates a new instance of class <see cref="ConfigContent"/> and 
        /// assigns the values of provided instance of type <typeparamref name="TInstance"/> 
        /// accordingly.
        /// </summary>
        /// <remarks>
        /// The parsing takes place by using current UI culture.
        /// </remarks>
        /// <param name="instance">
        /// An instance of type <typeparamref name="TInstance"/>.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/>.
        /// </returns>
        /// <seealso cref="ConfigParser{TInstance}.Parse(TInstance, CultureInfo)"/>
        public static ConfigContent Parse(TInstance instance)
        {
            return ConfigParser<TInstance>.Parse(instance, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// This method creates a new instance of class <see cref="ConfigContent"/> and 
        /// assigns the values of provided instance of type <typeparamref name="TInstance"/> 
        /// using provided culture.
        /// </summary>
        /// <remarks>
        /// It is strictly recommended to surround a call to this method by a <c>try ... catch</c> 
        /// block.
        /// </remarks>
        /// <param name="instance">
        /// An instance of type <typeparamref name="TInstance"/>.
        /// </param>
        /// <param name="culture">
        /// The culture to be used for value type conversion.
        /// </param>
        /// <returns>
        /// An instance of class <see cref="ConfigContent"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if <paramref name="instance"/> or <paramref name="culture"/> 
        /// is <c>null</c>.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception might be possible.
        /// </exception>
        public static ConfigContent Parse(TInstance instance, CultureInfo culture)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "Instance must not be null.");
            }

            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture), "Provided culture must not be null.");
            }

            ConfigContent result = new ConfigContent();

            foreach (SectionDescriptor current in DescriptorParser<TInstance>.ParseSections())
            {
                PropertyInfo parent = instance.GetType().GetRuntimeProperty(current.Property.Name);

                if (parent == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Section property for \"{current.Descriptor}\" not found.");
                    continue;
                }

                ConfigSection section = ConfigParser<TInstance>.CreateConfigSection(result, current, parent, instance, out Object source);

                foreach (PropertyInfo child in parent.PropertyType.GetRuntimeProperties())
                {
                    ConfigValue target = ConfigParser<TInstance>.CreateConfigValue(current, source, child, culture);

                    if (target != null)
                    {
                        section.Append(target);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method tries parsing all values for a provided section.
        /// </summary>
        /// <remarks>
        /// The value remains untouched as long as the configuration does 
        /// not contain a value, or if the value type is not supported.
        /// </remarks>
        /// <param name="instance">
        /// An instance of type <typeparamref name="TInstance"/>.
        /// </param>
        /// <param name="parent">
        /// The descriptor of the parent section.
        /// </param>
        /// <param name="section">
        /// The configuration section to be parsed.
        /// </param>
        /// <param name="culture">
        /// The culture to be used for value type conversion.
        /// </param>
        /// <exception cref="Exception">
        /// Any other exception might be possible.
        /// </exception>
        private static void ParseSection(TInstance instance, SectionDescriptor parent, ConfigSection section, CultureInfo culture)
        {
            Object target = ConfigParser<TInstance>.ConstructObject(parent.Property.PropertyType);
            parent.Property.SetValue(instance, target);

            foreach (ValueDescriptor current in parent.Values)
            {
                ConfigValue value = ConfigParser<TInstance>.TryFindConfigValue(current, section);

                if (value == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Value \"{current.Descriptor}\" not found.");
                    continue;
                }

                if (!ValueConverter.IsSupportedType(current.Property.PropertyType))
                {
                    System.Diagnostics.Debug.WriteLine($"Type of value \"{current.Descriptor}\" not supported.");
                    continue;
                }

                if (ValueConverter.TryConvert(value.Value, current.Property.PropertyType, culture, out Object data))
                {
                    current.Property.SetValue(target, data);
                }
            }
        }

        /// <summary>
        /// This method tries to create a new instance of type <typeparamref name="TInstance"/> 
        /// and returns it.
        /// </summary>
        /// <remarks>
        /// The type <typeparamref name="TInstance"/> must support a public default constructor. 
        /// Otherwise an instance construction is impossible.
        /// </remarks>
        /// <returns>
        /// An instance of type <typeparamref name="TInstance"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// This exception is thrown either if no public default constructor could be found or 
        /// if constructing has caused an exception.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception might be possible.
        /// </exception>
        private static TInstance ConstructInstance()
        {
            ConstructorInfo ctor = typeof(TInstance).GetConstructor(new Type[] { });

            if (ctor == null)
            {
                throw new InvalidOperationException($"Type \"{typeof(TInstance).Name}\" does not contain a default constructor.");
            }

            try
            {
                // According to docs: Either an exceptions is thrown or an instance is returned (but not null)...
                return (TInstance)ctor.Invoke(new Object[] { });
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not use standard constructor of type \"{typeof(TInstance).Name}\".", exception);
            }
        }

        /// <summary>
        /// This method tries to create a new instance of type <paramref name="type"/> and 
        /// returns it.
        /// </summary>
        /// <remarks>
        /// The <paramref name="type"/> must support a public default constructor. Otherwise 
        /// an instance construction is impossible.
        /// </remarks>
        /// <param name="type">
        /// The type to create an instance of.
        /// </param>
        /// <returns>
        /// An instance of <paramref name="type"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// This exception is thrown either if no public default constructor could be found or 
        /// if constructing has caused an exception.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception might be possible.
        /// </exception>
        private static Object ConstructObject(Type type)
        {
            ConstructorInfo ctor = type.GetConstructor(new Type[] { });

            if (ctor == null)
            {
                throw new InvalidOperationException($"Type \"{type.Name}\" does not contain a default constructor.");
            }

            try
            {
                // According to docs: Either an exceptions is thrown or an instance is returned (but not null)...
                return ctor.Invoke(new Object[] { });
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not use standard constructor of type \"{type.Name}\".", exception);
            }
        }

        private static ConfigSection CreateConfigSection(ConfigContent content, SectionDescriptor section, PropertyInfo parent, Object source, out Object target)
        {
            ConfigSection result = new ConfigSection(section.Descriptor, section.Attribute.Comment);
            content.Append(result);

            target = parent.GetValue(source);

            return result;
        }

        private static ConfigValue CreateConfigValue(SectionDescriptor section, Object source, PropertyInfo property, CultureInfo culture)
        {
            ValueDescriptor value = ConfigParser<TInstance>.TryFindValueDescriptor(section.Values, property);

            if (value == null)
            {
                return null;
            }

            return new ConfigValue(
                value.Descriptor,
                ConfigParser<TInstance>.ValueToString(source, property, culture),
                value.Attribute.Comment);
        }

        private static String ValueToString(Object source, PropertyInfo property, CultureInfo culture)
        {
            Object result = property.GetValue(source);
            return result == null ? String.Empty : Convert.ToString(result, culture);
        }

        /// <summary>
        /// This method tries to find the section inside the <paramref name="content"/> 
        /// for provided <paramref name="descriptor"/>.
        /// </summary>
        /// <remarks>
        /// The equality comparison takes place by using method 
        /// <see cref="AttributeDescriptor{TAttribute}.Equals(Object)"/>. The details 
        /// for comparison are defined by <see cref="SectionDescriptor.Descriptor"/> 
        /// and <see cref="SectionDescriptor.Comparison"/>. The comparison is done against 
        /// <see cref="ConfigSection.Title"/>.
        /// </remarks>
        /// <param name="descriptor">
        /// The descriptor to find a configuration section for.
        /// </param>
        /// <param name="content">
        /// The content to search for a configuration section.
        /// </param>
        /// <returns>
        /// The corresponding configuration section or <c>null</c> if not found.
        /// </returns>
        private static ConfigSection TryFindConfigSection(SectionDescriptor descriptor, ConfigContent content)
        {
            return content.Sections.Where(x => descriptor.Equals(x.Title)).FirstOrDefault();
        }

        /// <summary>
        /// This method tries to find the value inside the <paramref name="section"/> 
        /// for provided <paramref name="descriptor"/>.
        /// </summary>
        /// <remarks>
        /// The equality comparison takes place by using method 
        /// <see cref="AttributeDescriptor{TAttribute}.Equals(Object)"/>. The details 
        /// for comparison are defined by <see cref="ValueDescriptor.Descriptor"/> 
        /// and <see cref="ValueDescriptor.Comparison"/>. The comparison is done against 
        /// <see cref="ConfigValue.Label"/>.
        /// </remarks>
        /// <param name="descriptor">
        /// The descriptor to find a configuration value for.
        /// </param>
        /// <param name="section">
        /// The section to search for a configuration section.
        /// </param>
        /// <returns>
        /// The corresponding configuration section or <c>null</c> if not found.
        /// </returns>
        private static ConfigValue TryFindConfigValue(ValueDescriptor descriptor, ConfigSection section)
        {
            return section.Values.Where(x => descriptor.Equals(x.Label)).FirstOrDefault();
        }

        private static ValueDescriptor TryFindValueDescriptor(IList<ValueDescriptor> values, PropertyInfo source)
        {
            return values.Where(x => String.Equals(x.Property.Name, source.Name)).FirstOrDefault();
        }

        #endregion
    }
}
