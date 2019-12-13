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

using NUnit.Framework;
using Plexdata.CfgParser.Extensions;
using System;

namespace Plexdata.CfgParser.Tests.Extensions
{
    [TestFixture]
    [TestOf(nameof(StringExtension))]
    public class StringExtensionTests
    {
        [Test]
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("is not hollow", false)]
        [TestCase("   is not hollow too  ", false)]
        public void IsHollow_VariousValues_ResultIsAsExpected(String buffer, Boolean expected)
        {
            Assert.AreEqual(expected, buffer.IsHollow());
        }

        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("is not comment", false)]
        [TestCase("   is not comment too  ", false)]
        [TestCase("is not # comment", false)]
        [TestCase("   is not # comment too  ", false)]
        [TestCase("is not ; comment", false)]
        [TestCase("   is not ; comment too  ", false)]
        [TestCase("# is comment   ", true)]
        [TestCase("  # is comment   ", true)]
        [TestCase("; is comment   ", true)]
        [TestCase("  ; is comment   ", true)]
        public void IsComment_VariousValues_ResultIsAsExpected(String buffer, Boolean expected)
        {
            Assert.AreEqual(expected, buffer.IsComment());
        }

        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("is not section", false)]
        [TestCase("   is not section too  ", false)]
        [TestCase("[is not section", false)]
        [TestCase("   [is not section too  ", false)]
        [TestCase("[]", true)]
        [TestCase("  []", true)]
        [TestCase("[is section]", true)]
        [TestCase("  [is section]", true)]
        [TestCase("[]   ", true)]
        [TestCase("  []   ", true)]
        [TestCase("[is section]   ", true)]
        [TestCase("  [is section]   ", true)]
        [TestCase("[is section]  appendix   ", true)]
        [TestCase("  [is section]  appendix   ", true)]
        [TestCase("[is section]appendix   ", true)]
        [TestCase("  [is section]appendix   ", true)]
        [TestCase("[]  appendix   ", true)]
        [TestCase("  []  appendix   ", true)]
        [TestCase("[]appendix   ", true)]
        [TestCase("  []appendix   ", true)]
        public void IsSection_VariousValues_ResultIsAsExpected(String buffer, Boolean expected)
        {
            Assert.AreEqual(expected, buffer.IsSection());
        }

        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("is not value", false)]
        [TestCase("   is not value", false)]
        [TestCase("\"is not value", false)]
        [TestCase("   \"is not value", false)]
        [TestCase("#is not value", false)]
        [TestCase("   #is not value", false)]
        [TestCase(";is not value", false)]
        [TestCase("   ;is not value", false)]
        [TestCase("is value=", true)]
        [TestCase("   is value=", true)]
        [TestCase("is value = ", true)]
        [TestCase("   is value = ", true)]
        [TestCase("is value:", true)]
        [TestCase("   is value:", true)]
        [TestCase("is value : ", true)]
        [TestCase("   is value : ", true)]
        public void IsValue_VariousValues_ResultIsAsExpected(String buffer, Boolean expected)
        {
            Assert.AreEqual(expected, buffer.IsValue());
        }
    }
}
