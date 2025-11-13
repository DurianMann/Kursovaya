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
        public List<TimeOnly> Time { get; set; }
        public string TimePreview
        {
            get
            {
                return string.Join(", ", Time.Select(t => t.ToShortTimeString()));
            }
        }
        public decimal Price { get; set; }
        public string Description { get; set; }

        // Инициализация списка фильмов
        public static Collection<Film> films = new()
        {
                new Film { idFilm = 1, MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/2057673/1199305647/S168x252_2x", Title = "Август", Time = new List<TimeOnly>{new TimeOnly(15, 30),new TimeOnly(17, 25),new TimeOnly(19, 45)} , Price = 200 , Description = "Август 1944 года. Глухие леса Западной Белоруссии. Средиземье, недавно освобожденная территория — особая зона, где действуют оставленные в советском тылу вражеские разведывательно-диверсионные группы. Советские войска переходят государственную границу, война поворачивает вспять. Помешать этому может удар в спину наступающей армии. Предотвратить нападение могут только контрразведчики СМЕРШ."},
                new Film { idFilm = 2, MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/7798118/687343407/S168x252_2x", Title = "Сумерки", Time = new List<TimeOnly>{new TimeOnly(18, 00),new TimeOnly(20, 30),new TimeOnly(22, 45)} , Price = 300 , Description = "Семнадцатилетняя девушка Белла переезжает к отцу в небольшой городок Форкс. Она влюбляется в загадочного одноклассника, который, как оказалось, происходит из семьи вампиров, отказавшихся от нападений на людей. Влюбиться в вампира. Это страшно? Это романтично, это прекрасно и мучительно, но это не может кончиться добром, особенно в вечном противостоянии вампирских кланов, где малейшее отличие от окружающих уже превращает вас во врага."},
                new Film { idFilm = 3, MoviePreview = "https://avatars.mds.yandex.net/get-entity_search/1989973/890707872/S88x132_2x", Title = "Бивень", Time = new List<TimeOnly>{new TimeOnly(23, 00),new TimeOnly(00, 30),new TimeOnly(1, 45)}, Price = 170 , Description = "Журналист Уоллес Брайтон уехал брать интервью и исчез навсегда. Его друг вместе с девушкой Уоллеса отправляется на поиски. Поиски приводят пару к полицейскому в отставке, который много лет занимался поиском пропавших людей и который рассказывает им, что есть некая закономерность в пропаже людей. Люди пропадают, а потом находят их обезображенные кости. Полицейский думает, что все это дело рук серийного убийцы." },
                new Film { idFilm = 4, MoviePreview = "https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/22eed812-f718-4a42-8987-a4499ff910e7/300x450", Title = "Гуррен-Лаганн", Time = new List<TimeOnly>{new TimeOnly(13, 00),new TimeOnly(15, 30),new TimeOnly(18, 45)}, Price = 250 , Description = "Сотни лет люди живут в глубоких пещерах, в постоянном страхе перед землетрясениями и обвалами. В одной из таких подземных деревень живет мальчик Симон и его духовный наставник — парень Камина. Камина верит, что наверху есть другой мир, без стен и потолков, его мечта — попасть туда. Но мечты остаются пустыми фантазиями, пока в один прекрасный день Симон случайно не находит сверло, оказавшееся ключом от странного железного лица в толще земли. В этот же день потолок пещеры рушится. Так начинается приключение Симона, Камины и их компаньонов в новом мире под открытым небом огромной вселенной." },
                new Film { idFilm = 5, MoviePreview = "https://sun9-33.userapi.com/s/v1/ig2/_qsHuCMy8bFuTssIBK1hypfxeaEbBUG_LKOATxzT1MI8IcsEXA2ARsw843vHR4cGDTAneaVpz540KfGcegOt5o9q.jpg?quality=95&as=32x43,48x64,72x96,108x144,160x213,240x320,360x480,480x640,540x720,640x853,720x960,1080x1440,1280x1707,1440x1920,1920x2560&from=bu&cs=1920x0", Title = "Качка-Боссы", Time = new List<TimeOnly>{new TimeOnly(19, 00),new TimeOnly(21, 00),new TimeOnly(22, 45)}, Price = 20000000 , Description = "Студент НМК Имамов Марат со своимим друзьями решили стать качками, но в качалке где они качаются не просто накачаться, нужно знать все тонкости этого дела, только так можно стать боссом качалки - Качка-Боссом" }

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
