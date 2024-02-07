using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace GacUtilTool.Native;

[ComVisible(false)]
internal sealed class NativeMethods
{
    delegate int CreateAssemblyCacheDelegate(out IAssemblyCache ppAsmCache, uint dwReserved);
    static IntPtr _libPointer = IntPtr.Zero;
    
    [SupportedOSPlatform("windows")]
    private static unsafe int CreateAssemblyCache(out IAssemblyCache ppAsmCache, uint dwReserved)
    {
        if (_libPointer == IntPtr.Zero)
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            string fusionFullPath;
            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitProcess ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(subkey))
            {
                var installPath = ndpKey?.GetValue("InstallPath")?.ToString() ?? @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319";
                fusionFullPath = Path.Combine(installPath, "fusion.dll");
            }

            _libPointer = NativeLibrary.Load(fusionFullPath);
        }
        
        var createAssemblyCachePointer = NativeLibrary.GetExport(_libPointer, nameof(CreateAssemblyCache));
        var functionPointer = (delegate* unmanaged[Stdcall] <out IAssemblyCache, uint, int>)createAssemblyCachePointer;
        return functionPointer(out ppAsmCache, dwReserved);
    }

    [SupportedOSPlatform("windows")]
    internal static IAssemblyCache CreateAssemblyCache()
    {
        var hr = CreateAssemblyCache(out var assemblyCache, 0);
        if (hr != 0)
        {
            throw new TargetInvocationException($"HRESULT = {hr}", null);
        }

        return assemblyCache;
    }
    
    internal static void Dispose()
    {
        if (_libPointer != IntPtr.Zero)
        {
            NativeLibrary.Free(_libPointer);
        }
    }
}
