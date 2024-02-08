
using System.Reflection;
using System.Runtime.InteropServices;
using GacTool;
using GacTool.Native;

Environment.ExitCode = 1;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    if (args.Length == 0)
    {
        Console.WriteLine("Missing command. Options: 'install' or 'uninstall'");
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
        var hr = container.AssemblyCache.UninstallAssembly(0, assemblyName, IntPtr.Zero, out var position);
        if (position == 3 /*IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED*/)
        {
            Console.WriteLine("Assembly '{0}' was already uninstalled from the GAC.", assemblyName);
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
    
    Console.WriteLine("Command '{0}' not found.", command);
}
else
{
    Console.WriteLine("This tool is only supported in Windows.");
}
