using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Windows.System;
using FeelingFresh.UI.WPF.Controls;
using FeelingFresh.UI.WPF.Helpers;
using FeelingFresh.UI.WPF.Models;
using FeelingFresh.UI.WPF.ViewModels;

namespace FeelingFresh.UI.WPF.Views;

public partial class MainWindow : Wpf.Ui.Controls.UiWindow
{
    ObservableCollection<Win32App> DesktopApps { get; set; } = new ObservableCollection<Win32App>();
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void AppList_Loaded(object sender, RoutedEventArgs e)
    {
        await ((DataContext as MainViewModel)!).GetDataCommand.ExecuteAsync(default);
        
        SortList();
    }

    private async void AppList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var item = (DataContext as MainViewModel).SelectedItem;

        await Launcher.LaunchUriAsync(
            new Uri($"https://duckduckgo.com/?q=!ducky+download+for+windows+{item.AppName}"));
    }

    private void CloseWindow_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void AddApp_Click(object sender, RoutedEventArgs e)
    {
        if (String.IsNullOrEmpty(tbxInput.Text) || DesktopApps == null || DesktopApps.Count == 0) return;

        var appName = tbxInput.Text;

        if (DesktopApps.Any(x => x.AppName.ToLower() == appName.ToLower()))
        {
            // Select Index
            SearchAndScroll(appName);
        }

        await DBHelper.AddApp(appName);
        // await LoadDesktopApps();

        AppList.SelectedIndex = AppList.Items.Count - 1;
        AppList.ScrollIntoView(AppList.SelectedItem);
    }

    private void SearchAndScroll(string appName)
    {
        if (DesktopApps.Any(x => x.AppName.ToLower() == appName.ToLower()))
        {
            // Select Index
            var item = DesktopApps.Where(x => x.AppName.ToLower() == appName.ToLower()).FirstOrDefault();
            AppList.SelectedIndex = AppList.Items.IndexOf(item);
            AppList.ScrollIntoView(AppList.SelectedItem);

            return;
        }
    }

    private void SortApps_Toggled(object sender, RoutedEventArgs e)
    {
        SortList();
    }

    private void SortList()
    {
        if (AppList.SelectedIndex != -1) AppList.ScrollIntoView(AppList.SelectedItem);
    }

    private void SearchApp_Click(object sender, RoutedEventArgs e)
    {
        if (String.IsNullOrEmpty(tbxInput.Text) || DesktopApps == null || DesktopApps.Count == 0) return;

        var appName = tbxInput.Text;

        if (DesktopApps.Any(x => x.AppName.ToLower() == appName.ToLower()))
        {
            // Select Index
            SearchAndScroll(appName);
        }
    }

    private async void DeleteApp_Click(object sender, RoutedEventArgs e)
    {
        var appName = (AppList.SelectedItem as Win32App).AppName;

        await DBHelper.RemoveApp(appName);
        // await LoadDesktopApps();
    }

    private void CustomMinimizeBtn_Click(object sender, RoutedEventArgs e)
    {
        App.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    private async void EditApp_Click(object sender, RoutedEventArgs e)
    {
        var appName = (AppList.SelectedItem as Win32App).AppName;
        int currentIndex = AppList.SelectedIndex;

        Window editDialog = new Window
        {
            ResizeMode = ResizeMode.NoResize,
            Title = "Edit App",
            Content = new EditAppDialog(appName),
            MaxWidth = 380,
            MaxHeight = 220
        };

        var d = editDialog.ShowDialog();
        // await LoadDesktopApps();

        AppList.SelectedIndex = currentIndex;
        AppList.ScrollIntoView(AppList.SelectedItem);
    }

    private void SearchBox_QuerySubmitted(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}