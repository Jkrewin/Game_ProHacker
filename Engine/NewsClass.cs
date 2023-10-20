using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Новости в игре и логи
    /// </summary>
    [Serializable]
    public  class NewsClass
    {
        private int NextNews = 0; //следующая новость                                

        /// <summary>
        /// Лента новостей
        /// </summary>
        public List<NewsСlass> News = new List<NewsСlass>();
        /// <summary>
        /// Логи игры
        /// </summary>
        public List<LogStruct> Logs = new List<LogStruct>();

        /// <summary>
        /// Получает новость из файла, с нотификацией. 
        /// </summary>           
        public void NewsFormFile()
        {
            string sFile = App.PatchAB + "News.txt";
            if (System.IO.File.Exists(sFile) == false)
            {
                App.GameGlobal.LogAdd("E Ошибка нет файла News.txt");
            }
            else
            {
                string[] vs = System.IO.File.ReadAllLines(sFile, System.Text.Encoding.Default);
                if (App.GameGlobal.News.NextNews < vs.Length)
                {
                    string[] f = vs[0].Split('\t');
                    NewsСlass news = new NewsСlass()
                    {
                        Title = f[0],
                        Topic = (NewsСlass.TopicEnum)Enum.Parse(typeof(NewsСlass.TopicEnum), f[2]),
                        TextNews = f[1],
                        ReadNews = false,
                        Date = App.GameGlobal.DataGM.ToString("dd/mm/yy")
                    };
                    News.Add(news);
                    App.GameGlobal.News.NextNews++;
                    // нотификация прихода новостей
                    App.GameGlobal.MainWindow.NewsIndicator.Source =
                        new BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\sel news.png"));
                }
            }
        }

        /// <summary>
        /// Добавляет новость 
        /// </summary>
        /// <param name="Topic"></param>
        /// <param name="Text"></param>
        /// <param name="Title"></param>
        public void AddNews(NewsСlass.TopicEnum Topic,string Text, string Title) {
           News.Add(new NewsClass.NewsСlass(Topic, Text, App.GameGlobal.DataGM.ToString("dd/mm/yy")) {  Title =Title});
        }

        [Serializable]
        public struct LogStruct {
            public string Text;
            public string Date;
        }

        [Serializable]
        public class NewsСlass
        {
            public string TextNews;           
            public TopicEnum Topic;
            public bool ReadNews;
            public string Date;
            public string Title;

            public NewsСlass() { }

            /// <summary>
            /// Содержит в себе нотификацию
            /// </summary>          
            public NewsСlass(TopicEnum topic, string text, string date) {
                Topic = topic;
                TextNews = text;
                Date = date;
                ReadNews = false;

                // нотификация прихода новостей
                App.GameGlobal.MainWindow.NewsIndicator .Source =
                    new BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\sel news.png"));
            }

            public enum TopicEnum { 
                Общие_Новости=0,
                Найдены_Баги=1,
                Новости_Шлак=2,
                Разное=3,
                Важное=4,
                НовостиКасательноИгрока=5
            }
        }
    }
}
