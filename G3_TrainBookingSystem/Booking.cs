using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3_TrainBookingSystem
{
    public class Booking
    {
        public string PassengerName { get; set; }
        public string Gender { get; set; }
        public string IC { get; set; }
        public string  Phonenumber { get; set; }
        public string trainNum { get; set; }
        public string TicketType { get; set; } //after seat selected
        public List<string> BookedSeats { get; set; } // List of booked seat numbers.
        public string  Price { get; set; } // during train selection
        public double TotalPrice { get; set; }// during train selection

        public string duration { get; set; }

        public string GoOrigin { get; set; } // begin
        public string GoDestination { get; set; }// begin
        public DateTime GoDate { get; set; }// begin
        public string GoDepartTime { get; set; } // during train selection
        public string GoArrivalTime { get; set; }// during train selection

        public string BackOrigin { get; set; }// begin
        public string BackDestination { get; set; }// begin
        public DateTime BackDate { get; set; }// begin
        public string BackDepartTime { get; set; }// during train selection
        public string BackArrivalTime { get; set; }// during train selection



        //public string PaymentDetails { get; set; }  // Simplified for the sake of example. Consider creating a separate Payment class for more details.
    }
}
