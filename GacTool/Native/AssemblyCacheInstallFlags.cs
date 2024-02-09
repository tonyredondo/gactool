namespace GacTool.Native;

// Code based on: https://github.com/dotnet/pinvoke/tree/main/src/Fusion

[Flags]
internal enum AssemblyCacheInstallFlags
{
    None = 0x0,
    IASSEMBLYCACHE_INSTALL_FLAG_REFRESH = 0x1,
    IASSEMBLYCACHE_INSTALL_FLAG_FORCE_REFRESH = 0x2,
}
