using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;

namespace SteraLauncher.Pages
{
    public partial class Check : Page
    {
        private const string ServerStatusUrl = "";
        private const string VersionUrl = "";
        private const string RequiredVersion = "0.0.00";

        public Check()
        {
            InitializeComponent();
            Loaded += CheckServerAndVersion;
        }

        private async void CheckServerAndVersion(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Check Server Status
                    StatusText.Text = "Checking server status...";
                    string serverStatus = await client.GetStringAsync(ServerStatusUrl);

                    if (serverStatus.Trim().ToLower() != "onlin")
                    {
                        StatusText.Text = "Servers offline!";
                        return;
                    }

                    // Check Launcher Version
                    StatusText.Text = "Checking launcher version...";
                    string latestVersion = await client.GetStringAsync(VersionUrl);

                    if (latestVersion.Trim() != RequiredVersion)
                    {
                        StatusText.Text = $"Launcher is outdated. Please update to version {latestVersion}.";
                        return;
                    }

                    // Redirect to Login.xaml
                    StatusText.Text = "All checks passed! Launching...";
                    await Task.Delay(0); // Small delay for UX

                    NavigationService?.Navigate(new Login());
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error checking server: {ex.Message}";
            }
        }
    }
}
