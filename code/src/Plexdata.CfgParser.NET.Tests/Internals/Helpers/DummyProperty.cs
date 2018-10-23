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
using System.Globalization;
using System.Reflection;

namespace Plexdata.CfgParser.Tests.Internals.Helpers
{
    public class DummyProperty : PropertyInfo
    {
        private readonly String name;

        public DummyProperty(String name)
            : base()
        {
            this.name = name;
        }

        public override Type PropertyType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override PropertyAttributes Attributes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Boolean CanRead
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Boolean CanWrite
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override String Name
        {
            get
            {
                return this.name;
            }
        }

        public override Type DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Type ReflectedType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MethodInfo[] GetAccessors(Boolean nonPublic)
        {
            throw new NotImplementedException();
        }

        public override Object[] GetCustomAttributes(Boolean inherit)
        {
            throw new NotImplementedException();
        }

        public override Object[] GetCustomAttributes(Type attributeType, Boolean inherit)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetGetMethod(Boolean nonPublic)
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetSetMethod(Boolean nonPublic)
        {
            throw new NotImplementedException();
        }

        public override Object GetValue(Object obj, BindingFlags invokeAttr, Binder binder, Object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override Boolean IsDefined(Type attributeType, Boolean inherit)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(Object obj, Object value, BindingFlags invokeAttr, Binder binder, Object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
