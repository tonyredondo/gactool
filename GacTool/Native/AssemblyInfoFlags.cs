namespace GacTool.Native;

// Code based on: https://github.com/dotnet/pinvoke/tree/main/src/Fusion

[Flags]
internal enum AssemblyInfoFlags
{
    /// <summary>
    /// No flags.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// Indicates that the assembly is installed. The current version of the .NET Framework always sets dwAssemblyFlags to this value.
    /// </summary>
    ASSEMBLYINFO_FLAG_INSTALLED = 0x1,

    /// <summary>
    /// Indicates that the assembly is a payload resident. The current version of the .NET Framework never sets dwAssemblyFlags to this value.
    /// </summary>
    ASSEMBLYINFO_FLAG_PAYLOADRESIDENT = 0x2,
}
