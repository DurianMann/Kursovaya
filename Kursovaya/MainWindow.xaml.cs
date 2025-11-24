using System.Windows;
using System.Collections.ObjectModel;
using Kursovaya;
using System.Windows.Controls;
using static Kursovaya.MainWindow;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel;

namespace Kursovaya
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        public static User currentUser;
        public Film SelectedFilm { get; set; }
        public static decimal UserBalance { get; set; } = 1000;
        public MainWindow(User user, AppDbContext context)
        {
            InitializeComponent();
            currentUser = user;
            _context = context;
            LoginLabel.Text = user.Username;
            UpdateBalanceLabel();
            LoadFilms();
            context.Users.Update(user);
        }
        private void LoadFilms()
        {
            try
            {
                var films = _context.Films.ToList();
                FilmList.ItemsSource = films;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фильмов: {ex.Message}");
            }
        }
        private void TopUp_Click(object sender, RoutedEventArgs e)
        {
            var topUpWindow = new TopUpWindow(currentUser, _context);
            topUpWindow.ShowDialog();
            UpdateBalanceLabel();
        }

        private void CheckSeats_Click(object sender, RoutedEventArgs e)
        {
            var passMovieWindow = new PassMovieWindow(_context);
            passMovieWindow.ShowDialog();
        }
        private void SelectSeats_Click(object sender, RoutedEventArgs e)
        {
            if (FilmList.SelectedItem is Film selectedFilm)
            {
                var selectSeatsWindow = new SelectSeatsWindow(selectedFilm, _context);
                selectSeatsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите фильм");
            }
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
        public void UpdateBalanceLabel()
        {
            BalanceLabel.Content = $"{currentUser.Balance} руб.";
            _context.SaveChanges();
        }
        private void FilmList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Сохраняем выбранный фильм
            SelectedFilm = FilmList.SelectedItem as Film;
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Закрываем все дочерние окна
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this && window.Owner == this)
                {
                    window.Close();
                }
            }
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
            descriptionWindow.Owner = this;
            descriptionWindow.ShowDialog();
        }
    }
}

