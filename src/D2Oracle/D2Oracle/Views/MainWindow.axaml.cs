using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using D2Oracle.ViewModels;
using Material.Icons;
using SukiUI.Controls;

namespace D2Oracle.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        
        // this.FindControl<Grid>("SideMenu")?.Children.Add(new SideMenu
        // {
        //     DataContext = new SideMenuModel
        //     {
        //         HeaderContentOverlapsToggleSidebarButton = true,
        //         HeaderContent = new Grid
        //         {
        //             Margin = new Thickness(10),
        //             VerticalAlignment = VerticalAlignment.Center,
        //             HorizontalAlignment = HorizontalAlignment.Center,
        //             Children =
        //             {
        //                 new TextBlock
        //                 {
        //                     Text = D2Oracle.Resources.AppName,
        //                     FontSize = 18,
        //                     FontWeight = FontWeight.Bold
        //                 }
        //             }
        //         },
        //         MenuItems = new ObservableCollection<SideMenuItem>
        //         {
        //             new()
        //             {
        //                 Icon = MaterialIconKind.CircleOutline,
        //                 Header = "Dashboard",
        //                 Content = viewModel.DashboardViewModel
        //             }
        //         },
        //         FooterMenuItems = new List<SideMenuItem>
        //         {
        //             new()
        //             {
        //                 Icon = MaterialIconKind.Settings,
        //                 Header = "Settings",
        //                 Content = viewModel.SettingsViewModel
        //             }
        //         }
        //     }
        // });
    }
}