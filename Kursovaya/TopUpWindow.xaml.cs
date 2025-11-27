using System.Windows;
namespace Kursovaya
{
    public partial class TopUpWindow : Window
    {
        private User currentUser;

        public TopUpWindow(User user, AppDbContext context)
        {
            InitializeComponent();
            currentUser = user;
            lblCurrentBalance.Text = $"Текущий баланс: {currentUser.Balance} руб.";

            // Подписываемся на событие изменения баланса
            currentUser.BalanceChanged += UpdateBalanceLabel;
        }

        private void TopUp_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                if (amount > 0)
                {
                    currentUser.Balance += amount;
                    Close();
                }
                else
                {
                    MessageBox.Show("Сумма должна быть положительной");
                }
            }
            else
            {
                MessageBox.Show("Введите корректную сумму");
            }
        }

        private void UpdateBalanceLabel(object sender, EventArgs e)
        {
            lblCurrentBalance.Text = $"{currentUser.Balance} руб.";
        }
    }
}