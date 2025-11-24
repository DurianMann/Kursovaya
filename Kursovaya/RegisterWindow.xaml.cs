using System.Windows;
using static Kursovaya.MainWindow;
using Microsoft.EntityFrameworkCore;

namespace Kursovaya
{
    public partial class RegisterWindow : Window
    {
        private readonly AppDbContext _context;
        public RegisterWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            this.DataContext = this;
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRegisterUsername.Text;
            string password = txtRegisterPassword.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (_context.Users.Any(u => u.Username == username))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var newUser = new User
                {
                    Username = username,
                    Password = password,
                    Balance = 0
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                MessageBox.Show("Регистрация успешна! Теперь вы можете войти в систему.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
    }
    
}

