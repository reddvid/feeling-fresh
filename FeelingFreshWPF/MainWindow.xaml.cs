using HelperLibrary;
using System;
using System.Collections.Generic;
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

namespace FeelingFreshWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FONTS_PATH = @"D:\OneDrive\Personal\Fonts\";
        private string CURSORS_PATH = @"D:\OneDrive\Personal\Fresh Install\Cursors\";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start(@"E:\Drivers\1.AMT_Intel_11.0.0.1155_W10x64\SetupME.exe");

            var btn = sender as Button;

            switch (btn.Tag)
            {
                case "wizard":
                    Process.Start(FONTS_PATH + "Fonts.lnk");
                    break;

                case "folder":
                    Process.Start(FONTS_PATH);
                    break;

                case "cursors":
                    Process.Start(CURSORS_PATH + "Mouse Pointers.lnk");
                    break;
            }
        }

        private void DriversView_Loaded(object sender, RoutedEventArgs e)
        {
            DriversView.ItemsSource = new ItemsSource().Drivers();
        }

        private void DriversView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = (sender as ListView).SelectedItem as DriverItem;

                if (item != null)
                {
                    Process.Start(item.ExePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void DesktopAppsView_Loaded(object sender, RoutedEventArgs e)
        {
            DesktopAppsView.ItemsSource = new ItemsSource().DesktopApps();
        }

        private void DesktopAppsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as LegacyApp;

            if (item != null)
            {
                Process.Start("https://www.google.com/search?q=download+" + item.AppName + "&btnI");
                //Debug.WriteLine(item.ExePath);

                //if (item.AppName == "Visual Studio Community 2017")
                //    Process.Start(@"E:\VS2017\vs2017workloads\vs_Community.exe", @"--add Microsoft.VisualStudio.Workload.Universal --add Microsoft.VisualStudio.Workload.ManagedDesktop --add Microsoft.VisualStudio.Workload.NetWeb --add Component.GitHub.VisualStudio --includeOptional");
                //else
                //{
                //    Process.Start(item.ExePath);
                //}
            }
        }

        private void VsixView_Loaded(object sender, RoutedEventArgs e)
        {
            VsixView.ItemsSource = new ItemsSource().VSExtensions();
        }

        private void VsixView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as Vsix;

            if (item != null)
            {
                Process.Start(item.VPath);
            }
        }

        private void RegView_Loaded(object sender, RoutedEventArgs e)
        {
            RegView.ItemsSource = new ItemsSource().RegKeys();
        }

        private void RegView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as RegistryItem;

            if (item != null)
            {
                Debug.WriteLine(item.RegPath);

                Process.Start(item.RegPath);
            }
        }

        private void CursorsView_Loaded(object sender, RoutedEventArgs e)
        {
            CursorsView.ItemsSource = new ItemsSource().Cursors();
        }

        private void CursorsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as HelperLibrary.Cursor;

            if (item != null)
            {
                Debug.WriteLine(item.InfPath);

                Process.Start("rundll32.exe", @"setupapi, InstallHinfSection DefaultInstall 132 " + item.InfPath);
            }
        }

        private void UWPAppsView_Loaded(object sender, RoutedEventArgs e)
        {
            UWPAppsView.ItemsSource = new ItemsSource().StoreApps();
        }

        private void UWPAppsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as UWPApp;

            if (item != null)
            {
                Debug.WriteLine(item.AppName);

                Process.Start("ms-windows-store://pdp/?productid=" + item.AppId);
            }
        }

        private void CustomCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
