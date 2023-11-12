using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Необходимо для работы админ панели 
    /// </summary>
    [Serializable]
    public class Virtualization
    {
        private UpgradeStruct ProccesNowUpgadePrv;

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string MSG_error { get; set; }
        /// <summary>
        /// Список рабочих уровней 
        /// </summary>
        public List<InstaceClass> Instance = new List<InstaceClass>();
        /// <summary>
        /// Мощность сервера 
        /// </summary>
        public int MaxPower { get; set; }
        /// <summary>
        /// Посещаемость в день
        /// </summary>
        public int PeerDay { get; set; }
        /// <summary>
        /// Сумарная мощь всех инстансов
        /// </summary>
        public int SummarPower
        {
            get=> Instance.Sum(x => x.KVT);
        }
        /// <summary>
        /// Сумарная посещаемость всех инстансов
        /// </summary>
        public int SummarPopular
        {
            get => Instance.Sum(x => x.Popular);
        }
        /// <summary>
        /// В настоящий момент в улучшение софта
        /// </summary>
        public UpgradeStruct? ProccesNowUpgade { get => ProccesNowUpgadePrv; }
        /// <summary>
        /// Если в настоящий момент что то улучшаеться то <b> true </b>
        /// </summary>
        public bool CheckUpgradeNow { get => Instance.Find(x => x.UpdateSoft == true) == null ? false : true; }

        /// <summary>
        /// Запуск обновление софта
        /// </summary>
        /// <param name="new_instace">Новая версия софта</param>
        /// <param name="dateEnd">Дата заврешения</param>
        /// <param name="svr_name">Какой сервер</param>
        public void UpgadeSoft(InstaceClass new_instace, DateTime dateEnd, string svr_name ) {
            GameEvenStruct evenStruct = new GameEvenStruct(dateEnd, new GameEvenStruct.UpgradeSoft()
            {
                Instace = new_instace,
                ServerName = svr_name,
            });
            App.GameGlobal.AllEventGame.Add(evenStruct);
            ProccesNowUpgadePrv = new UpgradeStruct() { DataStart = App.GameGlobal.DataGM, NewVer = new_instace , EvenLink =evenStruct };      
        }
        /// <summary>
        /// Делаем для таблици ListBoxItem
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public ListBoxItem CreateListBoxItem(InstaceClass instance)
        {
            ListBoxItem item = new ListBoxItem();
            var bc = new BrushConverter();
            // статус сервера
            string sts;
            System.Windows.Media.Brush col = (System.Windows.Media.Brush)bc.ConvertFrom("#FF2F8B6F");
            ImageSource img = App.UriResImage("/Content/AdminPanel/rackserver.png");
            switch (instance.StatusInstance)
            {
                case StatusInstanceEnum.Stopping:
                    sts = "ПАУЗА";
                    img = App.UriResImage("/Content/AdminPanel/pause.png");
                    col = (System.Windows.Media.Brush)bc.ConvertFrom("#FFF2C94D");
                    break;
                case StatusInstanceEnum.ErrorCritical:
                    sts = "ОШИБКА";
                    img = App.UriResImage("/Content/AdminPanel/rackservererror.png");
                    col = (System.Windows.Media.Brush)bc.ConvertFrom("#FFA93A2D");
                    break;
                default:
                    sts = "НОРМАЛЬНО";
                    break;
            }

            // Текущая операция с сервисом
            string state="";
            if (instance.UpdateSoft) state = "";

            // тут создаем контролы
            Grid grid = new Grid() { Width = 670 };

            TextBlock text1 = new TextBlock() { Text = GetTextName(instance.InstaceType) , Margin = new Thickness(0, 3, 343, -3) };
            TextBlock text2 = new TextBlock() { Text = "Версия:", Margin = new Thickness(390, 3, 227, -3) };
            TextBlock text3 = new TextBlock() { Text = instance.Version, Margin = new Thickness(435, 3, 194, -3), FontWeight = FontWeights.Bold };
            TextBlock text4 = new TextBlock() { Text = sts, Foreground = col, Margin = new Thickness(497, 3, 90, -3), FontWeight = FontWeights.Bold, TextAlignment = TextAlignment.Center, FontFamily = new System.Windows.Media.FontFamily("Arial Unicode MS") };
            TextBlock text5 = new TextBlock() { Margin = new Thickness(638, 2, 10, -2) ,Text= state, FontWeight = FontWeights.Bold , TextAlignment =TextAlignment.Center, FontFamily =new FontFamily ("Segoe MDL2 Assets") , FontSize =20};
            Image image = new Image() { HorizontalAlignment = HorizontalAlignment.Left, Height =24, Width =24, VerticalAlignment = VerticalAlignment.Center , Margin = new Thickness (355, 0, 0, 0), Source= img };

            grid.Children.Add(text1);
            grid.Children.Add(image);
            grid.Children.Add(text2);
            grid.Children.Add(text3);
            grid.Children.Add(text4);
            grid.Children.Add(text5);
            item.Content = grid;
            item.Tag = instance;
            return item;
        }
        /// <summary>
        /// Поверка работы сервисов
        /// </summary>
        public void ServiceControl() {
            bool b = true;           
            for (;;){
                foreach (var item in Instance)
                {
                    if (CheckDependencies(item) == false & item.StatusInstance == StatusInstanceEnum.Working ) {
                        item.StatusInstance = StatusInstanceEnum.ErrorCritical;
                        b = false;
                    }
                }
                if (b) break;
                b = true;
            }       
        }
        /// <summary>
        /// Проверка на работу зависимостей <b>MSG_error - накапливает ошибки если они есть </b>>
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>true - работает отлично </returns>
        public bool CheckDependencies(InstaceClass instance)
        {
            foreach (var str in instance.Dependencies)
            {
                foreach (var item in Instance)
                {
                    if (instance == item)
                    {
                        //пропускаем эту роль
                    }
                    else if (item.InstaceType == str)
                    {
                        if (item.StatusInstance == StatusInstanceEnum.Working)
                        {
                            goto good;
                        }
                        else
                        {
                            MSG_error = "Зависимости ролей: Служба (" + GetTextName(item.InstaceType) + ") отключена или не работает";
                            return false;
                        }
                    }
                }
                MSG_error = "Зависимости ролей: Служба (" + GetTextName(str) + ") недоступна или не установлена";
                return false;
            good:;
            }
            MSG_error = "";
            return true;
        }
        /// <summary>
        /// Шаблоны ролей
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public InstaceClass Role_Templates(InstaceTypeEnum TypeName) => TypeName switch
        {
            InstaceTypeEnum.FTP =>
               new InstaceClass()
               {
                   InstaceType = TypeName,
                   VerA = 1,
                   VerB = 0,
                    KVT_Ver =10,
                     Popular_Ver =60,
                   Dependencies = new InstaceTypeEnum[] {  }
               },
            InstaceTypeEnum.MySql =>
                new InstaceClass()
                {
                    InstaceType = TypeName,
                    VerA = 1,
                    VerB = 0,
                    KVT_Ver = 10,
                    Popular_Ver = 80,
                    Dependencies = new InstaceTypeEnum[] { }
                },
            InstaceTypeEnum.WebForum =>
                new InstaceClass()
                {
                    InstaceType = TypeName,
                    VerA = 1,
                    VerB = 0,
                    KVT_Ver = 10,
                    Popular_Ver = 200,
                    Dependencies = new InstaceTypeEnum[] { InstaceTypeEnum.MySql, InstaceTypeEnum.BanerAD, InstaceTypeEnum.Mail }
                },
            InstaceTypeEnum.BanerAD =>
                new InstaceClass()
                {
                    InstaceType = TypeName,
                    VerA = 1,
                    VerB = 0,
                    KVT_Ver = 10,
                    Popular_Ver = 50,
                    Dependencies = new InstaceTypeEnum[] { InstaceTypeEnum.Mail }
                },
            InstaceTypeEnum.Mail =>
                new InstaceClass()
                {
                    InstaceType = TypeName,
                    VerA = 1,
                    VerB = 0,
                    KVT_Ver = 10,
                    Popular_Ver = 200,
                    Dependencies = new InstaceTypeEnum[] { InstaceTypeEnum.FTP }
                },
            InstaceTypeEnum.Coordinator =>
                new InstaceClass()
                {
                    InstaceType = TypeName,
                    VerA = 1,
                    VerB = 0,
                    KVT_Ver = 10,
                    Popular_Ver = 0,
                    Dependencies = new InstaceTypeEnum[] { }
                },
            _ =>
                  null
        };
        public string GetTextName(InstaceTypeEnum TypeName) =>
            TypeName switch
            {
                InstaceTypeEnum.FTP => "Файловый Сервер FTP",
                InstaceTypeEnum.BanerAD => "Управление, реклама, баннеры",
                InstaceTypeEnum.Mail => "Почтовый сервер",
                InstaceTypeEnum.MySql => "MySql Сервер",
                InstaceTypeEnum.WebForum => "Веб-сайт скрипт: Форум PHP",
                InstaceTypeEnum.Coordinator => "Создает точку управление сервисами",
                _ => "<Роль не определена>",
            };
        

        /// <summary>
        /// Нужен для объекта роли сервера
        /// </summary>
        public class InstaceClass {           
           private StatusInstanceEnum StatusInstancePrv = StatusInstanceEnum.Stopping;

            public InstaceTypeEnum InstaceType { get; set; }
            public byte VerA = 1;
            public byte VerB = 0;
            public string Version { get  => VerA + "." + VerB;}
            public StatusInstanceEnum StatusInstance { get => StatusInstancePrv; set => StatusInstancePrv = value; } 
            public InstaceTypeEnum[] Dependencies = new InstaceTypeEnum[] { InstaceTypeEnum.MySql };

            /// <summary>
            /// Количество потребления за каждую версию A
            /// </summary>
            public int KVT_Ver = 10;
            /// <summary>
            /// Количество посещение за каждую версию B
            /// </summary>
            public int Popular_Ver = 10;
            /// <summary>
            /// В процесае улучшения
            /// </summary>
            public bool UpdateSoft;

            /// <summary>
            /// Потребление энергии
            /// </summary>
            public int KVT { get => StatusInstance == StatusInstanceEnum.Working ? KVT_Ver * VerA : 0; }
            /// <summary>
            /// Посещение сайта в день
            /// </summary>
            public int Popular { get => StatusInstance == StatusInstanceEnum.Working ? Popular_Ver * VerB : 0; }

        }

        public struct UpgradeStruct
        {
            public DateTime DataStart;
            public InstaceClass NewVer;
            public GameEvenStruct EvenLink;
        }

        /// <summary>
        /// Доступные инстансы
        /// </summary>
        public enum InstaceTypeEnum { 
            FTP,
            MySql,
            WebForum,
            BanerAD,
            Mail,
            Coordinator
        
        }

        /// <summary>
        /// Статус работы инстанса
        /// </summary>
        public enum StatusInstanceEnum
        {
            Working,
            Stopping,
            ErrorCritical
        }
    }
}
