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
using Plexdata.CfgParser.Converters;
using Plexdata.CfgParser.Entities;
using Plexdata.CfgParser.Processors;
using Plexdata.CfgParser.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Plexdata.CfgParser.Tester
{
    class Program
    {

        private class IniSection1
        {
            [ConfigValue(Label = "section-1-value-1")]
            public String Section1Value1 { get; set; }

            [ConfigValue]
            public String Section1Value2 { get; set; }

            [ConfigValue]
            public String Section1Value3 { get; set; }
        }

        private class IniSection2
        {
            [ConfigValue]
            public String Section2Value1 { get; set; }

            [ConfigValue]
            public String Section2Value2 { get; set; }

            [ConfigValue]
            public String Section2Value3 { get; set; }

            [ConfigValue]
            public Decimal Section2Value4 { get; set; }

            [ConfigIgnore]
            public String IgnoredInSection2 { get; set; }
        }

        private class IniContent
        {
            [ConfigSection]
            public IniSection1 IniSection1 { get; set; }

            [ConfigSection(Title = "MY FUNNY SECTION 2")]
            public IniSection2 IniSection2 { get; set; }

            [ConfigIgnore]
            public String NotPartofSection2 { get; set; }


        }

        // public static class ConfigImporter<TInstance> where TInstance : class

            public enum Testaa
        {
            unknown,
            test1,
            test2,
            test3,
        }


        static void Main(String[] args)
        {




            Testaa o1 = (Testaa)ValueConverter.Convert("TEST1", typeof(Testaa));

            try
            {
                Testaa o4 = (Testaa)ValueConverter.Convert("test42", typeof(Testaa));
            }
            catch { }


            //Testaa? heimer = null;
            Testaa? o2 = (Testaa?)ValueConverter.Convert("test1", typeof(Testaa?));

            Testaa? o3 = (Testaa?)ValueConverter.Convert("", typeof(Testaa?));

            Testaa? o5 = (Testaa?)ValueConverter.Convert("test42", typeof(Testaa?));



            ConfigContent content = new ConfigContent();

            content["IniSection1"] = new ConfigSection();
            content["IniSection1"]["section-1-value-1"] = new ConfigValue();
            content["IniSection1"]["Section1Value2"] = new ConfigValue();
            content["IniSection1"]["Section1Value3"] = new ConfigValue();

            content["MY FUNNY SECTION 2"] = new ConfigSection();
            content["MY FUNNY SECTION 2"]["Section2Value1"] = new ConfigValue();
            content["MY FUNNY SECTION 2"]["Section2Value2"] = new ConfigValue();
            content["MY FUNNY SECTION 2"]["Section2Value3"] = new ConfigValue();
            content["MY FUNNY SECTION 2"]["Section2Value4"] = new ConfigValue();



            content["IniSection1"]["section-1-value-1"].Value = "section1-value1-data1";
            content["IniSection1"]["Section1Value2"].Value = "section1-value2-data1";
            content["IniSection1"]["Section1Value3"].Value = "section1-value3-data1";

            content["MY FUNNY SECTION 2"]["Section2Value1"] .Value = "section2-value1-data1";
            content["MY FUNNY SECTION 2"]["Section2Value2"] .Value = "section2-value2-data1";
            content["MY FUNNY SECTION 2"]["Section2Value3"] .Value = "section2-value3-data1";
            content["MY FUNNY SECTION 2"]["Section2Value4"] .Value = "1.234,56";



            IniContent result = ConfigParser<IniContent>.Parse(content, CultureInfo.GetCultureInfo("de-DE"));


            IniContent iniContent = new IniContent();
            iniContent.NotPartofSection2 = "Not visible";

            iniContent.IniSection1 = new IniSection1();
            iniContent.IniSection1.Section1Value1 = "Hello world";
            iniContent.IniSection1.Section1Value2 = "sometimes something is strange";
            iniContent.IniSection1.Section1Value3 = "Fuck them all";

            iniContent.IniSection2 = new IniSection2();
            iniContent.IniSection2.IgnoredInSection2 = "ignoree";
            iniContent.IniSection2.Section2Value1 = "s2v1blah";
            iniContent.IniSection2.Section2Value2 = "alles so anstrengend";
            iniContent.IniSection2.Section2Value3 = "ich könnte kotzen";
            iniContent.IniSection2.Section2Value4 = 6543.21M;

            ConfigContent heimer = ConfigParser<IniContent>.Parse(iniContent, CultureInfo.GetCultureInfo("de-DE"));

            foreach (String output in heimer.ToOutput())
            {
                Console.WriteLine(output);
            }

            int i = 0;



            //String filename = @"C:\temp\input-test - Kopie.cfg";
            //List<ConfigWarning> warnings = new List<ConfigWarning>();
            //ConfigContent content = ConfigReader.Read(filename, warnings);

            //foreach (String output in content.ToOutput())

            //{
            //    Console.WriteLine(output);
            //}

            //foreach (ConfigWarning warning in warnings)
            //{
            //    Console.WriteLine(warning);
            //}

            //ConfigSettings.Settings = new ConfigSettingsUnix();
            //content = new ConfigContent();
            //content.Header = ConfigSettings.CreateDefaultHeader("Heimer & Co. KG.", true);


            //content["Config Section 23"] = new ConfigSection();
            //content["Config Section 24"] = new ConfigSection();
            //content["Config Section 25"] = new ConfigSection();
            //content["Config Section 26"] = new ConfigSection();

            //content["Config Section 42"] = new ConfigSection();

            //content["Config Section 42"] = new ConfigSection("Config Section 23");
            //content["Config Section 23"] = new ConfigSection("Config Section 42");

            //content["Config Section 24"]["val-1"] = new ConfigValue("value 11");
            //content["Config Section 24"]["val-2"] = new ConfigValue("value 12");
            //content["Config Section 24"]["val-3"] = new ConfigValue("value 13");

            //ConfigSection section = content.Append("section 1");
            //section.Append("label 1").Value = "value 11";
            //section.Append("label 2").Value = "value 12";
            //section.Append("label 3").Value = "value 13";

            //section = content.Append("section 2");
            //section.Append("label 1").Value = "value 21";
            //section.Append("label 2").Value = "value 22";
            //section.Append("label 3").Value = "value 23";

            //ConfigWriter.Write(content, filename + ".out", true);


            Console.WriteLine("Do something useful...");
            Console.ReadKey();
        }
    }
}
