using FeelingFreshWPF.Helpers;
using System;
using System.Collections.Generic;
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

namespace Components
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
			await DbHelper.UpdateApp(originalName, tbxAppName.Text);

			Window.GetWindow(this).Close();
		}
	}
}
