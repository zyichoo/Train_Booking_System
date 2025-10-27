using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3_TrainBookingSystem
{
    public class Passenger
    {
        public string FirebaseId { get; set; } // Unique identifier in Firebase, if needed
        public int PassengerId { get; set; }   // Unique identifier for the passenger
        public string FullName { get; set; }    // Passenger's full name
        public string IC { get; set; }          // Passenger's IC (Identity Card) number
        public string PhoneNumber { get; set; } // Passenger's phone number
        public string Gender { get; set; }      // Passenger's gender
        public string Email { get; set; }       // Passenger's email
        public string Password { get; set; }    // Passenger's hashed and salted password
    }
}
