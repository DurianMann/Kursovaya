using System.Windows;



namespace Kursovaya
{
    public partial class ConfirmOrderWindow : Window
    {
        public string FilmTitle { get; set; }
        public string SessionTime { get; set; }
        public List<string> SelectedSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public User ThisUser { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;


        public ConfirmOrderWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public ConfirmOrderWindow(string filmTitle, string sessionTime, List<string> selectSeats,
            decimal totalPrice, User user) : this()
        {
            FilmTitle = filmTitle;
            SessionTime = sessionTime;
            SelectedSeats = selectSeats;
            TotalPrice = totalPrice;
            ThisUser = user;

            UpdateUI();
        }
        private void UpdateUI()
        {
            FilmTitleTextBlock.Text = FilmTitle;
            SessionTimeTextBlock.Text = SessionTime;
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

            foreach (var seat in SelectedSeats)
            {
                SeatManager.BookSeat(FilmTitle, SessionTime, seat);
            }
            // Создаем запись о бронировании
            User.Booking booking = new User.Booking
            {
                FilmTitle = FilmTitle,
                SessionTime = SessionTime,
                Seats = string.Join(", ", SelectedSeats),
                TotalPrice = TotalPrice,
                BookingDate = BookingDate
            };

            // Сохраняем в историю пользователя
            if (booking != null && ThisUser.Bookings != null)
            {
                ThisUser.Bookings.Add(booking);
                ThisUser.Balance -= TotalPrice;
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