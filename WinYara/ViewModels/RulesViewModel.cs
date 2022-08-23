using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using WinYara.Contracts.ViewModels;
using WinYara.Core.Contracts.Services;
using WinYara.Core.Models;

namespace WinYara.ViewModels;

public partial class RulesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRulesDataService _yaraDataService;

    [ObservableProperty]
    private YaraRule? _selected;

    public ObservableCollection<YaraRule> RuleItems { get; } = new();

    public RulesViewModel(IRulesDataService yaraDataService)
    {
        _yaraDataService = yaraDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        RuleItems.Clear();
        var data = await _yaraDataService.GetListDetailsDataAsync();

        foreach (var item in data)
        {
            RuleItems.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        Selected ??= RuleItems.FirstOrDefault();
    }
}
