using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Сервер
    /// </summary>
    [Serializable]
    public sealed class Server
    {
        [NonSerialized] public DrawingHubClass DrawingHub;
        public delegate void UpdateFileSystem (string patch);
        /// <summary>
        /// событие сообщает что были изменения в файловой системе этого сервера
        /// </summary>
        [field: NonSerialized] public event UpdateFileSystem ChangeFileSys;

        private StatisticInfoClass StatisticInfo = new StatisticInfoClass();
        private TypeOSEnum PrvOS = TypeOSEnum.Unknown;
        private const int MAXPORTS = 10;                                               //масксимально число портов на сервере 
        private System.Drawing.Point _LocateTextura = new System.Drawing.Point(0, 0);  // сохраняет положение на карте 
        private List<Port> Allports;                                                   // тут все порты
        private bool PrvActSrv = true;
        private PremissionServerEnum PrvPremision= PremissionServerEnum.none;

        /// <summary>
        /// Необходим для сохранение положение координат серверов
        /// </summary>
        public System.Drawing.Point LocateTextura
        {
            get
            {
                if (DrawingHub == null) { return _LocateTextura; }
                else
                {
                    _LocateTextura = new System.Drawing.Point(DrawingHub.Top, DrawingHub.Left);
                    return _LocateTextura;
                }
            }
        }

        /// <summary>
        /// Название сервера www.www.ww
        /// </summary>
        public string NameSrv { get; set; }
        /// <summary>
        /// IP адрес сервера
        /// </summary>
        public string IP
        {
            get
            {
                byte[] a = Encoding.ASCII.GetBytes(NameSrv);
                string A1, A2, A3, A4;

                A1 = a[1].ToString();
                A2 = a[a.Length / 2].ToString();
                A3 = a[a.Length / 3].ToString();
                A4 = a[^1].ToString();
                return A1 + "." + A2 + "." + A3 + "." + A4;
            }
        }
        /// <summary>
        /// Рейтинг популярности сервера от 1 до 100 эти сервера зарабатывают больше стоят дороже
        /// </summary>
        public int PopularSRV { get; set; } = 1;
        /// <summary>
        /// Работает в текущий момент сервер или нет <i>True - да</i>
        /// </summary>
        public bool ActSrv
        {
            get => PrvActSrv; set
            {
                PrvActSrv = value;
                if (DrawingHub != null)
                {
                    if (PrvActSrv)
                    {
                        DrawingHub.Ellipse.Fill = System.Windows.Media.Brushes.GreenYellow;
                    }
                    else
                    {
                        DrawingHub.Ellipse.Fill = System.Windows.Media.Brushes.DarkRed;
                    }
                }
            }
        }
        /// <summary>
        /// Пинг до сервера
        /// </summary>
        public short Ping { get; set; }
        /// <summary>
        ///  тип сервера картинка или иконка отображаемая
        /// </summary>
        public TypesSrvEnum TypesSrv
        {
            get
            {
                if (PopularSRV >= 1 & PopularSRV <= 10)
                    return TypesSrvEnum.Ones;
                else if (PopularSRV >= 11 & PopularSRV <= 25)
                    return TypesSrvEnum.Standart;
                else if (PopularSRV >= 26 & PopularSRV <= 41)
                    return TypesSrvEnum.StandSvr;
                else if (PopularSRV >= 42 & PopularSRV <= 60)
                    return TypesSrvEnum.BigSvr;
                else if (PopularSRV >= 61 & PopularSRV <= 72)
                    return TypesSrvEnum.ServerTI;
                else if (PopularSRV >= 73 & PopularSRV <= 88)
                    return TypesSrvEnum.Standelone;
                else if (PopularSRV >= 89 & PopularSRV <= 98)
                    return TypesSrvEnum.BigData;
                else if (PopularSRV >= 99 & PopularSRV <= 100)
                    return TypesSrvEnum.Mainframe;
                else
                    return TypesSrvEnum.Ones;
            }
        }
        /// <summary>
        /// операционка
        /// </summary>
        public TypeOSEnum OS
        {
            get => PrvOS; 
            set
            {
                PrvOS = value;
                var rand = new Random();
                string[] txt = App.GameGlobal.MainWindow.OS_TXT;
                string key = "Unknown";
                string v = "Low";

                if (PrvOS == TypeOSEnum.WinSrv)
                {
                    key = "WIN system ";
                    string[] txt2 = App.GameGlobal.MainWindow.Firewall_TXT;
                    FireWall = txt2[rand.Next(0, txt2.Length)].Split(',')[1];
                }
                else if (PrvOS == TypeOSEnum.Logerhead) goto w10;
                else if (PrvOS == TypeOSEnum.Linux) key = "BSD system ";
                
                if (PopularSRV >= 40) v = "High";

                foreach (var item in txt)
                {
                    if (item.Split('=')[0] == key + v)
                    {
                        string t = item.Split('=')[1];
                        string[] tt = t.Split(',');
                        OSName = tt[rand.Next(0, tt.Length)];
                        goto w10;
                    }
                }
                OSName = "Unknown";
                PrvOS = TypeOSEnum.Unknown;
            w10:;
            }
        }
        /// <summary>
        /// Содержит название ОС  для совместимости shell кодов
        /// </summary>
        public string OSName { get; set; }
        /// <summary>
        /// Фаервол по умоланию не установлен
        /// </summary>
        public string FireWall { get; set; } = "";
        /// <summary>
        /// текущий уровень допуска
        /// </summary>
        public PremissionServerEnum Premision { get=> PrvPremision; set {
                PrvPremision = value;
                App.GameGlobal.EventIntroduce(Enums.ConditionEnum.ИзменениеПравДоступа,  NameSrv, PrvPremision.ToString());
            } }
        /// <summary>
        /// Максимальная вычислительная можность сервера 
        /// </summary>
        public int CPUP_Max { get; set; } = 10;
        /// <summary>
        /// Рабочие процессы на сервере
        /// </summary>
        public List<AppProcess> Idle;
        /// <summary>
        /// Рабочие порты на сервере
        /// </summary>
        public List<Port> Ports
        {
            get
            {
                if (Allports == null)
                {
                    Random rnd = new Random();
                    int allports = rnd.Next(3, MAXPORTS);
                    string[] txt = (Properties.Resources.Ports).Split('\n');
                    int spet = (txt.Length - 10) / MAXPORTS;
                    int t = 0;
                    Allports = new List<Port>();
                    for (int i = 0; i < allports; i++)
                    {
                        Port p = new Port();
                        int j = spet * i;
                        if (t > (j - t)) { j = t; } else { j = rnd.Next(t, (j - t)); }
                        p.Active = true;
                        p.Num = ushort.Parse(txt[j].Split('\t')[0]);
                        p.NameTitle = txt[j].Split('\t')[1].Replace('\r', '!');
                        if (p.NameTitle == "") { p.NameTitle = "Unknown"; }
                        p.Rationo = rnd.Next(1, PopularSRV + 10);
                        string[] tv = txt[j].Split('\t');
                        if (tv.Length != 3) { p.Text = txt[j].Split('\t')[1]; }
                        else
                        {
                            p.Text = txt[j].Split('\t')[2];
                        }
                        Allports.Add(p);
                    }
                }
                return Allports;
            }
            set
            {
                Allports = value;
                while (Allports.Count > MAXPORTS) { 
                    Allports.RemoveAt(0);
                }
            }
        }
        /// <summary>
        /// Файлы на сервере
        /// </summary>
        public FileServerClass FileSys { get; set; } = new FileServerClass()
        {
            Dir = new List<FileServerClass>(),
            FileСontents = new FileServerClass.ParameterClass() { TypeInformation = Enums.TypeParam.dir },
            FileName = "root",
            Size = 1,
            Rights = FileServerClass.PremisionEnum.none,
            Perfix = ""
        };
        /// <summary>
        /// Почта на этом сервере
        /// </summary>
        public List<MailInBox> Mails = new List<MailInBox>();
        /// <summary>
        /// Логин и пароль для доступа к серверу <i>Разделитель :</i>
        /// </summary>
        public string LoginAndPass = "";
        /// <summary>
        /// Указывает что вы засветились в логах этого сервера 
        ///  <b>0 - значение указывает что логи очистили или вы не были на сервере</b>
        /// </summary>
        public int LogSaver = 0;        
        /// <summary>
        /// Виртуальные сервера
        /// </summary>
        public Virtualization VirtualizationServer;
        /// <summary>
        /// Текствые файлы на сервере которые содержат информацию тут находиться <i>(имя файла : текст)</i>
        /// </summary>
        public Dictionary<string, string> FileTextInfo = new Dictionary<string, string>();

        /// <summary>
        /// Закрывает дыры в безопасности 
        /// </summary>
        public void CheckAuditSecurity() {
            Premision = PremissionServerEnum.none;

            ushort Num;
            string NameTitle;
            int Rationo;
            ushort VerA;
            string Text;

            List<Port> ls = new List<Port>();
            foreach (var port in Ports)
            {
                Num = port.Num;
                NameTitle = port.NameTitle;
                Rationo = port.Rationo;
                VerA = port.VerA;
                Text = port.Text;

                if (port.BugPort) VerA++;

                ls.Add ( new Port() { Active = true, BugPort = false, NameTitle = NameTitle, Num = Num, Rationo = Rationo, Text = Text, VerA = VerA });
            }
            Ports = ls;
        }
        /// <summary>
        /// создает логин и пароль или обновляет его 
        /// </summary>
        public void CreateLoginPass() {
            string[] login = Properties.Resources.login.Split("\r\n");
            string[] pass = Properties.Resources.pwd.Split("\r\n");
            Random rnd = new Random();
            LoginAndPass = login[rnd.Next(0, login.Length - 1)] + ":" + pass[rnd.Next(0, pass.Length - 1)];
        }
        /// <summary>
        /// Создает файл на сервере
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="nameFile"></param>
        /// <param name="comment"></param>
        /// <param name="size"></param>
        /// <param name="rights"></param>
        /// <param name="perfix"></param>
        /// <param name="systemFile"></param>
        /// <param name="createDirAuto"></param>
        public void CreateFiles(string patch, string nameFile, FileServerClass.ParameterClass comment, int size, FileServerClass.PremisionEnum rights, string perfix, bool systemFile = false, bool createDirAuto = true)
        {
            string[] p = patch.Split('/');
            FileServerClass dir = FileSys;
            FileServerClass oldDir = FileSys;

            for (int i = 1; i < p.Length; i++)
            {
                if (p[i] == "") break;
                dir = dir.Dir.Find(x => x.FileName == p[i]);
                if (dir == null) if (createDirAuto)
                    {
                        // создает каталог если его при createDirAuto = true
                        oldDir.Dir.Add(new FileServerClass()
                        {
                            FileСontents = new FileServerClass.ParameterClass() { TypeInformation = Enums.TypeParam.dir },
                            SystemFile = systemFile,
                            FileName = p[i],
                            Size = 1,
                            Rights = rights,
                            Perfix = "",
                            Dir = new List<FileServerClass>()
                        });
                        dir = oldDir.Dir[^1];
                    }
                    else
                    {
                        throw new Exception("Не найден игровой каталог " + patch);
                    }
                oldDir = dir;
            }

            if (perfix == "" & nameFile.IndexOf('.') != -1) perfix = nameFile.Split('.')[^1];           
            if (nameFile != "") dir.Dir.Add(new FileServerClass() { FileСontents = comment, FileName = nameFile, Size = size, Rights = rights, Perfix = perfix, SystemFile = systemFile });
            ChangeFileSys?.Invoke(patch);
        }
        /// <summary>
        /// Создакт файл из готового экземпляра
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="fileServer"></param>
        public void CreateFiles(string patch, FileServerClass fileServer)
        {
            string perfix = fileServer.FileName.Split('.')[^1];
            string s = fileServer.FileName.Substring(0, fileServer.FileName.Length - perfix.Length-1);
            CreateFiles(patch, s, fileServer.FileСontents, (int)fileServer.Size, fileServer.Rights, perfix, fileServer.SystemFile, false);
        }
        /// <summary>
        /// Создает файл на сервере, но название перфикса в имени файла
        /// </summary>
        /// <param name="patch">Путь где будет файл </param>
        /// <param name="nameFile">название с перфексом</param>
        /// <param name="comment"></param>
        /// <param name="size"></param>
        /// <param name="rights"></param>
        /// <param name="systemFile"></param>
        /// <param name="createDirAuto"></param>
        public void CreateFiles(string patch, string nameFile, FileServerClass.ParameterClass comment, int size, FileServerClass.PremisionEnum rights, bool systemFile = false, bool createDirAuto = true)
        {
            string perfix;
            string s;

            if (nameFile.Contains('.'))
            {
                perfix = nameFile.Split('.')[^1];
                s = nameFile.Substring(0, nameFile.Length - perfix.Length - 1);
            }
            else
            {
                perfix = "";
                s = nameFile;
            }

            CreateFiles(patch, s, comment, size, rights, perfix, systemFile, createDirAuto);
        }
        /// <summary>
        /// Создание для файлов авто генерации
        /// </summary>        
        public void CreateFiles(string patch, string nameFile, string comment, int size, FileServerClass.PremisionEnum rights,  bool systemFile = false, bool createDirAuto = true)
        {
            CreateFiles(patch, nameFile, new FileServerClass.ParameterClass()
            {
                TypeInformation = Enums.TypeParam.file,
                TextCommand = comment
            }, size, rights, systemFile, createDirAuto);
        }
        /// <summary>
        /// Создает каталог на сервере
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="dirName"></param>
        /// <param name="rights"></param>
        /// <param name="systemFile"></param>
        public void CreateDir(string patch, string dirName, FileServerClass.PremisionEnum rights, bool systemFile = false)
        {
            string[] p = patch.Split('/');
            Engine.FileServerClass dir = FileSys;
            for (int i = 1; i < p.Length; i++)
            {
                if (p[i] == "") break;
                dir = dir.Dir.Find(x => x.FileName == p[i]);
                if (dir == null) { throw new Exception("Не найден игровой каталог " + patch); }
            }
            dir.Dir.Add(new FileServerClass()
            {
                FileСontents = new FileServerClass.ParameterClass() { TypeInformation = Enums.TypeParam.dir },
                SystemFile = systemFile,
                FileName = dirName,
                Size = 1,
                Rights = rights,
                Perfix = "",
                Dir = new List<FileServerClass>()
            });
            ChangeFileSys?.Invoke(patch);           
        }
        /// <summary>
        /// Генерирует случайные файлы для сервера
        /// </summary>
        public void FileGenerator()
        {
            switch (OS)
            {               
                case TypeOSEnum.Unknown:
                    CreateFiles("/MyFile/", "txt.txt", "Файлы пользователей", 1423, FileServerClass.PremisionEnum.AdminUserGuest,  true);
                    CreateFiles("/Core/", "core", "Системные файлы", 1423, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    break;  
                case TypeOSEnum.WinSrv:
                    CreateFiles("/Windows/", "log", "Системные файлы", 123, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "adb.exe", "Системные файлы", 1513, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "comsetup", "Системные файлы", 7834, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "DPINST", "Системные файлы", 468, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "gdrv.sys", "Системные файлы", 74343, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "explorer.exe", "Системные файлы", 7349, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "ntbtlog", "Системные файлы", 457, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "RtlExUpd.dll", "Системные файлы", 6976, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "splwow64", "Системные файлы", 42, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "system.ini", "Системные файлы", 45378, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "twain_32.dll", "Системные файлы", 4792, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "WMSysPr9.prx", "Системные файлы", 768, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/", "HelpPane.exe", "Системные файлы", 667, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/System32/", "CbsApi.dll", "Системные файлы", 57, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/Speech/", ".log", "Системные файлы", 452, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/Web/", "wiatwain.ds", "Системные файлы", 654, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/IME/", "amd64_installed.exe", "Системные файлы", 78, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/INF/", "amdi2c.ini", "Системные файлы", 452, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/Windows/L2Schemas/", "Dkk.dll", "Системные файлы", 54, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    Ports.ForEach(x => CreateFiles("/Program Files/", x.NameTitle, "Необходимые файлы для работы сервера", (x.Text.Length) * 58, FileServerClass.PremisionEnum.OnlyAdmin, true));
                    CreateFiles("/User/", ".logs", "Файлы пользователей ", 356, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/User/Admin/", "configstore", "Файлы пользователей ", 6422, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/User/Admin/", "profiledata", "Файлы пользователей ", 2562, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/User/Admin/", "qttmlanguage.ini", "Файлы пользователей ", 24566, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/User/Admin/", "IntelME.ini", "Файлы пользователей ", 2543, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/User/Admin/", "lock.bit", "Файлы пользователей ", 234, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/User/User/", "profiledata", "Файлы пользователей ", 2455, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/User/User/", "configstore", "Файлы пользователей ", 2542, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/User/User/", "qttmlanguage.ini", "Файлы пользователей ", 2545, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/User/User/", "IntelME.ini", "Файлы пользователей ", 245243, FileServerClass.PremisionEnum.AdminAndUser, true);
                    break;
                case TypeOSEnum.Linux:
                    Ports.ForEach(x => CreateFiles("/bin/", x.NameTitle, "Необходимые файлы для работы сервера" + x.VerA, (x.Text.Length) * 52, FileServerClass.PremisionEnum.OnlyAdmin, true));
                    CreateDir("/", "bin", FileServerClass.PremisionEnum.AdminAndUser);
                    CreateFiles("/bin/", "ls", "Необходимые файлы для работы сервера", 521, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/bin/", "kill", "Необходимые файлы для работы сервера", 521, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/bin/", "curl", "Необходимые файлы для работы сервера", 55, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/bin/", "id", "Необходимые файлы для работы сервера", 100, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/bin/", "found", "Необходимые файлы для работы сервера", 555, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateDir("/", "boot", FileServerClass.PremisionEnum.AdminAndUser);
                    CreateFiles("/boot/", "x54", "", 8899, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateDir("/boot/", "grub", FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/boot/grub", "libattr.so ", "Ядро Линукс и загрузка", 1500, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/boot/grub", "libattr.so.1", "Ядро Линукс и загрузка", 569, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/boot/grub", "libcrypto.so", "Ядро Линукс и загрузка", 2345, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/boot/grub", "libeinfo.so", "Ядро Линукс и загрузка", 97555, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/boot/grub/apk", "lids", "Ядро Линукс и загрузка", 255, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/boot/grub/mdev", "libz.so", "Ядро Линукс и загрузка", 6432, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/etc/", "lftp.conf", "Файлы настройки некоторых программ", 5325, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/etc/", "sysctl.conf", "Файлы настройки некоторых программ", 6674, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles("/etc/", "passwd", "Файлы настройки некоторых программ", 4224, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/etc/", "profile", "Файлы настройки некоторых программ", 22, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/etc/", "rc.conf", "Файлы настройки некоторых программ", 6257, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles("/etc/zsh/", "zsh.cnf", "Файлы настройки некоторых программ", 235324, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles("/dev/", "vcs1", "Файлы устройств", 3435, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "tty49", "Файлы устройств", 24, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "loop0", "Файлы устройств", 5234, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "loop2", "Файлы устройств", 33345, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "vcsa1", "Файлы устройств", 54325, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "console", "Файлы устройств", 54676, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "urandom", "Файлы устройств", 2555, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "zero", "Файлы устройств", 2545, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/dev/", "fer", "Файлы устройств", 25435, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/var/apk/", "logs", "Файлы часто меняющие", 25435, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/var/", "fdj", "Файлы часто меняющие", 34, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/var/svn/", "pps", "Файлы часто меняющие", 55232, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/var/", ".lock", "Файлы часто меняющие", 42, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/bin/", "make", "Установленные пакеты программм", 422, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/bin/", "xxd", "Установленные пакеты программм", 2422, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/bin/", "zfgrep", "Установленные пакеты программм", 21422, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/bin/", "znew", "Установленные пакеты программм", 3422, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/bin/", "lollipop", "Установленные пакеты программм", 11422, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/lib", "ytasm", "Установленные пакеты программм", 2342, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/lib", "xset", "Установленные пакеты программм", 2342, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/local", "ffasn1dump", "Установленные пакеты программм", 3442, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/sbin", "delgroup", "Установленные пакеты программм", 3442, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/usr/sbin", "sudo_logsrvd", "Установленные пакеты программм", 3442, FileServerClass.PremisionEnum.AdminAndUser, true);
                    CreateFiles("/home/", "log", "Файлы пользователей", 42, FileServerClass.PremisionEnum.AdminAndUser, true);
                    break;
                case TypeOSEnum.Logerhead:
                    CreateFiles("/", "logs", "sys error",  1500, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/", "start", ".os", 589, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/", "pref_files_524621", ".nns", 1500, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/etc/grafics/", "gr", ".os", 58900, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/etc/grafics/", "thumbnail", ".os", 154002, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/etc/", "programm", ".os", 54200, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/", "kernel64", ".os", 89055, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/", "main", ".os", 874522, FileServerClass.PremisionEnum.OnlyAdmin, true);
                    CreateFiles("/desk/", "cvd", ".os", 88522, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/desk/desk/", "cvd", ".os", 12522, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/desk/desk/", "desk_des", ".os", 82522, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/desk/desk/", "fl", ".os", 5452, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/desk/desk/ns", "desk_des", ".os", 82522, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/desk/", "olen", ".os", 5452, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/apps/", "connects", ".bin", 8741, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles("/apps/", "mail", ".bin", 4521, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles("/apps/", "lookfile", ".bin", 4521, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles("/apps/", "gb", ".bin", 2210, FileServerClass.PremisionEnum.OnlyAdmin,  true);
                    CreateFiles("/user/Hpro4/", "set", ".bin", 411, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles("/user/Hpro4/docs", "", "", 0, FileServerClass.PremisionEnum.AdminAndUser,  true);
                    CreateFiles(FrmSoft.FrmFile.PatchEnviron.Download, "", "", 0, FileServerClass.PremisionEnum.AdminUserGuest,  true);
                    CreateFiles(FrmSoft.FrmFile.PatchEnviron.Exploit, "", "", 0, FileServerClass.PremisionEnum.AdminUserGuest,  true);
                    CreateFiles(FrmSoft.FrmFile.PatchEnviron.HDoc, "", "", 0, FileServerClass.PremisionEnum.AdminUserGuest,  true);
                    CreateFiles("/user/Hpro4/Tools/", "", "", 0, FileServerClass.PremisionEnum.AdminUserGuest, true);
                    break;
                default:
                    break;
            }


        }
        /// <summary>
        /// Перезапускает сервер
        /// </summary>
        public void Shutdown() {
            ActSrv = false;
            GameEvenClass gameEven = new GameEvenClass(App.GameGlobal.DataGM.AddMonths(1), new GameEvenClass.EventShutdown(NameSrv));           
            StatisticInfo.UnitsShutdown++;
            App.GameGlobal.AllEventGame.Add(gameEven);
            List<string> TextAnimation = new List<string>
            {
                "// Start Shutdown server ...",
                "// 0012F741 00 11 14 44 55 84 10 36|44 82 21 22 00 95 86",
                "// 0012F742 00 61 13 44 35 34 19 34|49 81 23 22 08 95 66",
                "// 0012F743 01 12 14 44 53 84 20 26|44 04 51 22 03 34 66",
                "// 0012F744 00 14 1f 44 25 34 18 86|84 76 23 22 80 35 46",
                "// 0012F745 00 12 1f 44 f5 85 70 56|44 84 23 22 24 45 16",
                ">   0012F742 0012F743 0012F744 0012F745",
                ">>  0012F742 0012F743 0012F744 0012F745",
                ">>> 0012F742 0012F743 0012F744 0012F745",
                ">>  0012F742 0012F743 0012F744 0012F745",
                "> Wait",
                "> Сервер отключен " + NameSrv,
                "= "+IP+" ="
            };
            App.GameGlobal.MainWindow.StartConsoleText(TextAnimation);
            App.GameGlobal.GamerInfo.AddExp(PopularSRV / 2);
            CheckAuditSecurity();
            AdministratorWarning();
            
        }
        /// <summary>
        ///  Дефейс сервера
        /// </summary>
        /// <param name="lvl">Уровень скрипта дефейса, если ниже то работать не будет дефейс </param>
        public void Deface(int lvl, string msg) {
            if (lvl >= StatisticInfo.UnitDefece)
            {
                GameEvenClass gameEven = new GameEvenClass(App.GameGlobal.DataGM.AddMonths(1), new GameEvenClass.RestatService(this));              
                App.GameGlobal.AllEventGame.Add(gameEven);
                StatisticInfo.UnitDefece++;
                App.GameGlobal.GamerInfo.AddExp(85);
                VirtualizationServer.DefeceString = msg;
                AdministratorWarning();
            }
        }
        /// <summary>
        /// Создает виртуальный сервер автаматически 
        /// </summary>
        public void CreateVirtualAuto()
        {
            VirtualizationServer = new Virtualization { MaxPower = PopularSRV * 45 };
            // автоматическое создание инстансев          
            var rnd = new Random();
            int need_power = (int)(VirtualizationServer.MaxPower * 0.9);
            var items = new[] {
                            new { typ=Enums.InstaceTypeEnum.BookingApi, b = rnd.Next (0,2)==0  },
                            new { typ=Enums.InstaceTypeEnum.MailApi, b = rnd.Next (0,1)==0 },
                            new { typ=Enums.InstaceTypeEnum.SerWebApi, b = rnd.Next (0,3)==0 },
                            new { typ=Enums.InstaceTypeEnum.StreamVideoApi, b = rnd.Next (0,3)==0 },
                            new { typ=Enums.InstaceTypeEnum.WebChatApi, b = rnd.Next (0,3)==0 },
                            new { typ=Enums.InstaceTypeEnum.WebForumApi, b = rnd.Next (0,1)==0 },
                            new { typ=Enums.InstaceTypeEnum.ShopApi, b = rnd.Next (0,3)==0 },
                      };

            var first = VirtualizationServer.Role_Templates(Enums.InstaceTypeEnum.TranceSrv);
            VirtualizationServer.Instance.Add(first);

            long ra = items.Where(x => x.b).LongCount();
            int pwr = (int)(need_power / ra);

            foreach (var item in items)
            {
                if (item.b == true)
                {
                    Virtualization.InstaceClass instaceClass = VirtualizationServer.Role_Templates(item.typ);
                    instaceClass.VerA = (byte)rnd.Next(1, 5);
                    instaceClass.VerB = (byte)rnd.Next(1, 25);
                    instaceClass.KVT_Ver = pwr / instaceClass.VerA;
                    instaceClass.Popular_Ver = pwr / instaceClass.VerB;
                    instaceClass.StatusInstance = Enums.StatusInstanceEnum.Working;

                    VirtualizationServer.Instance.Add(instaceClass);
                }
            }

            first.KVT_Ver = Math.Min(0, need_power - VirtualizationServer.SummarPower);
            first.StatusInstance = Enums.StatusInstanceEnum.Working;

            VirtualizationServer.Hardware = new Virtualization.HardwareClass(VirtualizationServer);
        }
        /// <summary>
        /// Создает виртуальный сервер для квестов 
        /// </summary>
        public void CreateVirtualManual()
        {
            VirtualizationServer = new Virtualization { MaxPower = PopularSRV * 45 };
            // создаем истансы не активные           
            Enums.InstaceTypeEnum[] defaultTypes = new Enums.InstaceTypeEnum[] {      
                    Enums.InstaceTypeEnum.FTP ,
                    Enums.InstaceTypeEnum.MySql ,
                    Enums.InstaceTypeEnum.WebForum ,
                    Enums.InstaceTypeEnum.BanerAD ,
                    Enums.InstaceTypeEnum.Mail
            };
                       
            foreach (var item in defaultTypes) VirtualizationServer.Instance.Add(VirtualizationServer.Role_Templates(item));
            
            VirtualizationServer.Hardware = new Virtualization.HardwareClass(VirtualizationServer);
        }
        /// <summary>
        ///  Сумирующий рейтинг всех взломов этого сервера
        /// </summary>
        /// <returns></returns>
        public int RatingSrv() => StatisticInfo.UnitsShutdown + StatisticInfo.UnitDefece;
        /// <summary>
        /// Сканирует сервер на наличие данных банка
        /// </summary>
        /// <param name="info">Текст сообщения</param>
        /// <returns>true - завершено успешно</returns>
        public bool ScanBankInfo(out string info)
        {
            if (StatisticInfo.ScannedBank == false & NameSrv != "localhost")
            {
                StatisticInfo.ScannedBank = true;
                var rnd = new Random();
                if (rnd.Next(0, 4) == 0)
                {
                    info = "[УСПЕХ] Удалось найти данные на этом сервер";
                    return true;
                }
                else
                {
                    info = "[ПРОВАЛ] Не удалось найти данные на сервере";
                    return false;
                }
            }
            else
            {
                info = "На сервере нет банковской информации";
                return false;
            }
        }
        /// <summary>
        /// администратор предупрежден об ваших действиях на этом сервере 
        /// </summary>
        public void AdministratorWarning() {

            // Проверка засветился в логах игрок
            if (LogSaver != 0)
            {
                App.GameGlobal.Msg("", "в логах", FrmSoft.FrmError.InformEnum.Критическая_ошибка);
            }

            //предупреждение и выписка штрафа если юзер был обнаружен
        }      
               

        #region "Структуры и перечисления"
        [Serializable]
        public struct Port
        {     
            /// <summary>
            /// номер порта 
            /// </summary>
            public ushort Num;
            /// <summary>
            ///  Имя порта
            /// </summary>
            public string NameTitle;
            /// <summary>
            ///  рейтин безопасности
            /// </summary>
            public int Rationo;
            /// <summary>
            /// Версия программы
            /// </summary>
            public ushort VerA;
            /// <summary>
            ///  Коментарий к порту 
            /// </summary>
            public string Text;
            /// <summary>
            /// Работает порт или нет
            /// </summary>
            public bool Active;
            /// <summary>
            /// Этот порт был взломан
            /// </summary>
            public bool BugPort; 

            public Port(ushort num, string nameTitle, int rationo, ushort verA, string text, bool act)
            {
                Num = num;
                NameTitle = nameTitle;
                Rationo = rationo;
                VerA = verA;
                Text = text;
                Active = act;
                BugPort = false;
            }
        }
        [Serializable]
        public struct AppProcess
        {
            /// <summary>
            /// Название программы
            /// </summary>
            public string NameProg;
            /// <summary>
            /// ид программы
            /// </summary>
            public ushort ID;
        }

        public enum PremissionServerEnum
        {
            /// <summary>
            /// Нет доступа
            /// </summary>
            none,
            FullControl,
            UserControl,
            GuestControl,
            Zombies
        }

        public enum TypeOSEnum
        {
            Unknown,
            WinSrv,
            Linux,
            Logerhead
        }

        public enum TypesSrvEnum
        {
            Ones = 0,
            Standart = 1,
            StandSvr = 2,
            BigSvr = 3,
            ServerTI = 4,
            Standelone = 5,
            BigData = 6,
            Mainframe = 7
        }
        #endregion

        /// <summary>
        /// Статистика этого сервера 
        /// </summary>
        [Serializable]
        private sealed class StatisticInfoClass
        {

            /// <summary>
            /// Ранее сканнер просканировал на наличие банковских счетов
            /// </summary>
            public bool ScannedBank = false;
            /// <summary>
            /// Число событий отключений
            /// </summary>
            public int UnitsShutdown=0;
            /// <summary>
            /// Число дефейсов на сервере
            /// </summary>
            public int UnitDefece=0;
                       
        }
    }
}
