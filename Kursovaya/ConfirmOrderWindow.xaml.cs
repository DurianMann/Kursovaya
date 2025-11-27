using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Windows;
namespace Kursovaya
{
    public partial class ConfirmOrderWindow : Window
    {
        public string FilmTitle { get; set; }
        public int FilmId { get; set; }
        public TimeOnly SessionTime { get; set; }
        public List<string> SelectedSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public User ThisUser { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        private AppDbContext _context;
        public ConfirmOrderWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public ConfirmOrderWindow(string filmTitle, int filmId, TimeOnly sessionTime, List<string> selectSeats,
            decimal totalPrice, User user, AppDbContext context) : this()
        {
            FilmTitle = filmTitle;
            FilmId = filmId;
            SessionTime = sessionTime;
            SelectedSeats = selectSeats;
            TotalPrice = totalPrice;
            ThisUser = user;
            _context = context;
            UpdateUI();
        }
        private void UpdateUI()
        {
            FilmTitleTextBlock.Text = FilmTitle;
            SessionTimeTextBlock.Text = Convert.ToString(SessionTime);
            SelectedSeatsTextBlock.Text = string.Join(", ", SelectedSeats);
            TotalPriceTextBlock.Text = $"{TotalPrice} руб.";
            BalanceTextBlock.Text = $"{ThisUser.Balance - TotalPrice} руб.";
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (TotalPrice > ThisUser.Balance)
            {
                MessageBox.Show("Недостаточно средств на балансе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // 1. Проверяем доступность ВСЕХ мест
                foreach (string seat in SelectedSeats)
                {
                    if (!SeatManager.IsSeatAvailable(FilmTitle, SessionTime, seat))
                    {
                        MessageBox.Show($"Место {seat} уже занято. Выберите другие места.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // 2. Создаём ОДНО бронирование на все места
                var booking = new Booking
                {
                    FilmTitle = FilmTitle,
                    FilmId = FilmId,
                    SessionTime = SessionTime,
                    Seats = SelectedSeats, // Все места в одном списке
                    TotalPrice = TotalPrice,
                    BookingDate = BookingDate,
                    UserId = ThisUser.Id
                };

                // 3. Обновляем баланс пользователя
                ThisUser.Balance -= TotalPrice;

                // 4. Сохраняем в БД
                _context.Bookings.Add(booking);
                _context.Users.Update(ThisUser);
                _context.SaveChanges();

                MessageBox.Show("Бронирование успешно оформлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}