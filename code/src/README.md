
## Project Build

Best way to build the whole project is to use _Visual Studio 2017 Community_. Thereafter, download the complete sources, open the solution file ``Plexdata.CfgParser.NET.sln``, switch to release and rebuild all.

## Limitations

- Data parts of configuration values must not contain additional double quotes like ``value-label="Inner double quote characters (") are not supported"``. Escaping them like ``"inner \"value"`` will also not work.
- Creating a configuration from scratch or reading an existing configuration file may cause trouble when writing it back to disk. See section _Known Issues_ for more information.
- Leading and/or trailing section and/or value comments are not supported. Comments can only be placed behind sections and/or values.
- Multi-line comments behind sections and/or variables like shown below are not supported. Only single line comments are possible.

Multi-line comments won't work:
```
label-1=value-1 # comment line 1
                # comment line 2
                # comment line 3
label-2=value-2 
```

## Known Issues

The complete content of an already existing file is disposed as soon as a configuration content is written back to this file. This means, it is impossible to selectively update only the data that have been changed.

