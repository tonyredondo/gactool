using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Principal;

namespace GacTool;

#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
internal static class AdministratorHelper
{
    private static bool? _isElevated;

    public static bool IsElevated
    {
        get
        {
            _isElevated ??= new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            return _isElevated.Value;
        }
    }

    public static void EnsureIsElevated()
    {
        if (!IsElevated)
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
#if NET6_0_OR_GREATER
            var processPath = Environment.ProcessPath ?? commandLineArguments[0];
#else
            var processPath = commandLineArguments[0];
#endif

            var processInfo = new ProcessStartInfo(processPath)
            {
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true
            };

            foreach (var arg in commandLineArguments.Skip(1))
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
