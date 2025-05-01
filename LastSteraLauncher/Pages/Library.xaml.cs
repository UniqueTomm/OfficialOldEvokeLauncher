using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using SteraLauncher.Services;
using Microsoft.Win32;
using System.Windows.Controls;

namespace SteraLauncher.Pages
{
    public partial class Library : Page
    {
        public Library()
        {
            InitializeComponent();
            LoadSavedPath();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Home());
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Settings());
        }

        private void DiscordButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://discord.gg/hsvx9Ag7HV",
                UseShellExecute = true
            });
        }

        private void LoadSavedPath()
        {
            string savedPath = UpdateINI.ReadValue("Paths", "FortnitePath");
            if (!string.IsNullOrEmpty(savedPath) && Directory.Exists(savedPath))
            {
                ShowFortniteBuild(savedPath);
            }
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedPath = Path.GetDirectoryName(dialog.FileName);
                UpdateINI.WriteToConfig("Paths", "FortnitePath", selectedPath);
                ShowFortniteBuild(selectedPath);
            }
        }

        private void ShowFortniteBuild(string fortnitePath)
        {
            string version = "6.21";
            string versionFile = Path.Combine(fortnitePath, "FortniteGame", "Binaries", "Win64", "version.txt");

            if (File.Exists(versionFile))
            {
                version = File.ReadAllText(versionFile);
            }

            FortniteBuildTextBlock.Text = "Fortnite " + version;
            FortniteBuildContainer.Visibility = Visibility.Visible;

            LoadSplashImage(fortnitePath);
        }

        private void LoadSplashImage(string fortnitePath)
        {
            string splashPath = Path.Combine(fortnitePath, "FortniteGame", "Content", "Splash", "Splash.bmp");

            if (File.Exists(splashPath))
            {
                SplashImage.Source = new BitmapImage(new Uri(splashPath));
                SplashImage.Visibility = Visibility.Visible;
            }
            else
            {
                SplashImage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
