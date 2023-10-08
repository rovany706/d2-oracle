using Avalonia.ReactiveUI;
using D2Oracle.Core.ViewModels;

namespace D2Oracle.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}