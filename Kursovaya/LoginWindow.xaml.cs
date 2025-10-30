using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using Kursovaya;

namespace Kursovaya
{
    public partial class LoginWindow : Window
    {
        private Dictionary<string, User> users = Users.users;
        private User currentUser;
        
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = txtUsername.Text;
            string enteredPassword = txtPassword.Password;

            if (!users.TryGetValue(enteredUsername, out User user))
            {
                MessageBox.Show("Пользователь не найден!");
                return;
            }

            if (user.Password != enteredPassword)
            {
                MessageBox.Show("Неверный пароль!");
                return;
            }

            currentUser = user;

            try
            {
                new MainWindow(currentUser).Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Вызываем диалог с окном, где происходит регистрация
            var registerWindow = new RegisterWindow(users);
            registerWindow.ShowDialog();
        }
    }

}
