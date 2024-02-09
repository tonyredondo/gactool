namespace GacTool.Native;

// Code based on: https://github.com/dotnet/pinvoke/tree/main/src/Fusion

[Flags]
internal enum QueryAssemblyInfoFlag
{
    /// <summary>
    /// No flags.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// Validates the assembly files in the side-by-side assembly store against the assembly manifest. This includes the verification of the assembly's hash and strong name signature.
    /// </summary>
    QUERYASMINFO_FLAG_VALIDATE = 0x1,

    /// <summary>
    /// Returns the size of all files in the assembly.
    /// </summary>
    QUERYASMINFO_FLAG_GETSIZE = 0x2,
}
