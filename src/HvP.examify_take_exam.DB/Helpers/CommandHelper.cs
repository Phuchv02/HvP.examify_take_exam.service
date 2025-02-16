using System.Diagnostics;

namespace HvP.examify_take_exam.DB.Helpers
{
    public static class CommandHelper
    {
        public static async Task<string> RunCommand(string workingDirectory, string command)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {command}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };

            process.Start();
            string result = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return result;
        }
    }
}
