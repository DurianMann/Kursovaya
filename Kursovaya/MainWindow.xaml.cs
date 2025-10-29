using System.Windows;
using System.Collections.ObjectModel;
using Kursovaya;
using System.Windows.Controls;
using static Kursovaya.MainWindow;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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
            FilmList.Focus();

            // Инициализация списка фильмов
            films = new ObservableCollection<Film>
            {
                new Film { MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/2057673/1199305647/S168x252_2x", Title = "Август", Time = "18:00" , Price = 200},
                new Film { MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/7798118/687343407/S168x252_2x", Title = "Сумерки", Time = "20:00" , Price = 300},
                new Film { MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/1989973/890707872/S88x132_2x", Title = "Бивень", Time = "23:00" , Price = 170},
                new Film { MoviePreview = "https://sun9-33.userapi.com/s/v1/ig2/_qsHuCMy8bFuTssIBK1hypfxeaEbBUG_LKOATxzT1MI8IcsEXA2ARsw843vHR4cGDTAneaVpz540KfGcegOt5o9q.jpg?quality=95&as=32x43,48x64,72x96,108x144,160x213,240x320,360x480,480x640,540x720,640x853,720x960,1080x1440,1280x1707,1440x1920,1920x2560&from=bu&cs=1920x0", Title = "Качка-Боссы", Time = "12:00" , Price = 20000000}
            };

            FilmList.ItemsSource = films;
        }

        private void FilmList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 25);
            else
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 25);
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
        private void CheckSeats_Click(object sender, RoutedEventArgs e)
        {
            // Передаем забронированные места
            var passMovieWindow = new PassMovieWindow(currentUser);
                passMovieWindow.Show();
        }
    }
}

