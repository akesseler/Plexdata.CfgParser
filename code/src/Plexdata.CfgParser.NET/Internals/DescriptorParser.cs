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

using Plexdata.CfgParser.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plexdata.CfgParser.Internals
{
    /// <summary>
    /// This internal class parses the generic class for required information.
    /// </summary>
    /// <remarks>
    /// Task of this internal class is to parse the generic class for the 
    /// information required to assign values of a configuration.
    /// </remarks>
    /// <typeparam name="TInstance">
    /// The type to be parsed by this class.
    /// </typeparam>
    internal static class DescriptorParser<TInstance> where TInstance : class
    {
        #region Public methods

        /// <summary>
        /// This method tries to determine the predefined configuration sections.
        /// </summary>
        /// <remarks>
        /// A configuration section is a property that has been tagged by attribute 
        /// <see cref="ConfigSectionAttribute"/>. Additionally, such a property must 
        /// have a public setter and getter. All other properties are ignored.
        /// </remarks>
        /// <returns>
        /// A list of found configuration sections.
        /// </returns>
        public static IEnumerable<SectionDescriptor> ParseSections()
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;

            // According to docs: At least empty but not null...
            PropertyInfo[] properties = typeof(TInstance).GetProperties(flags);

            if (properties == null || !properties.Any())
            {
                return new List<SectionDescriptor>();
            }

            List<SectionDescriptor> sections = new List<SectionDescriptor>();

            foreach (PropertyInfo property in properties)
            {
                // Continue only in case of having both, public getter and public setter!
                if (property.GetGetMethod() == null || property.GetSetMethod() == null)
                {
                    continue;
                }

                // According to docs: At least empty but not null...
                IEnumerable<Attribute> attributes = property.GetCustomAttributes();

                if (attributes == null || !attributes.Any())
                {
                    continue;
                }

                foreach (Attribute current in attributes)
                {
                    if (current is ConfigSectionAttribute)
                    {
                        sections.Add(DescriptorParser<TInstance>.AddValueDescriptors(new SectionDescriptor(current as ConfigSectionAttribute, property)));
                        break;
                    }
                }
            }

            return sections;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method adds all configuration values to its configuration section.
        /// </summary>
        /// <remarks>
        /// This method tries to add all available properties representing a configuration 
        /// value to the provided configuration section. Additionally, such a property must 
        /// have a public setter and getter. All other properties are ignored.
        /// </remarks>
        /// <param name="section">
        /// The descriptor representing the parent section of all found value descriptors.
        /// </param>
        /// <returns>
        /// The provided section descritor.
        /// </returns>
        private static SectionDescriptor AddValueDescriptors(SectionDescriptor section)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;

            // According to docs: At least empty but not null...
            PropertyInfo[] properties = section.Property.PropertyType.GetProperties(flags);

            if (properties == null || !properties.Any())
            {
                return section;
            }

            foreach (PropertyInfo property in properties)
            {
                // Continue only in case of having both, public getter and public setter!
                if (property.GetGetMethod() == null || property.GetSetMethod() == null)
                {
                    continue;
                }

                // According to docs: At least empty but not null...
                IEnumerable<Attribute> attributes = property.GetCustomAttributes();

                if (attributes == null || !attributes.Any())
                {
                    continue;
                }

                foreach (Attribute current in attributes)
                {
                    if (current is ConfigValueAttribute)
                    {
                        section.Values.Add(new ValueDescriptor(current as ConfigValueAttribute, property));
                        break;
                    }
                }
            }

            return section;
        }

        #endregion
    }
}
