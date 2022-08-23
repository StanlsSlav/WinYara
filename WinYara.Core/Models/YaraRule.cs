namespace WinYara.Core.Models;

public class YaraRule
{
    public string Name
    {
        get; set;
    }

    public string Author
    {
        get; set;
    }

    public string Version
    {
        get; set;
    }

    public string ShortDescription
    {
        get; set;
    }

    public string Description
    {
        get; set;
    }

    public IEnumerable<string> Tags
    {
        get; set;
    }

    public DateTime CreationDate
    {
        get; set;
    }

    public DateTime LastModified
    {
        get; set;
    }

    public YaraRuleDetail Body
    {
        get; set;
    }

    public int SymbolCode
    {
        get; set;
    }

    public string SymbolName
    {
        get; set;
    }

    public char Symbol => (char)SymbolCode;
}
