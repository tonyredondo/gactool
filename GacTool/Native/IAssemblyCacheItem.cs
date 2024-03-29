﻿using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace GacTool.Native;

// Code based on: https://github.com/dotnet/pinvoke/tree/main/src/Fusion

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("9e3aaeb4-d1cd-11d2-bab9-00c04f8eceae")]
internal interface IAssemblyCacheItem
{
    void CreateStream([MarshalAs(UnmanagedType.LPWStr)] string pszName, uint dwFormat, uint dwFlags, uint dwMaxSize, out IStream ppStream);

    void IsNameEqual(IAssemblyName pName);

    void Commit(uint dwFlags);

    void MarkAssemblyVisible(uint dwFlags);
}
