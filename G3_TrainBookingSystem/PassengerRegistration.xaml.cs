using G3_TrainBookingSytem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace G3_TrainBookingSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PassengerRegistration : Page
    {
        private static int lastAssignedPassengerId = 0;

        public PassengerRegistration()
        {
            this.InitializeComponent();

            // Initialize the last assigned passenger ID from local settings
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("LastAssignedPassengerId"))
            {
                lastAssignedPassengerId = (int)localSettings.Values["LastAssignedPassengerId"];
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Increment the passenger ID counter
            lastAssignedPassengerId++;

            // Create a Passenger object and populate it with form data
            var passenger = new Passenger
            {
                PassengerId = lastAssignedPassengerId, // Assign the new passenger ID
                FullName = FullNameTextBox.Text,
                IC = ICTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                Gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Email = EmailTextBox.Text,
                // You should hash and salt the password securely before storing it
                Password = PasswordBox.Password,
            };

            try
            {
                // Initialize FirebaseHelper
                FirebaseHelper firebaseHelper = new FirebaseHelper();

                // Validate input fields
                if (!IsValidRegistrationInput(passenger))
                {
                    DisplayDialog("Input Error", "Please check your input fields.");
                    return;
                }

                // Add the passenger to the database using the AddPassenger method
                await firebaseHelper.AddPassenger(passenger);

                // Display a success message
                DisplayDialog("Success", "Passenger Registered Successfully");

                // Clear the form fields
                ClearFormFields();

                // Save the updated passenger ID counter to storage
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["LastAssignedPassengerId"] = lastAssignedPassengerId;

                // Navigate to the login page upon successful registration
                Frame.Navigate(typeof(PassengerLoginPage));
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the registration process
                DisplayDialog("Error", "Error: " + ex.Message);
            }
        }

        // Helper method to validate input fields
        private bool IsValidRegistrationInput(Passenger passenger)
        {
            return !string.IsNullOrWhiteSpace(passenger.FullName) &&
                    IsAlphabetOnly(passenger.FullName) &&
                    !string.IsNullOrWhiteSpace(passenger.IC) &&
                    IsICValid(passenger.IC) &&
                    !string.IsNullOrWhiteSpace(passenger.PhoneNumber) &&
                    IsPhoneNumberValid(passenger.PhoneNumber) &&
                    !string.IsNullOrWhiteSpace(passenger.Email) &&
                    IsEmailValid(passenger.Email) &&
                    !string.IsNullOrWhiteSpace(passenger.Password) &&
                    passenger.Password.Length >= 8;
        }

        // Helper method to validate if the input contains only alphabetic characters
        private bool IsAlphabetOnly(string input)
        {
            return input.Replace(" ", "").All(char.IsLetter);
        }

        // Helper method to validate the IC format (xxxxxx-xx-xxxx)
        private bool IsICValid(string input)
        {
            if (input.Length != 14)
                return false;

            return input[6] == '-' && input[9] == '-';
        }

        // Helper method to validate the phone number format (xxx-xxxxxxx)
        private bool IsPhoneNumberValid(string input)
        {
            if (input.Length != 11)
                return false;

            return input[3] == '-';
        }

        // Helper method to validate email format
        private bool IsEmailValid(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$");
        }

        // Helper method to display a dialog
        private async void DisplayDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };

            await dialog.ShowAsync();
        }

        // Helper method to clear form fields
        private void ClearFormFields()
        {
            FullNameTextBox.Text = "";
            ICTextBox.Text = "";
            PhoneNumberTextBox.Text = "";
            GenderComboBox.SelectedItem = null;
            EmailTextBox.Text = "";
            PasswordBox.Password = "";
            ConfirmPasswordBox.Password = "";
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassengerLoginPage)); // Navigate back to the Login page
        }
    }
}
