using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Compression;
using SteraLauncher.Services;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace SteraLauncher.Pages
{
    public partial class Home : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string NewsUrl = "";

        private string Username;
        public Home()
        {
            InitializeComponent();
            LoadNewsAsync();
            Username = UpdateINI.ReadValue("User", "Username") ?? "Guest";
            UserNameBox.Text = $"Welcome Back, {Username}!";
        }
        private async void LoadNewsAsync()
        {
            try
            {
                // Download the news file
                string newsData = await _httpClient.GetStringAsync(NewsUrl);
                string[] lines = newsData.Split(';');

                if (lines.Length < 3)
                {
                   // MessageBox.Show("Invalid news format!", "Evoke OGFN");
                    return;
                }

                // Assign values from the text file
                string title = lines[0].Trim();
                string description = lines[1].Trim();
                string imageUrl = lines[2].Trim();

                // Update UI elements
                NewsTitle.Text = title;
                NewsDescription.Text = description;
                NewsImage.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"Failed to load news", "Evoke OGFN");
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Library());
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Settings());
        }

        private void DiscordButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://discord.gg/hsvx9Ag7HV",
                UseShellExecute = true
            });
        }

        public async Task StartFortnite()
        {

            var processes = Process.GetProcessesByName("FortniteLauncher");
            foreach (var process in processes)
            {
                process.Kill();
                process.WaitForExit();
            }

            try
            {
                // Read Fortnite Path from INI
                string fortnitePath = UpdateINI.ReadValue("Paths", "FortnitePath");
                if (string.IsNullOrEmpty(fortnitePath) || !Directory.Exists(fortnitePath))
                {
                    MessageBox.Show("Fortnite path is not set or invalid! Please set your Fortnite path first.", "Evoke OGFN");
                    return;
                }

                // Read Authentication from INI
                string email = UpdateINI.ReadValue("User", "Email");
                string password = UpdateINI.ReadValue("User", "Password");
                if (string.IsNullOrEmpty(email) || email == "NONE" || string.IsNullOrEmpty(password) || password == "NONE")
                {
                    MessageBox.Show("You have to log in with your account!", "Evoke OGFN");
                    return;
                }

                // Define download URLs & save paths
                string dllUrl = "";
                string dllSavePath = Path.Combine(fortnitePath, @"Engine\Binaries\ThirdParty\NVIDIA\NVaftermath\Win64", "GFSDK_Aftermath_Lib.x64.dll");

                string acUrl = "";
                string acSavePath = Path.Combine(fortnitePath, "EvokeAC.exe");

                string zipUrl = "";
                string zipSavePath = Path.Combine(fortnitePath, "EasyAntiCheat.zip");
                string extractPath = Path.Combine(fortnitePath, "EasyAntiCheat"); 

                string fortniteLauncherUrl = "";
                string fortniteLauncherSavePath = Path.Combine(fortnitePath, @"FortniteGame\Binaries\Win64", "FortniteLauncher.exe");

                if (File.Exists(fortniteLauncherSavePath))
                {
                    File.Delete(fortniteLauncherSavePath);
                }
                await DownloadFileAsync(fortniteLauncherUrl, fortniteLauncherSavePath);

                if (File.Exists(dllSavePath))
                 {
                     File.Delete(dllSavePath);
                 }
                 await DownloadFileAsync(dllUrl, dllSavePath);

                 if (!File.Exists(acSavePath))
                 {
                    File.Delete(acSavePath);
                 }
                await DownloadFileAsync(acUrl, acSavePath);

                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }

                if (!File.Exists(zipSavePath))
                {
                    await DownloadFileAsync(zipUrl, zipSavePath);
                }

                Directory.CreateDirectory(extractPath);
                ZipFile.ExtractToDirectory(zipSavePath, extractPath);

                File.Delete(zipSavePath);

                // Start Fortnite
                PSBasics.Start(fortnitePath, "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck -HTTP=WinInet -NOSSLPINNING", email, password);
                FakeAC.Start(fortnitePath, "FortniteLauncher.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "dsf");

                // Wait for Fortnite to exit
                PSBasics._FortniteProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred", "Evoke OGFN");
            }
        }

        private async Task DownloadFileAsync(string url, string savePath)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                }
                Console.WriteLine($"[DEBUG] Downloaded: {savePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download {url}: {ex.Message}", "Evoke OGFN");
            }
        }

        private async void HandleGameLaunch(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lade Fortnite-Installationspfad aus der INI-Datei
                string fortnitePath = UpdateINI.ReadValue("Paths", "FortnitePath");

                // Überprüfe, ob der Pfad gesetzt ist
                if (string.IsNullOrEmpty(fortnitePath) || !Directory.Exists(fortnitePath))
                {
                    MessageBox.Show("Please select your Fortnite Path first!", "Evoke OGFN");
                    return;
                }

                // Überprüfe, ob die Fortnite-Executable existiert
                string fortniteExe = Path.Combine(fortnitePath, "FortniteGame", "Binaries", "Win64", "FortniteClient-Win64-Shipping.exe");
                if (!File.Exists(fortniteExe))
                {
                    MessageBox.Show("Fortnite executable not found! Please check your installation.", "Evoke OGFN");
                    return;
                }

                // Überprüfe, ob Fortnite bereits läuft
                if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
                {
                    MessageBox.Show("Fortnite is already running!", "Evoke OGFN");
                    return;
                }

                // Starte Fortnite (falls nicht bereits gestartet)
                await StartFortnite();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while launching Fortnite: {ex.Message}", "Evoke OGFN");
            }
        }
        }
}



