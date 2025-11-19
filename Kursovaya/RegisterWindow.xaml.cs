using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kursovaya
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRegisterUsername.Text;
            string password = txtRegisterPassword.Password;
            decimal balance = 0m;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверяем, существует ли пользователь
            bool exists = await User.ExistsAsync(username);
            if (exists)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создаём нового пользователя
            var newUser = new User(
                idUser: new Random().Next(1000, 9999), // Временное ID (лучше использовать автоинкремент в БД)
                username: username,
                password: password
            )
            {
                Balance = balance
            };

            try
            {
                await newUser.SaveAsync();
                MessageBox.Show("Регистрация успешна! Теперь вы можете войти.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
