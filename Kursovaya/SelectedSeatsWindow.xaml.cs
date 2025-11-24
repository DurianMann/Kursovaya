using Kursovaya;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
namespace Kursovaya
{
    public partial class SelectSeatsWindow : Window
    {
        private List<string> selectedSeats = new List<string>();
        public Film SelectedFilm { get; set; }
        public TimeOnly SelectedTime { get; set; }
        public decimal TotalPrice { get; set; }
        private AppDbContext _context;
        public SelectSeatsWindow(Film film, AppDbContext context)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedFilm = film;
            _context = context;
            CreateSeatsGrid();
            MarkBookedSeats();
        }
        public SelectSeatsWindow(Film film,TimeOnly selectedTime, AppDbContext context)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedFilm = film;
            SelectedTime = selectedTime;
            _context = context;
            CreateSeatsGrid();
            MarkBookedSeats();
        }
        private void UpdatePriceLabel()
        {
            BalanceLabel.Content = $"Итого: {TotalPrice} руб.";
        }
        private void CreateSeatsGrid()
        {
            for (int j = 1; j <= 8; j++)
            {
                Label label = new Label 
                {
                    Content = $"{j}",
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(2),
                };
                SeatsGrid.Children.Add(label);

                for (int i = 1; i <= 10; i++)
                {
                    Button seatButton = new Button
                    {
                        Content = $"{j}.{i}",
                        Width = 30,
                        Height = 30,
                        Margin = new Thickness(2),
                        Padding = new Thickness(0),
                        Style = (Style)Application.Current.Resources["BaseButtonStyle"]
                    };
                    seatButton.Click += Seat_Click;
                    SeatsGrid.Children.Add(seatButton);
                }
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTime = (TimeOnly)comboBox1.SelectedItem;
            Debug.WriteLine("Время сеанса изменена");
            MarkBookedSeats(); // Перепроверяем статусы
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
                    if (!SeatManager.IsSeatAvailable(SelectedFilm.Title, SelectedTime, seat))
                    {
                        button.IsEnabled = false;
                        button.ToolTip = "Место занято";
                    }
                    else
                    {
                        button.IsEnabled = true;
                    }
                }
            }
        }
        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var grid = (UniformGrid)Parent;
            string seat = button.Content.ToString();
            if (selectedSeats.Contains(seat))
            {
                // Снятие выбора
                selectedSeats.Remove(seat);
                TotalPrice -= SelectedFilm.Price;
                button.Background = (SolidColorBrush)Application.Current.FindResource("PrimaryColor");
            }
            else
            {
                // Проверка через глобальный менеджер
                if (!SeatManager.IsSeatAvailable(SelectedFilm.Title, SelectedTime, seat))
                {
                    MessageBox.Show($"Место {seat} уже забронировано!");
                    return;
                }
                // Выбор места
                selectedSeats.Add(seat);
                TotalPrice += SelectedFilm.Price;
                button.Background = (SolidColorBrush)Application.Current.FindResource("DisabledBrush");
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
            if (SelectedTime == TimeOnly.Parse("00:00:00"))
            {
                MessageBox.Show("Выберите сеанс для бронирования!");
                return;
            }
            foreach (var seat in selectedSeats)
            {
                if (!SeatManager.IsSeatAvailable(SelectedFilm.Title, SelectedTime, seat))
                {
                    MessageBox.Show($"Некоторые места уже забронированы!");
                    Close(); return;
                }
            }
            var confirmWindow = new ConfirmOrderWindow(
                SelectedFilm.Title,
                SelectedTime,
                selectedSeats,
                TotalPrice,
                MainWindow.currentUser,
                _context
            );
            confirmWindow.ShowDialog();
            selectedSeats.Clear();
            TotalPrice = 0;
            UpdatePriceLabel();
            MarkBookedSeats();
            Close();
        }
    }
}
