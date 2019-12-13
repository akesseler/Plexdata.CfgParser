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
using Plexdata.CfgParser.Entities;
using Plexdata.CfgParser.Settings;
using System;

namespace Plexdata.CfgParser.Tests.Entities
{
    [TestFixture]
    [TestOf(nameof(ConfigValue))]
    public class ConfigValueTests
    {
        private ConfigSettingsBase lastConfigSettings = null;

        [SetUp]
        public void Setup()
        {
            this.lastConfigSettings = ConfigSettings.Settings;
        }

        [TearDown]
        public void Cleanup()
        {
            ConfigSettings.Settings = this.lastConfigSettings;
        }

        [Test]
        public void ConfigValue_DefaultConstruction_ResultIsInvalid()
        {
            ConfigValue instance = new ConfigValue();
            Assert.IsFalse(instance.IsValid);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ConfigValue_ConstructionLabelInvalid_ThrowsArgumentException(String label)
        {
            Assert.Throws<ArgumentException>(() => { new ConfigValue(label); });
        }

        [Test]
        [TestCase("LabelNoSpaces", "LabelNoSpaces")]
        [TestCase("Label Inner Spaces", "Label Inner Spaces")]
        [TestCase("   LabelOuterSpaces  ", "LabelOuterSpaces")]
        [TestCase("  Label Inner + Outer Spaces  ", "Label Inner + Outer Spaces")]
        public void ConfigValue_ConstructionLabelValid_ResultIsExpected(String label, String expected)
        {
            ConfigValue instance = new ConfigValue(label);
            Assert.AreEqual(expected, instance.Label);
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("ValueNoSpaces", "ValueNoSpaces")]
        [TestCase("Value Inner Spaces", "Value Inner Spaces")]
        [TestCase("   ValueOuterSpaces  ", "ValueOuterSpaces")]
        [TestCase("  Value Inner + Outer Spaces  ", "Value Inner + Outer Spaces")]
        public void ConfigValue_ConstructionValueValid_ResultIsExpected(String value, String expected)
        {
            ConfigValue instance = new ConfigValue("label", value);
            Assert.AreEqual(expected, instance.Value);
        }

        [Test]
        public void ConfigValue_ConstructionCommentValid_CommentIsNotNull()
        {
            ConfigValue instance = new ConfigValue("label", "value", "comment");
            Assert.IsNotNull(instance.Comment);
        }

        [Test]
        public void SetGetMarker_MarkerUnsupported_ThrowsArgumentException()
        {
            ConfigValue instance = new ConfigValue("label", "value");
            Assert.Throws<ArgumentException>(() => { instance.Marker = 'b'; });
        }

        [Test]
        [TestCase(':', ':')]
        [TestCase('=', '=')]
        public void SetGetMarker_MarkerSupported_ResultIsExpected(Char marker, Char expected)
        {
            ConfigValue instance = new ConfigValue("label", "value");
            instance.Marker = marker;
            Assert.AreEqual(expected, instance.Marker);
        }

        [Test]
        [TestCase("label=", true)]
        [TestCase(" ", false)]
        public void TryParse_BufferAsIs_ResultIsExpected(String buffer, Boolean expected)
        {
            Assert.AreEqual(expected, ConfigValue.TryParse(buffer, out ConfigValue instance));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Parse_BufferInvalid_ThrowsArgumentException(String buffer)
        {
            Assert.Throws<ArgumentException>(() => { ConfigValue.Parse(buffer); });
        }

        [Test]
        [TestCase("=")]
        [TestCase("  =")]
        [TestCase("lab#el=")]
        [TestCase("  lab#el=")]
        [TestCase("lab;el=")]
        [TestCase("  lab;el=")]
        [TestCase("lab\"el=")]
        [TestCase("  lab\"el=")]
        [TestCase("label@value")]
        public void Parse_BufferInvalid_ThrowsFormatException(String buffer)
        {
            Assert.Throws<FormatException>(() => { ConfigValue.Parse(buffer); });
        }

        [Test]
        [TestCase("label=", "label = ")]
        [TestCase("label =", "label = ")]
        [TestCase("label  =", "label = ")]
        [TestCase("label=\"\"", "label = ")]
        [TestCase("label=\"   \"", "label = ")]
        [TestCase("label=value", "label = value")]
        [TestCase("label = value", "label = value")]
        [TestCase("label  = value", "label = value")]
        [TestCase("label=value  ", "label = value")]
        [TestCase("label = value  ", "label = value")]
        [TestCase("label  = value  ", "label = value")]
        [TestCase("label  =  value  ", "label = value")]
        [TestCase("label=\"value\"", "label = value")]
        [TestCase("label = \"value\"", "label = value")]
        [TestCase("label  = \"value\"", "label = value")]
        [TestCase("label=\"value\"  ", "label = value")]
        [TestCase("label = \"value\"  ", "label = value")]
        [TestCase("label  = \"value\"  ", "label = value")]
        [TestCase("label  =  \"value\"  ", "label = value")]
        [TestCase("label=\"  value\"", "label = value")]
        [TestCase("label=\"value \"", "label = value")]
        [TestCase("label=\" value \"", "label = value")]
        [TestCase("label=\"value #\"", "label = \"value #\"")]
        [TestCase("label=\" value # \"", "label = \"value #\"")]
        [TestCase("label=value \"#\"", "label = \"value #\"")]
        [TestCase("label= value \"#\" ", "label = \"value #\"")]
        [TestCase("label=\"value =\"", "label = \"value =\"")]
        [TestCase("label=\" value = \"", "label = \"value =\"")]
        [TestCase("label=value \"=\"", "label = \"value =\"")]
        [TestCase("label= value \"=\" ", "label = \"value =\"")]
        [TestCase("label=value#comment", "label = value # comment")]
        [TestCase("label = value # comment", "label = value # comment")]
        [TestCase("label  =  value  #  comment", "label = value # comment")]
        [TestCase("label=val\"#\"ue#comment", "label = \"val#ue\" # comment")]
        [TestCase("label =\"val#ue\" # comment", "label = \"val#ue\" # comment")]
        public void Parse_BufferValid_ResultIsExpected(String buffer, String expected)
        {
            ConfigValue instance = ConfigValue.Parse(buffer);
            Assert.AreEqual(expected, instance.ToOutput());
        }
    }
}
