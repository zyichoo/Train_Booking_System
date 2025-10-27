using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3_TrainBookingSystem
{
    public class TrainDetail
    {
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public DateTime DepartureDate { get; set; }
        public int AvailableSeats { get; set; }
        public TimeSpan Duration { get; set; }
        public double TicketPrice { get; set; }

        public string EstimatedArrivalTime
        {
            get
            {
                var departureDateTime = DateTime.Parse(DepartureTime);
                return departureDateTime.Add(Duration).ToString("hh:mm tt");
            }
        }

        public string DepartureDateFormatted => DepartureDate.ToString("dd MMM yyyy");
        public string DurationFormatted => Duration.ToString(@"hh\:mm") + " hrs";
        public string TicketPriceFormatted => "RM " + TicketPrice.ToString("F2");
    }
}
