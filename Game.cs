using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace PH4_WPF
{
    [Serializable]
    public sealed class Game
    {
        //private etc.Translator Tr { get; } = App.TranslatorTest;
        private GameSpeedEnum gameSpeed = GameSpeedEnum.Pause;      // Начальный режим времени
        private DateTime stDataGM = new DateTime(2001, 1, 1);       // Начальная дата игры

        /// <summary>
        /// Окна/Процесс в игре
        /// </summary>
        [NonSerialized] public Dictionary<string, Window> ActiveApp = new Dictionary<string, Window>();
        /// <summary>
        /// Основная форма
        /// </summary>
        [NonSerialized] public MainWindow MainWindow;
        // Загруженна игра или еще обрабатывает параметры 
        public bool GameLoaded = false;
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
        public BankClass Bank = new BankClass();
        /// <summary>
        /// Все маршруты серверов тут
        /// </summary>
        public List<RouterClass> Routers = new List<RouterClass>();
        /// <summary>
        /// Cписок всех заплаинированых событий в игре
        /// </summary>
        public List<GameEvenClass> AllEventGame = new List<GameEvenClass>();
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
                MainWindow.IconTimeSpeed(value);
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
        /// Список вирусов работающих в сети 
        /// </summary>
        public VirusListClass VirusList = new VirusListClass();
        /// <summary>
        /// Сумма штрафа подлежащая уплате игроку
        /// </summary>
        public int FineSum = 0;

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
                //if (item == MyServer) continue;
                if (name_or_ip.Trim() == item.IP) return item;
                if (item.NameSrv.ToLower() == name_or_ip.ToLower().Trim()) return item;
            }
            return null;
        }
        /// <summary>
        /// Добавляет в лог информацию
        /// </summary>
        /// <param name="text">Текст инф. в начале разделитель пример : <i>*тут лог</i></param>
        public void LogAdd(string text,Enums.LogTypeEnum logType) {
            string str = logType switch
            {
                Enums.LogTypeEnum.Money => "",
                Enums.LogTypeEnum.Error => "",
                Enums.LogTypeEnum.Server => "",
                Enums.LogTypeEnum.Problem => "",
                Enums.LogTypeEnum.Exp => "",
                _ => "",
            };
            News.Logs.Add(new NewsClass.LogStruct() { Text = str + text, Date = DataGM.ToString("dd/mm/yy") }); 
        }
        /// <summary>
        /// Звуковой сиг оповещений
        /// </summary>
        /// <param name="str"></param>
        public void SoundSignal(Enums.Sounds sound) {            
            if (App.SoundDisable == false)
            {
                System.Windows.Media.MediaPlayer media = new System.Windows.Media.MediaPlayer();
                media.Open(new Uri(App.PatchAB + @"sound\" + sound.ToString () + ".mp3", UriKind.Relative));
                media.Play();
            }
        }
        /// <summary>
        /// Рисует маршрут на карте 
        /// </summary>
        /// <param name="srvA">От сервера</param>
        /// <param name="srvB">До сервера</param>
        public void AddRouter(Server srvA, Server srvB)
        {
            if (srvA.NameSrv != srvB.NameSrv)
            {
                RouterClass.LineArgumentStruct lineArgument = new RouterClass.LineArgumentStruct(2,
                                                        y1:srvA.DrawingHub.GetLocateRec.Y + (App.H_GRIND / 2),
                                                        y2:srvB.DrawingHub.GetLocateRec.Y + (App.H_GRIND / 2),
                                                        x1:srvA.DrawingHub.GetLocateRec.X + (App.W_GRIND / 2),
                                                        x2:srvB.DrawingHub.GetLocateRec.X + (App.W_GRIND / 2));
                Routers.Add(new RouterClass() { FirstServer = srvA, EndServer = srvB, LineArgument = lineArgument });
                MainWindow.AddLineRoute(Routers[^1]);
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
        /// Выполняет событие  
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="StringElemen"></param>
        public void EventIntroduce(Enums.ConditionEnum condition, string target, string parametor="") {
            if (GameLoaded==false) return;
            string[] StringElemen = new string[] { target, parametor };
            GameScen.ActiveScen.EventIntroduce(condition, StringElemen);
        }

        /// <summary>
        /// Находит уязвимости для серверов по портам, создает инструкции 
        /// </summary>
        public void Instructions_V() {
            string[] aa = new string[] {
             "SuperMace",
             "Avico",
             "Selica",
             "GaudSys",
             "Evo",
             "Rec",
             "TVB",
             "GF",
             "IMMP",
             "Forces",
             "Bloker",
             "Infin",
             "Tema",
             "Werty",
             "Raizor",
             "Taiman",
             "Ui",
             "R",
             "Fixer",
             "mWMR",
             "DEF",
             "AKT",
             "Lev",
             "Soft",
             "SoftS",
             "11xc",
             "Rextor",
             "Admin",
             "Sys",
             "Trabbt",
             "Suspetch"};

            var rand = new Random();
            foreach (var item in Servers )
            {
                foreach (var tv in item.Ports) {
                    if (tv.NameTitle == "Unknown") continue;
                    if (tv.Rationo <= GamerInfo.HiTecLevel) //Тех уровень игрока высокий он обнаружит уязвимость 
                    {
                        if (VulnerabilitiesList.Any(x => x.NameBug == tv.NameTitle & x.VerB == tv.Rationo) ==false )
                        {
                            int fg = (App.GameGlobal.GamerInfo.HiTecLevel/10)+1;                         
                            Vulnerabilities vulnerabilities = new Vulnerabilities()
                            {
                                CName = tv.NameTitle,
                                Shareware = true,
                                Studied = true,
                                NameBug = aa[rand.Next(0, 31)] + "_" + tv.NameTitle,
                                VerA = tv.VerA,
                                VerB = tv.Rationo,
                                GrantPremission = (Server.PremissionServerEnum)rand.Next(0, 4)
                            };
                            //случайно создает эксплойт 
                            int pp = rand.Next(0, fg);
                            if (pp == 1)  vulnerabilities.Exploid = true; 
                            else if (pp == 2)  vulnerabilities.Shareware = false; 

                            VulnerabilitiesList.Add(vulnerabilities);
                        }
                    }                
                }
            }
        
        }

        /// <summary>
        /// Чат в игре
        /// </summary>
        [Serializable]
        public sealed class GameChatClass {
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
