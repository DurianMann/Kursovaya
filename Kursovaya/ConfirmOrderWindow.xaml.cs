using System.Windows;



namespace Kursovaya
{
    public partial class ConfirmOrderWindow : Window
    {
        public string FilmTitle { get; set; }
        public string SessionTime { get; set; }
        public string SelectedSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UserBalance { get; set; }

        public ConfirmOrderWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (TotalPrice > UserBalance)
            {
                MessageBox.Show("Недостаточно средств на балансе!");
                return;
            }
            FilmTitleTextBlock.Text = FilmTitle;
            SessionTimeTextBlock.Text = SessionTime;
            SelectedSeatsTextBlock.Text = SelectedSeats;
            TotalPriceTextBlock.Text = $"{TotalPrice} руб.";
            MainWindow.UserBalance -= TotalPrice;
            BalanceTextBlock.Text = $"{MainWindow.UserBalance} руб.";
            MessageBox.Show($"Заказ успешно оформлен!\n" +
                       $"Списано: {TotalPrice} руб.\n" +
                       $"Остаток: {MainWindow.UserBalance} руб.");
            BalanceTextBlock.Text = $"{UserBalance} руб.";
            Close();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}