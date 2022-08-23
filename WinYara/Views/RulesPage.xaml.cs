using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using WinYara.ViewModels;

namespace WinYara.Views;

public sealed partial class RulesPage : Page
{
    public RulesViewModel ViewModel
    {
        get;
    }

    public RulesPage()
    {
        ViewModel = App.GetService<RulesViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
