using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace PH4_WPF
{
    [Serializable]
    public class Game
    {
        private GameSpeedEnum gameSpeed = GameSpeedEnum.Pause;      // Начальный режим времени
        private DateTime stDataGM = DateTime.Parse("01/01/01");     // Начальная дата игры

        /// <summary>
        /// Окна/Процесс в игре
        /// </summary>
        [NonSerialized] public Dictionary<string, Window> ActiveApp = new Dictionary<string, Window>(); 
        /// <summary>
        /// Основная форма
        /// </summary>
        [NonSerialized ] public MainWindow MainWindow;
        /// <summary>
        /// Сервер игрока
        /// </summary>
        public Server MyServer { get => Servers[0]; } 
        /// <summary>
        /// Все сервера тут
        /// </summary>
        public List<Server> Servers = new List<Server>();
        /// <summary>
        /// Счета банков и другое
        /// </summary>
        public BankClass  Bank = new BankClass();
        /// <summary>
        /// Все маршруты серверов тут
        /// </summary>
        public List<RouterClass> Routers = new List<RouterClass>();
        /// <summary>
        /// Cписок всех заплаинированых событий в игре
        /// </summary>
        public List<GameEvenStruct> AllEventGame = new List<GameEvenStruct>();
        /// <summary>
        /// Список уязвимостей доступных в игре 
        /// </summary>
        public List<Vulnerabilities> VulnerabilitiesList = new List<Vulnerabilities>();
        /// <summary>
        /// Устанавливает скорость игры
        /// </summary>
        public GameSpeedEnum GameSpeed
        {
            get => gameSpeed; set
            {
                gameSpeed = value;
                switch (value)
                {
                    case GameSpeedEnum.Pause:
                        MainWindow.StatusSpeedImg.Source = App.UriResImage("Content/Desktop/bPanel/SpeedPause.png");
                        break;
                    case GameSpeedEnum.Speed1X:
                        MainWindow.StatusSpeedImg.Source = App.UriResImage("Content/Desktop/bPanel/Speed1x.png");
                        break;
                    case GameSpeedEnum.Speed2X:
                        MainWindow.StatusSpeedImg.Source = App.UriResImage("Content/Desktop/bPanel/Speed2x.png");
                        break;
                    case GameSpeedEnum.Speed4X:
                        MainWindow.StatusSpeedImg.Source = App.UriResImage("Content/Desktop/bPanel/Speed4x.png");
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Профиль игрока
        /// </summary>
        public GamerInfoClass GamerInfo = new GamerInfoClass();
        /// <summary>
        /// Внутреннее игровое время
        /// </summary>
        public DateTime DataGM
        {
            get => stDataGM; 
            set
            {
                stDataGM = value;               
            }
        }
        /// <summary>
        /// Игровые новости
        /// </summary>
        public NewsClass News = new NewsClass();
        /// <summary>
        /// Игровой сценарий тут
        /// </summary>
        public GameScen GameScen = new GameScen();       
        /// <summary>
        /// Вызывает активный чат
        /// </summary>
        public GameChatClass GameChat;
        /// <summary>
        /// Открытые ресурсы с которых можно скачать 
        /// </summary>
        public Dictionary<string, FileServerClass> OpenUrl = new Dictionary<string, FileServerClass>();

        /// <summary>
        /// создает карту серверов
        /// </summary>
        public void CreativeMap()
        {
            string[] nano = (PH4_WPF.Properties.Resources.SiteName).Split('\n');
            Server srv;
            Random rnd = new Random();

            // первый сервер это наш сервер 
            srv = new Server()
            {
                ActSrv = true,
                NameSrv = "localhost",
                OS = Server.TypeOSEnum.Logerhead,
                PopularSRV = 0,
                CPUP_Max = 1,
                Premision = Server.PremissionServerEnum.FullControl,
                Ping = 1
            };
            srv.DrawingHub = new DrawingHubClass(srv);           
            Servers.Add(srv);

            //Другие сервера
            int x, y;
            x = App.W_GRIND;   // Top
            y = App.H_GRIND;  // Left           
            short ping = 30;
            bool[] hp = new bool[] { true, true, false, true, false };
            foreach (var item in nano)
            {
                for (; ; )
                {
                    x = rnd.Next(x, x + 20);
                    y += rnd.Next(60, 200);
                    if (y > 3000) { x += 80; y = App.H_GRIND; }
                    if (hp[rnd.Next(0, hp.Length)]) break;
                }

                string name = item.Split('\t')[0];
                int pop = int.Parse(item.Split('\t')[1]);
                ping += 30;

                srv = new Server()
                {
                    ActSrv = true,
                    NameSrv = name,
                    OS = (Server.TypeOSEnum)rnd.Next(0, 8),
                    PopularSRV = pop,
                    CPUP_Max = pop * 123,
                    Premision = Server.PremissionServerEnum.none,
                    Ping = ping
                };

                srv.DrawingHub = new DrawingHubClass(srv,x,y) ;                
                Servers.Add(srv);

            }

            // создает линии
            short searc = 500;
            for (int i = 0; i < Servers.Count; i++)
            {
                System.Drawing.Point point = Servers[i].LocateTextura;
                System.Drawing.Rectangle rec = new System.Drawing.Rectangle(point.Y - (searc / 2), point.X - (searc / 2), searc, searc);
                for (int ii = 0; ii < Servers.Count; ii++)
                {
                    if (rec.IntersectsWith(Servers[ii].DrawingHub.GetLocateRec))
                    {
                        AddRouter(Servers[i], Servers[ii]);
                    }
                }
            }
        }
        /// <summary>
        /// Находит сервер по названию или по IP
        /// </summary>
        /// <param name="name_or_ip">Название сервера или IP</param>
        /// <returns>Server</returns>
        public Server FindServer(string name_or_ip)
        {
            foreach (var item in Servers)
            {
                if (item == MyServer) continue;
                if (name_or_ip.Trim() == item.IP) return item;
                if (item.NameSrv.ToLower() == name_or_ip.ToLower().Trim()) return item;
            }
            return null;
        }
        /// <summary>
        /// Добавляет в лог информацию
        /// </summary>
        /// <param name="text">Текст инф. в начале разделитель пример : <i>*тут лог</i></param>
        public void LogAdd(string text) => News.Logs.Add(new NewsClass.LogStruct() { Text = text, Date = DataGM.ToString("dd/mm/yy") });
        /// <summary>
        /// Звуковой сиг оповещений
        /// </summary>
        /// <param name="str"></param>
        public void SoundSignal(string str) {
            //iqWav - новое сообщение в чат
            //newMail - новая почта
            //gameover - игра закончена
            //buy - Покупка
            if (MainWindow.SoundDisable == false)
            {
                System.Windows.Media.MediaPlayer media = new System.Windows.Media.MediaPlayer();
                media.Open(new Uri(App.PatchAB + @"sound\" + str + ".mp3", UriKind.Relative));
                media.Play();
            }
        }
        /// <summary>
        /// Рисует маршрут на карте 
        /// </summary>
        /// <param name="srvA">От сервера</param>
        /// <param name="srvB">До сервера</param>
        private void AddRouter(Server srvA, Server srvB)
        {
            if (srvA.NameSrv != srvB.NameSrv)
            {
               
                RouterClass.LineArgumentStruct lineArgument = new RouterClass.LineArgumentStruct()
                {
                    StrokeThickness = 2,
                    Y1 = srvA.DrawingHub.GetLocateRec.Y + (App.H_GRIND / 2),
                    X1 = srvA.DrawingHub.GetLocateRec.X + (App.W_GRIND / 2),
                    Y2 = srvB.DrawingHub.GetLocateRec.Y + (App.H_GRIND / 2),
                    X2 = srvB.DrawingHub.GetLocateRec.X + (App.W_GRIND / 2)
                };
                Routers.Add(new RouterClass() { FirstServer = srvA, EndServer = srvB, LineArgument = lineArgument });
                MainWindow.MyCanvas.Children.Add(Routers[^1].Line);
                Canvas.SetZIndex(Routers[^1].Line, 0);
            }
        }
        /// <summary>
        /// Выводит сообщение на экран
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Text"></param>
        /// <param name="Inform"></param>
        public void Msg(string Title, string Text, FrmSoft.FrmError.InformEnum Inform) {
            FrmSoft.FrmError msg = new FrmSoft.FrmError(Title, Text, Inform);
            msg.Show();
            msg.Activate();
            msg.Topmost = true;
        }

        /// <summary>
        /// Находит уязвимости для серверов по портам, создает инструкции 
        /// </summary>
        public void Instructions_V() {  
            string[] aa = new string[31];
            aa[0] = "SuperMace";
            aa[1] = "Avico";
            aa[2] = "Selica";
            aa[3] = "GaudSys";
            aa[4] = "Evo";
            aa[5] = "Rec";
            aa[6] = "TVB";
            aa[7] = "GF";
            aa[8] = "IMMP";
            aa[9] = "Forces";
            aa[10] = "Bloker";
            aa[11] = "Infin";
            aa[12] = "Tema";
            aa[13] = "Werty";
            aa[14] = "Raizor";
            aa[15] = "Taiman";
            aa[16] = "Ui";
            aa[17] = "R";
            aa[18] = "Fixer";
            aa[19] = "mWMR";
            aa[20] = "DEF";
            aa[21] = "AKT";
            aa[22] = "Lev";
            aa[23] = "Soft";
            aa[24] = "SoftS";
            aa[25] = "11xc";
            aa[26] = "Rextor";
            aa[27] = "Admin";
            aa[28] = "Sys";
            aa[29] = "Trabbt";
            aa[30] = "Suspetch";
            var rand = new Random();
            foreach (var item in Servers )
            {
                foreach (var tv in item.Ports) {
                    if (tv.NameTitle == "Unknown") continue;
                    if (tv.Rationo < GamerInfo.HiTecLevel)
                    {
                        if (VulnerabilitiesList.Find(x => x.NameBug == tv.NameTitle & x.VerB == tv.Rationo) == null)
                        {
                            int fg = (App.GameGlobal.GamerInfo.HiTecLevel/10)+1;                         
                            Vulnerabilities vulnerabilities = new Vulnerabilities()
                            {
                                CName = tv.NameTitle,
                                Shareware = true,
                                Studied = true,
                                NameBug = aa[rand.Next(0, 31)] + "_" + tv.NameTitle,
                                VerA = 1,
                                VerB = tv.Rationo,
                                GrantPremission = (Server.PremissionServerEnum)rand.Next(0, 4)
                            };
                            //случайно создает эксплойт 
                            int pp = rand.Next(0, fg);
                            if (pp == 1) { vulnerabilities.Exploid = true; }
                            else if (pp == 2) { vulnerabilities.Shareware = false; }

                            VulnerabilitiesList.Add(vulnerabilities);
                        }
                    }                
                }
            }
        
        }

        [Serializable]
        public class GameChatClass {
            public int IndexChat = 1;
            public GameScen.ScenStruct.ICQ MyChat;
            [NonSerialized] public FrmSoft.FrmICQ ICQ_Win;

            public GameChatClass(GameScen.ScenStruct.ICQ icq)
            {
                MyChat = icq;   
            }            

            public void OpenWin() {
                ICQ_Win.WindowState = WindowState.Normal;
                ICQ_Win.Show();
                ICQ_Win.Activate();
            }

            /// <summary>
            /// Востанавливает настройки при загрузке
            /// </summary>
            public void InLoadChat() {
                ICQ_Win = new FrmSoft.FrmICQ();
                App.GameGlobal.MainWindow.MessageIcon.Opacity = 100;               
                ICQ_Win.WindowState = WindowState.Minimized;
            }
        }

       
        /// <summary>
        /// Игровые режимы времени
        /// </summary>
        public enum GameSpeedEnum
        {
            Pause,
            Speed1X,
            Speed2X,
            Speed4X
        }
    }
}
