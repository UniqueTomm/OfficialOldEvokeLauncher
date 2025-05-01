using System;
using System.Windows;
using System.Windows.Controls;
using SteraLauncher.Services;

namespace SteraLauncher.Pages
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Home());
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Library());
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DiscordButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://discord.gg/hsvx9Ag7HV",
                UseShellExecute = true
            });
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove user section from INI
            UpdateINI.DeleteSection("User");

            // Confirm logout
            //MessageBox.Show("You have been logged out.", "Logout Successful", MessageBoxButton.OK, MessageBoxImage.Information);

            // Redirect to login page
            NavigationService?.Navigate(new Login());
        }
    }
}
