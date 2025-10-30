using System;
using System.Collections.Generic;
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

namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для PassMovieWindow.xaml
    /// </summary>
    public partial class PassMovieWindow : Window
    {
        private User _user;
        public List<Booking> Bookings { get; set; }
        public string FilmTitle { get; set; }
        public string SessionTime { get; set; }
        public List<string> SelectedSeats { get; set; }
        public PassMovieWindow()
        {
            InitializeComponent();
        }

        public PassMovieWindow(User user) : this()
        {
            _user = user;
            Bookings = user.Bookings;
            FilmTitle = Bookings.GetType().ToString();
            Debug.WriteLine(FilmTitle);
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
