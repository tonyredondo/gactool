using System.Runtime.InteropServices;

namespace GacTool.Native;

// Code based on: https://github.com/dotnet/pinvoke/tree/main/src/Fusion

[StructLayout(LayoutKind.Sequential)]
internal struct AssemblyInfo
{
    private uint _cbAssemblyInfo;
    private AssemblyInfoFlags _assemblyFlags;
    private long _assemblySizeInKB;

    [MarshalAs(UnmanagedType.LPWStr)]
    private string _currentAssemblyPath;
    private int _cchBuf;

    public AssemblyInfo()
    {
        _cchBuf = 1024;
        _currentAssemblyPath = new('\0', _cchBuf);
    }

    public AssemblyInfoFlags AssemblyFlags => _assemblyFlags;

    public long AssemblySizeInKb => _assemblySizeInKB;

    public string CurrentAssemblyPath => _currentAssemblyPath;
}
