using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kursovaya
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        public static User currentUser;
        public Film SelectedFilm { get; set; }
        public static decimal UserBalance { get; set; }

        public MainWindow(User user, AppDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            currentUser = user ?? throw new ArgumentNullException(nameof(user));

            RefreshUserBalance();
            UpdateBalanceLabel();

            LoginLabel.Text = currentUser.Username;
            LoadFilms();
        }

        /// <summary>
        /// Перезагружает баланс пользователя из базы данных.
        /// </summary>
        private void RefreshUserBalance()
        {
            try
            {
                currentUser = _context.Users.Find(currentUser.Id)
                    ?? throw new InvalidOperationException("Пользователь не найден в базе данных.");

                UserBalance = currentUser.Balance;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при обновлении баланса: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                UserBalance = 0m;
            }
        }

        /// <summary>
        /// Загружает список фильмов из базы данных и отображает в интерфейсе.
        /// </summary>
        private void LoadFilms()
        {
            try
            {
                var films = _context.Films.ToList();
                FilmList.ItemsSource = films;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка загрузки фильмов: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void TopUp_Click(object sender, RoutedEventArgs e)
        {
            var topUpWindow = new TopUpWindow(currentUser, _context);
            topUpWindow.ShowDialog();

            RefreshUserBalance();
            UpdateBalanceLabel();
        }

        private void CheckSeats_Click(object sender, RoutedEventArgs e)
        {
            var passMovieWindow = new PassMovieWindow(_context);
            passMovieWindow.ShowDialog();

            RefreshUserBalance();
            UpdateBalanceLabel();
        }

        private void SelectSeats_Click(object sender, RoutedEventArgs e)
        {
            if (FilmList.SelectedItem is not Film selectedFilm)
            {
                MessageBox.Show("Выберите фильм.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectSeatsWindow = new SelectSeatsWindow(selectedFilm, _context);
            selectSeatsWindow.ShowDialog();

            RefreshUserBalance();
            UpdateBalanceLabel();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            currentUser = null;
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            if (loginWindow.IsVisible)
            {
                Close();
            }
        }

        /// <summary>
        /// Обновляет отображение баланса на интерфейсе.
        /// </summary>
        public void UpdateBalanceLabel()
        {
            BalanceLabel.Content = $"{UserBalance:F2} руб.";
        }

        private void FilmList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFilm = FilmList.SelectedItem as Film;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
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
            if (FilmList.SelectedItem is Film film)
            {
                ShowFilmDescription(film);
            }
        }

        private void ShowFilmDescription(Film film)
        {
            var descriptionWindow = new FilmDescriptionWindow(film);
            descriptionWindow.Owner = this;
            descriptionWindow.ShowDialog();
        }
    }
}
