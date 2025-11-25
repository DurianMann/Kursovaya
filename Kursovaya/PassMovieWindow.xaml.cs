using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для PassMovieWindow.xaml
    /// </summary>
    public partial class PassMovieWindow : Window
    {
        public Booking SelectedBooking { get; set; }
        private ObservableCollection<Booking> passes;
        private AppDbContext _context = new AppDbContext();

        public PassMovieWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadPasses();
            PassesList.ItemsSource = passes;

            // Подписываемся на изменение коллекции
            passes.CollectionChanged += OnPassesCollectionChanged;
        }

        private void LoadPasses()
        {
            passes = new ObservableCollection<Booking>();
            try
            {
                var bookings = _context.Bookings.ToList();
                foreach (Booking booking in bookings)
                {
                    if (booking.UserId == MainWindow.currentUser.Id)
                    {
                        passes.Add(booking);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке бронирований: {ex.Message}");
                passes = new ObservableCollection<Booking>(); // Гарантируем, что список не null
            }
        }

        // Обработчик изменения коллекции бронирований
        private void OnPassesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine($"Коллекция бронирований изменена. Действие: {e.Action}");

            if (e.NewItems != null)
            {
                foreach (Booking item in e.NewItems)
                {
                    Debug.WriteLine($"Добавлен билет: {item.FilmTitle} на {item.SessionTime}");
                    ValidateBooking(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (Booking item in e.OldItems)
                {
                    Debug.WriteLine($"Удален билет: {item.FilmTitle} на {item.SessionTime}");
                }
            }
        }

        private void ViewBooking_Changed(object sender, SelectionChangedEventArgs e)
        {
            SelectedBooking = PassesList.SelectedItem as Booking;

            if (SelectedBooking != null)
            {
                Debug.WriteLine($"Выбран билет: {SelectedBooking.FilmTitle}");
                ValidateBooking(SelectedBooking);
            }
            else
            {
                Debug.WriteLine("Нет выбранного билета");
            }

            PassesList.UpdateLayout();
        }

        private bool ValidateBooking(Booking booking)
        {
            if (booking == null)
            {
                MessageBox.Show("Ошибка: бронирование не найдено.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(booking.FilmTitle))
            {
                MessageBox.Show("Ошибка: название фильма не указано в бронировании.");
                return false;
            }

            if (booking.SessionTime == default)
            {
                MessageBox.Show("Ошибка: время сеанса не указано в бронировании.");
                return false;
            }

            if (booking.UserId != MainWindow.currentUser.Id)
            {
                MessageBox.Show("Ошибка: это бронирование не принадлежит текущему пользователю.");
                return false;
            }

            Debug.WriteLine($"Бронирование валидировано: {booking.FilmTitle} на {booking.SessionTime}");
            return true;
        }

        public void ViewBookingFilmClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateBooking(SelectedBooking))
                {
                    return;
                }

                var film = _context.Films
                    .FirstOrDefault(f => f.Title == SelectedBooking.FilmTitle);

                if (film == null)
                {
                    MessageBox.Show($"Фильм '{SelectedBooking.FilmTitle}' не найден в базе данных.");
                    return;
                }

                Debug.WriteLine($"Найден фильм: {film.Title}");
                var selectSeatsWindow = new SelectSeatsWindow(film, SelectedBooking.SessionTime, _context);
                selectSeatsWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при просмотре бронирования: {ex.Message}");
            }
        }

        public void CancelBookingClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateBooking(SelectedBooking))
                {
                    return;
                }

                // Отмена бронирования с возвратом средств
                if (SeatManager.RemoveBookSeat(SelectedBooking.Id))
                {
                    // Перезагружаем список бронирований и баланс
                    LoadPasses();
                    PassesList.ItemsSource = passes;

                    // Дополнительно обновим баланс в MainWindow, если он отображается
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.UpdateBalanceLabel(); // Предполагаем, что такой метод есть
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отмене бронирования: {ex.Message}");
            }
        }

    }
}
