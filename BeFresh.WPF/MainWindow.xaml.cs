using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Path = System.IO.Path;

namespace BeFresh.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const string RegKey =
        "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\CloudStore\\Store\\Cache\\DefaultAccount";

    private readonly string _oneDrivePersonal = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    private readonly string _layout = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                      "\\Microsoft\\Windows\\Shell";

    private readonly string _host = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                    "\\Packages\\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy\\LocalState";

    private readonly string _minecraft =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\";

    private readonly string _streamDeck =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Elgato\\StreamDeck\\";

    private readonly string? _temp = Environment.GetEnvironmentVariable("Temp");


    private const string FolderName = "BeFreshBackups";

    public MainWindow()
    {
        InitializeComponent();

        CreateBackupFolder();
    }

    private void CreateBackupFolder()
    {
        var path = Path.Combine(_oneDrivePersonal, FolderName);
        Console.WriteLine(path);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Button)?.Tag as string;

        if (string.IsNullOrWhiteSpace(tag)) return;

        switch (tag)
        {
            case "start-menu":
                BackupStartMenuLayout();
                break;
            case "minecraft":
                await BackupMinecraftFolder();
                break;
            case "streamdeck":
                await BackupElgatoStreamDeck();
                break;
        }
    }

    private async Task BackupElgatoStreamDeck()
    {
        // Close elgato stream deck
        btnStreamDeckBackup.IsEnabled = false;

        var path = Path.Combine(_oneDrivePersonal, FolderName) + "\\StreamDeck\\" +
                   DateTime.Now.ToString("yyyyMMdd_HHmmss");
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        try
        {
            CloseApp("StreamDeck");
            
            if (Directory.Exists(path.Trim()))
            {
                var fileNameInitial = $"{path}\\streamdeck.zip";

                var backupFileName = fileNameInitial;
                int count = 1;

                while (File.Exists(backupFileName))
                {
                    backupFileName = GenerateFileName(fileNameInitial, count++);
                }

                Console.WriteLine(backupFileName);
                await ScanFiles(_streamDeck, "streamdeck", backupFileName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            btnStreamDeckBackup.IsEnabled = true;

            StartApp("C:\\Program Files\\Elgato\\StreamDeck\\StreamDeck.exe");
        }
    }

    /// <summary>
    /// Kill app
    /// </summary>
    /// <param name="appname">app name without the .exe extension</param>
    private void CloseApp(string appname)
    {
        Process[] processes = Process.GetProcessesByName(appname);
        foreach (var process in processes)
        {
            process.Kill();
        }
    }

    private void StartApp(string exePath)
    {
        Process.Start(exePath);
    }

    private void BackupStartMenuLayout()
    {
        var path = Path.Combine(_oneDrivePersonal, FolderName) + "\\StartMenu\\" +
                   DateTime.Now.ToString("yyyyMMdd_HHmmss");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        try
        {
            var filePath = path + "\\DefaultAccount.reg";

            using var proc = new Process();
            proc.StartInfo.FileName = "reg.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;

            // Start Menu Registry
            proc.StartInfo.Arguments = $@"export ""{RegKey}"" ""{filePath}"" /y";
            proc.Start();
            proc.WaitForExit();
            proc.Close();

            // Start Menu Layout
            filePath = _layout + "\\DefaultLayouts.xml";
            Console.WriteLine(filePath);
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = $@"/C copy ""{filePath}"" ""{path}""";
            proc.Start();
            proc.WaitForExit();
            proc.Close();

            // Start bin
            filePath = _host + "\\start2.bin";
            Console.WriteLine(filePath);
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = $@"/C copy ""{filePath}"" ""{path}""";
            proc.Start();
            proc.WaitForExit();
            proc.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task BackupMinecraftFolder()
    {
        btnMinecraftBackup.IsEnabled = false;

        var path = Path.Combine(_oneDrivePersonal, FolderName) + "\\Minecraft\\" +
                   DateTime.Now.ToString("yyyyMMdd_HHmmss");
        Console.WriteLine(_minecraft);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        try
        {
            if (Directory.Exists(path.Trim()))
            {
                var fileNameInitial = $"{path}\\minecraft.zip";

                var backupFileName = fileNameInitial;
                int count = 1;

                while (File.Exists(backupFileName))
                {
                    backupFileName = GenerateFileName(fileNameInitial, count++);
                }

                Console.WriteLine(backupFileName);
                await ScanFiles(_minecraft, "minecraft", backupFileName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            btnMinecraftBackup.IsEnabled = true;
        }
    }

    private async Task ScanFiles(string path, string filename, string filePath)
    {
        var tempFileName = $"{_temp}\\{filename}.zip";
        var fsOut = File.Create(tempFileName);
        var zipStream = new ZipOutputStream(fsOut);
        zipStream.SetLevel(7); //0-9, 9 being the highest level of compression
        int folderOffset = (path).Length + ((path).EndsWith("\\") ? 0 : 1);

        await Task.Run(() => CompressFolder(path, zipStream, folderOffset));

        zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
        zipStream?.Close();

        if (File.Exists(tempFileName)) File.Move(tempFileName, filePath);
    }

    private void CompressFolder(string dataFolder, ZipOutputStream zipStream, int folderOffset)
    {
        Console.WriteLine(dataFolder);
        string[] files = Directory.GetFiles(dataFolder, ".", SearchOption.AllDirectories);

        foreach (string filename in files)
        {
            if (filename.Contains(@".minecraft\backups\") || filename.Contains(@".minecraft\assets\")) continue;

            Console.WriteLine(filename);

            try
            {
                FileInfo fi = new FileInfo(filename);

                string entryName =
                    string.Concat("\\", filename.AsSpan(folderOffset)); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                var newEntry = new ZipEntry(entryName)
                {
                    DateTime = fi.LastWriteTime, // Note the zip format stores 2 second granularity
                    // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                    // A password on the ZipOutputStream is required if using AES.
                    //   newEntry.AESKeySize = 256;
                    // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                    // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                    // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                    // but the zip will be in Zip64 format which not all utilities can understand.
                    //   zipStream.UseZip64 = UseZip64.Off;
                    Size = fi.Length
                };

                zipStream.PutNextEntry(newEntry);
                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];

                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }

                zipStream.CloseEntry();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }

    private string GenerateFileName(string fileNameInitial, int count)
    {
        return Path.GetDirectoryName(fileNameInitial) +
               Path.DirectorySeparatorChar +
               Path.GetFileNameWithoutExtension(fileNameInitial) +
               " (" + count.ToString() + ")" +
               Path.GetExtension(fileNameInitial);
    }

    private async void Backup_Click(object sender, RoutedEventArgs e)
    {
    }
}