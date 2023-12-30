using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.System;
using FeelingFresh.UI.WPF.Components;
using FeelingFresh.UI.WPF.Helpers;
using FeelingFresh.UI.WPF.Models;

namespace FeelingFresh.UI.WPF.Views;

public partial class MainWindow : Wpf.Ui.Controls.UiWindow
{
    ObservableCollection<Win32App> DesktopApps { get; set; } = new ObservableCollection<Win32App>();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void AppList_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadDesktopApps();
    }

    private async Task LoadDesktopApps()
    {
        DesktopApps = await DBHelper.GetApps();
        listViewAppList.ItemsSource = DesktopApps;

        SortList();
    }

    private async void AppList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if ((sender as ListView)?.SelectedItem is Win32App item)
        {
#pragma warning disable CA1416
            await Launcher.LaunchUriAsync(new Uri("https://duckduckgo.com/?q=!ducky+download+for+windows+" +
                                                  item.AppName));
#pragma warning restore CA1416
        }
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
        await LoadDesktopApps();

        listViewAppList.SelectedIndex = listViewAppList.Items.Count - 1;
        listViewAppList.ScrollIntoView(listViewAppList.SelectedItem);
    }

    private void SearchAndScroll(string appName)
    {
        if (DesktopApps.Any(x => x.AppName.ToLower() == appName.ToLower()))
        {
            // Select Index
            var item = DesktopApps.Where(x => x.AppName.ToLower() == appName.ToLower()).FirstOrDefault();
            listViewAppList.SelectedIndex = listViewAppList.Items.IndexOf(item);
            listViewAppList.ScrollIntoView(listViewAppList.SelectedItem);

            return;
        }
    }

    private void SortApps_Toggled(object sender, RoutedEventArgs e)
    {
        SortList();
    }

    private void SortList()
    {
        bool? isChecked = tggleBtnSort.IsChecked;

        listViewAppList.ItemsSource =
            (bool)isChecked ? DesktopApps.OrderBy(x => x.AppName) : DesktopApps.OrderBy(x => x.Id);

        if (listViewAppList.SelectedIndex != -1) listViewAppList.ScrollIntoView(listViewAppList.SelectedItem);
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
        var appName = (listViewAppList.SelectedItem as Win32App).AppName;

        await DBHelper.RemoveApp(appName);
        await LoadDesktopApps();
    }

    private void CustomMinimizeBtn_Click(object sender, RoutedEventArgs e)
    {
        App.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    private async void EditApp_Click(object sender, RoutedEventArgs e)
    {
        var appName = (listViewAppList.SelectedItem as Win32App).AppName;
        int currentIndex = listViewAppList.SelectedIndex;

        Window editDialog = new Window
        {
            ResizeMode = ResizeMode.NoResize,
            Title = "Edit App",
            Content = new EditAppDialog(appName),
            MaxWidth = 380,
            MaxHeight = 220
        };

        var d = editDialog.ShowDialog();
        await LoadDesktopApps();

        listViewAppList.SelectedIndex = currentIndex;
        listViewAppList.ScrollIntoView(listViewAppList.SelectedItem);
    }
}