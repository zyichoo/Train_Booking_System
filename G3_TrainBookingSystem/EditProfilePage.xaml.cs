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
    public sealed partial class EditProfilePage : Page
    {
        private Passenger currentPassenger;

        public EditProfilePage()
        {
            this.InitializeComponent();
        }

        // Load the current passenger's information when the page is navigated to.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Retrieve the currently logged-in passenger's information from your authentication system
            currentPassenger = GetCurrentLoggedInPassenger();

            // Populate the UI elements with the passenger's current information
            if (currentPassenger != null)
            {
                EditFullNameTextBox.Text = currentPassenger.FullName;
                EditICTextBox.Text = currentPassenger.IC;
                EditPhoneNumberTextBox.Text = currentPassenger.PhoneNumber;
                EditEmailTextBox.Text = currentPassenger.Email;
                EditGenderComboBox.SelectedItem = currentPassenger.Gender;
                // Populate other fields as needed
            }
        }

        private void UpdateProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPassenger != null)
            {
                // Update the passenger's information with the new values from the UI elements
                currentPassenger.FullName = EditFullNameTextBox.Text;
                currentPassenger.IC = EditICTextBox.Text;
                currentPassenger.PhoneNumber = EditPhoneNumberTextBox.Text;
                currentPassenger.Email = EditEmailTextBox.Text;
                currentPassenger.Gender = EditGenderComboBox.SelectedItem?.ToString();
                // Update other fields as needed

                // Call a method to update the passenger's information in the database
                UpdatePassengerInformation(currentPassenger);

                // Display a success message to the passenger
                DisplayDialog("Success", "Profile Updated Successfully");

                // You can navigate the passenger back to their profile page or another page
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        // Implement methods to retrieve the currently logged-in passenger and update their information in the database.
        // You should handle authentication and database interaction as needed.

        private Passenger GetCurrentLoggedInPassenger()
        {
            // Retrieve the logged-in passenger's information from your authentication system
            // This method depends on your specific authentication logic
            // Return the passenger object once authenticated
            return new Passenger
            {
                FirebaseId = "123456", // Replace with the actual Firebase ID
                FullName = "John Doe",
                IC = "123456-78-9012",
                PhoneNumber = "012-3456789",
                Email = "john.doe@example.com",
                Gender = "Male"
                // Populate other fields as needed
            };
        }

        private async void UpdatePassengerInformation(Passenger passenger)
        {
            try
            {
                // Initialize FirebaseHelper
                FirebaseHelper firebaseHelper = new FirebaseHelper();

                // Update the passenger's information in the Firebase database
                await firebaseHelper.UpdatePassenger(passenger.FirebaseId, passenger);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the update process
                DisplayDialog("Error", "Error updating passenger information: " + ex.Message);
            }
        }

        private async void DisplayDialog(string title, string content)
        {
            ContentDialog noDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noDialog.ShowAsync();
        }
    }
}
