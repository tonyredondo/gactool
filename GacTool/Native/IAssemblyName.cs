﻿using System.Runtime.InteropServices;

namespace GacTool.Native;

// Code based on: https://github.com/dotnet/pinvoke/tree/main/src/Fusion

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
internal interface IAssemblyName
{
    [PreserveSig]
    int SetProperty(uint propertyId, IntPtr pvProperty, uint cbProperty);

    [PreserveSig]
    int GetProperty(uint propertyId, IntPtr pvProperty, ref uint pcbProperty);

    [PreserveSig]
#pragma warning disable CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation
    int Finalize();
#pragma warning restore CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation

    [PreserveSig]
    int GetDisplayName(IntPtr szDisplayName, ref uint pccDisplayName, uint dwDisplayFlags);

    [PreserveSig]
    int BindToObject(
        object /*REFIID*/ refIID,
        object /*IAssemblyBindSink*/ pAsmBindSink,
        IApplicationContext pApplicationContext,
        [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase,
        long llFlags,
        int pvReserved,
        uint cbReserved,
        out int ppv);

    [PreserveSig]
    int GetName(out uint lpcwBuffer, out int pwzName);

    [PreserveSig]
    int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

    [PreserveSig]
    int IsEqual(IAssemblyName pName, uint dwCmpFlags);

    [PreserveSig]
    int Clone(out IAssemblyName pName);
}
