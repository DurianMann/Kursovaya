using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    public class Booking
    {
        public string FilmTitle { get; set; }
        public string SessionTime { get; set; }
        public List<string> Seats { get; set; } = new List<string>();
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}
