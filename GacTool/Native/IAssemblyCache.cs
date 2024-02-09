using System.Runtime.InteropServices;

namespace GacTool.Native;

// Code based on: https://github.com/dotnet/pinvoke/tree/main/src/Fusion

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
internal interface IAssemblyCache
{
    [PreserveSig]
    int UninstallAssembly(UninstallAssemblyFlags dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, IntPtr pvReserved, out UninstallDisposition pulDisposition);

    [PreserveSig]
    int QueryAssemblyInfo(QueryAssemblyInfoFlag dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, ref AssemblyInfo pAsmInfo);

    [PreserveSig]
    int CreateAssemblyCacheItem(uint dwFlags, IntPtr pvReserved, out IAssemblyCacheItem ppAsmItem, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName);

    [PreserveSig]
    int CreateAssemblyScavenger(out object ppAsmScavenger);

    [PreserveSig]
    int InstallAssembly(AssemblyCacheInstallFlags dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszManifestFilePath, IntPtr pvReserved);
}
