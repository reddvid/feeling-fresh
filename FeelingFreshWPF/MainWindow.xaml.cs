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

        private async void DesktopAppsView_Loaded(object sender, RoutedEventArgs e)
        {
            DesktopAppsView.ItemsSource = await DBHelper.DesktopApps();
        }

        private void DesktopAppsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as LegacyApp;

            if (item != null)
            {
                Process.Start("https://duckduckgo.com/?q=!ducky+" + item.AppName);
                //Debug.WriteLine(item.ExePath);

                //if (item.AppName == "Visual Studio Community 2017")
                //    Process.Start(@"E:\VS2017\vs2017workloads\vs_Community.exe", @"--add Microsoft.VisualStudio.Workload.Universal --add Microsoft.VisualStudio.Workload.ManagedDesktop --add Microsoft.VisualStudio.Workload.NetWeb --add Component.GitHub.VisualStudio --includeOptional");
                //else
                //{
                //    Process.Start(item.ExePath);
                //}
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
