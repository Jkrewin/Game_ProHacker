using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Com = PH4_WPF.GameScen.ScenStruct.ICQ.Message.Answer.CommandAnswerEnum;
using Script = PH4_WPF.Engine.GameEvenStruct;
using PH4_WPF.Engine;

namespace PH4_WPF
{
    /// <summary>
    /// Коллекция игровых сценариев
    /// </summary>
    [Serializable]
    public class GameScen
    {
        private ScenStruct ActiveScenPrv;

        /// <summary>
        /// Текущий рабочий сценарий
        /// </summary>
        public ScenStruct ActiveScen
        {
            get => ActiveScenPrv;
            set
            {
                ActiveScenPrv = value;
                if (ActiveScenPrv.Script.ContainsKey("Инцилизация"))
                    ActiveScenPrv.Script["Инцилизация"].ForEach(x => x.Run());
            }
        }


        public ScenStruct Scen_start()
        {
            ScenStruct scen = new ScenStruct("Завершите задание в почтовом ящике") ;

            scen.CreateNewICQChat("icq", "Cheterman", "DDC.jpg");
            scen.AddNewMessage(1, "Привет. Я нашёл для тебя работу, которую ты долго искал. Эта самая суперская работа которую ты можешь найти для себя. Вот один чувак хочет открыть свой сервер и ему нужен админ. Ну вот, работа по управлению сервером удалённо.", 6);
            scen.AddAnswer("Круто", Com.Переход, 2);
            scen.AddAnswer("Какой сервер", Com.Переход, 3);

            scen.AddNewMessage(2, "Сидишь себе дома, открыл терминал конектировался к серверу и посмотрел работает он, или нет и так каждый день, а деньга капает. Вообще работа простая очень. Требования таковы ты должен быть хакером", 6);
            scen.AddAnswer("Я должен быть админом", Com.Переход, 4);
            scen.AddAnswer("Подробнее плиз", Com.Переход, 5);

            scen.AddNewMessage(3, "Да, хакерский сервер-портал.", 4);
            scen.AddAnswer("Это как ??", Com.Переход, 5);

            scen.AddNewMessage(4, "Тебе самому решать, быть хорошим админом или использовать сервер в своих целях.", 4);
            scen.AddAnswer("Что еще нужно", Com.Переход, 5);

            scen.AddNewMessage(5, "Ну вот, твоего будущего работодателя зовут Алексанкин С, так он себя называл в письме. Он хочет создать первый хакерский сервер-портал. Свободное распространение вирусов, программы и. т. д. Поэтому ему нужен хакер со стажем", 4);
            scen.AddAnswer("Я не хакер", Com.Переход, 6);
            scen.AddAnswer("С чего начать", Com.Переход, 7);

            scen.AddNewMessage(6, "Дело в том что Алексанкиий не хакер и не шарит в этом вобще, поэтому тебе надо ломануть самый гнилой сервер и ты будеш героем", 2);
            scen.AddAnswer("Ага понятно", Com.Переход, 8);

            scen.AddNewMessage(7, "Найти самый слабый сервер и завалить его", 3);
            scen.AddAnswer("Это вроде не трудно", Com.Переход, 8);

            scen.AddNewMessage(8, "И так ты хочешь научится ломать сервера", 2);
            scen.AddAnswer("Научи меня", Com.ВыходЗапуститьЧат, 0, "icq_beginer");
            scen.AddAnswer("Не надо я  уже эксперт", Com.Переход, 9);

            scen.AddNewMessage(9, "Ну давай, ты хочешь сейчас поговорить с ним сейчас", 1);
            scen.AddAnswer("Да", Com.ВыходЗапуститьСкрипт, 0, "start");

            // icq_beginer
            scen.CreateNewICQChat("icq_beginer", "Cheterman", "DDC.jpg");
            scen.AddNewMessage(1, "Good сначало мы должны в [Сетевом Окружении] (на вашем рабочем столе) найти нужней сервер ", 6);
            scen.AddAnswer("Да, я вошёл сюда", Com.Переход, 3);
            scen.AddAnswer("Для чего оно нужно", Com.Переход, 2);

            scen.AddNewMessage(2, "Не задавай лишних вопросов, просто делай так как я тебе сказал", 2);
            scen.AddAnswer("А ясно", Com.Переход, 3);
            scen.AddAnswer("Как скажешь", Com.Переход, 3);

            scen.AddNewMessage(3, "А теперь мы найдем сервер www.test.ru Найди его и затем выберете его. Быстро можно найти через поиск верхнем правом углу экрана.", 3);
            scen.AddAnswer("Ну вот нашёл", Com.Переход, 4);
            scen.AddAnswer("Что дальше делать", Com.Переход, 4);

            scen.AddNewMessage(4, "Скачай себе сканер уязвимостей, скачать новые сканеры уязвимостей сможешь на сайте explot.in. И жди результат сканирования.", 4);
            scen.AddAnswer("Очень долго сканит", Com.Переход, 5);
            scen.AddAnswer("Всё сканинг закончен", Com.Переход, 6);
            scen.AddAnswer("Расскажи подробнее о сканирование уязвимостей", Com.Переход, 17);

            scen.AddNewMessage(5, "Можешь остановить процесс сканирования но это повлияет на сам процесс поиска", 4);
            scen.AddAnswer("Понятно", Com.Переход, 6);
            scen.AddAnswer("Все есть уязвимость", Com.Переход, 6);

            scen.AddNewMessage(6, "Теперь, скачай эксплойт для данной уязвимости. Найди уязвимость и скачай с сайта ", 4);
            scen.AddAnswer("Я так и сделал", Com.Переход, 8);
            scen.AddAnswer("Что такое эксплойт?", Com.Переход, 7);

            scen.AddNewMessage(7, "Эта программа которая поможет использовать уязвимость, написать сплойт ты можешь сам, в сети есть описание дыры и поэтому описанию написать сплойт", 2);
            scen.AddAnswer("Я скачал", Com.Переход, 8);

            scen.AddNewMessage(8, "Я не хочу вдаваться в подробности, этот сплойт выполнение произвольных данных Redis 0.2. Теперь нужно запустить программу через консоль ", 2);
            scen.AddAnswer("Так теперь что с консолью делать", Com.Переход, 9);
            scen.AddAnswer("Как теперь запускать его ????", Com.Переход, 9);

            scen.AddNewMessage(9, "Теперь займёмся консолью ", 8);
            scen.AddAnswer("Давай", Com.Переход, 10);           

            scen.AddNewMessage(10, "Запускай консоль и запусти свой эксплойт. Внимание сплойт понимает только IP адрес сервера, поэтому тебе надо ping к серверу, запомни IP адрес, введи эти сведения, номер порта где эта дыра была найдена и жди эффект ", 8);
            scen.AddAnswer("IP что это ?", Com.Переход, 11);
            scen.AddAnswer("Сплойт работает", Com.Переход, 12);
            scen.AddAnswer("Сплойт не работает", Com.Переход, 13);

            scen.AddNewMessage(11, "Есть адрес IP 127,0,0,1 тоже самое localhost или primer.ru - 201.028.123.222 просто некоторые проги не могут работать с именами", 2);
            scen.AddAnswer("Понятно", Com.Переход, 12);            

            scen.AddNewMessage(12, "Вот все система позволила выполнить произвольные данные, Этими данными может стать шелл-shell. Прога которая поможет тебе перемещаться по файлам сервера. Шелл ты можешь найти в сети в браузере описание на стартовой странице. ", 3);
            scen.AddAnswer("Подожди как мне использовать команды", Com.Переход, 14);
            scen.AddAnswer("Все ясно, дальше я сам", Com.Переход, 15);

            scen.AddNewMessage(13, "Значит ты чайник ознакомся в браузере на стартовой странице гайд ", 4);
            scen.AddAnswer("Все прочитал", Com.Переход, 16);
            scen.AddAnswer("Я и так его знаю", Com.Переход, 16);

            scen.AddNewMessage(14, "Это просто некоторые команды понимает windows семейство, другие Unix системы. Ls -la аналог win команды dir чтобы просмотреть файлы. Для скачки файла wget или download - win систем", 10);
            scen.AddAnswer("Ага", Com.Переход, 15);
            scen.AddAnswer("Все спасибо, давай связь с боссом", Com.ВыходЗапуститьСкрипт, 0, "start");

            scen.AddNewMessage(15, "Ты быстро учишься, хочешь поговорить с Алексанкиным", 4);
            scen.AddAnswer("Да", Com.ВыходЗапуститьСкрипт, 0, "start");

            scen.AddNewMessage(16, "Все прочитал, ну и хорошо. Надо верно выбрать номер порта и нужный сплойт к нему, далее шело ты можеш конектится к серверу", 4);
            scen.AddAnswer("Понятно", Com.Переход, 15);

            scen.AddNewMessage(17, "Сканер уязвимостей содержит список уязвимостей и проводит проверку на наличии на каждом порте сервера. Это можно сделать вручную, но быстрее использовать программу", 4);
            scen.AddAnswer("Понятно", Com.Переход, 6);

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            // start
            ls = new List<Script.IEventGame> {
            new   Script.SendMail(){
                Mail = new Engine.MailInBox (){
                Title ="Знакомство",
                BodyText ="Сюда приходит почта. Мы благодарим что вы использовали наш клиент.",
                Mailto ="boticq@q.com"  } },
                new Script.GetNews (){ Text ="В последнее время образовалась очень высокая хакерская активность.", Topic = Engine.NewsClass.NewsСlass.TopicEnum.НовостиКасательноИгрока  },
                new Script.NextScen (){ ScenName ="Scen_2mis" },
                new Script.GetExp() { Exp = 20 }
            };
            scen.Script.Add("start", ls);
                     
            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat() { NameChat = "icq" } };
            ls.Add(new Script.CreatePort() { UrlServer = "www.test.ru", NameTitle = "Redis 0.2", PortNumber = 665, Rationo = 1, Text = "" });
            ls.Add(new Script.VulnerabilitiesAdd() { CName = "Redis 0.2", Exploid = true, GrantPremission = Server.PremissionServerEnum.FullControl, NameBug = "ExRedis 0.2", NewsInform = true, Shareware = false, VerA = 0, VerB = 50 });
            scen.Script.Add("Инцилизация", ls);
            
