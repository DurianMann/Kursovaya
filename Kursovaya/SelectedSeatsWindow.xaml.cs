using Kursovaya;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace Kursovaya
{
    public partial class SelectSeatsWindow : Window
    {
        private List<string> selectedSeats = new List<string>();
        public Film SelectedFilm { get; set; }
        public decimal TotalPrice { get; set; }// Добавляем свойство для фильма

        public SelectSeatsWindow()
        {
            InitializeComponent();
        }

        private void UpdatePriceLabel()
        {
            BalanceLabel.Content = $"Итого: {TotalPrice} руб.";
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            
                if (selectedSeats.Contains(button.Content))
                {
                    button.Background = Brushes.White;
                    selectedSeats.Remove(button.Content.ToString());
                    TotalPrice -= SelectedFilm.Price;
                    UpdatePriceLabel();
                }
                else
                {
                    button.Background = Brushes.Red;
                    selectedSeats.Add(button.Content.ToString());
                    TotalPrice += SelectedFilm.Price;
                    UpdatePriceLabel();
                }
            
            
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Выберите места для бронирования!");
                return;
            }
            

            // Передаем выбранные места и информацию о фильме на страницу подтверждения
            var confirmWindow = new ConfirmOrderWindow
            {
                SelectedSeats = string.Join(", ", selectedSeats),
                FilmTitle = SelectedFilm.Title,
                SessionTime = SelectedFilm.Time,
                TotalPrice = TotalPrice,
                UserBalance = MainWindow.UserBalance
            };

            confirmWindow.ShowDialog();

            // Очищаем выбранные места после подтверждения
            foreach (var seat in selectedSeats)
            {
                var button = FindName(seat) as Button;
                if (button != null)
                    button.Background = Brushes.White;
            }
            selectedSeats.Clear();
            TotalPrice = 0;
            UpdatePriceLabel();
        }
    }
}
