using System;
using System.Collections.Generic;
using System.Windows;
using Kursovaya;

namespace Kursovaya
{
    public partial class LoginWindow : Window
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        private User currentUser;

        public LoginWindow()
        {
            InitializeComponent();
            // Добавляем тестового пользователя
            users.Add("admin", new User("admin", "admin"));
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = users.FirstOrDefault(x => x.Key == txtUsername.Text);
            if (user.Value != null && user.Value.Password == txtPassword.Password)
            {
                currentUser = user.Value;

                try
                {
                    // Попытка открыть главное окно
                    new MainWindow(currentUser).Show();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии главного окна: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
        }

    }
}