            return scen;
        }
        
        public ScenStruct Scen_2mis()
        {
            ScenStruct scen = new ScenStruct("Украсть файл winer.rar с сервера www.test.ru и открыть у себя на компе");

            scen.CreateNewICQChat("start", "Алексанкин", "job05.gif");

            scen.AddNewMessage(1, "Привет, мне сказал один знакомый что ты специалист, так что тебе будет не трудно пройти тест", 23);
            scen.AddAnswer("Я специалист номер один в этом деле", Com.Переход, 2);
            scen.AddAnswer("Что за тест?", Com.Переход, 2);

            scen.AddNewMessage(2, "Если ты хочешь получить эту работу, ты должен выполнить задание, если выполнишь работа твоя. Ты согласен?", 5);
            scen.AddAnswer("Какая будет у меня зарплата???", Com.Переход, 3);
            scen.AddAnswer("Согласен", Com.Переход, 4);

            scen.AddNewMessage(3, "Я буду тебе платить за задание 100 баксов", 5);
            scen.AddAnswer("Согласен", Com.Переход, 4);

            scen.AddNewMessage(4, "Задание высылаю по почте. Ты уже не первый кандидат, думаю что у тебя получится.", 10);
            scen.AddAnswer("Пока", Com.ВыходЗапуститьСкрипт, 0, "start");

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            // start
            ls = new List<Script.IEventGame> {
            new   Script.SendMail(){Mail = new MailInBox (){
                Title ="Задание от Алексанкина",
                BodyText ="Твоя цель взломать сервер primer.ru и похитить файл winer.rar (для завершение задания запусти файл у себе на компе) если ты сможешь то ты победил",
                Mailto ="alex@mail.ru"  } },           
            new Script.GetNews (){ FromFile =true  }
            };
            scen.Script.Add("start", ls);

            // win
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp () { Exp =25 },
                 new   Script.SendMail(){Mail = new MailInBox (){
                Title ="Оплата",
                BodyText ="Ваши деньги за работу 100$.",
                Mailto ="alex@mail.ru", CommandList = new Script.GetMoney (){ Money =100, TypeMoney = BankClass.BankAccount.TypeMoneyEnum.Dollar  }  } },
                new Script.MessageWin (){ Inform = FrmSoft.FrmError.InformEnum.Информация , Text ="Ты прошёл тест", Title ="Сообщение" },
                new Script.NextScen (){ ScenName ="Scen_3mis" }
            };
            scen.Script.Add("win", ls);

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver() };
            scen.Script.Add("loser", ls);           

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat() { NameChat = "start" } };
            scen.Script.Add("Инцилизация", ls);

            scen.NewCondition_FileDownload ("winer.rar", new Script.RunScript() { NameScript = "win" });

            Server server = App.GameGlobal.FindServer("www.test.ru");
            if (server != null) server.CreateFiles("/", new FileServerClass()
            {
                FileName = "winer",
                Perfix = "rar",
                Rights = FileServerClass.PremisionEnum.AdminUserGuest,
                Size = 52441,
                SystemFile = false,
                FileСontents = new FileServerClass.ParameterClass() { TypeInformation = FileServerClass.ParameterClass.TypeParam.goal_file }
            });

            return scen;
        }

        public ScenStruct Scen_3mis() {
            ScenStruct scen = new ScenStruct("Пройти курс молодого бойца по серверам") ;

            scen.CreateNewICQChat("start", "Spuler", "Pajero.gif");

            scen.AddNewMessage(1, "OK моя задача расказать вам о сервере, к вашему управлению переходит сервер www.ddospell.com. Консольная программа connect, логин и пароль я пришлю по почте", 10);
            scen.AddAnswer("Давай я жду", Com.Переход, 2);
            scen.AddAnswer("Переходим к делу", Com.Переход, 2);

            scen.AddNewMessage(2, "Также вы можете подобрать пароль и логин, но это уже навыки. В вашем профиле вы можете прокачивать свои способности вам нужны экстра очки вы их получите после нового уровня.", 10);
            scen.AddAnswer("Теперь я могу прокачаться", Com.Переход, 3);

            scen.AddNewMessage(3, "Установим бекдор теперь. Тебе нужно ввести коммаду id или ver когда конектишся к серверу", 10);
            scen.AddAnswer("Какая именно команда id или ver ?", Com.Переход, 3);
            scen.AddAnswer("Потом что ?", Com.Переход, 3);

            scen.AddNewMessage(4, "Ты увидишь название OS, теперь через браузер ты можешь скачать нужный тебе бекдор", 10);
            scen.AddAnswer("С какого сайта качать ?", Com.Переход, 3);
            scen.AddAnswer("Да я скачал уже ", Com.Переход, 3);
            scen.AddAnswer("Зачем нужен бекдор ", Com.Переход, 3);

            scen.AddNewMessage(5, "Бекдор(Backdoor) приводиться как задняя дверь поможет тебе получить доступ к серверу. Повысить права доступа или получить доступ к админ панели как сейчас ", 10);
            scen.AddAnswer("Все понял", Com.Переход, 3);

            scen.AddNewMessage(6, "Скаченному Бекдору открываем общий доступ к файлу ", 10);
            scen.AddAnswer("С какого сайта качать ?", Com.Переход, 3);
            scen.AddAnswer("Да я скачал уже ", Com.Переход, 3);
            scen.AddAnswer("Зачем нужен бекдор ", Com.Переход, 3);

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            // start
            ls = new List<Script.IEventGame> {            
            new   Script.SendMail(){Mail = new MailInBox (){
                Title ="Пароль доступа.",
                BodyText ="Доступ к www.ddospell.com. Логин:login Пароль:passwd",
                Mailto ="alex@mail.ru", CommandList =null  } },
            new Script.GetNews (){ FromFile =true  }
            };
            scen.Script.Add("start", ls);

            // win
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp () { Exp =25 },               
                new Script.NextScen (){ ScenName ="Scen_serveredit" }
            };
            scen.Script.Add("win", ls);

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver() };
            scen.Script.Add("lose", ls);

            ls = new List<Script.IEventGame>
            {
                new Script.GetExp () { Exp =5 },
                new Script.NextScen (){ ScenName ="Scen_serveredit" }                
            };
            scen.Script.Add("loser", ls);

            Server server = App.GameGlobal.FindServer("www.ddospell.com");
            if (server != null)  server.LoginAndPass = "login:passwd";

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat() { NameChat = "start" } };
            scen.Script.Add("Инцилизация", ls);

            return scen;
        }

        public ScenStruct Scen_serveredit() {
            ScenStruct scen = new ScenStruct("Создай форум или чат") ;

            scen.CreateNewICQChat("start", "Алексанкин", "job05.gif");


            //несломать сервер

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            ls = new List<Script.IEventGame> {
            
            };
            scen.Script.Add("start", ls);

            // win
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp () { Exp =30 },
                new   Script.SendMail(){Mail = new Engine.MailInBox (){
                Title ="Ваши деньги",
                BodyText ="В расчете",
                Mailto ="alex@mail.ru", CommandList = new Script.GetMoney (){ Money =100, TypeMoney = Engine.BankClass.BankAccount.TypeMoneyEnum.Dollar  }  } },
                new Script.NextScen (){ ScenName ="Scen_tourmode" }
            };
            scen.Script.Add("win", ls);

            Server server = App.GameGlobal.FindServer("www.ddospell.com");
            if (server != null) server.LoginAndPass = "login:passwd";

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat() { NameChat = "start" } };
            scen.Script.Add("Инцилизация", ls);

            return scen;
        }

       [Serializable]
        public struct ScenStruct
        {
            public string Title;
            public Dictionary<string, ICQ> Chat;
            public Dictionary<string, List<Engine.GameEvenStruct.IEventGame>> Script;

            /// <summary>
            /// Условия установленные сценарием
            /// </summary>
            readonly private List<ConditionStruct> GameCondition;
            private ICQ lastChat;
            private ICQ.Message lastMessage;

            public ScenStruct(string title)
            {
                Title = title;
                Chat = new Dictionary<string, ICQ>();
                Script = new Dictionary<string, List<Script.IEventGame>>();
                GameCondition = new List<ConditionStruct>();
                lastChat = new ICQ();
                lastMessage = new ICQ.Message();
            }

            public void EventIntroduce(GameScen.ScenStruct.ConditionStruct.ConditionEnum condition, string[] StringElemen)
            {
                int  forDel;
                for (int i = 0; i < GameCondition.Count; i++)
                {
                    if (GameCondition[i].CheckCondition(condition, StringElemen))
                    {
                        GameCondition[i].Action.Run();
                        forDel = i;
                        goto del;
                    }
                }
                return;
            del:;
                GameCondition.RemoveAt(forDel);
            }

            /// <summary>
            /// Создать новый чат
            /// </summary>
            /// <param name="name"></param>
            public void CreateNewICQChat(string name, string n, string img)
            {
                if (Chat == null) Chat = new Dictionary<string, ICQ>();
                ICQ i = new ICQ() { Nicke = n, Img = img, Messages = new List<ICQ.Message>() };
                i.Messages = new List<ICQ.Message>() { new ICQ.Message() };//пустая строка. необходим для совместимости с прошлым сценарием.
                Chat.Add(name, i);
                lastChat = i;
            }
            /// <summary>
            /// Добовляет сообщение в последний созданный чат 
            /// </summary>
            public void AddNewMessage(ushort id, string text, short sec)
            {
                lastChat.Messages.Add(new ICQ.Message() { Id_Message =id, Text = text, Sec = sec, Answers = new List<ICQ.Message.Answer>() });
                lastMessage = lastChat.Messages[^1];
            }
            /// <summary>
            /// Добавить ответ к прош. сообщению
            /// </summary>          
            public void AddAnswer(string text, ICQ.Message.Answer.CommandAnswerEnum command, int i = 0, string t = "")
            {
                lastMessage.Answers.Add(new ICQ.Message.Answer() { CommandAnswer = command, IntArgument = i, StrArgument = t, TextAnswer = text });
            }

            /// <summary>
            /// Создает Условие при котором <b >файл скачан </b>
            /// </summary>
            public void NewCondition_FileDownload(string file_name_download, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { file_name_download },
                    Condition = ConditionStruct.ConditionEnum.ФайлСкачан
                };               
                GameCondition.Add(condition);
            }

            /// <summary>
            /// Создает Условие при котором <b >Сервак отключен, перезапущен</b>
            /// </summary>
            public void NewCondition_SrvDown(string url, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url },
                    Condition = ConditionStruct.ConditionEnum.СерверОтключен
                };               
                GameCondition.Add(condition);
            }

            /// <summary>
            /// Создает Условие при котором <b >Сервер успешно запустил службу</b>
            /// </summary>
            public void NewCondition_RoleWork(string url, Engine.Virtualization.InstaceTypeEnum instaceType , Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url, instaceType.ToString() },
                    Condition = ConditionStruct.ConditionEnum.ЗапущенаСлужбаНаСервере
                };               
                GameCondition.Add(condition);
            }

            [Serializable]
            public struct ICQ
            {
                public List<Message> Messages;
                public string Nicke;
                public string Img;

                [Serializable]
                public struct Message
                {
                    public ushort Id_Message;
                    public short Sec;
                    public string Text;
                    public List<Answer> Answers;

                    [Serializable]
                    public struct Answer
                    {
                        public string TextAnswer;
                        public CommandAnswerEnum CommandAnswer;
                        public int IntArgument;
                        public string StrArgument;

                        public enum CommandAnswerEnum
                        {
                            Переход,
                            ВыходЗапуститьСкрипт,
                            ВыходЗапуститьЧат,
                            ПростоВыход
                        }
                    }
                }
            }

            [Serializable]
            public struct ConditionStruct
            {           
                /// <summary>
                /// Это действие совершиться
                /// </summary>
                public Script.IEventGame Action;
                /// <summary>
                /// Текст для условия должно равняться ему
                /// </summary>
                public string[] StringElement;               
                /// <summary>
                /// Тут условие 
                /// </summary>
                public ConditionEnum Condition;               

                /// <summary>
                /// Проверяет на выполнение условия
                /// </summary>
                /// <param name="condition">Событие</param>
                /// <param name="str">текствое значения которое должно быть равно StringElement</param>
                /// <param name="i">числовое значение = IntElement</param>
                /// <returns>True - соотвует</returns>
                public bool CheckCondition(ConditionEnum condition, string[] str)
                {              
                    switch (condition)
                    {
                        case ConditionEnum.ФайлСкачан:
                            if (str[0].ToLower() == StringElement[0].ToLower()) return  true;
                            break;
                        case ConditionEnum.ЗапущенаСлужбаНаСервере :
                            if (str[0].ToLower() == StringElement[0].ToLower() & str[1] == StringElement[1]) return true;
                            break;
                        case ConditionEnum.ВходНаСервер:
                            return false;//Test test 
                           
                        default:
                            return false;
                    }
                    return false;
                }              

                /// <summary>
                /// Событие при котором случаеться действие
                /// </summary>
                public enum ConditionEnum
                {
                    ФайлСкачан,
                    ВходНаСервер,
                    ЗапущенаСлужбаНаСервере,
                    СерверОтключен
                }
            }
        }

    }
}
