using WinYara.Core.Models;

namespace WinYara.Core.Contracts.Services;

public interface IRulesDataService
{
    Task<IEnumerable<YaraRule>> GetListDetailsDataAsync();
}
