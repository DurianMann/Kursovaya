using System.Windows;
using System.Collections.ObjectModel;
using Kursovaya;
using System.Windows.Controls;
using static Kursovaya.MainWindow;
using System.Runtime.CompilerServices;

namespace Kursovaya
{
    public partial class MainWindow : Window
    {
        public static User currentUser;
        private ObservableCollection<Film> films;
        
        public Film SelectedFilm { get; set; }
        public static decimal UserBalance { get; set; } = 1000;


        public MainWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
            currentUser.BalanceChanged += (sender, e) => UpdateBalanceLabel();
            UpdateBalanceLabel();
            // Инициализация списка фильмов
            films = new ObservableCollection<Film>
            {
                new Film { Title = "Фильм 1", Time = "18:00" , Price = 200},
                new Film { Title = "Фильм 2", Time = "20:00" , Price = 300},
                new Film { Title = "Зеленый слоник", Time = "23:00" , Price = 170},
                new Film { Title = "Груз 200", Time = "12:00" , Price = 200}
            };

            FilmList.ItemsSource = films;
        }

        private void TopUp_Click(object sender, RoutedEventArgs e)
        {
            new TopUpWindow(currentUser).Show();
        }
        public void UpdateBalanceLabel()
        {
            BalanceLabel.Content = $"{currentUser.Balance} руб.";
        }
        private void FilmList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Сохраняем выбранный фильм
            SelectedFilm = FilmList.SelectedItem as Film;
            UpdateBalanceLabel() ;
        }

        private void SelectSeats_Click(object sender, RoutedEventArgs e)
        {
            // Передаем выбранный фильм в окно выбора мест
            if (SelectedFilm == null)
            {
                MessageBox.Show("Выберите фильм");
            }
            else 
            {
                var selectSeatsWindow = new SelectSeatsWindow(SelectedFilm)
                {
                    SelectedFilm = SelectedFilm
                };
                
                selectSeatsWindow.Show();

            }

        }
    }

    // Модель фильма
    public class Film
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public decimal Price { get; set; }
    }
}
