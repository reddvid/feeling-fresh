using Components;
using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.System;
using Windows.UI.Popups;

namespace FeelingFreshWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ObservableCollection<LegacyApp> DesktopApps { get; set; } = new ObservableCollection<LegacyApp>();
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
		}

		private async void AppList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var item = (sender as ListView).SelectedItem as LegacyApp;

			if (item != null)
			{
#pragma warning disable CA1416 // Validate platform compatibility
				await Launcher.LaunchUriAsync(new Uri("https://duckduckgo.com/?q=!ducky+" + item.AppName));
#pragma warning restore CA1416 // Validate platform compatibility
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
			bool? isChecked = tggleBtnSort.IsChecked;

			listViewAppList.ItemsSource = (bool)isChecked ? DesktopApps.OrderBy(x => x.AppName) : DesktopApps.OrderBy(x => x.AppId);

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
			var appName = (listViewAppList.SelectedItem as LegacyApp).AppName;

			await DBHelper.RemoveApp(appName);
			await LoadDesktopApps();
		}

		private void CustomMinimizeBtn_Click(object sender, RoutedEventArgs e)
		{
			App.Current.MainWindow.WindowState = WindowState.Minimized;
		}

		private async void EditApp_Click(object sender, RoutedEventArgs e)
		{
			var appName = (listViewAppList.SelectedItem as LegacyApp).AppName;

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
		}
	}
}
