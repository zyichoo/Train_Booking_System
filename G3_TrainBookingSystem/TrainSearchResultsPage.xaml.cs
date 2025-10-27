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
    public sealed partial class TrainSearchResultsPage : Page
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        private int paxCount;

        public TrainSearchResultsPage()
        {
            this.InitializeComponent();
        }

        private async void LoadAvailableTrains(Tuple<string, string, DateTime, int> searchParameters)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Loading available trains...");

                var allTrains = await firebaseHelper.GetAllTrains();
                System.Diagnostics.Debug.WriteLine($"Total trains retrieved from Firebase: {allTrains.Count}");

                // Filter out the trains that match the origin, destination, and train number criteria
                var availableTrains = allTrains
                    .Where(t => t.Origin == searchParameters.Item1 && t.Destination == searchParameters.Item2)
                 
                    //.Where(t => t.TrainNumber.StartsWith("KL") && int.Parse(t.TrainNumber.Substring(2)) >= 101 && int.Parse(t.TrainNumber.Substring(2)) <= 110)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"Number of matching trains: {availableTrains.Count}");

                var random = new Random();

                var displayedTrains = availableTrains.Select(t =>
                {
                    //var departureHour = t.DepartureTime;
                    ////var departureMinute = t.;
                    //var durationHour = t.ArrivalTime;
                    //var durationMinute =t.Duration;
                    //var arrivalHour = departureHour + durationHour;
                    //var arrivalMinute = departureMinute + durationMinute;

                    //if (arrivalMinute >= 60)
                    //{
                    //    arrivalHour++;
                    //    arrivalMinute -= 60;
                    //}
                    var obj = App.Current as App;

                    obj.trainNumber = t.TrainNumber;
                    obj.duration = t.Duration;
                    obj.ticketPrice = t.TicketPrice;

                    return new DisplayTrain
                    {
                        TrainID = t.TrainID,
                        TrainNumber = t.TrainNumber,
                        Origin = t.Origin,
                        Destination = t.Destination,
                        DepartureTime = t.DepartureTime,
                        ArrivalTime = t.ArrivalTime,
                        Duration = t.Duration,
                        TicketPrice = t.TicketPrice,

                        
                };
                }).ToList();

                lstAvailableTrains.ItemsSource = displayedTrains;

                // Show/hide the message and ListView based on the search results
                if (displayedTrains.Count > 0)
                {
                    lstAvailableTrains.Visibility = Visibility.Visible;
                    NoTrainsMessagePanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lstAvailableTrains.Visibility = Visibility.Collapsed;
                    NoTrainsMessagePanel.Visibility = Visibility.Visible;
                }

                System.Diagnostics.Debug.WriteLine($"Number of displayed trains: {displayedTrains.Count}");
            }
            catch (Exception ex)
            {
                await new MessageDialog($"Error loading available trains: {ex.Message}", "Error").ShowAsync();
                System.Diagnostics.Debug.WriteLine($"Exception in LoadAvailableTrains: {ex.Message}");
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Tuple<string, string, DateTime, int> searchParameters)
            {
                string origin = searchParameters.Item1;
                string destination = searchParameters.Item2;
                DateTime date = searchParameters.Item3;
                paxCount = searchParameters.Item4;

                System.Diagnostics.Debug.WriteLine("Received parameters for TrainSearchResultsPage:");
                System.Diagnostics.Debug.WriteLine($"Origin: {origin}");
                System.Diagnostics.Debug.WriteLine($"Destination: {destination}");
                System.Diagnostics.Debug.WriteLine($"Date: {date.ToShortDateString()}"); // Using ToShortDateString for cleaner output
                System.Diagnostics.Debug.WriteLine($"PaxCount: {paxCount}");

                // Assuming your LoadAvailableTrains method is updated to accept the new tuple with 4 items
                LoadAvailableTrains(searchParameters);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error: TrainSearchResultsPage did not receive the expected parameters.");

                // Additional debugging
                if (e.Parameter == null)
                {
                    System.Diagnostics.Debug.WriteLine("Received parameter is null.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Received parameter type: {e.Parameter.GetType().FullName}");
                    System.Diagnostics.Debug.WriteLine($"Received parameter value: {e.Parameter}");
                }
            }
        }

        private void OnTrainItemClicked(object sender, ItemClickEventArgs e)
        {
            var selectedTrain = e.ClickedItem as DisplayTrain;
            var combinedData = Tuple.Create(selectedTrain, paxCount);
            Frame.Navigate(typeof(SeatSelectionPage), combinedData);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
