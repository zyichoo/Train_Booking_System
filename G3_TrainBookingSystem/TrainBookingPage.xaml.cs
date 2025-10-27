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
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace G3_TrainBookingSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrainBookingPage : Page
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public DateTime SelectedDate { get; set; }
        public string TripType { get; set; }

        public TrainBookingPage()
        {
            this.InitializeComponent();
        }

        private async void TrainBookingPage_Loaded(object sender, RoutedEventArgs e)
        {
            await firebaseHelper.PopulateTrainStopsAsync();
            await firebaseHelper.PopulateSampleTrainsAsync();  
            LoadStops();
            LoadPaxOptions();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Tuple<string, string, object, DateTime?> navigationData)
            {
                // Populate previous selections if available
                originComboBox.SelectedItem = navigationData.Item1;
                destinationComboBox.SelectedItem = navigationData.Item2;
                paxComboBox.SelectedItem = navigationData.Item3;

                if (navigationData.Item4.HasValue)
                {
                    SelectedDate = navigationData.Item4.Value;
                    SelectDateButton.Content = SelectedDate.ToString("dd MMM yyyy");
                }
            }
            else if (e.Parameter is Tuple<DateTime, string> dateAndType)
            {
                SelectedDate = dateAndType.Item1;
                TripType = dateAndType.Item2;
                SelectDateButton.Content = SelectedDate.ToString("dd MMM yyyy");
            }
        }

        private bool stopsLoaded = false;

        private async void LoadStops()
        {
            System.Diagnostics.Debug.WriteLine("LoadStops method called.");

            if (originComboBox.ItemsSource != null && destinationComboBox.ItemsSource != null) return;

            try
            {
                var stopData = await firebaseHelper.GetAllStops();
                if (stopData != null && stopData.Any())
                {
                    var stops = stopData.Select(s => s.Name).ToList();
                    originComboBox.ItemsSource = stops;
                    destinationComboBox.ItemsSource = stops;

                    searchButton.IsEnabled = true;
                }
                else
                {
                    await new MessageDialog("No stops found.", "Error").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog($"Error loading stops: {ex.Message}", "Error").ShowAsync();
            }
        }

        private void LoadPaxOptions()
        {
            List<int> paxOptions = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                paxOptions.Add(i);
            }
            paxComboBox.ItemsSource = paxOptions;
        }

        private void SelectDateButton_Click(object sender, RoutedEventArgs e)
        {
            var origin = originComboBox.SelectedItem?.ToString();
            var destination = destinationComboBox.SelectedItem?.ToString();
            var pax = paxComboBox.SelectedItem;

            var navigationData = new Tuple<string, string, object, DateTime?>(origin, destination, pax, SelectedDate);


            Frame.Navigate(typeof(DateSelectionPage), navigationData);
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Origin Selected: {originComboBox.SelectedItem}");
            System.Diagnostics.Debug.WriteLine($"Destination Selected: {destinationComboBox.SelectedItem}");
            System.Diagnostics.Debug.WriteLine($"Pax Selected: {paxComboBox.SelectedItem}");

            if (originComboBox.SelectedItem == null || destinationComboBox.SelectedItem == null || paxComboBox.SelectedItem == null)
            {
                await new MessageDialog("Please select the origin, destination, and number of passengers.", "Error").ShowAsync();
                return;
            }

            var origin = originComboBox.SelectedItem.ToString();
            var destination = destinationComboBox.SelectedItem.ToString();
            var date = SelectedDate;
            var paxCount = int.Parse(paxComboBox.SelectedItem.ToString());

            var obj = App.Current as App;
            obj.origin = origin;
            obj.destinantion  = destination;
            obj.date = date;
            obj.paxCount = paxCount;



            var parameters = Tuple.Create(origin, destination, date, paxCount);
            Frame.Navigate(typeof(TrainSearchResultsPage), parameters);

           

            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{(sender as ComboBox).Name} selection changed to: {(sender as ComboBox).SelectedItem}");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditProfilePage));
        }
    }
}
