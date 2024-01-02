using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.System;
using FeelingFresh.UI.WPF.Controls;
using FeelingFresh.UI.WPF.Helpers;
using FeelingFresh.UI.WPF.Models;
using FeelingFresh.UI.WPF.ViewModels;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace FeelingFresh.UI.WPF.Views;

public partial class MainWindow : Wpf.Ui.Controls.UiWindow
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void AppList_Loaded(object sender, RoutedEventArgs e)
    {
        await ((DataContext as MainViewModel)!).GetAppsCommand.ExecuteAsync(default);

        SortList();
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private async void AppList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var item = (DataContext as MainViewModel)?.SelectedItem;

        await Launcher.LaunchUriAsync(
            new Uri($"https://duckduckgo.com/?q=!ducky+download+for+windows+{item!.AppName}"));
    }


    private void SearchAndScroll(string appName)
    {
        if (listViewApps.Items.Cast<Win32App>().Any(x => (x?.AppName!.ToLower()!).Equals(appName, StringComparison.CurrentCultureIgnoreCase)))
        {
            // Select Index
            var item = listViewApps.Items.Cast<Win32App>().FirstOrDefault(x => string.Equals(x!.AppName!, appName, StringComparison.CurrentCultureIgnoreCase));
            listViewApps.SelectedIndex = listViewApps.Items.IndexOf(item!);
            listViewApps.ScrollIntoView(listViewApps.SelectedItem);
    
            return;
        }
    }

    private void ToggleButtonSortAppList_Toggled(object sender, RoutedEventArgs e)
    {
        SortList();
    }

    private void SortList()
    {
        listViewApps.ScrollIntoView(listViewApps.SelectedIndex != -1 ? listViewApps.SelectedItem : 0);
    }

    private async void DeleteApp_Click(object sender, RoutedEventArgs e)
    {
        var appName = (listViewApps.SelectedItem as Win32App).AppName;

        await DBHelper.RemoveApp(appName);
        // await LoadDesktopApps();
    }

    private async void EditApp_Click(object sender, RoutedEventArgs e)
    {
        var appName = (listViewApps.SelectedItem as Win32App).AppName;
        int currentIndex = listViewApps.SelectedIndex;

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

        listViewApps.SelectedIndex = currentIndex;
        listViewApps.ScrollIntoView(listViewApps.SelectedItem);
    }

    private void ButtonSearchApp_Clicked(object sender, RoutedEventArgs e)
    {
        (DataContext as MainViewModel).SearchAppCommand.Execute(default);

        if ((DataContext as MainViewModel).SelectedItem is not null)
            SearchAndScroll((DataContext as MainViewModel).SelectedItem.AppName);
    }

    private async void AddApp_Click(object sender, RoutedEventArgs e)
    {
        (DataContext as MainViewModel).CheckAppCommand.Execute(default);

        if ((DataContext as MainViewModel).SelectedItem is not null)
        {
            SearchAndScroll((DataContext as MainViewModel).SelectedItem.AppName);
            return;
        }

        var dialog = new MessageBox();
        dialog.Title = "Add App";
        dialog.Content = $"Do you want to add {inputTextbox.Text}?";
        dialog.ButtonLeftName = "Yes";
        dialog.ButtonRightName = "No";
        dialog.ButtonLeftClick += DialogOnButtonLeftClick;
        dialog.ButtonRightClick += DialogOnButtonRightClick;

        dialog.ShowDialog();
       
        // Select to last item
        listViewApps.SelectedIndex = listViewApps.Items.Count - 1;
        listViewApps.ScrollIntoView(listViewApps.SelectedItem);
    }

    private void DialogOnButtonRightClick(object sender, RoutedEventArgs e)
    {
        (sender as MessageBox)?.Close();
    }

    private async void DialogOnButtonLeftClick(object sender, RoutedEventArgs e)
    {
        (DataContext as MainViewModel)?.AddAppCommand.Execute(default);
        
        // Refresh list view
        await (DataContext as MainViewModel)!.GetAppsCommand.ExecuteAsync(default);
        
        // Sort
        (DataContext as MainViewModel)!.SortAppsCommand.Execute(default);
        
        (sender as MessageBox)?.Close();
    }
}