using Avalonia.ReactiveUI;
using D2Oracle.ViewModels;

namespace D2Oracle.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}