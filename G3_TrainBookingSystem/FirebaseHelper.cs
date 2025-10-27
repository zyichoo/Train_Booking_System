using Firebase.Database;
using Firebase.Database.Query;
using G3_TrainBookingSytem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3_TrainBookingSystem
{
    public class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient(GlobalData.firebaseDatabase);

        // Booking methods
        public async Task<List<Booking>> GetAllBookings()
        {
            return (await firebase
              .Child("Bookings")
              .OnceAsync<Booking>()).Select(item => new Booking
              {
                  PassengerName = item.Object.PassengerName,
                  IC = item.Object.IC,
                  TicketType = item.Object.TicketType,
                  BookedSeats = item.Object.BookedSeats,
                  Price = item.Object.Price,
                  TotalPrice = item.Object.TotalPrice,
                  GoOrigin = item.Object.GoOrigin,
                  GoDestination = item.Object.GoDestination,
                  BackOrigin = item.Object.BackOrigin,
                  BackDestination = item.Object.BackDestination,
                  GoDate = item.Object.GoDate,
                  BackDate = item.Object.BackDate,
                  GoDepartTime = item.Object.GoDepartTime,
                  GoArrivalTime = item.Object.GoArrivalTime,

              }).ToList();
        }

        public async Task AddBooking(Booking booking)
        {
            await firebase
             .Child("Bookings")
             .PostAsync(JsonConvert.SerializeObject(booking));
        }

        public async Task<Booking> GetBooking(string Ic)
        {
            var allBookings = await GetAllBookings();
            return allBookings.FirstOrDefault(a => a.IC == Ic);
        }

        public async Task UpdateBooking(string Ic, Booking booking)
        {
            await firebase
             .Child("Bookings")
             .Child(Ic)
             .PutAsync(JsonConvert.SerializeObject(booking));
        }

        public async Task DeleteBooking(string IC)
        {
            await firebase.Child("Bookings").Child(IC).DeleteAsync();
        }

        // Train methods
        public async Task<List<DisplayTrain>> GetAllTrains()
        {
            return (await firebase
                .Child("Trains")
                .OnceAsync<DisplayTrain>()).Select(item => new DisplayTrain
                {
                    TrainID = item.Key,
                    TrainNumber = item.Object.TrainNumber,
                    DepartureTime = item.Object.DepartureTime,
                    ArrivalTime = item.Object.ArrivalTime,
                    Duration = item.Object.Duration,
                    TicketPrice = item.Object.TicketPrice,
                    Origin = item.Object.Origin,
                    Destination = item.Object.Destination
                }).ToList();
        }

        public async Task AddTrain(DisplayTrain train)
        {
            await firebase
                .Child("Trains")
                .PostAsync(JsonConvert.SerializeObject(train));
        }

        public async Task<DisplayTrain> GetTrain(string trainId)
        {
            var allTrains = await GetAllTrains();
            return allTrains.FirstOrDefault(a => a.TrainID == trainId);
        }

        public async Task UpdateTrain(string trainId, DisplayTrain train)
        {
            await firebase
                .Child("Trains")
                .Child(trainId)
                .PutAsync(JsonConvert.SerializeObject(train));
        }

        public async Task DeleteTrain(string trainId)
        {
            await firebase.Child("Trains").Child(trainId).DeleteAsync();
        }

        public async Task PopulateSampleTrainsAsync()
        {
            try
            {
                // Check if trains already exist
                var existingTrains = await GetAllTrains();
                if (existingTrains != null && existingTrains.Count > 0)
                {
                    return; // Trains already exist, no need to initialize
                }

                // Sample train data (you can expand or modify this list as needed)
                var sampleTrains = new List<DisplayTrain>
                {
                    new DisplayTrain { TrainID = "1", TrainNumber = "KL101", Origin = "Kuala Lumpur", Destination = "Penang", DepartureTime = "08:00", ArrivalTime = "10:00", Duration = "2 hours", TicketPrice = "RM 45.00" },
                    new DisplayTrain { TrainID = "2", TrainNumber = "KL102", Origin = "Kuala Lumpur", Destination = "Penang", DepartureTime = "10:00", ArrivalTime = "12:00", Duration = "2 hours", TicketPrice = "RM 45.00" },
                    new DisplayTrain { TrainID = "3", TrainNumber = "KL103", Origin = "Kuala Lumpur", Destination = "Penang", DepartureTime = "13:00", ArrivalTime = "15:00", Duration = "2 hours", TicketPrice = "RM 45.00" },
                    new DisplayTrain { TrainID = "4", TrainNumber = "PG104", Origin = "Penang", Destination = "Kuala Lumpur", DepartureTime = "13:00", ArrivalTime = "15:00", Duration = "2 hours", TicketPrice = "RM 45.00" },
                    new DisplayTrain { TrainID = "5", TrainNumber = "PG105", Origin = "Penang", Destination = "Kuala Lumpur", DepartureTime = "10:00", ArrivalTime = "12:00", Duration = "2 hours", TicketPrice = "RM 45.00" },
                    new DisplayTrain { TrainID = "6", TrainNumber = "PG106", Origin = "Penang", Destination = "Kuala Lumpur", DepartureTime = "08:00", ArrivalTime = "10:00", Duration = "2 hours", TicketPrice = "RM 45.00" },
                    new DisplayTrain { TrainID = "7", TrainNumber = "PG107", Origin = "Penang", Destination = "Johor Bahru",  DepartureTime = "11:00", ArrivalTime = "13:00", Duration = "2 hours", TicketPrice = "RM 55.00" },
                    new DisplayTrain { TrainID = "8", TrainNumber = "PG108", Origin = "Penang", Destination = "Johor Bahru",  DepartureTime = "13:00", ArrivalTime = "15:00", Duration = "2 hours", TicketPrice = "RM 55.00" },
                    new DisplayTrain { TrainID = "9", TrainNumber = "PG109", Origin = "Penang", Destination = "Johor Bahru",  DepartureTime = "16:00", ArrivalTime = "18:00", Duration = "2 hours", TicketPrice = "RM 55.00" },
                    new DisplayTrain { TrainID = "10", TrainNumber = "JB110", Origin = "Johor Bahru", Destination = "Penang",  DepartureTime = "11:00", ArrivalTime = "13:00", Duration = "2 hours", TicketPrice = "RM 55.00" },
                    new DisplayTrain { TrainID = "11", TrainNumber = "JB111", Origin = "Johor Bahru", Destination = "Penang",  DepartureTime = "13:00", ArrivalTime = "15:00", Duration = "2 hours", TicketPrice = "RM 55.00" },
                    new DisplayTrain { TrainID = "12", TrainNumber = "JB112", Origin = "Johor Bahru", Destination = "Penang",  DepartureTime = "16:00", ArrivalTime = "18:00", Duration = "2 hours", TicketPrice = "RM 55.00" },


                    new DisplayTrain { TrainID = "13", TrainNumber = "KL113", Origin = "Kuala Lumpur", Destination = "Johor Bahru", DepartureTime = "09:00", ArrivalTime = "13:30", Duration = "4 hours 30 minutes", TicketPrice = "RM 50.00" },
                    new DisplayTrain { TrainID = "14", TrainNumber = "KL114", Origin = "Kuala Lumpur", Destination = "Ipoh", DepartureTime = "10:00", ArrivalTime = "13:00", Duration = "3 hours", TicketPrice = "RM 30.00" },
                    new DisplayTrain { TrainID = "15", TrainNumber = "KL115", Origin = "Kuala Lumpur", Destination = "Malacca City", DepartureTime = "07:30", ArrivalTime = "10:00", Duration = "2 hours 30 minutes", TicketPrice = "RM 35.00" },
                    new DisplayTrain { TrainID = "16", TrainNumber = "KL116", Origin = "Kuala Lumpur", Destination = "Kota Kinabalu", DepartureTime = "20:00", ArrivalTime = "08:00", Duration = "12 hours", TicketPrice = "RM 80.00" },
                    new DisplayTrain { TrainID = "17", TrainNumber = "PG117", Origin = "Penang", Destination = "Johor Bahru", DepartureTime = "11:00", ArrivalTime = "17:00", Duration = "6 hours", TicketPrice = "RM 55.00" },
                    new DisplayTrain { TrainID = "18", TrainNumber = "JB118", Origin = "Johor Bahru", Destination = "Ipoh", DepartureTime = "14:00", ArrivalTime = "18:00", Duration = "4 hours", TicketPrice = "RM 50.00" },
                    new DisplayTrain { TrainID = "19", TrainNumber = "IP119", Origin = "Ipoh", Destination = "Kota Kinabalu", DepartureTime = "19:00", ArrivalTime = "07:00", Duration = "12 hours", TicketPrice = "RM 85.00" },
                    new DisplayTrain { TrainID = "20", TrainNumber = "MC120", Origin = "Malacca City", Destination = "Kuching", DepartureTime = "09:30", ArrivalTime = "16:30", Duration = "7 hours", TicketPrice = "RM 60.00" },
                    new DisplayTrain { TrainID = "21", TrainNumber = "KC121", Origin = "Kuching", Destination = "Kota Kinabalu", DepartureTime = "10:00", ArrivalTime = "17:00", Duration = "7 hours", TicketPrice = "RM 65.00" }
                };

                // Add the sample trains to Firebase
                foreach (var train in sampleTrains)
                {
                    await AddTrain(train);
                }
            }
            catch (Exception ex)
            {
                // Log the error or show a message to the user
                System.Diagnostics.Debug.WriteLine($"Error populating sample trains: {ex.Message}");
            }
        }

        // Stops methods
        public async Task<List<Stop>> GetAllStops()
        {
            var result = (await firebase
                .Child("Stops")
                .OnceAsync<Stop>())
                .Select(item => new Stop
                {
                    StopID = item.Key,
                    Name = item.Object.Name
                })
                .ToList();

            System.Diagnostics.Debug.WriteLine($"Fetched stops count from Firebase: {result.Count}");
            return result;
        }

        public async Task AddStop(Stop stop)
        {
            await firebase
                .Child("Stops")
                .PostAsync(JsonConvert.SerializeObject(stop));
        }

        public async Task<Stop> GetStop(string stopId)
        {
            var allStops = await GetAllStops();
            return allStops.FirstOrDefault(a => a.StopID == stopId);
        }

        public async Task UpdateStop(string stopId, Stop stop)
        {
            await firebase
                .Child("Stops")
                .Child(stopId)
                .PutAsync(JsonConvert.SerializeObject(stop));
        }

        public async Task DeleteStop(string stopId)
        {
            await firebase.Child("Stops").Child(stopId).DeleteAsync();
        }

        public async Task PopulateTrainStopsAsync()
        {
            try
            {
                // Check if stops already exist
                var existingStops = await GetAllStops();
                if (existingStops != null && existingStops.Count > 0)
                {
                    return; // Stops already exist, no need to initialize
                }

                // List of train stops (you can add more stops if needed)
                var trainStops = new List<string>
                {
                    "Kuala Lumpur",
                    "Penang",
                    "Johor Bahru",
                    "Ipoh",
                    "Kota Kinabalu",
                    "Kuching",
                    "Malacca City",
                    // ... add other stops as needed
                };

                // Populate the new train stops
                foreach (var stop in trainStops)
                {
                    await AddStop(new Stop { Name = stop });
                }
            }
            catch (Exception ex)
            {
                // Log the error or show a message to the user
                System.Diagnostics.Debug.WriteLine($"Error populating train stops: {ex.Message}");
            }
        }

        FirebaseClient fbc = new FirebaseClient(GlobalData.firebaseDatabase);

        public async Task AddAdmin(Admin admin)
        {
            await fbc.Child("Admin").PostAsync(JsonConvert.SerializeObject(admin));
        }

        public async Task<List<Admin>> GetAllAdmin()
        {
            return (await fbc
                .Child("Passengers")
                .OnceAsync<Admin>()).Select(item => new Admin
                {
                    FullName = item.Object.FullName,
                    IC = item.Object.IC,
                    Phone = item.Object.Phone,
                    Gender = item.Object.Gender,
                    Email = item.Object.Email,
                    Password = item.Object.Password
                }).ToList();
        }

        public async Task<Admin> GetAdmin(string email, string password)
        {
            var allAdmins = await GetAllAdmin();
            return allAdmins.FirstOrDefault(a => a.Email == email && a.Password == password); 
        }

        public async Task UpdateAdmin(string IC, Admin a)
        {
            await firebase2
                .Child("Admin")
                .Child(IC)
                .PutAsync(a);
        }

        public async Task DeleteAdmin(string IC)
        {
            await firebase2.Child("Admin").Child(IC).DeleteAsync();
        }

        FirebaseClient firebase2 = new FirebaseClient(GlobalData.firebaseDatabase);

        public async Task<List<Passenger>> GetAllPassengers()
        {
            return (await firebase2
                .Child("Passengers")
                .OnceAsync<Passenger>()).Select(item => new Passenger
                {
                    FirebaseId = item.Key.ToString(),
                    PassengerId = item.Object.PassengerId,
                    FullName = item.Object.FullName,
                    IC = item.Object.IC,
                    PhoneNumber = item.Object.PhoneNumber,
                    Gender = item.Object.Gender,
                    Email = item.Object.Email,
                    Password = item.Object.Password
                }).ToList();
        }

        public async Task AddPassenger(Passenger p)
        {
            await firebase2
                .Child("Passengers")
                .PostAsync(p);
        }

        public async Task<Passenger> GetPassenger(string firebaseId)
        {
            var allPassengers = await GetAllPassengers();
            return allPassengers.FirstOrDefault(p => p.FirebaseId == firebaseId);
        }

        public async Task<Passenger> GetPassenger2(string IC)
        {
            var allPassengers = await GetAllPassengers();
            return allPassengers.FirstOrDefault(p => p.IC == IC);
        }

        public async Task UpdatePassenger(string firebaseId, Passenger p)
        {
            await firebase2
                .Child("Passengers")
                .Child(firebaseId)
                .PutAsync(p);
        }

        public async Task DeletePassenger(string firebaseId)
        {
            await firebase2.Child("Passengers").Child(firebaseId).DeleteAsync();
        }


      


        // Other methods for ImageTitle as needed
    }
}
