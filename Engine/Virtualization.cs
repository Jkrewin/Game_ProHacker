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
    public sealed class Virtualization
    {
        private UpgradeStruct ProccesNowUpgadePrv;
        private string _DefeceString;

        private bool SorceFileSec
        {
            get
            {
                if (FileServerClass.Exist("/user/Hpro4/", "SorceSec.php", App.GameGlobal.MyServer))
                {
                    if (FileServerClass.GetFile("/user/Hpro4/SorceSec.php", App.GameGlobal.MyServer).FileСontents.TextCommand == "SorceSec")
                    {
                        return false; // есть исходник файла     
                    }
                }
                return true; // нет исходника файла
            }
        }


        /// <summary>
        /// Текст который появиться при дефейсе сайта <b>Если строка пуста означает что нет дефейса</b> <i>Максимум 50 символов</i> 
        /// </summary>
        public string DefeceString {
            get => _DefeceString;
            set {
                _DefeceString = value.Substring(0, Math.Min(value.Length, 50));
            } }       
        /// <summary>
        /// Список рабочих уровней 
        /// </summary>
        public List<InstaceClass> Instance = new List<InstaceClass>();
        /// <summary>
        /// Мощность сервера 
        /// </summary>
        public int MaxPower { get; set; }        
        /// <summary>
        /// Железо на сервере
        /// </summary>
        public HardwareClass Hardware;
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
        public bool CheckUpgradeNow { get => Instance.Find(x => x.UpdateSoft == true) != null; }       

        /// <summary>
        /// Запуск обновление софта
        /// </summary>
        /// <param name="new_instace">Новая версия софта</param>
        /// <param name="dateEnd">Дата заврешения</param>
        /// <param name="svr_name">Какой сервер</param>
        public void UpgadeSoft(InstaceClass new_instace, DateTime dateEnd, string svr_name ) {
            GameEvenClass evenStruct = new GameEvenClass(dateEnd, new GameEvenClass.UpgradeSoft()
            {
                Instace = new_instace,
                ServerName = svr_name,
            });
            App.GameGlobal.AllEventGame.Add(evenStruct);
            ProccesNowUpgadePrv = new UpgradeStruct(App.GameGlobal.DataGM, new_instace, evenStruct );      
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
                case Enums.StatusInstanceEnum.Stopping:
                    sts = "ПАУЗА";
                    img = App.UriResImage("/Content/AdminPanel/pause.png");
                    col = (System.Windows.Media.Brush)bc.ConvertFrom("#FFF2C94D");
                    break;
                case Enums.StatusInstanceEnum.ErrorCritical:
                    sts = "ОШИБКА";
                    img = App.UriResImage("/Content/AdminPanel/rackservererror.png");
                    col = (System.Windows.Media.Brush)bc.ConvertFrom("#FFA93A2D");
                    break;
                default:
                    sts = "НОРМАЛЬНО";
                    break;
            }

            // Текущая операция с сервисом
            string state= instance.UpdateSoft ? "" : "";

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
                    if (CheckDependencies(item,out string msg_error) == false & item.StatusInstance == Enums.StatusInstanceEnum.Working ) {
                        item.StatusInstance = Enums.StatusInstanceEnum.ErrorCritical;
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
        public bool CheckDependencies(InstaceClass instance, out string msg_error)
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
                        if (item.StatusInstance == Enums.StatusInstanceEnum.Working)
                        {
                            goto good;
                        }
                        else
                        {
                            msg_error = "Зависимости ролей: Служба (" + GetTextName(item.InstaceType) + ") отключена или не работает";
                            return false;
                        }
                    }
                }
                msg_error = "Зависимости ролей: Служба (" + GetTextName(str) + ") недоступна или не установлена";
                return false;
            good:;
            }
            msg_error = "";
            return true;
        }
        /// <summary>
        /// Шаблоны ролей
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public InstaceClass Role_Templates(Enums.InstaceTypeEnum TypeName) => TypeName switch
        {
            Enums.InstaceTypeEnum.FTP =>        new InstaceClass(TypeName, 10, 60, new Enums.InstaceTypeEnum[] { }),
            Enums.InstaceTypeEnum.MySql =>      new InstaceClass(TypeName, 10, 80, new Enums.InstaceTypeEnum[] { }),
            Enums.InstaceTypeEnum.RigFrame => new InstaceClass(TypeName, 25, 0, new Enums.InstaceTypeEnum[] { }),
            Enums.InstaceTypeEnum.WebForum =>   new InstaceClass(TypeName, 10, 200, new Enums.InstaceTypeEnum[] {
                Enums.InstaceTypeEnum.MySql,
                Enums.InstaceTypeEnum.BanerAD,
                Enums.InstaceTypeEnum.Mail }),
            Enums.InstaceTypeEnum.Booking => new InstaceClass(TypeName, 10, 200, new Enums.InstaceTypeEnum[] {
                Enums.InstaceTypeEnum.MySql,
                Enums.InstaceTypeEnum.BanerAD,
                Enums.InstaceTypeEnum.Mail }),
            Enums.InstaceTypeEnum.BanerAD =>     new InstaceClass(TypeName, 10, 50, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.Mail }),
            Enums.InstaceTypeEnum.Mail =>        new InstaceClass(TypeName, 10, 200, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.FTP }),
            Enums.InstaceTypeEnum.Coordinator => new InstaceClass(TypeName, 10, 0, new Enums.InstaceTypeEnum[] { }),
            Enums.InstaceTypeEnum.Shop => new InstaceClass(TypeName, 25, 320, new Enums.InstaceTypeEnum[] { 
                Enums.InstaceTypeEnum.Mail,
                Enums.InstaceTypeEnum.MySql,
                Enums.InstaceTypeEnum.BanerAD }),
            Enums.InstaceTypeEnum.StreamVideo =>    new InstaceClass(TypeName, 100, 450, new Enums.InstaceTypeEnum[] { 
                Enums.InstaceTypeEnum.Mail,
                Enums.InstaceTypeEnum.BanerAD,
                Enums.InstaceTypeEnum.Coordinator }),
            Enums.InstaceTypeEnum.WebChat =>        new InstaceClass(TypeName, 25, 280, new Enums.InstaceTypeEnum[] { 
                Enums.InstaceTypeEnum.Mail,
                 Enums.InstaceTypeEnum.MySql,
                 Enums.InstaceTypeEnum.BanerAD }),
            Enums.InstaceTypeEnum.TranceSrv =>       new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { }, true),
            Enums.InstaceTypeEnum.ShopApi =>         new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, true),
            Enums.InstaceTypeEnum.SerWebApi =>       new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, true),
            Enums.InstaceTypeEnum.StreamVideoApi =>  new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, true),
            Enums.InstaceTypeEnum.WebChatApi =>      new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, true),
            Enums.InstaceTypeEnum.WebForumApi =>     new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, true),
            Enums.InstaceTypeEnum.MailApi =>         new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, true),
            Enums.InstaceTypeEnum.BookingApi =>      new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, true),
            Enums.InstaceTypeEnum.SecServerSorce =>  new InstaceClass(TypeName, 0, 0, new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.TranceSrv }, SorceFileSec),
            _ =>  null
        };
        /// <summary>
        /// Тип InstaceTypeEnum в название переводит
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public string GetTextName(Enums.InstaceTypeEnum TypeName) =>
            TypeName switch
            {
                Enums.InstaceTypeEnum.FTP => "Файловый Сервер FTP",
                Enums.InstaceTypeEnum.RigFrame => "Обеспечивает сервис по перебору хешей",
                Enums.InstaceTypeEnum.BanerAD => "Управление, реклама, баннеры",
                Enums.InstaceTypeEnum.Mail => "Почтовый сервер",
                Enums.InstaceTypeEnum.MySql => "MySql Сервер",
                Enums.InstaceTypeEnum.WebForum => "Веб-сайт скрипт: Форум PHP",
                Enums.InstaceTypeEnum.Coordinator => "Создает точку управление сервисами",
                Enums.InstaceTypeEnum.WebChat => "Чат",
                Enums.InstaceTypeEnum.Shop => "Магазин товаров",
                Enums.InstaceTypeEnum.StreamVideo  => "Каталог видео",
                Enums.InstaceTypeEnum.TranceSrv  => "Сервер нагрузки других сервисов",
                Enums.InstaceTypeEnum.Booking  => "Покупка билетов и аренда номеров",
                Enums.InstaceTypeEnum.WebChatApi => "Чат Уникальный",
                Enums.InstaceTypeEnum.ShopApi  => "Магазин товаров Уникальный",
                Enums.InstaceTypeEnum.StreamVideoApi => "Каталог видео Уникальный",
                Enums.InstaceTypeEnum.SerWebApi => "Веб сервис для пользователей",
                Enums.InstaceTypeEnum.BookingApi => "Покупка билетов и аренда Уникальный",
                Enums.InstaceTypeEnum.WebForumApi => "Веб-сайт Уникальный",
                Enums.InstaceTypeEnum.MailApi => "Почта Уникальный",
                Enums.InstaceTypeEnum.SecServerSorce => "Собственно написанный микро-сервис ",
                _ => "<Роль не определена>",
            };

        /// <summary>
        /// Железо на сервере
        /// </summary>
        [Serializable]
        public sealed class HardwareClass {
            /// <summary>
            /// Всего процессоров
            /// </summary>
            public int TotalProcessor;
            /// <summary>
            /// Всего ОЗУ
            /// </summary>
            public int TotalRAM;
            /// <summary>
            /// Всего жестких дисков
            /// </summary>
            public int TotalHDD;

            /// <summary>
            /// Общая мощность 
            /// </summary>
            public int PowerKVT { get => TotalProcessor * TotalRAM; }

            public HardwareClass() { }
            public HardwareClass(Virtualization vr ) {               
                TotalProcessor = (int)(vr.MaxPower * 0.4);
                TotalRAM = (int)(vr.MaxPower * 0.6);
                TotalHDD = vr.SummarPopular * 3;
            }
        
        }

        /// <summary>
        /// Нужен для объекта роли сервера
        /// </summary>
        [Serializable]
        public sealed class InstaceClass
        {
            private Enums.StatusInstanceEnum StatusInstancePrv = Enums.StatusInstanceEnum.Stopping;           

            public Enums.InstaceTypeEnum InstaceType { get; set; }
            public byte VerA = 1;
            public byte VerB = 1;
            public string Version { get => VerA + "." + VerB; }
            public Enums.StatusInstanceEnum StatusInstance { get => StatusInstancePrv; set => StatusInstancePrv = value; }
            public readonly Enums.InstaceTypeEnum[] Dependencies = new Enums.InstaceTypeEnum[] { Enums.InstaceTypeEnum.MySql };
            /// <summary>
            /// Эта роль уникальна так он авто генерирован
            /// </summary>
            public readonly bool API;

            /// <summary>
            /// Количество потребления за каждую версию A
            /// </summary>
            public int KVT_Ver = 10;
            /// <summary>
            /// Количество посещение за каждую версию B
            /// </summary>
            public int Popular_Ver = 10;
            /// <summary>
            /// В процесае улучшения <b>true - в процессе</b>
            /// </summary>
            public bool UpdateSoft =false;

            /// <summary>
            /// Потребление энергии
            /// </summary>
            public int KVT { get => StatusInstance == Enums.StatusInstanceEnum.Working ? KVT_Ver * VerA : 0; }
            /// <summary>
            /// Посещение сайта в день
            /// </summary>
            public int Popular { get => StatusInstance == Enums.StatusInstanceEnum.Working ? Popular_Ver * VerB : 0; }
           

            public InstaceClass(Enums.InstaceTypeEnum instaceType, int KVT, int popular_Ver, Enums.InstaceTypeEnum[] dependencies, bool api = false)
            {
                InstaceType = instaceType;
                VerA = 1;
                VerB = 0;
                KVT_Ver = KVT;
                Popular_Ver = popular_Ver;
                Dependencies = dependencies;
                API = api;
            }
        }

        [Serializable]
        public readonly struct UpgradeStruct
        {
            public readonly DateTime DataStart;
            public readonly InstaceClass NewVer;
            public readonly GameEvenClass EvenLink;

            public UpgradeStruct(DateTime dataStart, InstaceClass newVer, GameEvenClass evenLink) {
                EvenLink = evenLink;
                DataStart = dataStart;
                NewVer = newVer;            
            }
        }

       
    }
}
