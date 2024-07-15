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
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft";

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

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Button)?.Tag as string;

        if (string.IsNullOrWhiteSpace(tag)) return;

        if (tag.Equals("start-menu"))
        {
            BackupStartMenuLayout();
        }
        else if (tag.Equals("minecraft"))
        {
            BackupMinecraftFolder();
        }
    }

    private void BackupMinecraftFolder()
    {
        // Scan folder
        string path = MC_SAVES_FOLDER + selectedSaveName;
        if (Directory.Exists(path.Trim()))
        {
            string fileNameInitial = string.Empty;

            // If OneDrive is not found, save in Documents folder
            if (!String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OneDrive")))
            {
                fileNameInitial =
                    $"{ONEDRIVE_FOLDER}\\{selectedSaveName}_{Environment.MachineName}_{DateTime.Now:MMddyy}.zip";
            }
            else if (!String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OneDriveConsumer")))
            {
                fileNameInitial =
                    $"{ONEDRIVECONSUMER_FOLDER}\\{selectedSaveName}_{Environment.MachineName}_{DateTime.Now:MMddyy}.zip";
            }
            else
            {
                fileNameInitial =
                    $"{DOCS_FOLDER}\\{selectedSaveName}_{Environment.MachineName}_{DateTime.Now:MMddyy}.zip";
            }

            backupFileName = fileNameInitial;
            int count = 1;

            while (File.Exists(backupFileName))
            {
                backupFileName = GenerateFileName(fileNameInitial, count++);
            }

            await ScanFiles(backupFileName);
        }
    }

    private void BackupStartMenuLayout()
    {
        var path = Path.Combine(_oneDrivePersonal, FolderName) + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
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

    private async Task ScanFiles(string filename)
    {
        tempFileName = $"{TEMP_FOLDER}\\" + filename.Substring(filename.IndexOf(selectedSaveName.Trim() + "_"),
            filename.Length - filename.IndexOf(selectedSaveName.Trim() + "_"));

        var fsOut = File.Create(tempFileName);
        pBar.IsIndeterminate = isBusy = true;

        zipStream = new ZipOutputStream(fsOut);
        zipStream.SetLevel(5); //0-9, 9 being the highest level of compression
        int folderOffset = (MC_SAVES_FOLDER + selectedSaveName).Length +
                           ((MC_SAVES_FOLDER + selectedSaveName).EndsWith("\\") ? 0 : 1);

        await Task.Run(() => CompressFolder((MC_SAVES_FOLDER + selectedSaveName), zipStream, folderOffset));

        zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
        zipStream?.Close();

        pBar.IsIndeterminate = isBusy = false;

        if (File.Exists(tempFileName)) File.Move(tempFileName, filename);

        CompleteDialog.Show();
        //MessageBox.Show("Export complete.");

        BackupBtn.Content = "Start Backup";
    }

    private void CompressFolder(string DATA_FOLDER, ZipOutputStream zipStream, int folderOffset)
    {
        string[] files = Directory.GetFiles(DATA_FOLDER, ".", SearchOption.AllDirectories);

        foreach (string filename in files)
        {
            try
            {
                FileInfo fi = new FileInfo(filename);

                string entryName =
                    @"\" + selectedSaveName.Trim() + "\\" +
                    filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;
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