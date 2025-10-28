using static Kursovaya.MainWindow;
using System.Windows;
namespace Kursovaya
{
    public partial class RegisterWindow : Window
    {
        private Dictionary<string, User> users;

        public RegisterWindow(Dictionary<string, User> userDictionary)
        {
            InitializeComponent();
            users = userDictionary;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRegisterUsername.Text;
            string password = txtRegisterPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            if (users.ContainsKey(username))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            users.Add(username, new User(username, password));
            Close();
        }
    }

}
