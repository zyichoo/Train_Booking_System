using G3_TrainBookingSytem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class AdminSignUpPage : Page
    {
        FirebaseHelper fbhAdmin = new FirebaseHelper();
        public AdminSignUpPage()
        {
            this.InitializeComponent();
            aFullNameTBX.Text = "Starmer";
            aIcTBX.Text = "112233-44-5566";
            aPhoneTBX.Text = "012-3456789";
            aMaleRB.IsChecked = false;
            aFemaleRB.IsChecked = false;
            aEmailTBX.Text = "Starmer@gmail.com";
            aPasswordTBX.Text = "Starmer$888";
            aConfirmPasswordTBX.Text = "Starmer$888";
        }

        private async void DisplayDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK"
            };
            ContentDialogResult result = await dialog.ShowAsync();
        }

        private void aResetBTN_isClicked(object sender, RoutedEventArgs e)
        {
            aFullNameTBX.Text = "";
            aIcTBX.Text = "";
            aPhoneTBX.Text = "";
            aMaleRB.IsChecked = false;
            aFemaleRB.IsChecked = false;
            aEmailTBX.Text = "";
            aPasswordTBX.Text = "";
            aConfirmPasswordTBX.Text = "";
        }

        private async void aSignUpBTN_isClicked(object sender, RoutedEventArgs e)
        {
            aPasswordFormatTBK.Visibility = Visibility.Collapsed;
            try
            {
                if (aFullNameTBX.Text != "")
                {
                    if (aIcTBX.Text != "" && Regex.IsMatch(aIcTBX.Text, @"^\d{6}-\d{2}-\d{4}$"))
                    {
                        if (aPhoneTBX.Text != "" && Regex.IsMatch(aPhoneTBX.Text, @"^\d{3}-\d{7,8}$"))
                        {
                            if ((aMaleRB.IsChecked == true | aFemaleRB.IsChecked) == true)
                            {
                                if (aEmailTBX.Text != "" && Regex.IsMatch(aEmailTBX.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                                {
                                    if ((aPasswordTBX.Text != "" && Regex.IsMatch(aPasswordTBX.Text, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                                        && (aConfirmPasswordTBX.Text != "" && Regex.IsMatch(aConfirmPasswordTBX.Text, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                                        && (aPasswordTBX.Text == aConfirmPasswordTBX.Text))
                                    {
                                        Admin admin = new Admin();
                                        admin.FullName = aFullNameTBX.Text;
                                        admin.IC = aIcTBX.Text;
                                        admin.Phone = aPhoneTBX.Text;
                                        admin.Gender = aMaleRB.IsChecked == true ? aMaleRB.Tag.ToString() : aFemaleRB.Tag.ToString();
                                        admin.Email = aEmailTBX.Text;
                                        admin.Password = aPasswordTBX.Text;

                                        await fbhAdmin.AddAdmin(admin);
                                        DisplayDialog("Welcome! " + aFullNameTBX.Text, "We know we'll be turning to you when we need your unique talents!!");
                                        this.Frame.Navigate(typeof(MainPage));
                                    }
                                    else
                                    {
                                        aPasswordFormatTBK.Visibility = Visibility.Visible;
                                        aPasswordFormatTBK.Text =
                                            "Password must contain:" +
                                            "\nMinimum 8 characters long" +
                                            "\n1 Uppercase" +
                                            "\n1 Lowercase" +
                                            "\n1 Number" +
                                            "\n1 Special character" +
                                            "\nand same with confirm password";
                                    }
                                }
                                else
                                    DisplayDialog("Warning", "Email must contain:\n@ and dot (.)\nE.g Josh@gmail.com");
                            }
                            else
                                DisplayDialog("Warning", "Please select your gender.");
                        }
                        else
                            DisplayDialog("Warning", "Please enter your phone number correctly.\nE.g 012-3456789 / 012-34567890");
                    }
                    else
                        DisplayDialog("Warning", "Please enter your IC correctly.\nE.g 112233-44-6677");
                }
                else
                    DisplayDialog("Warning", "Please enter your full name.");
            }
            catch (Exception ex)
            {
                DisplayDialog("Error", ex.Message + ".");
            }
        }

        private void aBackBTN_isClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
