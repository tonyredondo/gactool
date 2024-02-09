#if !NETCOREAPP3_0_OR_GREATER

using System.Runtime.InteropServices;

namespace GacTool.Native;

internal sealed class NativeLibrary
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr LoadLibrary(string libName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern bool FreeLibrary(IntPtr hModule);
    
    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    public static IntPtr Load(string libraryPath)
    {
        var value = LoadLibrary(libraryPath);
        if (value != IntPtr.Zero)
        {
            return value;
        }

        var errorCode = Marshal.GetLastWin32Error();
        var hr = Marshal.GetHRForLastWin32Error();
        throw new Exception($"Failed to load library (ErrorCode: {errorCode}, HR: {hr})");

    }

    public static IntPtr GetExport(IntPtr handle, string name)
    {
        var value = GetProcAddress(handle, name);
        if (value != IntPtr.Zero)
        {
            return value;
        }

        var errorCode = Marshal.GetLastWin32Error();
        var hr = Marshal.GetHRForLastWin32Error();
        throw new Exception($"Failed to get the Export (ErrorCode: {errorCode}, HR: {hr})");

    }

    public static void Free(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
        {
            return;
        }

        FreeLibrary(handle);
    }
}

#endif
