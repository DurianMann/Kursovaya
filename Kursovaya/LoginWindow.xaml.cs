using System;
using System.Windows;
using Kursovaya;
using Microsoft.EntityFrameworkCore;

namespace Kursovaya
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = txtUsername.Text;
            string enteredPassword = txtPassword.Password;

            try
            {
                // Ищем пользователя в базе данных
                var context = App.Context;
                var user = context.Users
                    .FirstOrDefault(u => u.Username == enteredUsername && u.Password == enteredPassword);

                if (user == null)
                {
                    MessageBox.Show(
                        "Пользователь не найден или неверный пароль.\nПроверьте данные и повторите попытку.",
                        "Ошибка входа",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                // Загружаем связанные данные (бронирования)
                context.Entry(user).Collection(u => u.Bookings).Load();

                // Создаем главное окно с пользователем и контекстом БД
                var mainWindow = new MainWindow(user,context);
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            
                // Передаем контекст БД в окно регистрации
                var registerWindow = new RegisterWindow(App.Context);
                registerWindow.ShowDialog();
            
        }
    }

}
