using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Windows;
namespace Kursovaya
{
    public partial class ConfirmOrderWindow : Window
    {
        public string FilmTitle { get; set; }
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
        public ConfirmOrderWindow(string filmTitle, TimeOnly sessionTime, List<string> selectSeats,
            decimal totalPrice, User user, AppDbContext context) : this()
        {
            FilmTitle = filmTitle;
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
                MessageBox.Show("Недостаточно средств на балансе!");
                return;
            }
            foreach (string seat in SelectedSeats)
            {
                SeatManager.BookSeat(FilmTitle, SessionTime, seat);
            }
            // Создаем запись о бронировании
            Booking booking = new Booking
            {
                FilmTitle = FilmTitle,
                SessionTime = SessionTime,
                Seats = SelectedSeats,
                TotalPrice = TotalPrice,
                BookingDate = BookingDate,
                UserId = ThisUser.Id
            };
            // Сохраняем в историю пользователя
            if (booking != null && ThisUser.Bookings != null)
            {
                ThisUser.Bookings.Add(booking);
                ThisUser.Balance -= TotalPrice;
                _context.SaveChanges();
                Close();
            }
            else
            {
                MessageBox.Show("Произошла ошибка при покупке билета");
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}