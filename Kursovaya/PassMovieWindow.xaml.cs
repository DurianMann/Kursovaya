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


namespace Kursovaya
{
    /// <summary>
    /// Логика взаимодействия для PassMovieWindow.xaml
    /// </summary>
    public partial class PassMovieWindow : Window
    {
        public Booking SelectedBooking { get; set; }
        private List<Booking> passes;
        private AppDbContext _context = new AppDbContext();
        public PassMovieWindow(AppDbContext context)
        {
            InitializeComponent();
            passes = new List<Booking>();
            foreach (Booking booking in context.Bookings.ToList())
            {
                if (booking.UserId == MainWindow.currentUser.Id) 
                    passes.Add(booking);
            }
            PassesList.ItemsSource = passes;
        }
        private void ViewBooking_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Сохраняем выбранный билет
            SelectedBooking = PassesList.SelectedItem as Booking;
            PassesList.UpdateLayout();
            Debug.WriteLine(SelectedBooking);
        }
        public void ViewBookingFilmClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Film film in _context.Films.ToList())
                {
                    if (SelectedBooking.FilmTitle == film.Title) 
                    { 
                        Debug.WriteLine(SelectedBooking.FilmTitle);
                        var selectSeatsWindow = new SelectSeatsWindow(film, SelectedBooking.SessionTime, _context);
                        selectSeatsWindow.Show();
                    }
                }
            }
            catch (NullReferenceException ex) { MessageBox.Show(ex.Message + "\nВыберите билет и перейдите по купленным местам" + "\nЕсли билета нет, то удостоверьтесь в правильности платежа и попробуйте снова"); }
        }
        public void ReturnPassBookingClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SeatManager.IsSeatAvailable(SelectedBooking.FilmTitle, SelectedBooking.SessionTime, SelectedBooking.PreviewSeats))
                {
                    SeatManager.RemoveBookSeat(SelectedBooking.FilmTitle, SelectedBooking.SessionTime, SelectedBooking.Id);
                    _context.Users.Where(x => x.Id == MainWindow.currentUser.Id).ToList().ForEach(x => x.Balance += SelectedBooking.TotalPrice);
                    passes.Remove(SelectedBooking);
                    _context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Средства уже возвращены на счет, проверьте счет позже");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\nПроизошла ошибка при возврате средств"); }
        }
    }
}
