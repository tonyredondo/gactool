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

## Troubleshooting

If a message like this appears when running `dotnet gac`

```shell
System.ComponentModel.Win32Exception (740): An error occurred trying to start process 'C:\Users\Usuario\.dotnet\tools\dotnet-gac.exe' with working directory 'C:\temp\GacTool\artifacts'. The requested operation requires elevation.
   at System.Diagnostics.Process.StartWithCreateProcess(ProcessStartInfo startInfo)
   at Microsoft.DotNet.Cli.Utils.Command.Execute(Action`1 processStarted)
   at Microsoft.DotNet.Cli.Program.ProcessArgs(String[] args, TimeSpan startupTime, ITelemetry telemetryClient)
   at Microsoft.DotNet.Cli.Program.Main(String[] args)
```

Open a new console with Administrator permissions.