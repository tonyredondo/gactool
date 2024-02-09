
using System.Reflection;
using System.Runtime.InteropServices;
using GacTool;
using GacTool.Native;

Environment.ExitCode = 1;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    if (args.Length == 0)
    {
        Console.WriteLine("Missing command. Options: 'get' or 'install' or 'uninstall'");
        return;
    }

    var command = args[0].ToLowerInvariant();
    
    if (args.Length == 1)
    {
        Console.WriteLine("Missing assembly parameter.");
        return;
    }

    if (command == "install")
    {
        AdministratorHelper.EnsureIsElevated();
        var assemblyPath = Path.Combine(Environment.CurrentDirectory, args[1]);
        if (!File.Exists(assemblyPath))
        {
            Console.WriteLine("File '{0}' does not exist.", assemblyPath);
            return;
        }

        using var container = NativeMethods.CreateAssemblyCache();
        var hr = container.AssemblyCache.InstallAssembly(0, assemblyPath, IntPtr.Zero);
        if (hr == 0)
        {
            Console.WriteLine("Assembly '{0}' was installed in the GAC sucessfully.", assemblyPath);
        }
        else
        {
            Console.WriteLine("Error installing '{0}' in the GAC. HRESULT={1}", assemblyPath, hr);
        }

        Environment.ExitCode = hr;
        return;
    }

    if (command == "uninstall")
    {
        AdministratorHelper.EnsureIsElevated();
        var assemblyName = args[1];
        if (File.Exists(assemblyName))
        {
            try
            {
                var asmPath = Path.IsPathRooted(assemblyName)
                    ? assemblyName
                    : Path.Combine(Environment.CurrentDirectory, assemblyName);
                assemblyName = Assembly.LoadFile(asmPath).GetName().Name;
            }
            catch
            {
                // .
            }
        }

        using var container = NativeMethods.CreateAssemblyCache();
        var hr = container.AssemblyCache.UninstallAssembly(0, assemblyName!, IntPtr.Zero, out var position);

        switch (position)
        {
            case UninstallDisposition.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED:
                Console.WriteLine($"Assembly '{assemblyName}' was already uninstalled from the GAC.");
                Environment.ExitCode = hr;
                return;
            case UninstallDisposition.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND:
                Console.WriteLine($"Assembly '{assemblyName}' not found in the GAC.");
                Environment.ExitCode = hr;
                return;
            case UninstallDisposition.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE:
                Console.WriteLine($"Assembly '{assemblyName}' is still in use.");
                Environment.ExitCode = hr;
                return;
        }
        
        if (hr == 0)
        {
            Console.WriteLine("Assembly '{0}' was uninstalled from the GAC sucessfully.", assemblyName);
        }
        else
        {
            Console.WriteLine("Error uninstalling '{0}' from the GAC. HRESULT={1}", assemblyName, hr);
        }

        Environment.ExitCode = hr;
        return;
    }

    if (command == "get")
    {
        var assemblyName = args[1];
        if (File.Exists(assemblyName))
        {
            try
            {
                var asmPath = Path.IsPathRooted(assemblyName)
                    ? assemblyName
                    : Path.Combine(Environment.CurrentDirectory, assemblyName);
                assemblyName = Assembly.LoadFile(asmPath).GetName().Name;
            }
            catch
            {
                // .
            }
        }

        using var container = NativeMethods.CreateAssemblyCache();
        var asmInfo = new AssemblyInfo();
        var hr = container.AssemblyCache.QueryAssemblyInfo(QueryAssemblyInfoFlag.QUERYASMINFO_FLAG_GETSIZE, assemblyName!, ref asmInfo);
        if (hr == 0)
        {
            var asmFlags = asmInfo.AssemblyFlags switch
            {
                AssemblyInfoFlags.None => "None",
                AssemblyInfoFlags.ASSEMBLYINFO_FLAG_INSTALLED => "Installed",
                AssemblyInfoFlags.ASSEMBLYINFO_FLAG_PAYLOADRESIDENT => "Payload resident",
                _ => string.Empty
            };

            Console.WriteLine($"Assembly Found!");
            Console.WriteLine($"  Flag={asmFlags}");
            Console.WriteLine($"  Path={asmInfo.CurrentAssemblyPath}");
            Console.WriteLine($"  SizeInKb={asmInfo.AssemblySizeInKb}");
        }
        else
        {
            Console.WriteLine($"Error getting '{assemblyName}' from the GAC. HRESULT={hr}");
        }
        Environment.ExitCode = hr;
        return;
    }
    
    Console.WriteLine("Command '{0}' not found.", command);
}
else
{
    Console.WriteLine("This tool is only supported in Windows.");
}
