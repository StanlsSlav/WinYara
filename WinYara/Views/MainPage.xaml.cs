using Microsoft.UI.Xaml.Controls;
using WinYara.ViewModels;

namespace WinYara.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
