namespace WinYara.Core.Contracts.Services;

public interface IYaraService
{
    string InstalledVersion
    {
        get; set;
    }

    string RulesPath
    {
        get; set;
    }

    bool ExistsOnPath(string fileName);
    
    string GetFullPath(string fileName);
    
    string GetInstalledVersion();
}