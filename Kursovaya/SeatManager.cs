using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovaya
{
    public static class SeatManager
    {
        // Ключ: FilmTitle + SessionTime
        // Значение: список занятых мест (каждый элемент — место)
        private static Dictionary<string, List<string>> _bookedSeats =
            new Dictionary<string, List<string>>();
        private static AppDbContext context = App.Context;
        public static bool IsSeatAvailable(string filmTitle, TimeOnly sessionTime, string seat)
        {
            var key = $"{filmTitle}_{sessionTime}";
            // Если ключа нет — место свободно
            if (!_bookedSeats.ContainsKey(key))
                return true;
            // Проверяем, занято ли конкретное место
            return !_bookedSeats[key].Contains(seat);
        }
        public static void BookSeat(string filmTitle, TimeOnly sessionTime, string seat)
        {
            var key = $"{filmTitle}_{sessionTime}";
            // Создаём список для сеанса, если его ещё нет
            if (!_bookedSeats.ContainsKey(key))
                _bookedSeats[key] = new List<string>();
            // Добавляем занятое место
            _bookedSeats[key].Add(seat);
        }
        public static void RemoveBookSeat(string filmTitle, TimeOnly sessionTime, int id)
        {
            var key = $"{filmTitle}_{sessionTime}";
            if (_bookedSeats.ContainsKey(key))
            {
                _bookedSeats.Remove(key);
                foreach (Booking booking in context.Bookings)
                    if (booking.Id == id)
                    
                        context.Bookings.Remove(booking);
                        context.SaveChanges();
                    
                return;
            }
            if (!_bookedSeats.ContainsKey(key))
                MessageBox.Show("Произошла попытка возврата средств за возвращенный билет");
        }
    }

}
