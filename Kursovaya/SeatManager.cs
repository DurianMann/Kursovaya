using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    public static class SeatManager
    {
        // Ключ: FilmTitle + SessionTime, значение: список занятых мест
        private static Dictionary<string, List<string>> _bookedSeats =
            new Dictionary<string, List<string>>();

        public static bool IsSeatAvailable(string filmTitle, TimeOnly sessionTime, string seat)
        {
            var key = $"{filmTitle}_{sessionTime}";
            return !_bookedSeats.ContainsKey(key) || !_bookedSeats[key].Contains(seat);
        }

        public static void BookSeat(string filmTitle, TimeOnly sessionTime, string seat)
        {
            var key = $"{filmTitle}_{sessionTime}";
            if (!_bookedSeats.ContainsKey(key))
                _bookedSeats[key] = new List<string>();
            _bookedSeats[key].Add(seat);
        }
    }

}
