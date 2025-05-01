using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Text;
using SteraLauncher.Services;

namespace SteraLauncher.Pages
{
    public partial class Login : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public Login()
        {
            InitializeComponent();

            string savedEmail = UpdateINI.ReadValue("User", "Email");
            string savedPassword = UpdateINI.ReadValue("User", "Password");

            if (!string.IsNullOrWhiteSpace(savedEmail) && savedEmail != "NONE" &&
                !string.IsNullOrWhiteSpace(savedPassword) && savedPassword != "NONE")
            {
                EmailTextBox.Text = savedEmail;
                PasswordBox.Password = savedPassword;
                AutoLogin(savedEmail, savedPassword);
            }
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmailPlaceholder.Visibility = string.IsNullOrWhiteSpace(EmailTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = string.IsNullOrWhiteSpace(PasswordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void AutoLogin(string email, string password)
        {
            await PerformLogin(email, password, autoLogin: true);
        }

        private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Evoke OGFN");
                return;
            }

            await PerformLogin(email, password, autoLogin: false);
        }

        private async Task PerformLogin(string email, string password, bool autoLogin)
        {
            var requestUrl = $"{Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Code: {response.StatusCode}");
                Console.WriteLine($"Response Body: {responseBody}");

                switch ((int)response.StatusCode)
                {
                    case 200: // Login successful
                        //MessageBox.Show("Login Successful!", "Success");

                        // Save login details
                        UpdateINI.WriteToConfig("User", "Email", email);
                        UpdateINI.WriteToConfig("User", "Password", password);

                        // Navigate to home screen
                        NavigationService?.Navigate(new Home());
                        break;

                    case 401: // Password invalid
                        MessageBox.Show("Invalid password.", "Login Failed");
                        break;

                    case 404: // E-Mail not registered
                        MessageBox.Show("E-Mail not registered.", "Login Failed");
                        break;

                    default: // Other errors
                        MessageBox.Show("Login failed! Check your credentials.", "Login Failed");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: Servers may be offline!", "Error");
                Console.WriteLine($"Exception: {ex}");
            }
        }
    }
}
