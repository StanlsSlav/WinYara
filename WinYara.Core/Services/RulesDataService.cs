using WinYara.Core.Contracts.Services;
using WinYara.Core.Helpers;
using WinYara.Core.Models;

namespace WinYara.Core.Services;

public class RulesDataService : IRulesDataService
{
    private readonly IYaraService _yaraService;

    private List<YaraRule> _allRules = new();

    public RulesDataService(IYaraService yaraService)
    {
        _yaraService = yaraService;
    }

    private IEnumerable<YaraRule> AllRules()
    {
        if (_allRules.Any())
        {
            _allRules.Clear();
        }

        foreach (var file in GetAllRulesFiles())
        {
            var parser = new RuleParser();

            using (StreamReader stream = new(file.OpenRead()))
            {
                parser.Content = stream.ReadToEnd();
            }

            var rule = new YaraRule
            {
                Tags = parser.GetRuleTags(),
                Name = parser.GetRuleName(),
                Author = parser.GetRuleAuthor(),
                Version = parser.GetRuleVersion(),
                Description = parser.GetRuleDescription(),
                ShortDescription = parser.GetRuleShortDescription(),
                Body = new YaraRuleDetail
                {
                    Condition = parser.GetRuleCondition(),
                    Meta = parser.GetRuleMeta(),
                    Strings = parser.GetRuleStrings()
                },
                CreationDate = file.CreationTime,
                LastModified = file.LastWriteTime,
            };

            _allRules.Add(rule);
        }

        return _allRules;
    }

    public IReadOnlyList<FileInfo> GetAllRulesFiles()
    {
        var fileInfos = new List<FileInfo>();
        var rulesPath = _yaraService.RulesPath;

        foreach (var fileName in Directory.GetFiles(rulesPath)
            .Where(x => x.EndsWith("yara", StringComparison.CurrentCultureIgnoreCase)))
        {
            fileInfos.Add(new(Path.Combine(rulesPath, fileName)));
        }

        return fileInfos;
    }

    public async Task<IEnumerable<YaraRule>> GetListDetailsDataAsync()
    {
        _allRules = new(AllRules());

        await Task.CompletedTask;
        return _allRules;
    }
}
