using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static User;

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для PassMovieWindow.xaml
    /// </summary>
    public partial class PassMovieWindow : Window
    {
        private User _user;
        public List<Booking> Bookings { get; set; }
        public string Seats { get; set; }
        public Booking SelectedBooking { get; set; }
        public Collection<Film> Films { get; set; }

        private ObservableCollection<Booking> passes;
        public PassMovieWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public PassMovieWindow(User user) : this()
        {
            _user = user;
            Bookings = user.Bookings;
            passes = new ObservableCollection<Booking>();
            PassesList.ItemsSource = passes;
            Films = Film.films;
            foreach (Booking booking in user.Bookings)
            {
                Seats = booking.Seats;
                passes.Add(booking);
            }

        }

        private void ViewBooking_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Сохраняем выбранный билет
            SelectedBooking = PassesList.SelectedItem as Booking;
            Debug.WriteLine(SelectedBooking);
        }
        public void ViewBookingFilmClick(object sender, RoutedEventArgs e)
        {
            foreach (Film film in Films)
            {
                Debug.WriteLine(SelectedBooking.FilmTitle);
                if (SelectedBooking.FilmTitle == film.Title)
                {
                    Debug.WriteLine(SelectedBooking.FilmTitle);
                    var selectSeatsWindow = new SelectSeatsWindow(film);
                    selectSeatsWindow.Show();
                }
            }
        }

        public void PassesListPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 25);
            else
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 25);
        }
    }
}
