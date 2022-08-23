using System.Text.RegularExpressions;
using WinYara.Core.Models;

namespace WinYara.Core.Helpers;

public class RuleParser
{
    private static readonly RegexOptions defaultRegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;

    private readonly Regex _ruleName =
        new(@"(?<=rule\s+)[\w]+", defaultRegexOptions);

    private readonly Regex _ruleTags =
        new(@"(?<=:).*(?=\s*{)", defaultRegexOptions);

    private readonly Regex _ruleMetas =
        new($@"(?<{nameof(RuleKeyValuePair.Key)}>[^\$\s]\w+)\s*=\s*""(?<{nameof(RuleKeyValuePair.Value)}>[^""]*)""", defaultRegexOptions);

    private readonly Regex _ruleAuthor =
        new(@"(?<=author\s*=\s*"")[^""]+(?="")", defaultRegexOptions);

    private readonly Regex _ruleStrings =
        new($@"(?<{nameof(RuleKeyValuePair.Key)}>\$\w+)\s*=\s*""(?<{nameof(RuleKeyValuePair.Value)}>[^""]*)""", defaultRegexOptions);

    private readonly Regex _ruleVersion =
        new(@"(?<=version\s*=\s*"")[^""]+(?="")", defaultRegexOptions);

    private readonly Regex _ruleCondition =
        new(@"(?<=condition:\s+)\b.+\b", defaultRegexOptions);

    private readonly Regex _ruleDescription =
        new(@"(?<=description\s*=\s*"")[^""]*", defaultRegexOptions);

    private readonly Regex _ruleShortDescription =
        new(@"(?<=(short_)?desc\s*=\s*"")[^""]*", defaultRegexOptions);

    public string Content
    {
        get; set;
    }

    public string GetRuleName()
    {
        return GetValueForRegex(_ruleName);
    }

    public string GetRuleAuthor()
    {
        return GetValueForRegex(_ruleAuthor);
    }

    public string GetRuleVersion()
    {
        return GetValueForRegex(_ruleVersion);
    }

    public string GetRuleCondition()
    {
        return GetValueForRegex(_ruleCondition);
    }

    public string GetRuleDescription()
    {
        return GetValueForRegex(_ruleDescription);
    }

    public string GetRuleShortDescription()
    {
        return GetValueForRegex(_ruleShortDescription);
    }

    public IEnumerable<string> GetRuleTags()
    {
        // TODO: Make a proper expression
        return GetValueForRegex(_ruleTags)
            .Split("; ")
            .SelectMany(x => x.Split(" "))
            .Where(x => x != "")
            .Select(x => x.Trim());
    }

    public IEnumerable<RuleKeyValuePair> GetRuleMeta()
    {
        var rawMetas = GetPairValuesForRegex(_ruleMetas);
        var metas = new List<RuleKeyValuePair>();
        var expressionsToIgnore = new[] { _ruleAuthor, _ruleVersion, _ruleDescription, _ruleShortDescription };

        for (var i = 0; i < rawMetas.Count(); i++)
        {
            var metaData = rawMetas.Skip(i).First();
            var metaKey = metaData.Key;
            var metaValue = metaData.Value;

            if (!expressionsToIgnore.Any(x => x.IsMatch($@"{metaKey}=""{metaValue}""")))
            {
                metas.Add(metaData);
            }
        }

        return metas;
    }

    public IEnumerable<RuleKeyValuePair> GetRuleStrings()
    {
        return GetPairValuesForRegex(_ruleStrings);
    }

    public string GetValueForRegex(Regex expression)
    {
        if (string.IsNullOrWhiteSpace(Content) || string.IsNullOrWhiteSpace(expression.ToString()))
        {
            return "";
        }

        var matches = expression.Matches(Content);

        if (matches.Count > 1)
        {
            return string.Join("; ", matches);
        }

        var value = matches.FirstOrDefault()?.Value;
        return value is null ? "" : value;
    }

    public IEnumerable<RuleKeyValuePair> GetPairValuesForRegex(Regex expression)
    {
        if (string.IsNullOrWhiteSpace(Content) || string.IsNullOrWhiteSpace(expression.ToString()))
        {
            return Enumerable.Empty<RuleKeyValuePair>();
        }

        var matches = expression.Matches(Content);
        var pairs = new List<RuleKeyValuePair>();

        for (var i = 0; i < matches.Count; i++)
        {
            var match = matches[i];

            var key = match.Groups[nameof(RuleKeyValuePair.Key)].Value;
            var val = match.Groups[nameof(RuleKeyValuePair.Value)].Value;

            pairs.Add(new RuleKeyValuePair
            {
                Key = key,
                Value = string.IsNullOrEmpty(val) ? " " : val
            });
        }

        return pairs;
    }
}
