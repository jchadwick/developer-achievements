using System;
using System.Diagnostics;

namespace DeveloperAchievements.Subversion.InterOp
{
    internal static class InterOpHelper
    {
        public static string ExecutablePath { get; set; }

        internal static string Execute(string command)
        {
            string output, errors;
            ExecuteCommand(command, 20, out output, out errors);

            if (string.IsNullOrEmpty(errors) == false)
                throw new ApplicationException("Error executing command: " + errors);

            return output;
        }

        private static int ExecuteCommand(string command, int timeout, out string output, out string errors)
        {
            Debug.WriteLine("Calling " + command);

            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
                                               {
                                                   CreateNoWindow = true,
                                                   UseShellExecute = false,
                                                   RedirectStandardError = true,
                                                   RedirectStandardOutput = true
                                               };

            Process process = Process.Start(processInfo);
            output = process.StandardOutput.ReadToEnd();
            errors = process.StandardError.ReadToEnd();

            Debug.WriteLine("svnlook Output: " + output);
            Debug.WriteLine("svnlook Errors: " + errors);

            process.WaitForExit(timeout);

            int exitCode = process.ExitCode;

            process.Close();

            return exitCode;
        }
    }
}