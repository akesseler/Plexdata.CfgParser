<p align="center">
  <a href="https://github.com/akesseler/Plexdata.CfgParser/blob/master/LICENSE.md" alt="license">
    <img src="https://img.shields.io/github/license/akesseler/Plexdata.CfgParser.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.CfgParser/releases/latest" alt="latest">
    <img src="https://img.shields.io/github/release/akesseler/Plexdata.CfgParser.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.CfgParser/archive/master.zip" alt="master">
    <img src="https://img.shields.io/github/languages/code-size/akesseler/Plexdata.CfgParser.svg" />
  </a>
  <a href="https://akesseler.github.io/Plexdata.CfgParser" alt="docs">
    <img src="https://img.shields.io/badge/docs-guide-orange.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.CfgParser/wiki" alt="wiki">
    <img src="https://img.shields.io/badge/wiki-API-orange.svg" />
  </a>
</p>

## Plexdata CFG Parser

The _Plexdata CFG Parser_ represents a library allowing reading and writing of old-fashioned CFG and INI files into structured configuration items. It is also possible to configure the behavior of how files are read or written.

Furthermore, a set of  structured configuration items can be converted into user-defined classes. This makes it possible to directly read a configuration file into a class structure. Of course, writing a configuration file from a class structure is possible as well.

Different styles of configuration files are also supported. Already predefined are the Windows and Linux styles, but configuring user-defined styles is supported as well.

The software has been published under the terms of _MIT License_.

