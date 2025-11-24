using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kursovaya
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<FilmSearch> FilmSearch { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cinema.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmSearch>().HasNoKey();
        }

        public void InitializeDatabase()
        {
            Database.EnsureCreated();
            CreateVirtualTables();
            SeedInitialData();
        }

        private void CreateVirtualTables()
        {
            try
            {
                var connection = (SqliteConnection)Database.GetDbConnection();

                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                using var cmd = connection.CreateCommand();

                cmd.CommandText = @"
                    CREATE VIRTUAL TABLE IF NOT EXISTS FilmSearch USING fts5(
                        Id UNINDEXED,
                        Title,
                        Description
                    );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    CREATE TRIGGER IF NOT EXISTS sync_film_search AFTER INSERT ON Films
                    BEGIN
                        INSERT INTO FilmSearch(Id, Title, Description)
                        VALUES (new.Id, new.Title, new.Description);
                    END;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    INSERT OR IGNORE INTO FilmSearch(Id, Title, Description)
                    SELECT Id, Title, Description FROM Films;";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании виртуальных таблиц: {ex.Message}");
            }
        }
        private void SeedInitialData()
        {
            if (!Films.Any())
            {
                var films = new Collection<Film>
                {
                    new Film { Id = 1,
                        Title = "Август",
                        MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/2057673/1199305647/S168x252_2x",
                        Price = 200,
                        Description = "Август 1944 года. Глухие леса Западной Белоруссии. Средиземье, недавно освобожденная территория — особая зона, где действуют оставленные в советском тыле вражеские разведывательно-диверсионные группы. Советские войска переходят государственную границу, война поворачивает вспять. Помешать этому может удар в спину наступающей армии. Предотвратить нападение могут только контрразведчики СМЕРШ.",
                        TimeSlots = new List<TimeOnly> {
                            new TimeOnly(15, 30),
                            new TimeOnly(17, 25),
                            new TimeOnly(19, 45)
                        }
                    },
                    new Film { Id=2, 
                        Title = "Сумерки",
                        MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/7798118/687343407/S168x252_2x",
                        Price = 300,
                        Description = "Семнадцатилетняя девушка Белла переезжает к отцу в небольшой городок Форкс. Она влюбляется в загадочного одноклассника, который, как оказалось, происходит из семьи вампиров, отказавшихся от нападений на людей. Влюбиться в вампира. Это страшно? Это романтично, это прекрасно и мучительно, но это не может кончиться добром, особенно в вечном противостоянии вампирских кланов, где малейшее отличие от окружающих уже превращает вас во врага.",
                        TimeSlots = new List<TimeOnly> {
                            new TimeOnly(18, 00),
                            new TimeOnly(20, 30),
                            new TimeOnly(22, 45)
                        }
                    },
                    new Film { Id=3,
                        Title = "Бивень",
                        MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/1989973/890707872/S88x132_2x",
                        Price = 170,
                        Description = "Журналист Уоллес Брайтон уехал брать интервью и исчез навсегда. Его друг вместе с девушкой Уоллеса отправляется на поиски. Поиски приводят пару к полицейскому в отставке, который много лет занимался поиском пропавших людей и который рассказывает им, что есть некая закономерность в пропаже людей. Люди пропадают, а потом находят их обезображенные кости. Полицейский думает, что все это дело рук серийного убийцы.",
                        TimeSlots = new List<TimeOnly> {
                            new TimeOnly(23, 00),
                            new TimeOnly(00, 30),
                            new TimeOnly(1, 45)
                        }
                    },
                    new Film { Id=4, 
                        Title = "Гуррен-Лаганн",
                        MoviePreview = "https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/22eed812-f718-4a42-8987-a4499ff910e7/300x450",
                        Price = 250,
                        Description = "Сотни лет люди живут в глубоких пещерах, в постоянном страхе перед землетрясениями и обвалами. В одной из таких подземных деревень живет мальчик Симон и его духовный наставник — парень Камина. Камина верит, что наверху есть другой мир, без стен и потолков, его мечта — попасть туда. Но мечты остаются пустыми фантазиями, пока в один прекрасный день Симон случайно не находит сверло, оказавшееся ключом от странного железного лица в толще земли. В этот же день потолок пещеры рушится. Так начинается приключение Симона, Камины и их компаньонов в новом мире под открытым небом огромной вселенной.",
                        TimeSlots = new List<TimeOnly> {
                            new TimeOnly(13, 00),
                            new TimeOnly(15, 30),
                            new TimeOnly(18, 45)
                        }
                    },
                    new Film { Id=5,
                        Title = "Качка-Боссы",
                        MoviePreview = "https://sun9-33.userapi.com/s/v1/ig2/_qsHuCMy8bFuTssIBK1hypfxeaEbBUG_LKOATxzT1MI8IcsEXA2ARsw843vHR4cGDTAneaVpz540KfGcegOt5o9q.jpg?quality=95&as=32x43,48x64,72x96,108x144,160x213,240x320,360x480,480x640,540x720,640x853,720x960,1080x1440,1280x1707,1440x1920,1920x2560&from=bu&cs=1920x0",
                        Price = 200,
                        Description = "Студент НМК Имамов Марат со своимим друзьями решили стать качками, но в качалке где они качаются не просто накачаться, нужно знать все тонкости этого дела, только так можно стать боссом качалки - Качка-Боссом",
                        TimeSlots = new List<TimeOnly> {
                            new TimeOnly(19, 00),
                            new TimeOnly(21, 00),
                            new TimeOnly(22, 45)
                        }
                    }
                };
                Films.AddRange(films);
                SaveChanges();
            }
        }
    }
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public event EventHandler BalanceChanged;

        // Конструктор по умолчанию для EF Core
        public User() { }

        // Конструктор для регистрации
        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Balance = 0;
        }
    }
    [Table("Films")]
    public class Film
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string MoviePreview { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<TimeOnly> TimeSlots { get; set; }
        public string TimePreview
        {
            get
            {
                return string.Join(", ", TimeSlots.Select(t => t.ToShortTimeString()));
            }
        }
    }
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FilmId { get; set; }
        public TimeOnly SessionTime { get; set; }
        public List<string> Seats { get; set; } = new List<string>();
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        [NotMapped]
        public string PreviewSeats { get { return string.Join(", ", Seats); } }
        public string FilmTitle {  get; set; }
    }
    [Keyless]
    [Table("FilmSearch")]
    public class FilmSearch
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    
}
