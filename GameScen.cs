using System;
using System.Collections.Generic;
using Com = PH4_WPF.GameScen.ScenStruct.ICQ.Message.Answer.CommandAnswerEnum;
using Script = PH4_WPF.Engine.GameEvenClass;
using PH4_WPF.Engine;

/*
 *  
 * scen.CreateNewICQChat("otvet", "Azon", "016(208x144).jpg");
            scen.AddNewMessage(1, "Вовремя тебя админ предупредил", 8);
            scen.AddAnswer("", Com.Переход, 2);
            scen.AddNewMessage(2, "", 10);

            scen.AddNewMessage(3, "", 8);

            scen.AddNewMessage(4, "", 5);

            scen.AddNewMessage(5, "", 6);

            scen.AddNewMessage(6, "", 11);

            scen.AddNewMessage(7, "", 8);
 */


namespace PH4_WPF
{
    /// <summary>
    /// Коллекция игровых сценариев
    /// </summary>
    [Serializable]
    public class GameScen
    {
    #region "Заголовок переменный"
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
    #endregion

    #region "Сценарий игры тут по методам" 

        /// <summary>
        /// Начальный сценарий игры
        /// </summary>
        public ScenStruct Scen_start()
        {
            ScenStruct scen = new ScenStruct("Завершите обучение, задание в почтовом ящике");

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
            scen.AddAnswer("Научи меня", Com.ВыходЗапуститьСкрипт, 0, "icq_beginer");
            scen.AddAnswer("Не надо я  уже эксперт", Com.Переход, 9);

            scen.AddNewMessage(9, "Ну давай, ты хочешь сейчас поговорить с ним сейчас", 1);
            scen.AddAnswer("Да", Com.ВыходЗапуститьСкрипт, 0, "start");

            // icq_beginer
            scen.CreateNewICQChat("icq_beginer", "Cheterman", "DDC.jpg");
            scen.AddNewMessage(1, "Good сначало мы должны в [Сетевом Окружении] (на вашем рабочем столе) найти нужный сервер ", 6);
            scen.AddAnswer("Да, я вошёл сюда", Com.Переход, 3);
            scen.AddAnswer("Для чего оно нужно", Com.Переход, 2);

            scen.AddNewMessage(2, "Не задавай лишних вопросов, просто делай так как я тебе сказал", 2);
            scen.AddAnswer("А ясно", Com.Переход, 3);
            scen.AddAnswer("Как скажешь", Com.Переход, 3);

            scen.AddNewMessage(3, "А теперь мы найдем сервер www.test.ru Найди его и затем выберете его. Быстро можно найти через поиск верхнем правом углу экрана.", 3);
            scen.AddAnswer("Ну вот нашёл", Com.Переход, 4);
            scen.AddAnswer("Что дальше делать", Com.Переход, 4);

            scen.AddNewMessage(4, "Скачай себе сканер портов, скачать новые сканеры уязвимостей сможешь на сайте explot.in. И жди результат сканирования.", 4);
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
            new   Script.SendMail(new MailInBox (){
                Title ="Знакомство",
                BodyText ="Сюда приходит почта. Мы благодарим что вы использовали наш клиент.",
                Mailto ="boticq@q.com"  } ),
                new Script.GetNews("В последнее время образовалась очень высокая хакерская активность.", Enums.TopicEnum.НовостиКасательноИгрока ),
                new Script.NextScen("Scen_2mis"),
                new Script.GetExp(20),
                new Script.ShowMyCanvas()
            };
            scen.Script.Add("start", ls);

            // icq_beginer
            ls = new List<Script.IEventGame> {            
                new Script.ShowMyCanvas(),
                new Script.StartChat("icq_beginer")
            };
            scen.Script.Add("icq_beginer", ls);

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat("icq") };
            ls.Add(new Script.CreatePort() { UrlServer = "www.test.ru", NameTitle = "Redis 0.2", PortNumber = 665, Rationo = 1, Text = "" });
            ls.Add(new Script.VulnerabilitiesAdd() { CName = "Redis 0.2", Exploid = true, GrantPremission = Server.PremissionServerEnum.FullControl, NameBug = "ExRedis 0.2", NewsInform = true, Shareware = false, VerA = 0, VerB = 50 });
            scen.Script.Add("Инцилизация", ls);

