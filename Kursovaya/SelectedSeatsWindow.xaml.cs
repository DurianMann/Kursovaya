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
            MarkBookedSeats();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MarkBookedSeats(); // Перепроверяем статусы при загрузке
        }

        

        private void MarkBookedSeats()
        {
            foreach (var child in SeatsGrid.Children)
            {
                if (child is Button button)
                {
                    string seat = button.Content.ToString();
                    // Проверяем через глобальный менеджер
                    if (!SeatManager.IsSeatAvailable(SelectedFilm.Title, SelectedFilm.Time, seat))
                    {
                        button.Background = Brushes.Gray;
                        button.IsEnabled = false;
                        button.ToolTip = "Место занято";
                    }
                }
            }
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string seat = button.Content.ToString();

            if (selectedSeats.Contains(seat))
            {
                // Снятие выбора
                button.Background = Brushes.White;
                selectedSeats.Remove(seat);
                TotalPrice -= SelectedFilm.Price;
            }
            else
            {
                // Проверка через глобальный менеджер
                if (!SeatManager.IsSeatAvailable(SelectedFilm.Title, SelectedFilm.Time, seat))
                {
                    MessageBox.Show($"Место {seat} уже забронировано!");
                    return;
                }

                // Выбор места
                button.Background = Brushes.Red;
                selectedSeats.Add(seat);
                TotalPrice += SelectedFilm.Price;
            }

            UpdatePriceLabel();
        }



        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Выберите места для бронирования!");
                return;
            }

            var confirmWindow = new ConfirmOrderWindow(
                SelectedFilm.Title,
                SelectedFilm.Time,
                selectedSeats,
                TotalPrice,
                MainWindow.currentUser
            );

            confirmWindow.ShowDialog();

            selectedSeats.Clear();
            TotalPrice = 0;
            UpdatePriceLabel();
            MarkBookedSeats();
            Close();// Обновляем отображение мест
        }

        
    }
}
