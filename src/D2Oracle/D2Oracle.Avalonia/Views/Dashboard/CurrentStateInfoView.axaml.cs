using Avalonia.ReactiveUI;
using D2Oracle.Core.ViewModels.Dashboard;

namespace D2Oracle.Avalonia.Views.Dashboard;

public partial class CurrentStateInfoView : ReactiveUserControl<CurrentStateInfoViewModel>
{
    public CurrentStateInfoView()
    {
        InitializeComponent();
    }
}