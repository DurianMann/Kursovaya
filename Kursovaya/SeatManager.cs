using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Kursovaya
{
    public static class SeatManager
    {
        private static AppDbContext context = App.Context;

        /// <summary>
        /// Проверяет, доступно ли место на сеанс фильма.
        /// </summary>
        public static bool IsSeatAvailable(string filmTitle, TimeOnly sessionTime, string seat)
        {
            try
            {
                // Ищем бронирование с таким фильмом, временем и местом
                var existingBooking = context.Bookings
                    .FirstOrDefault(b =>
                        b.FilmTitle == filmTitle &&
                        b.SessionTime == sessionTime &&
                        b.Seats.Contains(seat));

                return existingBooking == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке доступности места: {ex.Message}");
                return false; // В случае ошибки считаем, что место недоступно
            }
        }

        /// <summary>
        /// Бронирует место на сеанс.
        /// </summary>
        public static bool BookSeat(string filmTitle, TimeOnly sessionTime, string seat, int userId, int filmId, decimal price)
        {
            try
            {
                using var transaction = context.Database.BeginTransaction();

                // Проверяем доступность места
                if (!IsSeatAvailable(filmTitle, sessionTime, seat))
                    return false;

                // Создаём новое бронирование
                var booking = new Booking
                {
                    UserId = userId,
                    FilmId = filmId,
                    SessionTime = sessionTime,
                    Seats = new List<string> { seat },
                    TotalPrice = price,
                    BookingDate = DateTime.Now,
                    FilmTitle = filmTitle
                };

                context.Bookings.Add(booking);
                context.SaveChanges();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при бронировании места: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Отменяет бронирование места (возвращает билет).
        /// </summary>
        public static bool RemoveBookSeat(int bookingId)
        {
            try
            {
                using var transaction = context.Database.BeginTransaction();

                var booking = context.Bookings
                    .Include(b => b.User) // Загружаем пользователя, чтобы обновить баланс
                    .FirstOrDefault(b => b.Id == bookingId);

                if (booking == null)
                {
                    MessageBox.Show("Бронирование не найдено.");
                    return false;
                }

                // Возвращаем деньги на баланс пользователя
                booking.User.Balance += booking.TotalPrice;

                context.Bookings.Remove(booking);
                context.Users.Update(booking.User); // Сохраняем изменения баланса
                context.SaveChanges();

                transaction.Commit();
                MessageBox.Show($"Билет успешно возвращён. На баланс возвращено {booking.TotalPrice} руб.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отмене бронирования: {ex.Message}");
                MessageBox.Show("Произошла ошибка при возврате билета.");
                return false;
            }
        }


        /// <summary>
        /// Получает список всех забронированных мест для конкретного сеанса.
        /// </summary>
        public static List<string> GetBookedSeats(string filmTitle, TimeOnly sessionTime)
        {
            try
            {
                var bookedSeats = context.Bookings
                    .Where(b => b.FilmTitle == filmTitle && b.SessionTime == sessionTime)
                    .SelectMany(b => b.Seats)
                    .ToList();

                return bookedSeats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении списка занятых мест: {ex.Message}");
                return new List<string>();
            }
        }
    }

}
