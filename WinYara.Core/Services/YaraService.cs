using System.Diagnostics;
using WinYara.Core.Contracts.Services;

namespace WinYara.Core.Services;

public class YaraService : IYaraService
{
    public string InstalledVersion { get; set; }
    public string RulesPath { get; set; } = Environment.CurrentDirectory;

    public string GetInstalledVersion()
    {
        if (InstalledVersion is not null)
        {
            return InstalledVersion;
        }

        if (!ExistsOnPath("yara.exe"))
        {
            return "Not installed";
        }

        var command = "yara -v && exit";
        var processInfo = new ProcessStartInfo
        {
            FileName = "cmd",
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };

        using var proc = new Process
        {
            StartInfo = processInfo
        };

        proc.Start();
        proc.StandardInput.WriteLine(command);

        var version = proc.StandardOutput.ReadToEnd().Split("\n")[^2];
        InstalledVersion = version;
        return version;
    }

    public bool ExistsOnPath(string fileName)
    {
        return GetFullPath(fileName) is not null;
    }

    public string GetFullPath(string fileName)
    {
        if (File.Exists(fileName))
        {
            return Path.GetFullPath(fileName);
        }

        var values = Environment.GetEnvironmentVariable("PATH");
        foreach (var path in values.Split(Path.PathSeparator))
        {
            var fullPath = Path.Combine(path, fileName);

            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        return null;
    }
}
