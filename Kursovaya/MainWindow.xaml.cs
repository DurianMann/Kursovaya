using System.Windows;
using System.Collections.ObjectModel;
using Kursovaya;
using System.Windows.Controls;
using static Kursovaya.MainWindow;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Diagnostics;

namespace Kursovaya
{
    public partial class MainWindow : Window
    {
        public static User currentUser;
        
        public Film SelectedFilm { get; set; }
        public static decimal UserBalance { get; set; } = 1000;


        public MainWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoginLabel.Text = user.Username;
            currentUser.BalanceChanged += (sender, e) => UpdateBalanceLabel();
            UpdateBalanceLabel();

            FilmList.ItemsSource = Film.films;
        }


        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            currentUser = null;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            if (loginWindow.IsVisible)
            {
                Close();
            }
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
            Debug.WriteLine(SelectedFilm);
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
        private void CheckSeats_Click(object sender, RoutedEventArgs e)
        {
            // Передаем забронированные места
            var passMovieWindow = new PassMovieWindow(currentUser);
                passMovieWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void FilmList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = FilmList.SelectedItem as Film;
            if (selectedItem != null)
            {
                ShowFilmDescription(selectedItem);
            }
        }
        private void ShowFilmDescription(Film film)
        {
            // Пример: создаём и показываем окно с описанием
            var descriptionWindow = new FilmDescriptionWindow(film);
            descriptionWindow.ShowDialog(); // или Show(), если не нужен модальный диалог
        }
    }
}

