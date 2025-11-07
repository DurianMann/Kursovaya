using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursovaya;

namespace Kursovaya
{
    //Модель фильма
    public class Film
    {
        public int idFilm;
        public string MoviePreview { get; set; }
        public string Title { get; set; }
        public TimeOnly Time { get; set; }
        public decimal Price { get; set; }

        // Инициализация списка фильмов
        public static Collection<Film> films = new()
        {
                new Film { idFilm = 1, MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/2057673/1199305647/S168x252_2x", Title = "Август", Time = TimeOnly.Parse("18:00:00") , Price = 200},
                new Film { idFilm = 2, MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/7798118/687343407/S168x252_2x", Title = "Сумерки", Time = TimeOnly.Parse("20:00:00") , Price = 300},
                new Film { idFilm = 3, MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/1989973/890707872/S88x132_2x", Title = "Бивень", Time = TimeOnly.Parse("23:00"), Price = 170 },
                new Film { idFilm = 4, MoviePreview = "https://sun9-33.userapi.com/s/v1/ig2/_qsHuCMy8bFuTssIBK1hypfxeaEbBUG_LKOATxzT1MI8IcsEXA2ARsw843vHR4cGDTAneaVpz540KfGcegOt5o9q.jpg?quality=95&as=32x43,48x64,72x96,108x144,160x213,240x320,360x480,480x640,540x720,640x853,720x960,1080x1440,1280x1707,1440x1920,1920x2560&from=bu&cs=1920x0", Title = "Качка-Боссы", Time = TimeOnly.Parse("12:00"), Price = 20000000 }

        };

        public void Main(string[] args)
        {
            string dbName = "KursBD.bd";

            string connectionString = $"DataSource={dbName};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();


            }
        }

    }
}
