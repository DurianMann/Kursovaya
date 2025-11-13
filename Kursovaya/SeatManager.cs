using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    public static class SeatManager
    {
        // Ключ: FilmTitle + SessionTime
        // Значение: список занятых мест (каждый элемент — пара (ряд, место))
        private static Dictionary<string, List<string>> _bookedSeats =
            new Dictionary<string, List<string>>();

        public static bool IsSeatAvailable(string filmTitle, TimeOnly sessionTime, string seat)
        {
            var key = $"{filmTitle}_{sessionTime}";
            // Если ключа нет — место свободно
            if (!_bookedSeats.ContainsKey(key))
                return true;
            // Проверяем, занято ли конкретное место (ряд + номер)
            return !_bookedSeats[key].Contains((seat));
        }
        public static void BookSeat(string filmTitle, TimeOnly sessionTime, string seat)
        {
            var key = $"{filmTitle}_{sessionTime}";
            // Создаём список для сеанса, если его ещё нет
            if (!_bookedSeats.ContainsKey(key))
                _bookedSeats[key] = new List<string>();
            // Добавляем занятое место (ряд + номер)
            _bookedSeats[key].Add(seat);
        }
    }

}
