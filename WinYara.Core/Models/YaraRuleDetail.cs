namespace WinYara.Core.Models;

public class YaraRuleDetail
{
    public IEnumerable<RuleKeyValuePair> Meta
    {
        get; set;
    }

    public IEnumerable<RuleKeyValuePair> Strings
    {
        get; set;
    }

    // TODO: The condition should be a tree
    public string Condition
    {
        get; set;
    }
}
