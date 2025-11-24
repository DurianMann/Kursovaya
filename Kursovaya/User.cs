using Kursovaya;

public class User
{
    public static Dictionary<string, User> users = new Dictionary<string, User>();
    public int idUser;
    public string Username { get; set; }
    public string Password { get; set; }
    private decimal _balance;
    public List<Booking> Bookings { get; set; } = new List<Booking>();

    public event EventHandler BalanceChanged;
    // Навигация: один пользователь — много бронирований
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

    public class Booking
    {
        public int idBooking;
        public int idFilm;
        public string Username { get; set; }
        public string FilmTitle { get; set; }
        public TimeOnly SessionTime { get; set; }
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
