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

        public SelectSeatsWindow(Film film)
        {
            InitializeComponent();

            SelectedFilm = film;

            CreateSeatsGrid();
        }

        private void UpdatePriceLabel()
        {
            BalanceLabel.Content = $"Итого: {TotalPrice} руб.";
        }
        private void CreateSeatsGrid()
        {
            // Пример создания 10 мест
            for (int i = 1; i <= 80; i++)
            {
                Button seatButton = new Button
                {
                    Content = $"М{i}",
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(2),
                    Background = Brushes.White
                };

                seatButton.Click += Seat_Click;
                SeatsGrid.Children.Add(seatButton);
            }
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
                User = MainWindow.currentUser
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
