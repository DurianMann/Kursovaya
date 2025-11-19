using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

public class User
{
    public int IdUser { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // В реальности — хешированный пароль!
    public decimal Balance { get; set; }
    public List<Booking> Bookings { get; set; }
    private static string _connectionString = "Data Source=KursBD.db;Cache=Shared";
    public event EventHandler BalanceChanged;

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
    
    public User(int idUser, string username, string password)
    {
        IdUser = idUser;
        Username = username;
        Password = password;
    }

    // Асинхронная проверка существования пользователя по имени
    public static async Task<bool> ExistsAsync(string username)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        var command = connection.CreateCommand();
        command.CommandText = "SELECT 1 FROM User WHERE Username = @Username";
        command.Parameters.AddWithValue("@Username", username);

        var result = await command.ExecuteScalarAsync();
        return result != null;

    }

    // Асинхронный поиск пользователя по имени и паролю
    public static async Task<User?> FindByCredentialsAsync(string username, string password)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT IdUser, Username, Password, Balance
            FROM Users
            WHERE Username = @Username AND Password = @Password";

        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@Password", password);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User(
                idUser: reader.GetInt32(1),
                username: reader.GetString(0),
                password: reader.GetString(2)
            )
            {
                Balance = reader.GetDecimal(3)
            };
        }
        return null;
    }

    // Асинхронное сохранение нового пользователя
    public async Task SaveAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (IdUser, Username, Password, Balance)
            VALUES (@IdUser, @Username, @Password, @Balance)";

        command.Parameters.AddWithValue("@IdUser", IdUser);
        command.Parameters.AddWithValue("@Username", Username);
        command.Parameters.AddWithValue("@Password", Password);
        command.Parameters.AddWithValue("@Balance", Balance);

        await command.ExecuteNonQueryAsync();
    }
}
