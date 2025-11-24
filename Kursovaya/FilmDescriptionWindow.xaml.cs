using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для FilmDescriptionWindow.xaml
    /// </summary>
    public partial class FilmDescriptionWindow : Window
    {
        public FilmDescriptionWindow(Film film)
        {
            InitializeComponent();
            DataContext = film;// Привязываем данные фильма к окну
        }
    }
}
