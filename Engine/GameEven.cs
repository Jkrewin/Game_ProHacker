using System;
using System.Collections.Generic;
using System.Windows;
using static PH4_WPF.Game;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Игровое событие
    /// </summary>
    [Serializable]
    public struct GameEvenStruct
    {
        /// <summary>
        /// Когда наступает событие или прошла эта дата уже
        /// </summary>
        public DateTime DataStart;
        /// <summary>
        /// содержит это событие
        /// </summary>
        public IEventGame GameEvent;

        public GameEvenStruct(DateTime dataStart, IEventGame gameEven) {
            DataStart = dataStart;
            GameEvent = gameEven;
        }

        /// <summary>
        /// Реализует патрен стратеджи 
        /// </summary>
        public interface IEventGame
        {            
            public void Run();
        }

        public struct UpgradeSoft : IEventGame
        {
            /// <summary>
            /// Для какого сервера это работает
            /// </summary>
            public string  ServerName;
            /// <summary>
            /// Это новый инсанс который заменит текущий
            /// </summary>
            public Virtualization.InstaceClass Instace;

            public void Run() {
                Server srv = App.GameGlobal.FindServer(ServerName);
                if (srv == null) return;

                int i;
                for ( i = 0; i < srv.VirtualizationServer.Instance.Count; i++)
                {
                    if (srv.VirtualizationServer.Instance[i].InstaceType == Instace.InstaceType) break;
                }
                srv.VirtualizationServer.Instance[i] = Instace;
                srv.VirtualizationServer.ServiceControl();
            }
        }

        /// <summary>
        /// Отправка почты
        /// </summary>
        [Serializable]
        public struct SendMail : IEventGame
        {            
            public MailInBox Mail;

            public void Run() => MailInBox.NewMail(Mail);
        }
        /// <summary>
        /// Добавить свою уязвимость
        /// </summary>
        [Serializable]
        public struct VulnerabilitiesAdd : IEventGame
        {           
            public string CName;           
            public string NameBug;           
            public bool Shareware;
            public ushort VerA;
            public int VerB;
            public Server.PremissionServerEnum GrantPremission;            
            public bool Exploid;
            /// <summary>
            /// Приходит нвость игроку о этой Vulnerabilities
            /// </summary>
            public bool NewsInform;

            public void Run()
            {
                App.GameGlobal.VulnerabilitiesList.Add(new Vulnerabilities() { 
                CName = CName,
                NameBug =NameBug,
                    Shareware= Shareware ,
                    VerA = VerA,
                    VerB=VerB,
                    GrantPremission = GrantPremission,
                    Exploid = Exploid,
                    Studied =true 
                });
              if (NewsInform)  App.GameGlobal.News.AddNews(NewsClass.NewsСlass.TopicEnum.Найдены_Баги, "Обнаружена необычная уязвимость " + NameBug + " Важно уделить ей внимание", "Bug");
            }
        }
        /// <summary>
        /// Событие на включение сервера, после сбоя
        /// </summary>
        [Serializable]
        public struct EventShutdown : IEventGame
        {           
            /// <summary>
            /// На этом сервере происходит перезагрузка
            /// </summary>
            public string UrlServer;           
               
            public void Run()
            {
                App.GameGlobal.FindServer(UrlServer).Shutdown();
            }
        }
        /// <summary>
        /// "Создает порт на сервере
        /// </summary>
        [Serializable]
        public struct CreatePort : IEventGame
        {           
            /// <summary>
            /// На каком сервере создать порт 
            /// </summary>
            public string UrlServer;
           
            public ushort PortNumber;
            public string NameTitle;           
            public int Rationo;           
            public string Text;          

            public void Run()
            {
                Engine.Server.Port port = new Server.Port(PortNumber, NameTitle, Rationo, 0, Text, true);
                string s = UrlServer; // Анонимный метод без ссылки в struct, делает ссылку тут
                App.GameGlobal.Servers.Find(x => x.NameSrv.ToLower() == s.ToLower()).Ports.Add(port);
            }
        }
        /// <summary>
        /// Показать сообщение на экран
        /// </summary>
        [Serializable]
        public struct MessageWin : IEventGame
        {
           
            public string Title;
            public string Text;
            public FrmSoft.FrmError.InformEnum Inform;

            public void Run()
            {
                App.GameGlobal.Msg(Title, Text, Inform);
            }
        }
        /// <summary>
        /// Запуск нового игрового сценария
        /// </summary>
        [Serializable]
        public struct NextScen : IEventGame
        {           
            /// <summary>
            /// Название колекции скриптов  из ActiveScenario
            /// </summary>
            public string ScenName;
            public void Run()
            {
                var r = App.GameGlobal.GameScen.GetType().GetMethod(ScenName);
                if (r == null)
                {
                    MessageBox.Show("Нет такого сценария " + ScenName);
                }
                else
                {
                    App.GameGlobal.GameScen.ActiveScen = (GameScen.ScenStruct)r.Invoke(App.GameGlobal.GameScen , new object[] {});
                }
            }
        }
        /// <summary>
        /// Запуск скриптов  из ActiveScenario
        /// </summary>
        [Serializable]
        public struct RunScript : IEventGame
        {            
            /// <summary>
            /// Название колекции скриптов  из ActiveScenario
            /// </summary>
            public string NameScript;
            public void Run()
            {
                List<GameEvenStruct.IEventGame> script = App.GameGlobal.GameScen.ActiveScen.Script[NameScript];
                script.ForEach(x => x.Run());
            }
        }
        /// <summary>
        /// Запускает чат
        /// </summary>
        [Serializable]
        public struct StartChat : IEventGame
        {           
            /// <summary>
            /// Название чата из ActiveScenario
            /// </summary>
            public string NameChat;
            public void Run()
            {
                App.GameGlobal.GameChat = new GameChatClass(App.GameGlobal.GameScen.ActiveScen.Chat[NameChat]);
                App.GameGlobal.GameChat.InLoadChat();
            }
        }
        /// <summary>
        /// Игра закончена
        /// </summary>
        [Serializable]
        public struct GameOver : IEventGame
        {
           
            public void Run()
            {
                App.GameGlobal.SoundSignal("gameover");

            }
        }
        /// <summary>
        /// Получить новую новость
        /// </summary>
        [Serializable]
        public struct GetNews : IEventGame
        {
            public string Text;
            public Engine.NewsClass.NewsСlass.TopicEnum Topic;
            /// <summary>
            /// Получает новости стандартные из файла = true
            /// </summary>
            public bool FromFile;

            public void Run()
            {
                if (FromFile == false)
                {
                    App.GameGlobal.News.News.Add(new NewsClass.NewsСlass(Topic, Text, App.GameGlobal.DataGM.ToString("dd/mm/yy")));
                    App.GameGlobal.MainWindow.NewsIndicator.Source =
                           new System.Windows.Media.Imaging.BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\sel news.png"));
                }
                else
                {
                    App.GameGlobal.News.NewsFormFile();
                }
            }
        }
        /// <summary>
        /// Получен опыт
        /// </summary>
        [Serializable]
        public struct GetExp : IEventGame
        {
            public int Exp;

            public void Run()
            {
                App.GameGlobal.GamerInfo.AddExp(Exp);
                App.GameGlobal.LogAdd("Вы получили опыт +" + Exp, LogTypeEnum.Exp);
            }
        }
        /// <summary>
        /// Получение дене
        /// </summary>
        [Serializable]
        public struct GetMoney : IEventGame
        {
            public int Money;
            public Engine.BankClass.BankAccount.TypeMoneyEnum TypeMoney;

            /// <summary>
            /// Проверяет возможна предача денг или есть ошибки 
            /// </summary>
            /// <returns></returns>
            public bool CheckLogik(ref string textError)
            {
                if (App.GameGlobal.Bank.DefaultBankAccount == null)
                {
                    textError = "У вас нет счета по умолчанию чтобы отправить на него деньги";
                    return false;
                }
                else
                {
                    if (App.GameGlobal.Bank.DefaultBankAccount.TypeMoney != TypeMoney)
                    {
                        textError = "Счет предназначен для валюты " + App.GameGlobal.Bank.DefaultBankAccount.TypeMoney.ToString()
                            + " а вам нужно перевести на счет с валютой " + TypeMoney.ToString();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            public void Run()
            {
                string txt = "";
                if (CheckLogik(ref txt))
                {
                    App.GameGlobal.Bank.DefaultBankAccount.Money += Money;
                    App.GameGlobal.LogAdd("Вы получили деньги +" + Money + " на счет " + App.GameGlobal.Bank.DefaultBankAccount.Rs, LogTypeEnum.Money );
                }
            }
        }
        /// <summary>
        /// Создан файл
        /// </summary>
        [Serializable]
        public struct CreateFile : IEventGame
        {
            public string Path;
            public string NameFile;
            public string Comment;
            public int Size;
            public FileServerClass.PremisionEnum Premision;
            public string Perfix;
            public bool SystemFile;
            public void Run() => App.GameGlobal.MyServer.CreateFiles(Path, NameFile, Comment, Size, Premision,  SystemFile);
        }
        /// <summary>
        /// Создает это событие во времени игры
        /// </summary>
        [Serializable]
        public struct CreateGameEven : IEventGame
        {
            public IEventGame GameEvent;
            public DateTime DataStart;
            
            public void Run() => App.GameGlobal.AllEventGame.Add(new GameEvenStruct() { DataStart = DataStart, GameEvent = GameEvent });
        }

    }
}
