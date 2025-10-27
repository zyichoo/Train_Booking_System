using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
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
    public sealed partial class DateSelectionPage : Page
    {
        public DateTimeOffset MinDate { get; } = DateTimeOffset.Now; // Set to today's date
        public DateTimeOffset MaxDate { get; } = new DateTimeOffset(new DateTime(2026, 12, 31));

        public DateTime GoDate { get; private set; }

       

        // Add properties to store the previous selections
        public string PreviousOrigin { get; set; }
        public string PreviousDestination { get; set; }
        public object PreviousPax { get; set; }

        public DateSelectionPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Extract the previous selections if they are passed as parameters
            if (e.Parameter is Tuple<string, string, object, DateTime?> navigationData)
            {
                PreviousOrigin = navigationData.Item1;
                PreviousDestination = navigationData.Item2; 
                PreviousPax = navigationData.Item3;


                

                //if (navigationData.Item4.HasValue && navigationData.Item4.Value.Date >= MinDate.Date && navigationData.Item4.Value.Date <= MaxDate.Date)
                //{
                //    SelectedDate = navigationData.Item4.Value;
                //    actualDatePicker.Date = SelectedDate;
                //}
                ////else
                //{
                //    // If the provided date is out of range, set it to today's date
                //    SelectedDate = DateTime.Now;
                //    actualDatePicker.Date = SelectedDate;
                //}
            }
        }

        private async void ConfirmDateButton_Click(object sender, RoutedEventArgs e)
        {
            //SelectedDate = actualDatePicker.Date.DateTime;
            //DateTime DepartDate;
            var obj = App.Current as App;

            if (actualDatePicker.SelectedDate.HasValue)
            {
                if (actualDatePicker.SelectedDate.HasValue && actualDatePicker2.SelectedDate.HasValue)
                {
                    GoDate = actualDatePicker.SelectedDate.Value.DateTime;
                    obj.BackDate = actualDatePicker2.SelectedDate.Value.DateTime;
                    obj.TripType = "Round trip";
                }
                else
                {
                    GoDate = actualDatePicker.SelectedDate.HasValue ? actualDatePicker.SelectedDate.Value.DateTime : actualDatePicker2.SelectedDate.Value.DateTime;
                    obj.TripType = "One-way";
                }
                var returnData = new Tuple<string, string, object, DateTime?>(PreviousOrigin, PreviousDestination, PreviousPax, GoDate);
                Frame.Navigate(typeof(TrainBookingPage), returnData);
            }
            else
                await new MessageDialog("Please select your departure date","Error").ShowAsync();

          
            

                // Determine if it's a Depart or Return based on the radio buttons
                //TripType = departRadioButton.IsChecked.Value ? "Depart" : "Return";

            // Create a tuple containing all the selections and navigate back
            
        }
    }
}
