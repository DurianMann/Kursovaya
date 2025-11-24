using System.Configuration;
using System.Data;
using System.Windows;

namespace Kursovaya
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static AppDbContext _context;

        public static AppDbContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppDbContext();
                    _context.Database.EnsureCreated();
                    _context.InitializeDatabase();
                }
                return _context;
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

    }

}
