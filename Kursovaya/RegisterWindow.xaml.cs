using static Kursovaya.MainWindow;
using System.Windows;
namespace Kursovaya
{
    public partial class RegisterWindow : Window
    {
        private Dictionary<string, User> users;

        public RegisterWindow()
        {
            InitializeComponent();
            users = (Application.Current.Resources["Users"] as Dictionary<string, User>) ??
                    new Dictionary<string, User>();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (users.ContainsKey(txtRegisterUsername.Text))
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return;
            }

            var newUser = new User(txtRegisterUsername.Text, txtRegisterPassword.Password);
            users.Add(txtRegisterUsername.Text, newUser);
            Application.Current.Resources["Users"] = users;
            MessageBox.Show("Регистрация успешна!");
            Close();
        }
    }
}
