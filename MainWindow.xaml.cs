using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PH4_WPF.FrmSoft;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PH4_WPF.Engine;
using static PH4_WPF.Engine.BankClass;

namespace PH4_WPF
{    
    public partial class MainWindow : Window
    {
        #region "Для файла config"
        private bool PrvCheatCode =false ;
        private bool PrvDebugMode = false;
        private bool PrvSoundDisable = false;

        /// <summary>
        /// Использовать режим отладки
        /// </summary>
        public bool DebugMode
        {
            get => PrvDebugMode;
            set
            {
                PrvCheatCode = value;
                SaveConfig("DebugMode", PrvDebugMode);
            }
        }
        /// <summary>
        /// Использовать чит коды
        /// </summary>
        public bool CheatCode
        {
            get => PrvCheatCode; 
            set
            {
                PrvCheatCode = value;
                SaveConfig("CheatCode", PrvCheatCode);
            }
        }
        /// <summary>
        /// Отключить звук
        /// </summary>
        public bool SoundDisable
        {
            get => PrvSoundDisable;
            set
            {
                PrvCheatCode = value;
                SaveConfig("SoundDisable", PrvSoundDisable);
            }
        }

        #endregion

        private readonly string FileConfig = System.AppDomain.CurrentDomain.BaseDirectory + @"Save\config.cnf";
        private readonly BitmapImage[,] ImageArr = new BitmapImage[8, 6];        
        private readonly System.Windows.Threading.DispatcherTimer GameTimer =new System.Windows.Threading.DispatcherTimer();
        private readonly System.Windows.Threading.DispatcherTimer AnimationTimer = new System.Windows.Threading.DispatcherTimer();
        private sbyte AnmIndex = 0;
        private float speedCanvas = 1;

        private List<string> LogGame = new List<string>();

       

