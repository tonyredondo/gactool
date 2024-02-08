using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace GacTool.Native;

[ComVisible(false)]
internal sealed class NativeMethods
{
    [SupportedOSPlatform("windows")]
    internal static unsafe AssemblyCacheContainer CreateAssemblyCache()
    {
        const string subKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
        string fusionFullPath;
        using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitProcess ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(subKey))
        {
            var installPath = ndpKey?.GetValue("InstallPath")?.ToString() ?? @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319";
            fusionFullPath = Path.Combine(installPath, "fusion.dll");
        }

        if (!File.Exists(fusionFullPath))
        {
            throw new FileNotFoundException($"{fusionFullPath} cannot be found.");
        }

        var libPointer = NativeLibrary.Load(fusionFullPath);
        var createAssemblyCachePointer = NativeLibrary.GetExport(libPointer, nameof(CreateAssemblyCache));
        var functionPointer = (delegate* unmanaged[Stdcall] <out IAssemblyCache, uint, int>)createAssemblyCachePointer;
        var hr = functionPointer(out var ppAsmCache, 0);
        if (hr != 0)
        {
            throw new TargetInvocationException($"HRESULT = {hr}", null);
        }

        return new AssemblyCacheContainer(libPointer, ppAsmCache);
    }
}