using System.Windows;
using System.Windows.Controls;
using FeelingFresh.UI.WPF.Helpers;

namespace FeelingFresh.UI.WPF.Controls
{
    /// <summary>
    /// Interaction logic for EditAppDialog.xaml
    /// </summary>
    public partial class EditAppDialog : UserControl
	{
		private string originalName;

		public EditAppDialog()
		{
			InitializeComponent();

			btnUpdate.IsEnabled = true;
		}

		public EditAppDialog(string appName) : this()
		{
			if (string.IsNullOrWhiteSpace(appName)) return;

			FillDetails(appName);
		}

		private void FillDetails(string appName)
		{
			originalName = appName;
			tbxAppName.Text = appName;

			btnUpdate.IsEnabled = false;
		}

		private void tbxAppName_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(tbxAppName.Text) || tbxAppName.Text == originalName) return;

			// Enable Update button
			btnUpdate.IsEnabled = true;
		}

		private void CloseDialog_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}


		private async void UpdateApp_Click(object sender, RoutedEventArgs e)
		{
			await DBHelper.UpdateApp(originalName, tbxAppName.Text);

			Window.GetWindow(this).Close();
		}
	}
}