        //Данные из файлов
        private string[] OS_txt;
        private string[] Firewall_txt;
        public string[] OS_TXT
        {
            get
            {
                if (OS_txt==null) OS_txt = System.IO.File.ReadAllLines(App.PatchAB + "OS.txt", System.Text.Encoding.Default);
                return OS_txt;
            }
        }
        public string[] Firewall_TXT
        {
            get
            {
                if (Firewall_txt==null) Firewall_txt = System.IO.File.ReadAllLines(App.PatchAB + "Firewall.txt", System.Text.Encoding.Default);
                return Firewall_txt;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            G_BackPanel.Visibility = Visibility.Hidden;
            SpeedPanel.Visibility = Visibility.Hidden;
            LentaNews.Visibility = Visibility.Hidden;
            Grid_Menu.Visibility = Visibility.Hidden;
            DeskCmd.Visibility = Visibility.Hidden;


        }


        private void test(object sender, RoutedEventArgs e)=> System.Windows.Application.Current.Shutdown();

        private void LoadValue() {
            try            {           
            //  Ones
            ImageArr[0, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Ones.png"));
            ImageArr[0, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Ones_1.png"));
            ImageArr[0, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Ones_2.png"));
            ImageArr[0, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Ones_3.png"));
            ImageArr[0, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Ones_4.png"));
            ImageArr[0, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Ones_5.png"));
            //  Standart
            ImageArr[1, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standart.png"));
            ImageArr[1, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standart_1.png"));
            ImageArr[1, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standart_2.png"));
            ImageArr[1, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standart_3.png"));
            ImageArr[1, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standart_4.png"));
            ImageArr[1, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standart_5.png"));
            //  StandSvr
            ImageArr[2, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\StandSvr.png"));
            ImageArr[2, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\StandSvr_1.png"));
            ImageArr[2, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\StandSvr_2.png"));
            ImageArr[2, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\StandSvr_3.png"));
            ImageArr[2, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\StandSvr_4.png"));
            ImageArr[2, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\StandSvr_5.png"));
            //  BigSvr
            ImageArr[3, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigSvr.png"));
            ImageArr[3, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigSvr_1.png"));
            ImageArr[3, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigSvr_2.png"));
            ImageArr[3, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigSvr_3.png"));
            ImageArr[3, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigSvr_4.png"));
            ImageArr[3, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigSvr_5.png"));
            // ServerTI
            ImageArr[4, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\ServerTI.png"));
            ImageArr[4, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\ServerTI_1.png"));
            ImageArr[4, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\ServerTI_2.png"));
            ImageArr[4, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\ServerTI_3.png"));
            ImageArr[4, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\ServerTI_4.png"));
            ImageArr[4, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\ServerTI_5.png"));
            // Standelone
            ImageArr[5, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standelone.png"));
            ImageArr[5, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standelone_1.png"));
            ImageArr[5, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standelone_2.png"));
            ImageArr[5, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standelone_3.png"));
            ImageArr[5, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standelone_4.png"));
            ImageArr[5, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Standelone_5.png"));
            // BigData
            ImageArr[6, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigData.png"));
            ImageArr[6, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigData_1.png"));
            ImageArr[6, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigData_2.png"));
            ImageArr[6, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigData_3.png"));
            ImageArr[6, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigData_4.png"));
            ImageArr[6, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\BigData_5.png"));
            // Mainframe
            ImageArr[7, 0] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Mainframe.png"));
            ImageArr[7, 1] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Mainframe_1.png"));
            ImageArr[7, 2] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Mainframe_2.png"));
            ImageArr[7, 3] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Mainframe_3.png"));
            ImageArr[7, 4] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Mainframe_4.png"));
            ImageArr[7, 5] = new BitmapImage(new Uri(App.PatchAB + @"Desktop\Srv\Mainframe_5.png"));
            //  ----------------
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message );
            }
        }

        private void AnimationTick(object sender, EventArgs e) {
            if (AnmIndex == 6) AnmIndex = 0;
            foreach (var item in App.GameGlobal.Servers)
            {
                 item.DrawingHub .TexturaSrv .Source = ImageArr[((int)item.TypesSrv), AnmIndex];
            }          

            AnmIndex++;        
        }

        private void LoadConfig()
        {
            string[] txt = System.IO.File.ReadAllLines(FileConfig, System.Text.Encoding.Default);

            foreach (var item in txt)
            {
                switch (item.Split('=')[0])
                {
                    case "CheatCode":
                        PrvCheatCode = Boolean.Parse(item.Split('=')[1]);
                        break;
                    case "DebugMode":
                        PrvDebugMode = Boolean.Parse(item.Split('=')[1]);
                        break;
                    case "SoundDisable":
                        PrvSoundDisable = Boolean.Parse(item.Split('=')[1]);
                        break;
                    default:
                        break;
                }
            }

        }


        private void Загруженно(object sender, RoutedEventArgs e)
        {
            App.GameGlobal.MainWindow = this;
            LoadValue();                        // Загрузка картинок из файлов


            LoadGame(null, null);

           

            //Настройка таймера анимации
            AnimationTimer.Tick += new EventHandler(AnimationTick);
            AnimationTimer.Interval = TimeSpan.FromMilliseconds(500);
            AnimationTimer.Start();

            //Настройка таймера игрового времени
            GameTimer.Tick += new EventHandler(ИгроваяЛогикаВремeни);
            GameTimer.Interval = TimeSpan.FromMilliseconds(1000);
            GameTimer.Start();

            //Маштабирование UI
            ButtonOption.Margin = new Thickness(DownPanel.Margin.Left + ButtonOption.Height, 0, 0, 0);


        }

        private void ИгроваяЛогикаВремeни(object sender, EventArgs e) {
            // Накидываем дни 
            switch (App.GameGlobal.GameSpeed)
            {           
                case Game.GameSpeedEnum.Speed1X:
                    App.GameGlobal.DataGM = App.GameGlobal.DataGM.AddDays(1);
                    break;
                case Game.GameSpeedEnum.Speed2X:
                    App.GameGlobal.DataGM= App.GameGlobal.DataGM.AddDays(2);
                    break;
                case Game.GameSpeedEnum.Speed4X:
                    App.GameGlobal.DataGM = App.GameGlobal.DataGM.AddDays(4);
                    break;
                case Game.GameSpeedEnum.Pause:
                default:
                    break;
            }
            DateGameIndicator.Content = App.GameGlobal.DataGM.ToString("dd.MM.yyyy");
            // Логика обновления статуса серверов

            // Игровые события в игре 
            var evnt = App.GameGlobal.AllEventGame.FindAll(x => x.DataStart < App.GameGlobal.DataGM);
            foreach (var item in evnt)
            {
                if (item.GameEvent == null) {
                    LogGame.Add("Случилось не запланированное событие: " + item.DataStart.ToString());
                    App.GameGlobal.AllEventGame.Remove(item);
                    continue;
                }
                LogGame.Add("Случилось игровое событие: " + item.DataStart .ToString () +" - " + item.GameEvent.NameEvent );
                item.GameEvent.Run();
                App.GameGlobal.AllEventGame.Remove(item);
            }
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            LoadConfig();
            App.GameGlobal.CreativeMap();                               // Создает карту
            App.GameGlobal.Servers.ForEach(x => x.FileGenerator());    // Создаем файлы
            App.GameGlobal.News.News.Add(new NewsClass.NewsСlass( 
            NewsClass.NewsСlass.TopicEnum.НовостиКасательноИгрока , "Это лента новостей, тут вы можете читать актуальные новости. Нажмите на новость чтобы пометить как прочитанная новость\n\n (Вы можете удалить новости двойным нажатием мышки.)","Подсказка") );
            App.GameGlobal.GameScen.ActiveScen = App.GameGlobal.GameScen.Scen_start();
            // Примерные счета
            App.GameGlobal.Bank.Accounts.Add(new BankAccount() { Login = "", Money = 100, Pass = "", Rs = "003", TypeMoney = BankAccount.TypeMoneyEnum.Dollar });
            App.GameGlobal.Bank.Accounts.Add(new BankAccount() { Login = "", Money = 100, Pass = "", Rs = "0022", TypeMoney = BankAccount.TypeMoneyEnum.Dollar });
            App.GameGlobal.Bank.Accounts.Add(new BankAccount() { Login = "", Money = 100, Pass = "", Rs = "0033", TypeMoney = BankAccount.TypeMoneyEnum.Karbovantsy });
            App.GameGlobal.Instructions_V(); //создадим уязвимости в начале
            // Настроки для тествого сервера
            Server srv = App.GameGlobal.FindServer("www.test.ru");
            srv.OS = Server.TypeOSEnum.WinSrv;
            srv.OSName = "win 95";
            // Создает доступный узел для скачки шела бесплатно 
            App.GameGlobal.OpenUrl.Add(@"localhost.cloud/win95_shell", new FileServerClass()
            {
                FileName = "Win 95",
                Perfix = ".shell",
                Rights = FileServerClass.PremisionEnum.AdminUserGuest,
                Size = 5000,
                FileСontents = new FileServerClass.ParameterClass() { TypeInformation = FileServerClass.ParameterClass.TypeParam.shell }
            });

        }

        /// <summary>
        /// Сохраняет настройки конфигурации в игре
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение для сохранения</param>
        public void SaveConfig(string key, object value)
        {
            string[] txt = System.IO.File.ReadAllLines(FileConfig, System.Text.Encoding.Default);

            for (int i = 0; i < txt.Length; i++)
            {
                if (key == txt[i].Split('=')[0])
                {
                    txt[i] = key + "" + value.ToString();
                    goto end;
                }
            }
            Array.Resize(ref txt, txt.Length + 1);
            txt[^1] = key + "" + value.ToString();
        end:;
            System.IO.File.WriteAllLines(FileConfig, txt);
        }

        /// <summary>
        /// обновляет список программ ниже
        /// </summary>
        public void Refreh_AppDeck() {
            PortScanerIcon1.Visibility = Visibility.Hidden;

            foreach (var item in Engine.FileServerClass.GetInfoFiles("/apps/", App.GameGlobal.MyServer))
            {
                switch (Engine.FileServerClass.PatchToFileName (item.FileName.ToLower ()) )
                {
                    case "portscaner":
                        PortScanerIcon1.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }

        }

        Server SelectedServer;
        public void OpenBackPanel(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedServer != null) {
                SelectedServer.DrawingHub.Ellipse.Fill = Brushes.GreenYellow; 
                SelectedServer.DrawingHub.Ellipse.Stroke = Brushes.GreenYellow;
            }
            G_BackPanel.Visibility = Visibility.Visible;
            L_SrvName.Content = sender;
            SelectedServer = App.GameGlobal.Servers.Find(x => x.NameSrv == L_SrvName.Content.ToString ());
            SelectedServer.DrawingHub.Ellipse.Fill = Brushes.DodgerBlue ; //GreenYellow
            SelectedServer.DrawingHub.Ellipse.Stroke  = Brushes.Blue ;
            LB_FindEl.Visibility = Visibility.Hidden;
            L_Status.Background = SelectedServer.ActSrv ? Brushes.LawnGreen : Brushes.OrangeRed;
            if (SelectedServer.ActSrv) {
                L_Status.Content = "Активный";
            } else if(SelectedServer.ActSrv==false ) {
                L_Status.Content = "Не работает";
            }// сюда добавить еще основания не работы сервера
            var bc = new BrushConverter();
            switch (SelectedServer.Premision)
            {
                case Server.PremissionServerEnum.none:
                    L_StatusLine.Text = "У вас нет доступа к этому серверу, сервер не был взломан.";
                    L_StatusLine.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#FF060505"); 
                    break;
                case Server.PremissionServerEnum.FullControl:
                    L_StatusLine.Text = "У вас есть полный доступ к серверу. ";
                    L_StatusLine.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#FFEE4949");
                    break;
                case Server.PremissionServerEnum.UserControl:
                    L_StatusLine.Text = "У вас есть ограниченный доступ к серверу, но вы можете оказывать влияние на работу.";
                    L_StatusLine.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#FFEFF476");
                    break;
                case Server.PremissionServerEnum.GuestControl:
                    L_StatusLine.Text = "У вас есть ограниченный доступ к информации и к паролям этого сервера.";
                    L_StatusLine.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#FF17BDA2");
                    break;
                case Server.PremissionServerEnum.Zombies:
                    L_StatusLine.Text = "Сервер заражен вирусом и может управляться вами, в зависимость от типа вируса. ";
                    L_StatusLine.Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#FF7CFC00");
                    break;
                default:
                    break;
            }
            PopularBar.Value = SelectedServer.PopularSRV;           
            ShutdownServer.Visibility = Visibility.Hidden;

            if (App.GameGlobal.GamerInfo.DefecerLvl > 0) ShutdownServer.Visibility = Visibility.Visible ;
        }

        public void SaveGame(object sender, RoutedEventArgs e)
        {

            LoadConfig();
            string sFile = System.AppDomain.CurrentDomain.BaseDirectory + @"Save\1.sav";
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(sFile, FileMode.OpenOrCreate))
                formatter.Serialize(fs, App.GameGlobal);

            
        }

        public void LoadGame(object sender, RoutedEventArgs e)
        {
            LoadConfig();
            string sFile = System.AppDomain.CurrentDomain.BaseDirectory + @"Save\1.sav";
            if (System.IO.File.Exists(sFile))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fs = new FileStream(sFile, FileMode.OpenOrCreate)) 
                    App.GameGlobal = formatter.Deserialize(fs) as Game;

                // Основная форма
                App.GameGlobal.MainWindow = this;
                // Обновляем Графические хабы
                App.GameGlobal. Servers.ForEach(x => x.DrawingHub = new DrawingHubClass(x, x.LocateTextura.X, x.LocateTextura.Y));               
                // Тепрь рисуем маршруты
                foreach (var item in App.GameGlobal.Routers)
                {
                    MyCanvas.Children.Add(item.Line);
                    Canvas.SetZIndex(item.Line, 0);
                }
                // проверка чата
                if (App.GameGlobal.GameChat != null) App.GameGlobal.GameChat.InLoadChat();
                // список программ новый
                App.GameGlobal.ActiveApp = new Dictionary<string, Window>();
                // Обнова иконок рабочего стола
                Refreh_AppDeck();
                
            }
           
        }

        private void OpenNews()
        {
            var bc = new BrushConverter();
            ListNewsLog.Items.Clear();
            var t = App.GameGlobal.News.News.Count;

            for (int i = 0; i < App.GameGlobal.News.News.Count; i++)
            {
                System.Windows.Media.Brush brush = (System.Windows.Media.Brush)bc.ConvertFrom("#FF4B566A");
                if (App.GameGlobal.News.News[i].ReadNews == false) brush = (System.Windows.Media.Brush)bc.ConvertFrom("#FF66799C");

                Grid grid = new Grid()
                {
                    Height = 172,
                    Margin = new Thickness(0, 10, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 625,
                    Background = brush
                };
                TextBlock textBlock = new TextBlock()
                {
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Margin = new Thickness(10, 31, 0, 0),
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 141,
                    Foreground = System.Windows.Media.Brushes.LightGoldenrodYellow ,
                    Text = App.GameGlobal.News.News[i].TextNews,
                    Width = 609,
                    FontFamily = new System.Windows.Media.FontFamily("Arial")
                };

                Label label = new Label()
                {
                    Content = App.GameGlobal.News.News[i].Date,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = System.Windows.Media.Brushes.White  ,
                    Width = 120,
                    FontWeight = FontWeights.Bold
                };

                Label labelTitle = new Label()
                {
                    Content = App.GameGlobal.News.News[i].Title,
                    HorizontalAlignment = HorizontalAlignment.Center ,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(50, 0, 0,0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = System.Windows.Media.Brushes.White,
                    Width = 540,
                    FontWeight = FontWeights.Bold
                };

                brush = App.GameGlobal.News.News[i].Topic switch
                {
                    NewsClass.NewsСlass.TopicEnum.Общие_Новости => Brushes.BlueViolet,
                    NewsClass.NewsСlass.TopicEnum.НовостиКасательноИгрока => Brushes.YellowGreen,
                    NewsClass.NewsСlass.TopicEnum.Найдены_Баги => Brushes.IndianRed,
                    NewsClass.NewsСlass.TopicEnum.Важное => Brushes.Red,
                    NewsClass.NewsСlass.TopicEnum.Новости_Шлак => Brushes.Blue,
                    NewsClass.NewsСlass.TopicEnum.Разное => Brushes.Firebrick,
                    _ => (System.Windows.Media.Brush)bc.ConvertFrom("#FF373131"),
                };
                Border border = new Border()
                {
                    BorderBrush = brush,
                    BorderThickness = new Thickness(2)
                };

                grid.Children.Add(label);
                grid.Children.Add(border);
                grid.Children.Add(labelTitle);
                grid.Children.Add(textBlock);

                ListNewsLog.Items.Add(grid);
            }
        }

        private void OpenLogs() {
            ListNewsLog.Items.Clear();
            for (int i = App.GameGlobal.News.Logs.Count - 1; i >= 0; i--)
            {
                var bc = new BrushConverter();
                Grid grid = new Grid()
                {
                    Height = 66,
                    Margin = new Thickness(0, 10, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 625
                };

                TextBlock textBlock = new TextBlock()
                {
                    
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Margin = new Thickness(74, 0, 0, 0),
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 141,
                    Foreground = (System.Windows.Media.Brush)bc.ConvertFrom("#FFDFDFCF"),
                    Text = App.GameGlobal.News.Logs[i].Text[1..],
                    Width = 555,
                    FontFamily = new System.Windows.Media.FontFamily("Arial")
                };
                Label label = new Label()
                {
                    Content = App.GameGlobal.News.Logs[i].Text.Substring (0,1),
                    RenderTransformOrigin = new System.Windows.Point(0.5, 0.5),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(9, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = System.Windows.Media.Brushes.White,
                    Width = 60,
                    Height = 53,
                    FontSize = 48,
                    Padding = new Thickness(0, 0, 0, 0),
                    FontWeight = new FontWeight()
                };
                grid.Children.Add(label);
                grid.Children.Add(textBlock);

                ListNewsLog.Items.Add(grid);
            }
        }

        /// <summary>
        /// Kонцетрирует внимание на сервере отображает на экране его 
        /// </summary>
        /// <param name="srv"></param>
        private void ConcetraneVisibleServer(string srv) {
            var s = App.GameGlobal.Servers.Find(x => x.NameSrv.ToLower() == T_Search.Text.ToLower());
           
            int y = s.DrawingHub.Top - (int)(this.Height /2 );
            int p = s.DrawingHub.Left - (int)(this.Width / 2);

            App.GameGlobal.Servers.ForEach(x => x.DrawingHub.Left -= p);
            App.GameGlobal.Servers.ForEach(x => x.DrawingHub.Top -= y);
            App.GameGlobal.Routers.ForEach(x => x.Left -= p);
            App.GameGlobal.Routers.ForEach(x => x.Top -= y);

            OpenBackPanel(srv, null);
        }

        #region "Панель скорости управление"

        private void Курсорнад4х(object sender, MouseEventArgs e)=> Img4x.Source = App.UriResImage("Content/Desktop/bPanel/spPanel4X.png");
        private void КурсорНад4x(object sender, MouseEventArgs e) => Img4x.Source = App.UriResImage("Content/Desktop/bPanel/spPanel4Xsel.png");
        private void КурсорНад2x(object sender, MouseEventArgs e) => Img2x.Source = App.UriResImage("Content/Desktop/bPanel/spPanel2Xsel.png"); 
        private void КурсорУшел2x(object sender, MouseEventArgs e)=> Img2x.Source = App.UriResImage("Content/Desktop/bPanel/spPanel2X.png");
        private void КурсорНадRec1x(object sender, MouseEventArgs e) => Img1x.Source = App.UriResImage("Content/Desktop/bPanel/spPanel1Xsel.png");
        private void КурсорУшелRec1x(object sender, MouseEventArgs e) => Img1x.Source = App.UriResImage("Content/Desktop/bPanel/spPanel1X.png");
        private void КурсорНадRecPause(object sender, MouseEventArgs e) => PauseImg.Source = App.UriResImage("Content/Desktop/bPanel/spPaneliisel.png");
        private void КурсорУшелPauseImg(object sender, MouseEventArgs e) =>PauseImg.Source = App.UriResImage("Content/Desktop/bPanel/spPanelii.png");
        private void Клик4x(object sender, MouseButtonEventArgs e) {            
            SpeedPanel.Visibility = Visibility.Hidden;
            App.GameGlobal.GameSpeed = Game.GameSpeedEnum.Speed4X;
        }
        private void Клик2x(object sender, MouseButtonEventArgs e)
        {            
            SpeedPanel.Visibility = Visibility.Hidden;
            App.GameGlobal.GameSpeed = Game.GameSpeedEnum.Speed2X;
        }
        private void Клики1x(object sender, MouseButtonEventArgs e)
        {            
            SpeedPanel.Visibility = Visibility.Hidden;
            App.GameGlobal.GameSpeed = Game.GameSpeedEnum.Speed1X;
        }
        private void КликПауза(object sender, MouseButtonEventArgs e)
        {           
            SpeedPanel.Visibility = Visibility.Hidden;
            App.GameGlobal.GameSpeed = Game.GameSpeedEnum.Pause ;
        }
        private void КликПоSpeedPanel(object sender, MouseButtonEventArgs e)
        {
            if (SpeedPanel.Visibility == Visibility.Hidden) { SpeedPanel.Visibility = Visibility.Visible; }
            else { SpeedPanel.Visibility = Visibility.Hidden; }
        }


        #endregion

        private void КлавишиУправление(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) { 
                App.GameGlobal.Servers.ForEach(x => x.DrawingHub.Left += (int)speedCanvas);
                App.GameGlobal.Routers.ForEach(x => x.Left += (int)speedCanvas);
            }
            else if (e.Key == Key.Right) { 
                App.GameGlobal.Servers.ForEach(x => x.DrawingHub.Left -= (int)speedCanvas);
                App.GameGlobal.Routers.ForEach(x => x.Left -= (int)speedCanvas);
            }
            else if (e.Key == Key.Up) { 
                App.GameGlobal.Servers.ForEach(x => x.DrawingHub.Top += (int)speedCanvas);
                App.GameGlobal.Routers.ForEach(x => x.Top += (int)speedCanvas);
            }
            else if (e.Key == Key.Down) { App.GameGlobal.Servers.ForEach(x => x.DrawingHub.Top -= (int)speedCanvas);
                App.GameGlobal.Routers.ForEach(x => x.Top -= (int)speedCanvas);
            }            
            speedCanvas += 0.6f;
        }

        private void ОтпуститьКнопку(object sender, KeyEventArgs e) => speedCanvas = 1;

        private void КликПоПочте(object sender, MouseButtonEventArgs e)
        {
            Mail mail = new Mail();
            mail.ShowForm();
        }

        private void Открыть_консоль(object sender, RoutedEventArgs e)
        {
            FrmSoft.Cmd cmd = new Cmd();
            cmd.ShowForm();
        }

        private void ОткрытьФайло(object sender, RoutedEventArgs e)
        {
            FrmSoft.FrmFile frm = new FrmFile();
            frm.ShowForm();
        }

        private void ОткрытьСканер(object sender, RoutedEventArgs e)
        {
            FrmSoft.PortScaner portScaner = new PortScaner();
            portScaner.Search.Text = L_SrvName.Content.ToString();
            portScaner .ShowForm();
        }             

        private void СворачиваниеОкна(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized) {
                App.GameGlobal.ActiveApp.Select(x=> x.Value.WindowState = WindowState.Minimized );
            
            }
        }       

        private void ОткрытьЛоги(object sender, MouseButtonEventArgs e)
        {
            RecL.Visibility = Visibility.Hidden;
            RecR.Visibility = Visibility.Visible;
            OpenLogs();
        }

        private void ВыборНовости(object sender, MouseButtonEventArgs e)
        {
            RecL.Visibility = Visibility.Visible;
            RecR.Visibility = Visibility.Hidden;
            OpenNews();
        }

        private void ЗакрытьЛенту(object sender, RoutedEventArgs e) => LentaNews.Visibility = Visibility.Hidden;

        private void ДвойнойПоНовости(object sender, MouseButtonEventArgs e)
        {
            int i = ListNewsLog.SelectedIndex;
            if (i == -1) return;
            if (RecL.Visibility == Visibility.Visible)
            {
                App.GameGlobal.News.News.RemoveAt(i);
                OpenNews();
            }           
        }

        private void ВыборЭлементаНовостей(object sender, SelectionChangedEventArgs e)
        {
            int i = ListNewsLog.SelectedIndex;
            if (i == -1) return;
            if (RecL.Visibility == Visibility.Visible)
            {
                App.GameGlobal.News.News[i].ReadNews = true;
                Grid grid = (Grid)ListNewsLog.Items[i];
                var bc = new BrushConverter();
                grid.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF4B566A");

                //проверка все новости прочитанны
                App.GameGlobal.MainWindow.NewsIndicator.Source = new BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\news.png"));
                foreach (var item in App.GameGlobal.News.News)
                {
                    if (item.ReadNews==false ) {
                        App.GameGlobal.MainWindow.NewsIndicator.Source = new BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\sel news.png"));
                        break;
                    }
                }             
            }
        }

        private void ОткрытьНовостиПанель(object sender, MouseButtonEventArgs e)
        {
            LentaNews.Visibility = Visibility.Visible ;
            RecL.Visibility = Visibility.Visible;
            RecR.Visibility = Visibility.Hidden;
            OpenNews();
        }

        private void ОткрытьБраузер(object sender, RoutedEventArgs e)
        {
            Browser.FrmBrowser frm = new Browser.FrmBrowser();
            frm.ShowForm();
        }

        private void ОткрытьQMess(object sender, RoutedEventArgs e)
        {
            var t = App.GameGlobal.GameChat;
            if (App.GameGlobal.GameChat != null) {                
                App.GameGlobal.GameChat.OpenWin();             
            }
        }

        private void ПоисковаяСтрока(object sender, TextChangedEventArgs e)
        {
            if (T_Search .Text.Length >= 2){
                G_BackPanel.Visibility = Visibility.Visible;
                LB_FindEl.Visibility = Visibility.Visible;
                LB_FindEl.Items.Clear();
              var x= App.GameGlobal.Servers.FindAll(x => x.NameSrv.Contains(T_Search.Text));
                for (int i = 0; i < Math.Min (x.Count ,10); i++)
                {
                    LB_FindEl.Items.Add(x[i].NameSrv );
                }

            }
        }      

        private void ВыходИзПоиска(object sender, MouseEventArgs e)
        {
            if (LB_FindEl.Visibility == Visibility.Visible) G_BackPanel.Visibility = Visibility.Hidden;
        }

        private void ЗакрытьПанель(object sender, RoutedEventArgs e)
        {
            G_BackPanel.Visibility = Visibility.Hidden;
            SelectedServer.DrawingHub.Ellipse.Fill = System.Windows.Media.Brushes.GreenYellow;
            SelectedServer.DrawingHub.Ellipse.Stroke = System.Windows.Media.Brushes.GreenYellow;
            SelectedServer = null;
        }

        private void ПоискЭлементаНачало(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) НачатьПоискСервера(null, null);

        }

        private void ВыделенниеЭлементаПоиска(object sender, SelectionChangedEventArgs e)
        {
            if (LB_FindEl.SelectedIndex != -1)
            {
                T_Search.Text = LB_FindEl.SelectedItem.ToString();
                ConcetraneVisibleServer(T_Search.Text);
            }
        }

        private void НачатьПоискСервера(object sender, RoutedEventArgs e)
        {
            var s = App.GameGlobal.Servers.Find(x => x.NameSrv.ToLower() == T_Search.Text.ToLower());
            if (s == null)
            {
                T_Search.Background = System.Windows.Media.Brushes.OrangeRed;
            }
            else
            {
                var bc = new BrushConverter();
                T_Search.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF4B566A");
                ConcetraneVisibleServer(T_Search.Text);
            }
        }

        private void MenuOpen(object sender, RoutedEventArgs e)=> Grid_Menu.Visibility = Visibility.Visible;
        private void ЗакрытьМенюИгры(object sender, RoutedEventArgs e) => Grid_Menu.Visibility = Visibility.Hidden;

        private void ОткрытьИД(object sender, RoutedEventArgs e)
        {
            FrmSoft.FrmIdUser frm = new FrmIdUser();
            frm.ShowForm();
        }

        bool MouseKeyCliker = false;
        Point OldPossCursor = new Point(0, 0);
        private void ЗахватПеремещение(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                OldPossCursor = Mouse.GetPosition(this);
                MouseKeyCliker = true;
            }
        }

        private void КупсорОтпущен(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released) MouseKeyCliker=false;
        }

        private void ПеремещениеКурсор(object sender, MouseEventArgs e)
        {
            if (MouseKeyCliker)
            {
                Point p = Mouse.GetPosition(this);
                OldPossCursor = new Point(OldPossCursor.X - p.X, OldPossCursor.Y - p.Y);
                App.GameGlobal.Servers.ForEach(x => { x.DrawingHub.Left -= (int)OldPossCursor.X; x.DrawingHub.Top -= (int)OldPossCursor.Y; });
                App.GameGlobal.Routers.ForEach(x => { x.Left -= (int)OldPossCursor.X; x.Top -= (int)OldPossCursor.Y; });
                OldPossCursor = Mouse.GetPosition(this);
            }
        }

        private void Перезапустить_сервер(object sender, RoutedEventArgs e)
        {
            FrmSoft.FrmError msg = new FrmSoft.FrmError("Действия", "Вы хотите перезапустить сервер? Это привлечёт внимание, уязвимости будут обновлены, вы получите опыт.  ", delegate {
               
            });
            msg.Show();
            msg.Activate();
            msg.Topmost = true;
        }
    }

    
}
