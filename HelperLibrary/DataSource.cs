using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HelperLibrary
{
    public class DataSource
    {
        public string ImagePath { get; set; }
        public string AppName { get; set; }
        public string ExePath { get; set; }

        public DataSource(string name, string imgpath, string exe)
        {
            AppName = name;
            ImagePath = imgpath;
            ExePath = exe;
        }
    }

    public class UWPApp : IIsEnabled
    {
        // TO-DO: change PREFIX
        private string PREFIX = "ms-store:";

        public string AppName { get; set; }
        public string AppId { get; set; }
        public string AppIcon { get; set; }

        public UWPApp(string name, string id, string icon)
        {
            AppName = name;
            AppId = id;
            AppIcon = icon;
        }

        public bool IsEnabled => AppId != "";

        bool IIsEnabled.IsEnabled { get => AppId != ""; set { } }
    }

    public class LegacyApp
    {
        private string INSTALLERS = "E:\\Installers\\";

        public string AppName { get; set; }
        public string ImagePath { get; set; }
        public string ExePath { get; set; }

        public LegacyApp()
        {

        }

        public LegacyApp(string name, string exepath, string imgpath = "") 
        {
            AppName = name;
            ImagePath = imgpath;

            if (exepath.Contains("http") || name.Contains("Office") || name.Contains("Download Manager"))
                ExePath = exepath;  // Launch bound custom exePath
            else
                ExePath = INSTALLERS + exepath + ".exe"; // Concat installer file with Installer Path
        }
    }

    public class DriverItem
    {
        private string DRIVERS = "E:\\Drivers\\";

        public string AppName { get; set; }
        public string ExePath { get; set; }

        public DriverItem(string name, string exe)
        {
            AppName = name;
            ExePath = DRIVERS + exe + "\\Setup.exe";
        }

    }

    public class RegistryItem
    {
        private string PATH = @"D:\OneDrive\Personal\Fresh Install\";

        public string RegName { get; set; }
        public string RegPath { get; set; }

        public RegistryItem(string name, string path)
        {
            RegName = name;
            RegPath = PATH + path + ".reg";
        }
    }

    public class Vsix
    {
        private string PATH = @"D:\OneDrive\Personal\Fresh Install\VSIX\";

        public string VName { get; set; }
        public string VPath { get; set; }

        public Vsix(string name, string path)
        {
            VName = name;
            VPath = PATH + path + ".vsix";
        }
    }

    public class Cursor
    {
        private string PATH = @"D:\OneDrive\Personal\Fresh Install\Cursors\";

        public string CurName { get; set; }
        public string InfPath { get; set; }

        public Cursor(string name)
        {
            CurName = name;
            InfPath = PATH + name + "\\Install.inf";
        }
    }

    public class ItemsSource
    {
        private ObservableCollection<DriverItem> _drivers = new ObservableCollection<DriverItem>();
        private ObservableCollection<LegacyApp> _apps = new ObservableCollection<LegacyApp>();
        private ObservableCollection<UWPApp> _uwp = new ObservableCollection<UWPApp>();
        private ObservableCollection<Vsix> _vsix = new ObservableCollection<Vsix>();
        private ObservableCollection<Cursor> _mse = new ObservableCollection<Cursor>();
        private ObservableCollection<RegistryItem> _reg = new ObservableCollection<RegistryItem>();

        public ObservableCollection<DriverItem> Drivers()
        {
            _drivers.Add(new DriverItem("AMT Intel", @"1.AMT_Intel_11.0.0.1155_W10x64"));
            _drivers.Add(new DriverItem("Realtek Audio", @"Audio_Realtek_6.0.1.7564_W10x64"));
            _drivers.Add(new DriverItem("Bluetooth", @"Bluetooth_Atheros_10.0.1.0_W10x64_(NFA222 Liteon)"));
            _drivers.Add(new DriverItem("Card Reader", @"CardReader_Realtek_6.3.9600.31213_W10x64"));
            _drivers.Add(new DriverItem("Intel Chipset", @"Chipset_Intel_10.1.1.9_W10x64"));
            _drivers.Add(new DriverItem("Realtek LAN", @"lan_Realtek_10.1.505.2015_W10x64"));
            _drivers.Add(new DriverItem("Power Management", @"Power Management_7.00.8109_W10x64"));
            _drivers.Add(new DriverItem("Intel Graphics", @"VGA_Intel_10.18.15.4248_W10x64"));
            _drivers.Add(new DriverItem("Atheros Wireless", @"Wireless LAN_Atheros_12.0.0.102_W10x64_(NFA344 HAI)"));
            _drivers.Add(new DriverItem("AHCI Control", @"AHCI"));
            _drivers.Add(new DriverItem("Nvidia Graphics", @"Nvidia"));

            return _drivers;
        }

        public ObservableCollection<LegacyApp> DesktopApps()
        {
            _apps.Add(new LegacyApp("Google Chrome", "ChromeSetup_3", "/images/chrome.png"));
            _apps.Add(new LegacyApp("Visual Studio Community 2017", "https://goo.gl/55bHFD", "/images/vs.png"));
            _apps.Add(new LegacyApp("Visual Studio Code", "VSCodeSetup-x64-1.25.0", "/images/code.png"));
            _apps.Add(new LegacyApp("FileZilla", "FileZilla_3.34.0_win64-setup_bundled", "/images/npp.png"));
            _apps.Add(new LegacyApp("Git for Windows", "Git-2.18.0-64-bit", "/images/git.png"));
            _apps.Add(new LegacyApp("GitHub Desktop", "GitHubDesktopSetup", "/images/github.png"));
            _apps.Add(new LegacyApp("Microsoft Teams", "Teams_windows_x64", "/images/github.png"));
            _apps.Add(new LegacyApp("Netspeed Monitor", "netspeedmonitor_2_5_4_0_x64_setup", "/images/github.png"));
            _apps.Add(new LegacyApp("Internet Download Manager", "E:\\Torrents\\Internet.Download.Manager.v6.30.1.exe", "/images/github.png"));

            _apps.Add(new LegacyApp("Discord", "DiscordSetup", "/images/discord.png"));
            _apps.Add(new LegacyApp("GeForce Experience", "DiscordSetup", "/images/discord.png"));
            _apps.Add(new LegacyApp("Twitch Desktop", "TwitchSetup_[usher-48365501]", "/images/twitch.png"));
            _apps.Add(new LegacyApp("Open Broadcaster Software (OBS)", "OBS-Studio-21.1.2-Full-Installer", "/images/obs.png"));
            _apps.Add(new LegacyApp("Logitech G Hub", "lghub_installer_2018.7.2535", "/images/lgs.png"));
            _apps.Add(new LegacyApp("Logitech Options", "Options_6.80.372", "/images/lgs.png"));
            _apps.Add(new LegacyApp("Micorosft Office", "E:\\ISO\\O365HomePremRetail.img", "/images/lgs.png"));

            _apps.Add(new LegacyApp("uTorrent", "uTorrent", "/images/CHANGEME.png"));
            _apps.Add(new LegacyApp("VLC Media Player", "vlc-3.0.3-win64", "/images/CHANGEME.png"));
            _apps.Add(new LegacyApp("WinRAR", "winrar-x64-560", "/images/CHANGEME.png"));
            _apps.Add(new LegacyApp("TeamViewer", "TeamViewer_Setup", "/images/CHANGEME.png"));
            _apps.Add(new LegacyApp("Android Messages", "android-messages-desktop-setup-0.3.0", "/images/CHANGEME.png"));
            _apps.Add(new LegacyApp("Spotify", "android-messages-desktop-setup-0.3.0", "/images/CHANGEME.png"));
            _apps.Add(new LegacyApp("Huion H950P Drivers", "android-messages-desktop-setup-0.3.0", "/images/CHANGEME.png"));
            _apps.Add(new LegacyApp("Viber Desktop", "android-messages-desktop-setup-0.3.0", "/images/CHANGEME.png"));


            return _apps;
        }

        public ObservableCollection<UWPApp> StoreApps()
        {
            //TO-DO, add new UWP apps (refer to OneNote and new Red David apps)
            _uwp.Add(new UWPApp("Trello", "9nblggh4xxvw", ""));
            _uwp.Add(new UWPApp("Spotify", "9ncbcszsjrsb", ""));
            _uwp.Add(new UWPApp("Microsoft News", "9wzdncrfhvfw", ""));
            _uwp.Add(new UWPApp("Character Map UWP", "9wzdncrdxf41", ""));
            _uwp.Add(new UWPApp("Musixmatch", "9wzdncrfj235", ""));
            _uwp.Add(new UWPApp("Notifications Visualizer", "9nblggh5xsl1", ""));
            _uwp.Add(new UWPApp("QuickLook", "9nv4bs3l1h4s", "")); 
            _uwp.Add(new UWPApp("RegEx Toolkit", "9nblggh59x8h", ""));
            _uwp.Add(new UWPApp("UWP DevKit", "9nblggh5p90f", ""));
            _uwp.Add(new UWPApp("UWP Technical Guide", "9nblggh5241d", ""));
            _uwp.Add(new UWPApp("Windows Community Toolkit Sample App", "9nblggh4tlcq", ""));
            _uwp.Add(new UWPApp("Microsoft To-Do", "9nblggh5r558", "")); 
            _uwp.Add(new UWPApp("XAML Controls Gallery", "9msvh128x2zt", ""));

            _uwp.Add(new UWPApp("Steam ID Viewer", "9nblggh4rp95", "/Assets/steamidviewer.png"));
            _uwp.Add(new UWPApp("BTSharer", "9pmfn8938cx5", "/Assets/steamidviewer.png")); 
            _uwp.Add(new UWPApp("Logitech Profiles Manager", "9ntxwhlbh9jb", "/Assets/steamidviewer.png"));  
            _uwp.Add(new UWPApp("ZIP Code PH", "9nblggh5gft6", "/Assets/zipcodeph.png"));
            _uwp.Add(new UWPApp("Symbol Icon Finder", "9p650nf68j50", "/Assets/zipcodeph.png")); 
            _uwp.Add(new UWPApp("SIM Net PH", "9pbwj21r2mzn", "/Assets/simnetph.png"));
            _uwp.Add(new UWPApp("1987 PH Constitution", "9nvl993hgmtx", "/Assets/constiph.png"));
            _uwp.Add(new UWPApp("The Prayer Book", "9nblgggzgk43", "/Assets/theprayerbook.png"));
            _uwp.Add(new UWPApp("See-Through Tiles", "9nblggh4pttp", "/Assets/seethroughtiles.png"));
            _uwp.Add(new UWPApp("URLtoSMS", "9nblggh4qlv0", "/Assets/urltosms.png"));
            _uwp.Add(new UWPApp("Clearer", "9pc903fgnm61", "/Assets/clear.png"));
            _uwp.Add(new UWPApp("Tagalog Bible", "9p4n369p108d", "/Assets/tagalogbible.png"));
            _uwp.Add(new UWPApp("RF Tools", "9nblggh41btt", "/Assets/rftools.png"));
            _uwp.Add(new UWPApp("Each Time", "9PMSR1QHK32F", "/Assets/eachtime2.png"));
            _uwp.Add(new UWPApp("MMDA Traffic Navigator", "9NBLGGH4R7MS", "/Assets/mtv.png"));
            _uwp.Add(new UWPApp("SubQuests", "9NPLBP33N95T", "/Assets/subquests.png"));
            _uwp.Add(new UWPApp("RPN Internet Radio", "9PHL469C0Z20", "/Assets/subquests.png"));


            return _uwp;
        }

        public ObservableCollection<RegistryItem> RegKeys()
        {
            _reg.Add(new RegistryItem("Add \"Take Ownership\" to Context Menu", "Add Take Ownership to Context menu"));
            _reg.Add(new RegistryItem("Enable Last Active Click", "Enable Last Active Click"));
            _reg.Add(new RegistryItem("Show Seconds in System Clock", "Show Seconds In System Clock"));

            return _reg;
        }

        public ObservableCollection<Vsix> VSExtensions()
        {
            _vsix.Add(new Vsix("Add New File", "Add New File v3.5.129"));
            _vsix.Add(new Vsix("Markdown Editor", "Add New File v3.5.129"));
            _vsix.Add(new Vsix("Image Optimizer", "Image_Optimizer_v4.0.131"));
            _vsix.Add(new Vsix("IntelliCode", "Image_Optimizer_v4.0.131"));
            _vsix.Add(new Vsix("Open in Visual Studio Code", "Open in Visual Studio Code v1.4.29"));
            _vsix.Add(new Vsix("VS Color Themes", "VSColorThemes"));
            _vsix.Add(new Vsix("XAML Styler", "XamlStyler3.Package (1)"));
            _vsix.Add(new Vsix("Code Maid", "CodeMaid_v10.5.119"));

            return _vsix;
        }

        public ObservableCollection<Cursor> Cursors()
        {
            _mse.Add(new Cursor("Android Material Blue"));
            _mse.Add(new Cursor("El Capitan"));
            _mse.Add(new Cursor("ML Blau"));
            _mse.Add(new Cursor("Simplify Dark"));
            _mse.Add(new Cursor("Simplify Light"));
            _mse.Add(new Cursor("Ubuntu"));

            return _mse;
        }

    }



}
