using Microsoft.UI.Xaml.Controls;
using SmashMall.Models.UserAccount;
using SmashMall.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmashMall.UWP.Controls
{
    public sealed partial class LoginCreateAccountTeachingTip : TeachingTip
    {
        public LoginCreateAccountTeachingTip()
        {
            this.InitializeComponent();
            LoginCreateAccount();
        }


        async private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Title.ToUpper() == "LOGIN")// Login
                {
                    ErrorTextBlock.Text = string.Empty;
                    if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                    {
                        ErrorTextBlock.Text = "Email is required";
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                    {
                        ErrorTextBlock.Text = "Password is required";
                        return;
                    }
                    var loginModel = new Login
                    {
                        Email = EmailTextBox.Text,
                        Password = PasswordBox.Password,
                        RememberMe = Convert.ToBoolean(RememberMeCheckBox.IsChecked)
                    };
                    var content = JsonSerializer.Serialize(loginModel);
                    var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var result = await UnAuthorizedHttp.Client.PostAsync($"api/ApplicationUsers/login", bodyContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        ErrorTextBlock.Text = await result.Content.ReadAsStringAsync();
                        return;
                    }
                    else
                    {
                        ErrorTextBlock.Text = string.Empty;
                        var authToken = await result.Content.ReadAsStringAsync();
                        App.RoamingDataContainer.Values["AuthToken"] = authToken;
                        AuthorizedHttp.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);
                        MainPage.LoginCreateAccountSuccessful = true;
                        this.IsOpen = false;
                    }
                }
                else // Create account
                {
                    ErrorTextBlock.Text = string.Empty;
                    if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text)) ErrorTextBlock.Text = "First name is required";
                    else if (string.IsNullOrWhiteSpace(LastNameTextBox.Text)) ErrorTextBlock.Text = "Last name is required";
                    else if (string.IsNullOrWhiteSpace(EmailTextBox.Text)) ErrorTextBlock.Text = "Email is required";
                    else if (string.IsNullOrWhiteSpace(PhonenumberBox.Text)) ErrorTextBlock.Text = "Phonenumber is required";
                    else if (string.IsNullOrWhiteSpace(PasswordBox.Password)) ErrorTextBlock.Text = "Password is required";
                    else if (PasswordBox.Password != ConfirmPasswordBox.Password) ErrorTextBlock.Text = "Passwords do not match";
                    else
                    {
                        var registerModel = new Register
                        {
                            FirstName = FirstNameTextBox.Text,
                            LastName = LastNameTextBox.Text,
                            Email = EmailTextBox.Text,
                            Phonenumber = PhonenumberBox.Text,
                            Password = PasswordBox.Password,
                            RememberMe = Convert.ToBoolean(RememberMeCheckBox.IsChecked)
                        };
                        var content = JsonSerializer.Serialize(registerModel);
                        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
                        var result = await UnAuthorizedHttp.Client.PostAsync($"api/ApplicationUsers/create-account", bodyContent);
                        if (!result.IsSuccessStatusCode)
                        {
                            ErrorTextBlock.Text = await result.Content.ReadAsStringAsync();
                            return;
                        }
                        else
                        {
                            ErrorTextBlock.Text = string.Empty;
                            var authToken = await result.Content.ReadAsStringAsync();
                            App.RoamingDataContainer.Values["AuthToken"] = authToken;
                            AuthorizedHttp.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);
                            MainPage.LoginCreateAccountSuccessful = true;
                            this.IsOpen = false;
                        }
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error occured");
                await md.ShowAsync();
            }
        }

        private void LoginCreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            LoginCreateAccount();
        }

        public void LoginCreateAccount()
        {
            if (Title.ToUpper() == "LOGIN")
            {
                Title = "Create account";
                LoginCreateAccountButton.Content = "Login";

                ConfirmPasswordBox.Visibility = Visibility.Visible;
                ConfirmPassword.Visibility = Visibility.Visible;
                FirstNameTextBlock.Visibility = Visibility.Visible;
                FirstNameTextBox.Visibility = Visibility.Visible;
                LastNameTextBlock.Visibility = Visibility.Visible;
                LastNameTextBox.Visibility = Visibility.Visible;
                PhonenumberTextBlock.Visibility = Visibility.Visible;
                PhonenumberBox.Visibility = Visibility.Visible;

            }
            else
            {
                Title = "Login";
                LoginCreateAccountButton.Content = "Create account";


                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                ConfirmPassword.Visibility = Visibility.Collapsed;
                FirstNameTextBlock.Visibility = Visibility.Collapsed;
                FirstNameTextBox.Visibility = Visibility.Collapsed;
                LastNameTextBlock.Visibility = Visibility.Collapsed;
                LastNameTextBox.Visibility = Visibility.Collapsed;
                PhonenumberTextBlock.Visibility = Visibility.Collapsed;
                PhonenumberBox.Visibility = Visibility.Collapsed;
            }
            SubmitButton.Content = Title;
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;
        }

        private void TeachingTip_CloseButtonClick(TeachingTip sender, object args)
        {

        }
    }
}
