using System.Runtime.InteropServices;

namespace GacTool.Native;

internal sealed class AssemblyCacheContainer : IDisposable
{
    private readonly IntPtr _libPointer;

    public AssemblyCacheContainer(IntPtr libPointer, IAssemblyCache assemblyCache)
    {
        _libPointer = libPointer;
        AssemblyCache = assemblyCache;
    }

    public IAssemblyCache AssemblyCache { get; private set; }

    public void Dispose()
    {
        if (_libPointer != IntPtr.Zero)
        {
            NativeLibrary.Free(_libPointer);
        }
    }
}
