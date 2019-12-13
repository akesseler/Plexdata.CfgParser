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

## Project Build

Best way to build the whole project is to use _Visual Studio 2017 Community_. Thereafter, download the complete sources, open the solution file ``Plexdata.CfgParser.NET.sln``, switch to release and rebuild all.

## Help Generation

The help file of type CHM is generated during release build process only. For this purpose the ``MSBuild.exe`` is used. The help configuration file ``Plexdata.CfgParser.NET.shfbproj`` has been made using [Sandcastle Help File Builder](https://ewsoftware.github.io/SHFB/html/bd1ddb51-1c4f-434f-bb1a-ce2135d3a909.htm). The final help file with name ``Plexdata.CfgParser.NET.chm`` is automatically put into the release sub-folder after a successful build.

You can disable the help file generation, if you like, by opening the _Project Settings_ and moving to tab _Build Events_. There you simply clear out the content of box _Post-build event command line_.

## Trouble Shooting

If you get an error during release build, you may need to install the _Sandcastle Help File Builder_ manually and edit the help configuration file ``Plexdata.CfgParser.NET.shfbproj``.

On the other hand, if you get an error that states something like ``MSBuild.exe not found``, then you may need to correct the path to file ``MSBuild.exe`` inside file ``post-build.cmd``.

## Limitations

- Data parts of configuration values must **not** contain additional double quotes like ``value-label="Inner double quote characters (") are not supported"``. Escaping them like ``"inner \"value"`` does not work.
- Creating a configuration from scratch or reading an existing configuration file may cause trouble when writing it back to disk. See section _Known Issues_ for more information.
- Leading and/or trailing section and/or value comments are not supported. Comments can only be placed behind sections and/or values.
- Multi-line comments behind sections and/or values like shown below are not supported. Only single line comments are possible.

Multi-line comments won't work:
```
label-1=value-1 # comment line 1
                # comment line 2
                # comment line 3
label-2=value-2 
```

## Known Issues

The complete content of an already existing file is disposed as soon as a configuration content is written back to this file. This means, it is impossible to selectively update only the data that have been changed.


