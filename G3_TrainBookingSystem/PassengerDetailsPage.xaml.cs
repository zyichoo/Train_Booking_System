using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class PassengerDetailsPage : Page
    {
        public PassengerDetailsPage()
        {
            this.InitializeComponent();
            AutoInputCheckBox1.IsChecked = false;

        }

        private bool IsNumeric(string input)
        {
            return input.All(char.IsDigit);
        }

        private async void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            ContentDialog cancelDialog = new ContentDialog
            {
                Title = "Cancel Booking",
                Content = "Booking will be cancelled if you go back from here. You want to cancel this booking?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            ContentDialogResult result = await cancelDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Navigate back to the TrainBookingPage
                Frame.Navigate(typeof(TrainBookingPage));
            }
        }

        private async void OnMakePaymentClicked(object sender, RoutedEventArgs e)
        {
          
            // Basic validation
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                await new MessageDialog("Please enter a name.", "Validation Error").ShowAsync();
                return;
            }

            if (GenderComboBox.SelectedItem == null)
            {
                await new MessageDialog("Please select a gender.", "Validation Error").ShowAsync();
                return;
            }

            //if (string.IsNullOrEmpty(ContactNoTextBox.Text) || !IsNumeric(ContactNoTextBox.Text))
            //{
            //    await new MessageDialog("Please enter a valid contact number.", "Validation Error").ShowAsync();
            //    return;
            //}

            if (TicketTypeComboBox.SelectedItem == null)
            {
                await new MessageDialog("Please select a ticket type.", "Validation Error").ShowAsync();
                return;
            }
            PassengerStaticClass.pName = NameTextBox.Text;
            PassengerStaticClass.pGender = GenderComboBox.Text;
           
            PassengerStaticClass.pPhone = ContactNoTextBox.Text;

            Frame.Navigate(typeof(Bookings));
        }

        private void autoFillUp_isChecked(object sender, RoutedEventArgs e)
        {
                int year;
                NameTextBox.Text = PassengerStaticClass.pName;
                if (PassengerStaticClass.pGender == "Male")
                    GenderComboBox.SelectedIndex = 0;
                else if (PassengerStaticClass.pGender == "Female")
                    GenderComboBox.SelectedIndex = 1;
                else
                    GenderComboBox.SelectedIndex = 2;
                ICTextBox.Text = PassengerStaticClass.pIC;
                ContactNoTextBox.Text = PassengerStaticClass.pPhone;

                year = int.Parse(ICTextBox.Text.ToString().Substring(0, 2));

                if (((DateTime.Now.Year % 100) - year) < 18)
                    TicketTypeComboBox.SelectedIndex = 0;
                else
                    TicketTypeComboBox.SelectedIndex = 1;

           

        }

        private void autoFillUp_isUnChecked(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = "";
            GenderComboBox.SelectedItem = null;
            ICTextBox.Text = "";
            ContactNoTextBox.Text = "";
            TicketTypeComboBox.SelectedValue = null;
        }
    }
}
