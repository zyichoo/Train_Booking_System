using Firebase.Database.Streaming;
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
    public sealed partial class Bookings : Page
    {
        FirebaseHelper fbh = new FirebaseHelper();

        private Booking book = new Booking();
        private Passenger p = new Passenger();
        public Bookings()
        {
            this.InitializeComponent();

            var obj = App.Current as App;
            goorigin.Text = obj.origin;
            godestination.Text = obj.destinantion;
            trainNum.Text = obj.trainNumber;
            godate.Text = obj.date.ToString();
            Duration.Text = obj.duration;

            txtPassengerName.Text = PassengerStaticClass.pName;
          
            
            txtPassengerIC.Text = PassengerStaticClass.pIC;
            txtPassengerPhoneNumber.Text = PassengerStaticClass.pPhone;
            txtPassengerTicketprice.Text = obj.ticketPrice;

        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    var obj = App.Current as App;
                  

                    book.GoOrigin = goorigin.Text;
                    book.GoDestination = godestination.Text;
                    book.trainNum = trainNum.Text;
                    book.GoDate = obj.date;
                    book.duration = Duration.Text;

                    book.PassengerName = txtPassengerName.Text;
                  
                    book.IC = txtPassengerIC.Text;
                    book.Phonenumber = txtPassengerPhoneNumber.Text;
                    book.Price = txtPassengerTicketprice.Text;


                   await fbh.AddBooking(book);

                    
                goorigin.Text = string.Empty;
                godestination.Text = string.Empty;
                trainNum.Text = string.Empty;
                godate.Text = string.Empty;
                Duration.Text = string.Empty;


                txtPassengerName.Text = string.Empty;
             
                txtPassengerIC.Text = string.Empty;
                txtPassengerPhoneNumber.Text = string.Empty;
                txtPassengerTicketprice.Text = string.Empty;


                DisplayDialog("Success", "Booking Added Successfully");
                    


            }
            catch (Exception theException)
            {
                // Handle all other exceptions.
                DisplayDialog("Error", "Error Message: " + theException.Message);
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