For an introduction see the Docs under [https://akesseler.github.io/Plexdata.CfgParser/](https://akesseler.github.io/Plexdata.CfgParser/).

## Special Features

- Support of different configuration styles.
  - _Windows style_ (labels and values are separated by equal signs `=`).
  - _UNIX style_ (labels and values are separated by colons `:`).
  - _Mixed styles_ are supported as well.
- Support of file comments.
  - Comments can be put behind each section as well as behind each value.
  - Configuration files can have either a small header or an extended header (describing the rules of usage).
- Unassignable configuration content is preserved.
  - It can be accessed through Property `Others` of class `ConfigContent`.
- Parsing of configuration content.
  - From an input file into a class structure.
  - From a class structure into an output file.
- Integrated data type conversion.
  - Manual data type conversion by using class `ValueConverter`.
  - Automatic data type conversion (but only together with parsing feature).
- Support of custom type processing.
  - Implementing interface `ICustomParser<TType>` enables this feature.

## Short Introduction

### Reading

Assuming a configuration file named `config.ini` with following content shall be read.

```
[general]
enable-show-pages = yes
enable-show-styles = no
default-language = english

[network] 
server-address = 192.168.5.42 
server-port = 45054
```

Reading that configuration file is done as shown below.

```
String filename = @"C:\config.ini";
ConfigContent content = ConfigReader.Read(filename);
```

After file reading, each configuration section as well as their values can be accessed as shown here.

```
Boolean isShowPages = (Boolean)ValueConverter.Convert(content["general"]["enable-show-pages"].Value, typeof(Boolean));
Boolean isShowStyles = (Boolean)ValueConverter.Convert(content["general"]["enable-show-styles"].Value, typeof(Boolean));
String defaultLanguage = content["general"]["default-language"].Value;

IPAddress serverAddress = (IPAddress)ValueConverter.Convert(content["network"]["server-address"].Value, typeof(IPAddress));
UInt16 serverPort = (UInt16)ValueConverter.Convert(content["network"]["server-port"].Value, typeof(UInt16));
```

### Writing

Writing a configuration file takes place by configuring its content as shown below.

```
ConfigContent content = new ConfigContent();
content["general"] = new ConfigSection();
content["general"].Comment = new ConfigComment("The general section contains global values.");
content["general"]["enable-show-pages"] = new ConfigValue();
content["general"]["enable-show-pages"].Value = "yes";
content["general"]["enable-show-styles"] = new ConfigValue();
content["general"]["enable-show-styles"].Value = "no";
content["general"]["default-language"] = new ConfigValue();
content["general"]["default-language"].Value = "english";
content["general"]["default-language"].Comment = new ConfigComment("Using english, german and french is possible.");
content["network"] = new ConfigSection();
content["network"].Comment = new ConfigComment("The network section contains network values.");
content["network"]["server-address"] = new ConfigValue();
content["network"]["server-address"].Value = "192.168.5.42";
content["network"]["server-address"].Comment = new ConfigComment("Using the host name is also possible.");
content["network"]["server-port"] = new ConfigValue();
content["network"]["server-port"].Value = "45054";
```

Thereafter, the configuration file can be saved like shown here.

```
String filename = @"C:\config.ini";
ConfigWriter.Write(content, filename, true);
```

As long as no error occurred, the written output file would look like this.

```
[general] # The general section contains global values.
enable-show-pages = yes
enable-show-styles = no
default-language = english # Using english, german and french is possible.

[network] # The network section contains network values.
server-address = 192.168.5.42 # Using the host name is also possible.
server-port = 45054
```

### Parsing

Reading and writing a configuration file as shown in the above examples is for sure a little bit work intensive. Because of that fact, this library provides the possibility to parse an appropriate class structure instead. How to do it is shown in here.

The first step is to create all needed classes that describe the content of the configuration file. With file `config.ini` from above, such a class structure could look like shown here.

```
public class ProgramSettings
{
    [ConfigSection("general", Comment = "The general section contains global values.")]
    public GeneralSettings GeneralSettings { get; set; }
    [ConfigSection("network", Comment = "The network section contains network values.")]
    public NetworkSettings NetworkSettings { get; set; }
}

public class GeneralSettings
{
    [ConfigValue("enable-show-pages", Default = "yes")]
    public Boolean EnableShowPages { get; set; }
    [ConfigValue("enable-show-styles", Default = "no")]
    public Boolean EnableShowStyles { get; set; }
    [ConfigValue("default-language", Comment = "Using english, german and french is possible.")]
    public String DefaultLanguage { get; set; }
}

public class NetworkSettings
{
    [ConfigValue("server-address", Comment = "Usage of IPv4 or IPv6 is possible.")]
    public IPAddress ServerAddress { get; set; }
    [ConfigValue("server-port")]
    public UInt16 ServerPort { get; set; }
}
```

With the above class structure in mind, reading and parsing the file `config.ini` is done by the following two lines of code.

```
String filename = @"C:\config.ini";
ProgramSettings settings = ConfigParser<ProgramSettings>.Parse(ConfigReader.Read(filename));
```

Parsing that class structure and writing it into file `config.ini` just needs the following two lines of code.

```
String filename = @"C:\config.ini";
ConfigWriter.Write(ConfigParser<ProgramSettings>.Parse(settings), filename, true);
```

As long as no error occurred, the written output file would look like this.

```
[general] # The general section contains global values.
enable-show-pages = True
enable-show-styles = False
default-language = english # Using english, german and french is possible.

[network] # The network section contains network values.
server-address = 192.168.5.42 # Usage of IPv4 or IPv6 is possible.
server-port = 45054
```

A detailed documentation and a list of limitations as well as some known issues can be found under [https://akesseler.github.io/Plexdata.CfgParser/](https://akesseler.github.io/Plexdata.CfgParser/).

## Supported Types

The integrated _Value Converter_ supports `String`, `Version`, `IPAddress`, `Char`, `Char?`, `Boolean`, `Boolean?`, `SByte`, `SByte?`, `Byte`, `Byte?`, `Int16`, `Int16?`, `UInt16`, `UInt16?`, `Int32`, `Int32?`, `UInt32`, `UInt32?`, `Int64`, `Int64?`, `UInt64`, `UInt64?`, `DateTime`, `DateTime?`, `Decimal`, `Decimal?`, `Double`, `Double?`, `Single`, `Single?`, `Guid`, `Guid?` as well as `Enum` types.

It is possible to convert any other type by implementing interface `ICustomParser<TType>` accordingly.
