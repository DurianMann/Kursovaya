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
        public List<User.Booking> Bookings { get; set; }
        public string FilmTitle { get; set; }
        public string SessionTime { get; set; }
        public List<string> SelectedSeats { get; set; } = new List<string>();
        public string Seats { get; set; }

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
            foreach (Booking booking in user.Bookings)
            {
                Seats = string.Join(",", booking.Seats);
                passes.Add(booking);
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
