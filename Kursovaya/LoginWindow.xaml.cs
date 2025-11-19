using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Kursovaya;

namespace Kursovaya
{
    public partial class LoginWindow : Window
    {
        private User currentUser;

        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = txtUsername.Text;
            string enteredPassword = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show(
                    "Пожалуйста, заполните все поля.",
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Асинхронно ищем пользователя по логину и паролю
                currentUser = await User.FindByCredentialsAsync(enteredUsername, enteredPassword);

                if (currentUser == null)
                {
                    MessageBox.Show(
                        "Пользователь не найден или неверный пароль.\nПроверьте данные и повторите попытку.",
                        "Ошибка входа",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                // Успешный вход — открываем главное окно
                new MainWindow(currentUser).Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при входе: {ex.Message}\nПроверьте подключение к базе данных.",
                    "Критическая ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(); // Теперь не передаём словарь — всё в БД
            registerWindow.ShowDialog();

            // После регистрации можно обновить список пользователей (если нужно)
        }
    }
}
