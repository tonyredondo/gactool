# Gac Tool
Gac tool is a dotnet tool for installing and uninstalling .NET Framework assemblies to the GAC (Global Assembly Cache)

## Installation
```shell
dotnet tool update -g gactool
```


## Usage

The tool require elevated permissions so an UAC prompt may appear if the tool is not being executed in an Administrator context already.

### Assembly install
```shell
dotnet gac install <assembly path>
```

### Assembly uninstall
```shell
dotnet gac uninstall <assembly name>
```