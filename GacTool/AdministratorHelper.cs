using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Principal;

namespace GacTool;

[SupportedOSPlatform("windows")]
internal static class AdministratorHelper
{
    private static bool? _isElevated;
    
    public static bool IsElevated
    {
        get
        {
            _isElevated ??= new WindowsPrincipal
                (WindowsIdentity.GetCurrent()).IsInRole
                (WindowsBuiltInRole.Administrator);

            return _isElevated.Value;
        }
    }

    public static void EnsureIsElevated()
    {
        if (!IsElevated && Environment.ProcessPath is { } processPath)
        {
            var processInfo = new ProcessStartInfo(processPath)
            {
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true
            };

            foreach (var arg in Environment.GetCommandLineArgs().Skip(1))
            {
                processInfo.ArgumentList.Add(arg);
            }

            var process = Process.Start(processInfo);
            process?.WaitForExit();
            Console.WriteLine("Returned: {0}", process?.ExitCode);
            Environment.Exit(process?.ExitCode ?? 0);
        }
    }
}