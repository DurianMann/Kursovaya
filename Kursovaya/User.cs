using Kursovaya;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    private decimal _balance;
    public List<Booking> Bookings { get; set; } = new List<Booking>();

    public event EventHandler BalanceChanged;

    public class Booking
    {
        public string FilmTitle { get; set; }
        public string SessionTime { get; set; }
        public string Seats { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
    }
    public decimal Balance
    {
        get => _balance;
        set
        {
            _balance = value;
            BalanceChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public User(string username, string password)
    {
        Username = username;
        Password = password;
        Balance = 0;
    }
}
