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

        public ConfirmOrderWindow()
        {
            InitializeComponent();
        }
        public ConfirmOrderWindow(string filmTitle, string sessionTime, List<string> selectedSeats,
            decimal totalPrice, User user) : this()
        {
            FilmTitle = filmTitle;
            SessionTime = sessionTime;
            SelectedSeats = selectedSeats;
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
            BalanceTextBlock.Text = $"{ThisUser.Balance} руб.";
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            FilmTitleTextBlock.Text = FilmTitle;
            SessionTimeTextBlock.Text = SessionTime;
            SelectedSeatsTextBlock.Text = SelectedSeats.ToString();
            TotalPriceTextBlock.Text = $"{TotalPrice} руб.";
            BalanceTextBlock.Text = $"{ThisUser.Balance - TotalPrice} руб.";
            
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
            var booking = new Booking
            {
                FilmTitle = FilmTitle,
                SessionTime = SessionTime,
                Seats = SelectedSeats,
                TotalPrice = TotalPrice
            };

            // Сохраняем в историю пользователя
            ThisUser.Bookings.Add(booking);
            ThisUser.Balance -= TotalPrice;
            Close();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}