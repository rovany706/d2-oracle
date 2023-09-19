using Avalonia.ReactiveUI;
using D2Oracle.ViewModels;
using D2Oracle.ViewModels.Dashboard;

namespace D2Oracle.Views.Dashboard;

public partial class CurrentStateInfoView : ReactiveUserControl<CurrentStateInfoViewModel>
{
    public CurrentStateInfoView()
    {
        InitializeComponent();
    }
}