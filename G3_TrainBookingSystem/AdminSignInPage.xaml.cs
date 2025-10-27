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
    public sealed partial class AdminSignInPage : Page
    {
        FirebaseHelper a = new FirebaseHelper();
        public AdminSignInPage()
        {
            this.InitializeComponent();
        }

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

        private async void aLogin(object sender, RoutedEventArgs e)
        {
            string email = aEmail.Text;
            string password = aPassword.Text;
            try
            { 
                var admnins = await a.GetAllAdmin();
                var matchedPassenger = admnins.FirstOrDefault(a => a.Email == email && a.Password == password);
                
                if(matchedPassenger != null)
                {
                    DisplayDialog("Login Successful", "You have successfully logged in.");
                    aEmail.Text = "";
                    aPassword.Text = "";
                    this.Frame.Navigate(typeof(Bookings));
                }
                else
                DisplayDialog("Error", "Please enter correct values.");
            }
            
            catch (Exception ex)
            {
                DisplayDialog("Error", "Error: " + ex.Message);
            }
        }

        private void aRegister(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdminSignUpPage));
        }
    }
}
