using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3_TrainBookingSystem
{
    public class DisplayTrain
    {
        public string TrainID { get; set; }
        public string TrainNumber { get; set; } 
        public string DepartureTime { get; set; }

       
        public string ArrivalTime { get; set; }
        public string Duration { get; set; }
        public string TicketPrice { get; set; }
        public string Origin { get; set; } 
        public string Destination { get; set; } 
    }
}
