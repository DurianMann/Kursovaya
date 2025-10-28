using System.Windows;



namespace Kursovaya
{
    public partial class ConfirmOrderWindow : Window
    {
        public string FilmTitle { get; set; }
        public string SessionTime { get; set; }
        public string SelectedSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public User User { get; set; }

        public ConfirmOrderWindow()
        {
            InitializeComponent();
        }
        public ConfirmOrderWindow(string filmTitle, string sessionTime, string selectedSeats,
            decimal totalPrice, User user) : this()
        {
            FilmTitle = filmTitle;
            SessionTime = sessionTime;
            SelectedSeats = selectedSeats;
            TotalPrice = totalPrice;
            User = user;

            UpdateUI();
        }
        private void UpdateUI()
        {
            FilmTitleTextBlock.Text = FilmTitle;
            SessionTimeTextBlock.Text = SessionTime;
            SelectedSeatsTextBlock.Text = SelectedSeats;
            TotalPriceTextBlock.Text = $"{TotalPrice} руб.";
            BalanceTextBlock.Text = $"{User.Balance} руб.";
            UpdateUI();
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            FilmTitleTextBlock.Text = FilmTitle;
            SessionTimeTextBlock.Text = SessionTime;
            SelectedSeatsTextBlock.Text = SelectedSeats;
            TotalPriceTextBlock.Text = $"{TotalPrice} руб.";
            BalanceTextBlock.Text = $"{User.Balance - TotalPrice} руб.";
            
            if (TotalPrice > User.Balance)
            {
                MessageBox.Show("Недостаточно средств на балансе!");
                return;
            }
            User.Balance -= TotalPrice;
            Close();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}