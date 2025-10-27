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
    public sealed partial class PassengerLoginPage : Page
    {
        public PassengerLoginPage()
        {
            this.InitializeComponent();
        }


        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input for email and password
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            // Initialize FirebaseHelper
            FirebaseHelper firebaseHelper = new FirebaseHelper();

            try
            {
                // Retrieve the list of passengers from Firebase
                var passengers = await firebaseHelper.GetAllPassengers();

                // Check if there's a passenger with matching email and password
                var matchingPassenger = passengers.FirstOrDefault(p => p.Email == email && p.Password == password);

                if (matchingPassenger != null)
                {
                    // Login successful, show a dialog
                    DisplayDialog("Login Successful", "You have successfully logged in.");

                    //Pass the data to the static variables in PassengerStaticClass(to be used in booking page)
                    PassengerStaticClass.pName = matchingPassenger.FullName;
                    PassengerStaticClass.pGender = matchingPassenger.Gender.ToString();
                    PassengerStaticClass.pIC = matchingPassenger.IC;
                    PassengerStaticClass.pPhone = matchingPassenger.PhoneNumber;

                    // Clear the email and password fields
                    EmailTextBox.Text = "";
                    PasswordBox.Password = "";

                    // Navigate to another page (e.g., home page) after successful login
                    this.Frame.Navigate(typeof(TrainBookingPage));
                }
                else
                {
                    // Login failed, show an error dialog
                    DisplayDialog("Login Failed", "Invalid email or password.");

                    // Clear the email and password fields for a retry
                    EmailTextBox.Text = "";
                    PasswordBox.Password = "";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the login process
                DisplayDialog("Error", "Error: " + ex.Message);
            }
        }


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PassengerRegistration)); // Navigate to the Registration page
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
    }
}