            //edit
            var srv = App.GameGlobal.FindServer("www.test.ru");
            srv.CreateVirtualManual();
            // Настроки для тествого сервера           
            srv.OS = Server.TypeOSEnum.WinSrv;
            srv.OSName = "win 95";
            // Создает доступный узел для скачки шела бесплатно 
            App.GameGlobal.OpenUrl.Add(@"localhost.cloud/win95_shell", new FileServerClass()
            {
                FileName = "Win_95",
                Perfix = ".shell",
                Rights = FileServerClass.PremisionEnum.AdminUserGuest,
                Size = 5000,
                FileСontents = new FileServerClass.ParameterClass() { TypeInformation = Enums.TypeParam.shell, TextCommand = "Win 95" }
            });

            return scen;
        }
        /// <summary>
        /// Второе обучающее задание
        /// </summary>
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
            new   Script.SendMail(new MailInBox (){
                Title ="Задание от Алексанкина",
                BodyText ="Твоя цель взломать сервер www.test.ru и похитить файл winer.rar (для завершение задания запусти файл у себе на компе) если ты сможешь то ты победил",
                Mailto ="alex@mail.ru"  } ),
            new Script.GetNews (true )
            };
            scen.Script.Add("start", ls);

            // win
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp (25),
                 new   Script.SendMail(new MailInBox (){
                Title ="Оплата",
                BodyText ="Ваши деньги за работу 100$.",
                Mailto ="alex@mail.ru", CommandList = new Script.GetMoney (100,  Enums.TypeMoneyEnum.Dollar  ) } ),
                new Script.MessageWin ("Сообщение","Ты успешно прошел тест. Теперь тебя взяли на новую работу",FrmSoft.FrmError.InformEnum.Информация ),
                new Script.NextScen ("Scen_3mis")
            };
            scen.Script.Add("win", ls);

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver() };
            scen.Script.Add("loser", ls);

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat("start") };
            scen.Script.Add("Инцилизация", ls);

            scen.When_FileDownload("winer.rar", new Script.RunScript("win"));

            //edit
            Server server = App.GameGlobal.FindServer("www.test.ru");
            server.CreateFiles(patch: "/", fileServer: new FileServerClass()
            {
                FileName = "winer",
                Perfix = "rar",
                Rights = FileServerClass.PremisionEnum.AdminUserGuest,
                Size = 52441,
                SystemFile = false,
                FileСontents = new FileServerClass.ParameterClass() { TypeInformation = Enums.TypeParam.goal_file }
            });

            return scen;
        }
        /// <summary>
        /// Третье обучающеее задание игры
        /// </summary>
        public ScenStruct Scen_3mis()
        {
            ScenStruct scen = new ScenStruct("Пройти курс молодого бойца по серверам");

            scen.CreateNewICQChat("start", "Spuler", "Pajero.gif");

            scen.AddNewMessage(1, "Привет. Я от Алексанкина. OK моя задача расказать вам о сервере, к вашему управлению переходит сервер www.ddospell.com. Консольная программа connect, логин и пароль я пришлю по почте", 10);
            scen.AddAnswer("Давай я жду", Com.Переход, 2);
            scen.AddAnswer("Переходим к делу", Com.Переход, 2);

            scen.AddNewMessage(2, "Также вы можете подобрать пароль и логин, но это уже навыки. В вашем профиле вы можете прокачивать свои способности вам нужны экстра очки вы их получите после нового уровня.", 10);
            scen.AddAnswer("Теперь я могу прокачаться", Com.Переход, 3);

            scen.AddNewMessage(3, "Установим бекдор теперь. Тебе нужно ввести коммаду id или ver когда конектишся к серверу", 10);
            scen.AddAnswer("Какая именно команда id или ver ?", Com.Переход, 7);
            scen.AddAnswer("Потом что ?", Com.Переход, 4);

            scen.AddNewMessage(4, "Когда ты увидишь название OS, ты сможешь скачать нужный тебе бекдор", 10);
            scen.AddAnswer("С какого сайта качать ?", Com.Переход, 8);
            scen.AddAnswer("Да я скачал уже ", Com.Переход, 6);
            scen.AddAnswer("Зачем нужен бекдор ", Com.Переход, 5);

            scen.AddNewMessage(5, "Бекдор(Backdoor) приводиться как задняя дверь поможет тебе получить доступ к серверу. Повысить права доступа или получить доступ к админ панели как сейчас ", 10);
            scen.AddAnswer("Все понял", Com.Переход, 6);

            scen.AddNewMessage(6, "Скаченному Бекдору открываем общий доступ к файлу. Ранее я уже нашел тебе специальный бекдор см. в письме котором я прислал тебе  ", 10);
            scen.AddAnswer("С какого сайта качать ?", Com.Переход, 8);
            scen.AddAnswer("Да я скачал уже ", Com.Переход, 9);
            scen.AddAnswer("Зачем нужен бекдор ", Com.Переход, 5);
            scen.AddAnswer("Пока", Com.Переход, 10);

            scen.AddNewMessage(7, "Ver для *win систем ОС. id - *unix систем  ", 10);
            scen.AddAnswer("Все понял", Com.Переход, 4);

            scen.AddNewMessage(8, "Этот сайт называеться milw0rm там есть специальный раздел ", 8);
            scen.AddAnswer("Все понял", Com.Переход, 6);

            scen.AddNewMessage(9, "Тебе нужно получить доступ к серверу, жду результатов ", 5);
            scen.AddAnswer("Что потом ??", Com.Переход, 10);
            scen.AddAnswer("В принципе это для меня легко давай сразу перейдем к сути", Com.Переход, 12);

            scen.AddNewMessage(10, " Чуть не забыл. В сетевом окружение тебе нужно выбрать сервер и войти в админ панель, чтобы перейти к другому заданию.  ", 15);
            scen.AddAnswer("Все понял", Com.ВыходЗапуститьСкрипт, "start");
            scen.AddAnswer("Потом какое будет задание ?", Com.Переход, 6);

            scen.AddNewMessage(11, "Это уже от меня не зависит, я просто тебе сказал что делать  ", 18);
            scen.AddAnswer("Все понял", Com.ВыходЗапуститьСкрипт, "start");

            scen.AddNewMessage(12, "Что реально ? Сразу к делу     ", 5);
            scen.AddAnswer("Да конечно", Com.Переход, 13);
            scen.AddAnswer("Просто я пошутил", Com.Переход, 10);

            scen.AddNewMessage(13, "Вижу ты профессионал тогда я сообщу о твоем завершение задания   ", 21);
            scen.AddAnswer("Так и есть я профи", Com.ВыходЗапуститьСкрипт, "speed");
            scen.AddAnswer("Нет, это была шутка", Com.Переход, 14);

            scen.AddNewMessage(14, "За такие шутки в зубах бывают промежутки   ", 15);
            scen.AddAnswer("Забей", Com.Переход, 10);

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            // start
            ls = new List<Script.IEventGame> {
            new   Script.SendMail(new MailInBox (){
                Title ="Пароль доступа.",
                BodyText ="Доступ к www.ddospell.com. Логин:login Пароль:passwd \nДля получение полного доступа 'ddospell.com/access.pl' - бекдор ",
                Mailto ="alex@mail.ru", CommandList =null  } ),
            new Script.GetNews (true),
            new Script.WriteLog ("Если вы потеряете контроль над сервером ddospell вы проиграете", Enums.LogTypeEnum.Server )
            };
            scen.Script.Add("start", ls);

            // win
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp (25),
                new Script.NextScen ("Scen_serveredit")
            };
            scen.Script.Add("win", ls);

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver("Причина поражения сервер вы потряли доступ к серверу www.ddospell.com ") };
            scen.Script.Add("lose", ls);

            // если вы отказались от задания
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp (5),
                new Script.NextScen ("Scen_serveredit")
            };
            scen.Script.Add("speed", ls);

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat("start") };
            scen.Script.Add("Инцилизация", ls);

            // edit
            scen.When_AccesssAP("www.ddospell.com", new Script.RunScript("win"));
            scen.When_ChangeControl("www.ddospell.com", Server.PremissionServerEnum.none, new Script.RunScript("lose"));

            var srv = App.GameGlobal.FindServer("www.ddospell.com");
            srv.LoginAndPass = "login:passwd";
            srv.OS = Server.TypeOSEnum.Linux;
            srv.OSName = "Linux";
            srv.CreateVirtualManual();
            srv.CreateFiles("/", "Programming PHP.doc", new FileServerClass.ParameterClass()
            {
                TypeInformation = Enums.TypeParam.goal_file,
                EventGame = new GameEvenClass.GetProf(Script.GetProf.ProfEnum.GetCoder)
            },
            5410, FileServerClass.PremisionEnum.AdminUserGuest);

            App.GameGlobal.OpenUrl.Add(@"ddospell.com/access.pl", new FileServerClass()
            {
                FileName = "access",
                Perfix = "pl",
                Rights = FileServerClass.PremisionEnum.AdminUserGuest,
                Size = 4000,
                FileСontents = new FileServerClass.ParameterClass()
                {
                    TypeInformation = Enums.TypeParam.backdoor,
                    TextCommand = "url=www.ddospell.com"
                }
            });

            return scen;
        }
        /// <summary>
        /// Организация сайта на сервере ddospell
        /// </summary>
        /// <returns></returns>
        public ScenStruct Scen_serveredit()
        {
            ScenStruct scen = new ScenStruct("Создай форум или чат на сервере www.ddospell.com");

            scen.CreateNewICQChat("start", "Алексанкин", "job05.gif");
            scen.AddNewMessage(1, "Итак начнём работать, мне нужно чтобы стал работать сайт", 23);
            scen.AddAnswer("Что мне надо сделать", Com.Переход, 2);
            scen.AddNewMessage(2, "Установи и создай сценарии на сайт, для того чтобы посещаемость сайта была не менее 30 человек в день. Файл на сервере www.ddospell.com поможет изучить програмирование Programming PHP.doc, если у тебя проблемы с этим", 10);
            scen.AddAnswer("Это простое задание", Com.Переход, 3);
            scen.AddAnswer("Что именно  ? ", Com.Переход, 4);
            scen.AddNewMessage(3, "Я надеюсь", 6);
            scen.AddAnswer("Я начинаю работать ", Com.Переход, 5);
            scen.AddNewMessage(4, "Нужен форум или чат. Попробуй запустить роли на сервере чтобы заработал сайт ", 10);
            scen.AddAnswer("Я начинаю работать ", Com.Переход, 5);
            scen.AddNewMessage(5, "Давай быстрей, я не буду тебе платить зарплату за безделие. ", 8);
            scen.AddAnswer("Я работаю", Com.ВыходЗапуститьСкрипт, 0, "start");

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            ls = new List<Script.IEventGame> {
            new Script.WriteLog ("Напоминаню еще раз, потеряете контроль над сервером вы проиграете", Enums.LogTypeEnum.Server ),
            new Script.UppHiTec ()
            };
            scen.Script.Add("start", ls);

            // win
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp (30),
                new   Script.SendMail(new MailInBox (){
                Title ="Ваши деньги",
                BodyText ="В расчете",
                Mailto ="alex@mail.ru", CommandList = new Script.GetMoney (100, Enums.TypeMoneyEnum.Dollar  )  } ),
                new Script.NextScen ("Scen_tourmode")
            };
            scen.Script.Add("win", ls);

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver("Причина поражения сервер вы потряли доступ к серверу www.ddospell.com ") };
            scen.Script.Add("lose", ls);

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat("start") };
            scen.Script.Add("Инцилизация", ls);

            // edit
            scen.When_PopularSrv("www.ddospell.com", 30, new Script.RunScript("win"));
            scen.When_ChangeControl("www.ddospell.com", Server.PremissionServerEnum.none, new Script.RunScript("lose"));

            return scen;
        }
        /// <summary>
        /// Апгрейд сервера обучение 
        /// </summary>
        /// <returns></returns>
        public ScenStruct Scen_tourmode()
        {
            ScenStruct scen = new ScenStruct("Увеличть объем жёстких дисков");

            scen.CreateNewICQChat("start", "Алексанкин", "job05.gif");

            scen.AddNewMessage(1, "Наш сервер слаб, его надо усилить, я выделяю деньги на увеличение дискового пространства сервера", 23);
            scen.AddAnswer("Что мне надо сделать?", Com.Переход, 2);
            scen.AddAnswer("Все зависит от суммы", Com.Переход, 2);

            scen.AddNewMessage(2, "В админ панели ты можешь заказать апгрейд железа. Заявка по почте придет на сервер ddospell, потом зайди на сервер, прочти почту и оплати заявку. ", 12);
            scen.AddAnswer("Просто сделать апгрейд", Com.Переход, 3);
            scen.AddAnswer("Что с деньгами", Com.Переход, 3);

            scen.AddNewMessage(3, "Деньги я пришлю по почте на расходы, отставшие деньги можешь забрать себе  ", 15);
            scen.AddAnswer("Пpиступаю к работе", Com.ВыходЗапуститьСкрипт, "start");
            scen.AddAnswer("А если бабок не хватит", Com.Переход, 4);

            scen.AddNewMessage(4, "Ищи их сам где хочешь денег всегда хватало", 8);
            scen.AddAnswer("Пpиступаю к работе", Com.ВыходЗапуститьСкрипт, "start");

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            ls = new List<Script.IEventGame> {
            new Script.UppHiTec (),
              new   Script.SendMail(new MailInBox (){
                Title ="Деньги на апгрейд",
                BodyText ="Трать деньги с умом",
                Mailto ="alex@mail.ru", CommandList = new Script.GetMoney (200, Enums.TypeMoneyEnum.Dollar)  } )
            };
            scen.Script.Add("start", ls);

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver("Причина поражения сервер вы потряли доступ к серверу www.ddospell.com ") };
            scen.Script.Add("lose", ls);

            // win
            ls = new List<Script.IEventGame>
            {
                new Script.GetExp (50),
                new Script.NextScen ("Scen_safe")
            };
            scen.Script.Add("win", ls);

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat("start") };
            scen.Script.Add("Инцилизация", ls);

            // edit
            scen.When_HardwareUp("www.ddospell.com", new Script.RunScript("win"));
            scen.When_ChangeControl("www.ddospell.com", Server.PremissionServerEnum.none, new Script.RunScript("lose"));

            return scen;
        }
        /// <summary>
        /// Финальное обучение апгрейд софта
        /// </summary>
        /// <returns></returns>
        public ScenStruct Scen_safe()
        {
            ScenStruct scen = new ScenStruct("Обнови софт на сервере");

            scen.CreateNewICQChat("start", "Алексанкин", "job05.gif");
            scen.AddNewMessage(1, "У нас появились проблемы, совсем недавно я говорил с группой онлайн рэкета России, они сказали, что наш сервер очень сильно начал давить на конкуренцию в сети", 13);
            scen.AddAnswer("То есть другие сайты", Com.Переход, 2);
            scen.AddAnswer("Да пошли они", Com.Переход, 3);
            scen.AddNewMessage(2, "Да в сети есть множество магазинов, вредных программ, кардерства, примерно я знаю кто это такой, один китайский хакер", 10);
            scen.AddAnswer("В чем его задача", Com.Переход, 3);
            scen.AddNewMessage(3, "Их задача подорвать репутацию нашего сервера, как мне сказали если хакер сможет повесить наш сервер то это скажется о нашей репутации как о новичках, а не профессионалов.", 10);
            scen.AddAnswer("Что он хочет сделать", Com.Переход, 4);
            scen.AddNewMessage(4, "Атаковать они будут на слабые стороны в сервере, надо обновить все программы чтобы хакер не смог проникнуть через брешь в системе", 15);
            scen.AddAnswer("Я понял я срочно займусь этим", Com.Переход, 5);
            scen.AddAnswer("Я успею обновить софт", Com.Переход, 5);
            scen.AddNewMessage(5, "Давай быстрее у нас мало времени", 11);
            scen.AddAnswer("ок", Com.ВыходЗапуститьСкрипт, "start");

            scen.CreateNewICQChat("otvet", "Azon", "016(208x144).jpg");
            scen.AddNewMessage(1, "Вовремя тебя админ предупредил", 8);
            scen.AddAnswer("Ты кто", Com.Переход, 2);
            scen.AddAnswer("Я тебя вычислю по IP", Com.Переход, 3);
            scen.AddAnswer("Тебе кабздец", Com.Переход, 3);
            scen.AddNewMessage(2, "Мое имя в сети знают многие", 10);
            scen.AddAnswer("Похоже не все знают", Com.Переход, 3);
            scen.AddNewMessage(3, "Ты надеюсь знаешь, что твой босс сказал", 8);
            scen.AddAnswer("Ты про чё?", Com.Переход, 4);
            scen.AddNewMessage(4, "Он втоптал в грязь многих хакеров, ради денег", 5);
            scen.AddAnswer("А сам ты живешь ради денег?", Com.Переход, 5);
            scen.AddAnswer("И что ты предлогаешь", Com.Переход, 6);
            scen.AddNewMessage(5, "Нет я живу ради победы", 6);
            scen.AddAnswer("У меня нет цели только путь ", Com.Переход, 6);
            scen.AddNewMessage(6, "Я нечего не предлагаю, но если ты хочешь получить доверие в сети и уважения ты должен показать на что ты способен, как например дефейс трёх сайтов.", 11);
            scen.AddAnswer("Без проблем", Com.Переход, 7);
            scen.AddAnswer("Я выбираю путь админа чем хакера", Com.Переход, 8);
            scen.AddNewMessage(7, "Это правильный выбор, я вышлю тебе почтой информацию", 8);
            scen.AddAnswer("Жду сообщения", Com.ВыходЗапуститьСкрипт, "win2");
            scen.AddNewMessage(8, "Мы с тобой ещё встретимся", 7);
            scen.AddAnswer("Жду не дождусь", Com.ВыходЗапуститьСкрипт, "win");
            scen.AddAnswer("Желаю успеха", Com.ВыходЗапуститьСкрипт, "win");

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            //start
            ls = new List<Script.IEventGame> {
             new GameEvenClass.GetNews (true),
              new   Script.SendMail(new MailInBox (){
                Title ="Деньги на обновление софта",
                BodyText ="Лишнее можешь забрать себе ",
                Mailto ="alex@mail.ru", CommandList = new Script.GetMoney (100, Enums.TypeMoneyEnum.Dollar)  } ),
               new  Script.UppHiTec (),
               new Script.CreateGameEven (App.GameGlobal.DataGM.AddMonths(6),  new Script.RunScript("lose")) // событие через это время вы потеряете над сервером контроль
            };
            scen.Script.Add("start", ls);

            //win
            ls = new List<Script.IEventGame> {
              new Script.NextScen("Scen_smt"),
              new Script.GetExp (80)
            };
            scen.Script.Add("win", ls);

            //win2
            ls = new List<Script.IEventGame> {
              new Script.NextScen("Scen_hackersite"),
              new Script.GetExp (80)
            };
            scen.Script.Add("win2", ls);

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver("Вы не успели обновить сервер и хакер повесил сервер, вас уволили, и вы проиграли ") };
            scen.Script.Add("lose", ls);

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat("start") };
            scen.Script.Add("Инцилизация", ls);

            // edit
            scen.When_SoftwareUp("www.ddospell.com", new Script.StartChat ("otvet"));     

            return scen;
        }
        /// <summary>
        /// Работа с хешами и перебором паролей
        /// </summary>
        /// <returns></returns>
        public ScenStruct Scen_smt()
        {
            ScenStruct scen = new ScenStruct("Создай системы перебора паролей и блоков на своих серверах "); // smt2 and smt3 пропущшенный

            scen.CreateNewICQChat("start", "Spuler", "Pajero.gif");
            scen.AddNewMessage(1, "Привет. Теперь ты можешь использовать свои сервера для перебора паролей. Если подключишь службу перебора хешей  на этом сервере", 8);
            scen.AddAnswer("Как это работает?", Com.Переход, 2);
            scen.AddAnswer("Зачем это мне надо?", Com.Переход, 3);

            scen.AddNewMessage(2, "Ну тут такая ситуация когда тебе нужно подключиться к серверу,   по логину и паролю. Просто запускаешь процесс подбора, он подбирает нужный пароль, это занимает много времени. ", 8);
            scen.AddAnswer("Ого полезная штука", Com.Переход, 4);
            scen.AddAnswer("А хеши? ", Com.Переход, 5);

            scen.AddNewMessage(3, "Чтобы получить доступ к серверу , чтобы потом его сломать ", 5);
            scen.AddAnswer("Зачем нужны хеши", Com.Переход, 5);

            scen.AddNewMessage(4, "Да пользуйся. Можно скачать программу на сайте, также можно использовать подборы хешей ", 8);
            scen.AddAnswer("Сейчас скачаю и попробую", Com.ВыходЗапуститьСкрипт, "start");

            scen.AddNewMessage(5, "Hash это текст, который зашифрован алгоритмом, к примеру md5. С помощи подбора текста шифрую этит способом, хеши можно найти пароль ", 8);
            scen.AddAnswer("Да это полезная штука надо попробовать", Com.ВыходЗапуститьСкрипт, "start");
            scen.AddAnswer("Сейчас скачаю и попробую", Com.ВыходЗапуститьСкрипт, "start");

            //Делаем скрипты
            scen.Script = new Dictionary<string, List<Script.IEventGame>>();
            List<Script.IEventGame> ls;

            //start
            ls = new List<Script.IEventGame> {
             
            };
            scen.Script.Add("start", ls);

            //win
            ls = new List<Script.IEventGame> {
              new Script.NextScen("Scen_blcaking"),
              new Script.GetExp (80)
            };

            // lose
            ls = new List<Script.IEventGame> { new Script.GameOver("Причина поражения сервер вы потряли доступ к серверу www.ddospell.com ") };
            scen.Script.Add("lose", ls);

            //Инцилизация сценария начальные настройки
            ls = new List<Script.IEventGame> { new Script.StartChat("start") };
            scen.Script.Add("Инцилизация", ls);

            // edit            
            scen.When_ChangeControl("www.ddospell.com", Server.PremissionServerEnum.none, new Script.RunScript("lose"));
            scen.When_BrutoforceEnd("", "", false, new Script.StartChat("win"));

            App.GameGlobal.Instructions_V();
            return scen;
        }

        public ScenStruct Scen_blcaking()
        {
            ScenStruct scen = new ScenStruct("Раздобыть денег на www.vica.ru, найди нужный файл. Или просто пришли ему деньги, просто завершить задание ");


            return scen;
        }

        public ScenStruct Scen_programer()
        {
            ScenStruct scen = new ScenStruct("На основе движка www.vica.ru создать свой движок и разместить на своем сайте");


            return scen;
        }

        public ScenStruct Scen_greencard()
        {
            ScenStruct scen = new ScenStruct("Если вы хотите работать Ramp испытайте уязвимость или откажитесь от задания и подпиши контракт с другими ");


            return scen;
        }

        public ScenStruct Scen_green2()
        {
            ScenStruct scen = new ScenStruct("Взломать сервер mts.ru.greencard и похитить базу данных абонентов МТС`а ");


            return scen;
        }

        public ScenStruct Scen_green3()
        {
            ScenStruct scen = new ScenStruct("Взломать сервер energo.lan.greencard и задось сервер");


            return scen;
        }

        public ScenStruct Scen_green4()
        {
            ScenStruct scen = new ScenStruct("Взломать сервер www.arhsystem.ru, найди control.alse открытия дверей");


            return scen;
        }

        public ScenStruct Scen_mentray()
        {
            ScenStruct scen = new ScenStruct("Вырудить любой сервер");


            return scen;
        }

        public ScenStruct Scen_mentray2()
        {
            ScenStruct scen = new ScenStruct("Совершить акта дефейса, или просто завершить задание");


            return scen;
        }

        public ScenStruct Scen_fast()
        {
            ScenStruct scen = new ScenStruct("Организовать Блокчейн");


            return scen;
        }

        public ScenStruct Scen_endglavai()
        {
            ScenStruct scen = new ScenStruct("Найти на сервере RSE.COM");


            return scen;
        }

        public ScenStruct Scen_endglavaii()
        {
            ScenStruct scen = new ScenStruct("Организовать торговлю криптой Ethereum");


            return scen;
        }

        public ScenStruct Scen_salf()
        {
            ScenStruct scen = new ScenStruct("Защитить своеи сервера от хакеров");


            return scen;
        }

        public ScenStruct Scen_hackersite()
        {
            ScenStruct scen = new ScenStruct("Заразить компьютера");


            return scen;
        }

        public ScenStruct Scen_hackersite2()
        {
            ScenStruct scen = new ScenStruct("Заразить компьютера еще больше");


            return scen;
        }

        public ScenStruct Scen_stimyl()
        {
            ScenStruct scen = new ScenStruct("Попробуй закрыть доступ для Markys`a (найди его имя  в сети)");


            return scen;
        }

        public ScenStruct Scen_tall()
        {
            ScenStruct scen = new ScenStruct("Помочь своему боссу подчисти логи r77.mos.gov");


            return scen;
        }

        public ScenStruct Scen_controlserver()
        {
            ScenStruct scen = new ScenStruct("Создай крупную эпедемию вирусов");


            return scen;
        }

        public ScenStruct Scen_bionicall()
        {
            ScenStruct scen = new ScenStruct("С помощью программы bionicall войди на сервер www.deposit.com (R11) (ищи гугло программу)");


            return scen;
        }

        public ScenStruct Scen_avtoritet()
        {
            ScenStruct scen = new ScenStruct("Увеличь свой престиж");


            return scen;
        }

    #endregion

    #region "Структуры и логика"

        /// <summary>
        /// Структура сценария
        /// </summary>
        [Serializable]        
        public struct ScenStruct
        {
            public string Title;
            public Dictionary<string, ICQ> Chat;
            public Dictionary<string, List<GameEvenClass.IEventGame>> Script;

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
            
            /// <summary>
            /// Выполняет событие если она было запланированно 
            /// </summary>
            /// <param name="condition"></param>
            /// <param name="StringElemen"></param>
            public void EventIntroduce(Enums.ConditionEnum condition, string[] StringElemen)
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
            del:
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
            public void AddAnswer(string text, Com command, int i = 0, string t = "")
            {
                lastMessage.Answers.Add(new ICQ.Message.Answer() { CommandAnswer = command, IntArgument = i, StrArgument = t, TextAnswer = text });
            }
            /// <summary>
            /// Добавить ответ к прош. сообщению
            /// </summary>          
            public void AddAnswer(string text, Com command,  string t = "")
            {
                lastMessage.Answers.Add(new ICQ.Message.Answer() { CommandAnswer = command, IntArgument = 0, StrArgument = t, TextAnswer = text });
            }

            /// <summary>
            /// Создает Условие при котором <b >файл скачан </b>
            /// </summary>
            public void When_FileDownload(string file_name_download, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { file_name_download },
                    Condition = Enums.ConditionEnum.ФайлСкачан
                };               
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b >Сервак отключен, перезапущен</b>
            /// </summary>
            public void When_SrvDown(string url, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url },
                    Condition = Enums.ConditionEnum.СерверОтключен
                };               
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b >Доступ получен на сервере при взломе</b>
            /// </summary>
            public void When_CrackServer(string url, Server.PremissionServerEnum premission, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url, premission.ToString() },
                    Condition = Enums.ConditionEnum.ВходНаСервер
                };
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b >Вход в админ панель</b>
            /// </summary>
            public void When_AccesssAP(string url, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url },
                    Condition = Enums.ConditionEnum.АдминПанельДоступ
                };
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b>На сервере посещаемость выше </b>
            /// </summary>
            public void When_PopularSrv(string url, int i, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url, i.ToString() },
                    Condition = Enums.ConditionEnum.ПосещениеСервера
                };
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b>Изменение прав доступа на сервере</b>
            /// </summary>
            public void When_ChangeControl(string url, Server.PremissionServerEnum premissionServer, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url , premissionServer.ToString() },
                    Condition = Enums.ConditionEnum.ИзменениеПравДоступа
                };
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b>Переборщик паролей завершил работу</b>
            /// </summary>
            /// <param name="url">Сервер на котором подбирался пароль</param>
            /// <param name="login_pass">лоин и пароль <b>если пустая строка пароль не найден</b> </param>
            /// <param name="successfully">true - пароль найден </param>
            /// <param name="act">действие</param>
            public void When_BrutoforceEnd(string url, string login_pass, bool successfully, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url, login_pass , successfully.ToString() }
                };
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b >Сервер успешно запустил службу</b>
            /// </summary>
            public void When_RoleWork(string url, Enums.InstaceTypeEnum instaceType , Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url, instaceType.ToString() },
                    Condition = Enums.ConditionEnum.ЗапущенаСлужбаНаСервере
                };               
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b >Сервер уличшил свое железо</b>
            /// </summary>
            public void When_HardwareUp(string url, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url },
                    Condition = Enums.ConditionEnum.МощностьСервераУвеличина
                };
                GameCondition.Add(condition);
            }
            /// <summary>
            /// Создает Условие при котором <b >Сервер уличшил Свой софт</b>
            /// </summary>
            public void When_SoftwareUp(string url, Script.IEventGame act)
            {
                ConditionStruct condition = new ConditionStruct()
                {
                    Action = act,
                    StringElement = new string[] { url },
                    Condition = Enums.ConditionEnum.СофтОбновлен
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
                public Enums.ConditionEnum Condition;

                /// <summary>
                /// Проверяет на выполнение условия
                /// </summary>
                /// <param name="condition">Событие</param>
                /// <param name="str">текствое значения которое должно быть равно StringElement</param>
                /// <param name="i">числовое значение = IntElement</param>
                /// <returns>True - соотвует</returns>
                public bool CheckCondition(Enums.ConditionEnum condition, string[] str)
                {
                    if (condition == Condition) switch (condition)
                        {
                            case Enums.ConditionEnum.ВходНаСервер:
                            case Enums.ConditionEnum.АдминПанельДоступ:
                            case Enums.ConditionEnum.ФайлСкачан:
                                if (str[0].ToLower() == StringElement[0].ToLower()) return true;
                                break;
                            case Enums.ConditionEnum.ЗапущенаСлужбаНаСервере:
                            case Enums.ConditionEnum.ИзменениеПравДоступа:
                            case Enums.ConditionEnum.МощностьСервераУвеличина:
                                if (str[0].ToLower() == StringElement[0].ToLower() & str[1] == StringElement[1]) return true;
                                break;
                            case Enums.ConditionEnum.ПосещениеСервера:
                                if (str[0].ToLower() == StringElement[0].ToLower())
                                {
                                    var v = App.GameGlobal.FindServer(str[0]).VirtualizationServer;
                                    if (v != null) if (v.SummarPopular >= int.Parse(StringElement[1])) return true;
                                }
                                break;
                            case Enums.ConditionEnum.ПодборПароляЗавершен:
                                if (str[2] == "false")
                                {
                                    // False - процесс завершен значит выполнено условие
                                    return true;
                                }
                                else
                                {
                                    if (str[1] != "")
                                    {
                                        string s = App.GameGlobal.FindServer(str[0]).LoginAndPass;
                                        if (str[1] == s) return true;
                                    }
                                }                
                                return false;
                            default:
                                return false;
                        }
                    return false;
                }


            }
        }
    #endregion 
    }
     
}
