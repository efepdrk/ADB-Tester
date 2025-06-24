using System.Diagnostics;
using System.Text;
using System.IO;

public static class AdbService
{
    private static readonly string AdbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb", "adb.exe");

    public static async Task<string> ExecuteCommand(string command, string deviceId = "")
    {
       
     
        
            try
            {
                using var process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = AdbPath,
                    Arguments = $"{(string.IsNullOrEmpty(deviceId) ? "" : $"-s {deviceId}")} {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                process.OutputDataReceived += (_, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                        outputBuilder.AppendLine(e.Data);
                };

                process.ErrorDataReceived += (_, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                        errorBuilder.AppendLine(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();

                var result = outputBuilder.ToString();
                if (errorBuilder.Length > 0)
                    result += "Errors:\n" + errorBuilder;
                Debug.WriteLine(result);
                return result;

                
            }
            catch(Exception ex)
            {
                return $"ADB Error: {ex.Message}";
            }

        
    }

    public static bool VerifyAdbExists() => File.Exists(AdbPath);
}