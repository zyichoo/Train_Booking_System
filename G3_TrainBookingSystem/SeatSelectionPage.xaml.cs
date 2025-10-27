using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
    public sealed partial class SeatSelectionPage : Page
    {
        private DisplayTrain selectedTrain;

        private int PaxCount { get; set; }
        private List<string> SelectedSeats { get; set; } = new List<string>();

        public SeatSelectionPage()
        {
            this.InitializeComponent();
            GenerateSeatLayout();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Tuple<DisplayTrain, int> parameters)
            {
                selectedTrain = parameters.Item1;
                PaxCount = parameters.Item2;

                // Debugging line
                System.Diagnostics.Debug.WriteLine($"PaxCount: {PaxCount}");

                SelectedTrainDetails.Text = $"{selectedTrain.TrainNumber} from {selectedTrain.Origin} to {selectedTrain.Destination}";
                txtDepartureTime.Text = $"Departure Time: {selectedTrain.DepartureTime}";
                txtArrivalTime.Text = $"Arrival Time: {selectedTrain.ArrivalTime}";
                txtDuration.Text = $"Duration: {selectedTrain.Duration}";
                txtTicketPrice.Text = $"Ticket Price: {selectedTrain.TicketPrice}";
            }
        }

        private void GenerateSeatLayout()
        {
            char[] coaches = { 'A', 'B', 'C', 'D', 'E', 'F' };

            foreach (char coach in coaches)
            {
                // Coach Header as Pivot Item
                PivotItem pivotItem = new PivotItem
                {
                    Header = $"Coach {coach}"
                };

                // Seat Grid
                Grid grid = new Grid { HorizontalAlignment = HorizontalAlignment.Center }; // Center alignment added
                for (int i = 0; i < 16; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                }

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) }); // Spacer for the corridor
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (j == 2) continue;  // This will leave a space in the middle

                        Button seatButton = new Button
                        {
                            Content = $"{i + 1}{(char)('A' + j)}",
                            Background = new SolidColorBrush(Windows.UI.Colors.LightBlue),
                            Margin = new Thickness(1)
                        };

                        seatButton.Click += OnSeatClicked;

                        Grid.SetRow(seatButton, i);
                        Grid.SetColumn(seatButton, j);
                        grid.Children.Add(seatButton);
                    }
                }
                pivotItem.Content = grid;
                CoachPivot.Items.Add(pivotItem);
            }
        }

        private List<Button> selectedSeats = new List<Button>();

        private async void OnSeatClicked(object sender, RoutedEventArgs e)
        {
            Button seatButton = sender as Button;

            if (seatButton.Background is SolidColorBrush brush)
            {
                if (brush.Color == Windows.UI.Colors.LightBlue)
                {
                    if (selectedSeats.Count < PaxCount)
                    {
                        seatButton.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                        selectedSeats.Add(seatButton);
                    }
                    else
                    {
                        await new MessageDialog($"You can only select {PaxCount} seats.", "Error").ShowAsync();
                        return; // Prevents the subsequent logic from being executed if the user has already selected maximum seats.
                    }
                }
                else if (brush.Color == Windows.UI.Colors.Blue)
                {
                    seatButton.Background = new SolidColorBrush(Windows.UI.Colors.LightBlue);
                    selectedSeats.Remove(seatButton);
                }
            }

            // Separate logic for seat confirmation after the seat has been selected/deselected.
            if (selectedSeats.Count == PaxCount)
            {
                var dialog = new ContentDialog
                {
                    Title = "Seat Selection",
                    Content = $"Do you wish to confirm the selected seats?",
                    PrimaryButtonText = "Confirm",
                    CloseButtonText = "Cancel"
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    var obj = App.Current as App;
                    

                    if (obj.TripType == "Round trip" && obj.count <2)
                    {
                        string temporigin, tempdesti;
                        temporigin = obj.origin;
                        tempdesti = obj.destinantion;

                        obj.origin = tempdesti;
                        obj.destinantion = temporigin;
                        var parameters = Tuple.Create(obj.origin, obj.destinantion, obj.date, obj.paxCount);
                        Frame.Navigate(typeof(TrainSearchResultsPage), parameters);
                        obj.count++;
                    }
                    else
                    Frame.Navigate(typeof(PassengerDetailsPage), selectedTrain);

                }
                else
                {
                    foreach (var seatBtn in selectedSeats)
                    {
                        seatBtn.Background = new SolidColorBrush(Windows.UI.Colors.LightBlue);
                    }
                    selectedSeats.Clear();
                }
            }
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            // Navigate back to previous page
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }


}
