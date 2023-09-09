using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using D2Oracle.ViewModels;

namespace D2Oracle.Views;

public partial class CurrentStateInfoView : ReactiveUserControl<CurrentStateInfoViewModel>
{
    public CurrentStateInfoView()
    {
        InitializeComponent();
    }
}