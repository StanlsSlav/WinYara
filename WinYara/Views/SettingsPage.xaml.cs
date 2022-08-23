using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;

using WinYara.ViewModels;

namespace WinYara.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private void OpenRulesDirectory_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Process.Start("explorer", ViewModel.YaraRulesPath);
    }
}
