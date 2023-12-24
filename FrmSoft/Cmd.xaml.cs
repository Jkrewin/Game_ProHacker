using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EventEnum= PH4_WPF.Enums.ConditionEnum;
using System.Xml.Serialization;

namespace PH4_WPF.FrmSoft
{
   
    public partial class Cmd : Window
    {
        readonly List<string> FF00_ls = new List<string>(); // Ускоряет доступ к списку файлов Продвинутый уровень  

        private string Pwd = "/";                                // Начальный каталог
        private Engine.Server GameSrv;                           // Куда на какой сервер подключена консоль 
        private readonly Button[] BBSelectButton;                         // Кнопки которые нужны для меню
        private readonly Brush StTextColor = Brushes.LawnGreen;  // Цвет текста по умолчанию
        private const short MAX_STRING = 110;                    // Максимальное количество символов в строке 
        private const short MAX_LINES = 37;                      // Максимальное количество строк в консоли 
        private readonly List<string> StoresList = new List<string>();               // История ввода текста 
        private List<ConsoleText> ScriptConsole = new List<ConsoleText>();           // Скрипт для консоли
        private readonly Timer ConsoleTimer = new Timer(100);                        // Таймер для ввода текста
        private readonly System.Windows.Threading.DispatcherTimer CursorTimer;       // Мигающий курсор
        private Point PointOos = new Point(0, 0);                                    // Управление перетаскиванием
        private readonly List<XmlInfoCommand> XmlInfoCommandList;                    // Описание комманд
        private string ClipBoard = "";                                               // Тут текст котрый сохраняеться в буфере при нажатии Insert он выводиться 
        private readonly string[] VisualTimer;                                       // Виуализация работы таймера
       

        /// <summary>
        /// Текст приветствие
        /// </summary>
        private string HaText
        {
            get
            {
                string r = GameSrv.Premision switch
                {
                    Server.PremissionServerEnum.FullControl => "admin",
                    Server.PremissionServerEnum.UserControl => "user",
                    Server.PremissionServerEnum.GuestControl => "guest",
                    _ => "web",
                };
                string s = GameSrv.NameSrv;
                return "[" + r + "@" + s + "]:";
            }
        }
        /// <summary>
        /// отображает тамер логов
        /// </summary>
        private int TimerLog
        {
            get => GameSrv.LogSaver;
            set
            {
                GameSrv.LogSaver = value;
                TimerAdminLog.Content = value.ToString();
            }
        }

        public Cmd()
        {
            InitializeComponent();
            ConsoleTimer.Elapsed += Bash;
            ConsoleTimer.AutoReset = true;
            ConsoleTimer.Enabled = true;
            ConsoleTimer.Stop();

            CursorTimer = new System.Windows.Threading.DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(200) };
            CursorTimer.Tick += new EventHandler(CursorUpdate);
            CursorTimer.Start();

            MenuCommand.Visibility = Visibility.Hidden;
            BBSelectButton = new Button[] { BB1, BB2, BB3, BB4, BB5, BB6, BB7, BB8, BB9 };

            GameSrv = App.GameGlobal.Servers[0];// сервер пользователя 

            VisualTimer = new string[4];
            VisualTimer[0] = "|||";
            VisualTimer[1] = "///";
            VisualTimer[2] = "---";
            VisualTimer[3] = @"\\\";


            // получаем информацию о командах
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<XmlInfoCommand>));
            using FileStream fs = new FileStream(App.PatchAB + "XmlInfoCommand.xml", FileMode.OpenOrCreate);
            XmlInfoCommandList = xmlSerializer.Deserialize(fs) as List<XmlInfoCommand>;

        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {
            CmdText.Text = "";
            AddTextConsole("Welcome v0.1", 1, true);
            BashConsole();
        }


        /// <summary>
        /// Открыть меню
        /// </summary>
        /// <param name="menuText"></param>
        /// <param name="coord"></param>
        private void ShowMenu(int menuText, int coord)
        {
            MenuCommand.Visibility = Visibility.Visible;
            foreach (var item in BBSelectButton) item.Visibility = Visibility.Hidden;

            List<XmlInfoCommand> l = XmlInfoCommandList.FindAll(x => x.Menu == menuText);

            for (int i = 0; i < 9; i++)
            {
                if (i == l.Count) break;
                BBSelectButton[i].Visibility = Visibility.Visible;
                BBSelectButton[i].Content = l[i].NameCommand;
                BBSelectButton[i].ToolTip = l[i].Info;
            }

            MenuCommand.Margin = new Thickness(coord, 1, 0, 0);
        }

        /// <summary>
        /// Останавливает все таймеры 
        /// </summary>
        public void StopTimers()
        {
            ConsoleTimer.Stop();
            CursorTimer.Stop();

        }

        /// <summary>
        ///  Добавляет сразу в ScriptConsole
        /// </summary>      
        private void AddTextConsole(string text, int timer = 0, bool jumpLine = true)
        {
            if (ScriptConsole == null) ScriptConsole = new List<ConsoleText>();
            ScriptConsole.Add(TextConsole(text, timer, jumpLine));
        }

        /// <summary>
        ///  Добавляет сразу в ScriptConsole и выполяет действие 
        /// </summary>      
        private void AddTextConsole(string text, Action action, int timer = 0, bool jumpLine = true)
        {
            if (ScriptConsole == null) ScriptConsole = new List<ConsoleText>();
            ScriptConsole.Add(TextConsole(text, timer, jumpLine, action));
        }

        /// <summary>
        /// Запустить обработчик сценария
        /// </summary>
        public void BashConsole()
        {
            if (ScriptConsole.Count == 0) return;
            CursorText.Visibility = Visibility.Hidden;
            ConsoleTimer.Start();
            EnteringText.Visibility = Visibility.Hidden;
        }


        //+++ Работа таймера и скрипта
        int indexText = 0;
        readonly Stack<Inline> lastLine = new Stack<Inline>();
        private void Bash(Object source, ElapsedEventArgs e)
        {
            if (indexText == ScriptConsole.Count)
            {
                ConsoleTimer.Stop();
                indexText = 0;
                ScriptConsole.Clear();
                this.Dispatcher.Invoke(() =>
                {
                    cellText = (short)HaText.Length;
                    EnteringText.Text = "";
                    for (int i = 0; i < linesText; i++) EnteringText.Text += "\r\n";
                    EnteringText.Text += HaText;
                    EnteringText.Visibility = Visibility.Visible;
                    CursorText.Visibility = Visibility.Visible;
                });
                return;
            }

            //Создает задержку 
            if (ScriptConsole[indexText].Delay != 0)
            {
                ScriptConsole[indexText].Delay--;
                return;
            }

            // Выполняет удаление прошлой строки если это нужно 
            if (ScriptConsole[indexText].JumpNewLine == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    while (lastLine.Count != 0)
                    {
                        CmdText.Inlines.Remove(lastLine.Pop());
                    }
                });
                linesText--;
            }

            // Выполняет добавление строки 
            lastLine.Clear();
            this.Dispatcher.Invoke(() =>
            {
                // тут запрос ввода текста 
                if (ScriptConsole[indexText].OpenBufferIN)
                {
                    string t = ScriptConsole[indexText].CommandText;

                    cellText = (short)t.Length;
                    cellText += 2;
                    EnteringText.Text = "";
                    for (int i = 0; i < linesText; i++) EnteringText.Text += "\r\n";
                    EnteringText.Text += t + " >";
                    EnteringText.Visibility = Visibility.Visible;
                    CursorText.Visibility = Visibility.Visible;

                    ConsoleTimer.Stop();
                    indexText = 0;
                    ScriptConsole.Clear();

                }
                else
                {
                    // тут добовляет текст в консоль
                    foreach (var r in ScriptConsole[indexText].Text)
                    {
                        CmdText.Inlines.Add(r);
                        lastLine.Push(CmdText.Inlines.LastInline);
                    }
                    CmdText.Inlines.Add(new Run("\r\n"));
                    lastLine.Push(CmdText.Inlines.LastInline);
                    ClearUpperText();                                       // Удаляет переизбыток строк 
                    if (ScriptConsole[indexText].TiggerAction != null)
                    {
                        ScriptConsole[indexText].TiggerAction.Invoke();// Выполяен действие если оно есть 
                    }   
                }
            });
            indexText++;
        }

        //+++ обновление курсора
        bool flicker = false;
        int indexTimerAdmin = 0;
        private void CursorUpdate(object sender, EventArgs e)
        {
            flicker = !flicker;
            CursorText.Text = "";
            for (int i = 0; i < linesText; i++) CursorText.Text += "\r\n";
            CursorText.Text += new string(' ', cellText);
            CursorText.Text += flicker == true ? "" : "_";

            // Работа с таймером обнаружения
            if (TimerAdminLog.Content.ToString() != "---")
            {
                TimerLog--;
                TimerAdminStr.Content = VisualTimer[indexTimerAdmin];
                indexTimerAdmin++;
                if (indexTimerAdmin == VisualTimer.Length) indexTimerAdmin = 0;
            }
        }

        /// <summary>
        /// Выполняет комманды из текста
        /// </summary>
        /// <param name="com"></param>
        private void ShellCommand(string com)
        {
            if (com.Length == 0) return;
            com = System.Text.RegularExpressions.Regex.Replace(com, @"\s+", " ");
            var r = this.GetType().GetMethod("Cmd_" + com.Split(' ')[0].ToLower());
            ScriptConsole.Clear();
            if (r == null)
            {
                AddTextConsole("Ъ`Red=Error:: Такой команды не существует.Ъ");
            }
            else
            {
                r.Invoke(this, new object[] { com });
            }
            BashConsole();
        }



        /// <summary>
        /// Создает текст для консоли 
        /// </summary>
        /// <param name="text">Текст который может содержать цветовые выделения Ъ`тут_цвет=Тут выделенное словоЪ</param>
        /// <param name="timer">Таймер задержки по умолчанию это 0</param>
        /// <param name="jumpLine">Следует прыгнутиь на новую строку или заменить текущию по умолчанию это True</param>
        /// <returns>AddTextConsole</returns>
        public ConsoleText TextConsole(string text, int timer = 0, bool jumpLine = true, Action action =null)
        {
            List<Inline> txt = new List<Inline>();
            var bc = new BrushConverter();

            foreach (string item in text.Split('Ъ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (item.Substring(0, 1) == "`")
                {
                    Brush brush = (Brush)bc.ConvertFrom(item.Split('=')[0][1..]);
                    txt.Add(new Run(item[(item.IndexOf('=') + 1)..]) { Foreground = brush });
                }
                else
                {
                    txt.Add(new Run(item) { Foreground = StTextColor });
                }
            }
            return new ConsoleText() { Delay = timer, JumpNewLine = jumpLine, Text = txt.ToArray(), TiggerAction =action };
        }

        public void TextConsoleOpenBuffer(string command, int timer = 0)
        {
            ScriptConsole.Add(new ConsoleText() { Text = new Inline[] { new Run(command) }, CommandText = command, Delay = timer, OpenBufferIN = true });
        }

        /// <summary>
        /// Текст для консоли 
        /// </summary>
        public class ConsoleText
        {
            public Inline[] Text;
            public int Delay;
            public bool JumpNewLine;
            public bool OpenBufferIN = false;
            public string CommandText;
            /// <summary>
            /// Выполняет действие в момент выполеннеия строк в консоли 
            /// </summary>
            public Action TiggerAction;  
        }



                    #region "Кнопки Синий и красный кружок"
        Brush SaveColor;
        private void КурсорНадКраснымКружком(object sender, MouseEventArgs e)
        {
            if (SaveColor == null) SaveColor = RedButton.Fill;
            RedButton.Fill = Brushes.OrangeRed;
        }
        private void КурсорСошелСкрасногоКружка(object sender, MouseEventArgs e)
        {
            RedButton.Fill = SaveColor;
            SaveColor = null;
        }
        private void КурсорНадСиним(object sender, MouseEventArgs e)
        {
            if (SaveColor == null) SaveColor = BlueButton.Fill;
            BlueButton.Fill = Brushes.LightBlue;
        }
        private void КурсорУшелСинего(object sender, MouseEventArgs e)
        {
            BlueButton.Fill = SaveColor;
            SaveColor = null;
        }
        private void НажатКрасный(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void НажатСиний(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
                    #endregion

                    #region "Консоль текста ввод"    

        int cellText = 0;
        int linesText = 0;
        int nowElement = 0;


        private void ClearUpperText()
        {
            if (MAX_LINES == linesText)
            {
                Stack<Run> cl = new Stack<Run>();
                // собирает строку 
                foreach (Run item in CmdText.Inlines)
                {
                    cl.Push(item);
                    if (item.Text.Contains("\r\n")) break;
                }
                // удаляет сборку
                while (cl.Count != 0)
                {
                    CmdText.Inlines.Remove(cl.Pop());
                }
            }
            else { linesText++; }
        }
        private void НажатиеКлав(object sender, KeyEventArgs e)
        {
            if (CursorText.Visibility == Visibility.Hidden) return;

            string txt = e.Key.ToString();
            bool shift = false;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) { shift = true; } // Шифт зажат сейчас



            if (e.Key == Key.Enter)      // Ввод текста и выполнение команды              
            {
                string text = EnteringText.Text.Replace("\r\n", "");
                // проверка в буфере
                foreach (var item in BufferConsole.GetType().GetProperties())
                {
                    string t = item.Name + " >";
                    if (text.Substring(0, Math.Min(t.Length, text.Length)) == t)
                    {
                        item.SetValue(BufferConsole, text[Math.Min(t.Length, text.Length)..]);
                        ClearUpperText();   // Удаляет переизбыток строк 
                        CmdText.Inlines.Add(new Run(EnteringText.Text.Replace("\r\n", "") + "\r\n") { Foreground = StTextColor });
                        cellText = (short)HaText.Length;
                        EnteringText.Text = "";
                        for (int i = 0; i < linesText; i++) EnteringText.Text += "\r\n";
                        EnteringText.Text += HaText;

                        BufferConsole.WaitCommand.Invoke();
                        return;
                    }
                }

                cellText = (short)HaText.Length;
                text = text[cellText..];
                StoresList.Add(text);
                nowElement = StoresList.Count;
                ClearUpperText();   // Удаляет переизбыток строк 
                CmdText.Inlines.Add(new Run(EnteringText.Text.Replace("\r\n", "") + "\r\n") { Foreground = StTextColor });

                EnteringText.Text = "";
                for (int i = 0; i < linesText; i++) EnteringText.Text += "\r\n";
                EnteringText.Text += HaText;
                ShellCommand(text);
                return;
            }
            else if (e.Key == Key.Back && (cellText - (short)HaText.Length) != 0)    // Удаляет символ назад 
            {
                cellText--;
                EnteringText.Text = EnteringText.Text[0..^1];
            }
            else if (e.Key == Key.Up && nowElement != 0)    // История ввода вверх
            {
                nowElement--;
                EnteringText.Text = EnteringText.Text.Substring(0, EnteringText.Text.Length - cellText);
                EnteringText.Text += HaText + StoresList[nowElement];
                cellText = (short)(StoresList[nowElement].Length + HaText.Length);
            }
            else if (e.Key == Key.Down && nowElement != StoresList[nowElement].Length)  //История ввода вниз
            {
                nowElement++;
                EnteringText.Text = EnteringText.Text.Substring(0, EnteringText.Text.Length - cellText);
                EnteringText.Text += HaText + StoresList[nowElement];
                cellText = (short)(StoresList[nowElement].Length + HaText.Length);
            }
            else if ((e.Key >= Key.D0) && (e.Key <= Key.D9)) { txt = txt[1..]; }    // Нажатия цифр
            else if (txt[0..^1] == "NumPad") { txt = txt[6..]; }          //Нажатия цифр на доп клав            
            else if (txt == "Space") { txt = " "; }
            else if (txt == "Subtract" | txt == "OemMinus") { txt = "-"; }
            else if (txt == "OemPlus" | txt == "Add") { txt = "+"; }
            else if (txt == "OemQuestion" | txt == "Divide") { txt = "/"; }
            else if (txt == "Oem5") { txt = "\\"; }
            else if (txt == "Oem3") { txt = "~"; }
            else if (txt == "OemPeriod" | txt == "Decimal") { txt = "."; }

            if (MAX_STRING == cellText) return;

            if (txt.Length == 1 & shift == false)
            {
                EnteringText.Text += txt.ToLower();
                cellText += 1;
            }
            else if (txt.Length == 1 & shift == true)
            {
                if (txt == "-") { txt = "_"; }
                else if (txt == "5") { txt = "%"; }
                EnteringText.Text += txt;
                cellText += 1;
            }

            if (e.Key == Key.Insert) // Вставка из буфера
            {
                EnteringText.Text += ClipBoard;
                cellText += ClipBoard.Length;
            }
            else
            {
                FindInfo(EnteringText.Text.Replace("\r\n", "")); // описывает комманду помощь
            }


        }

        private void FindInfo(string txt)
        {

            foreach (var item in XmlInfoCommandList)
            {
                if (txt.Substring(0, Math.Min((HaText + item.NameCommand).Length, txt.Length)) == HaText + item.NameCommand)
                {
                    CommandInfo.Inlines.Clear();
                    InfoArgument.Content = item.Info;
                    List<Inline> tt = new List<Inline>();
                    foreach (var tv in item.Comment)
                    {
                        if (txt.Contains(tv.Split(';')[0]))
                        {
                            tt.Add(new Run(tv.Split(';')[1] + " ") { Foreground = Brushes.Aquamarine });
                        }
                        else
                        {
                            tt.Add(new Run(tv.Split(';')[1] + " "));
                        }
                    }

                    CommandInfo.Inlines.AddRange(tt.ToArray());
                    return;
                }
            }
            InfoArgument.Content = "";
            CommandInfo.Inlines.Clear();
        }

                    #endregion

        /// <summary>
        /// структура содержания комманд и описания
        /// </summary>
        public struct XmlInfoCommand
        {
            [XmlAttribute("NameCommand")] public string NameCommand;
            public string[] Comment;
            public string Info;
            public int Menu;
        }

        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowMenu(1, -5);
            Button01.Focusable = false;

        }

        private void СистемКлик(object sender, RoutedEventArgs e)
        {
            ShowMenu(2, 155);
            Button02.Focusable = false;
        }

        private void КурсорНадКонсольюТекст(object sender, MouseEventArgs e)
        {

        }

        private void КурсорВышелИзМеню(object sender, MouseEventArgs e)
        {
            MenuCommand.Visibility = Visibility.Hidden;
            EnteringText.Focus();
        }

        private void ЗакрытаФорма(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        private void BBAtc(object sender, RoutedEventArgs e)
        {
            string txt = ((Button)sender).Content.ToString().Split(' ')[0];
            EnteringText.Text += txt;
            cellText += txt.Length;
        }

        private void ОбщиеКлик(object sender, RoutedEventArgs e)
        {
            ShowMenu(3, 317);
            Button03.Focusable = false;
        }

        private void TestSub(object sender, RoutedEventArgs e)
        {
            Cmd_route("r localhost www.test.ru");
        }
    }



    //***************************************************************************************************************************************************************************
    //***************************************************************************************************************************************************************************
    //***************************************************************************************************************************************************************************
    /// <summary>
    /// команды консоли тут
    /// </summary>
    public partial class Cmd
    {
        public void Cmd_make(string txt)
        {
            if (GameSrv.OS == Server.TypeOSEnum.WinSrv)
            {
                AddTextConsole("bg.exe -f " + txt, 1);
                AddTextConsole("ver. 1.4", 1);
                AddTextConsole("", 1);
            }
            string[] vs = txt.Split(' ');
            if (vs.Length == 1)
            {
                AddTextConsole("Ъ`Red=Вы не указали путь к файлу или файл Ъ");
                return;
            }
            FileServerClass fileServer = OpenFileEx(txt[5..]);
            if (fileServer == null) return;

           
           
                AddTextConsole("Ъ`OrangeRed=Файл не соотвествует рабочему исполнительному файлу", 1);
            
        }

        public void Cmd_copy(string txt)
        {
            string[] vs = txt.Split(' ');

            Server serverOUT = GameSrv;
            Server serverIn = GameSrv;

           FileServerClass fileServer= OpenFileEx(vs[1]);
            if (fileServer != null)
            {
                if (vs[2] == "download")
                {
                    serverIn = App.GameGlobal.MyServer;
                    vs[2] = "/user/Hpro4/Download/";
                }
                var ls = FileServerClass.GetInfoFiles(vs[2], serverIn);
                if (ls.Find(x => x.FileName == fileServer.FileName) != null)
                {
                    AddTextConsole("Ъ`Red= Такой файл тут уже существует в папке: " + serverIn.NameSrv + "://" + vs[2] + "Ъ", 1);
                    return;
                }
                // имитируем загрузку с другова сервера
                if (serverIn == App.GameGlobal.MyServer & serverOUT != App.GameGlobal.MyServer)
                {
                    AddTextConsole("Download: [          ]", 1);
                    AddTextConsole("Download: [=         ]", 3, false);
                    AddTextConsole("Download: [==        ]", 3, false);
                    AddTextConsole("Download: [===       ]", 3, false);
                    AddTextConsole("Download: [====      ]", 3, false);
                    AddTextConsole("Download: [=====     ]", 3, false);
                    AddTextConsole("Download: [======    ]", 3, false);
                    AddTextConsole("Download: [=======   ]", 3, false);
                    AddTextConsole("Download: [========  ]", 3, false);
                    AddTextConsole("Download: [========= ]", 3, false);
                    AddTextConsole("Download: [==========]", 3, false);
                    AddTextConsole("Download: [  = ok=   ]", 3, false);
                }
                AddTextConsole("Файл скопирован успешно в каталог " + serverIn.NameSrv + "://" + vs[2], () => serverIn.CreateFiles(vs[2], fileServer),  1);
                App.GameGlobal.EventIntroduce(EventEnum.ФайлСкачан, fileServer.FileName);
            }
        }

        public void Cmd_openurl(string txt)
        {
            if (GameSrv.OS != Server.TypeOSEnum.Logerhead)
            {
                AddTextConsole("Ъ`Red=\"openurl\" неверно введенная команда или такой исполнительный файл отсутствуетЪ", 1);
                return;
            }
            string[] vs = txt.Split(' ');
            bool numr = false;
            int i = 1;
            if (vs.Length > 1) if (vs[1] == "-n") numr = true;

            AddTextConsole("Списки ресурсов открытых на сервере:", 1);
            foreach (var item in App.GameGlobal.OpenUrl)
            {
                if (item.Key.Substring(0, 10) == "localhost/")
                {
                    var l = numr ? i.ToString() : " " + "    " + item.Key + " | " + item.Value.FileName;
                    AddTextConsole("Ъ`White=" + l + "Ъ", 1);
                    i++;
                }
            }
            AddTextConsole("------------------------", 1);

        }

        public void Cmd_logclear(string txt)
        {           
            string[] vs = txt.Split(' ');
            if (GameSrv.OS == Server.TypeOSEnum.Logerhead)
            {
                AddTextConsole("На этом сервер запись логов отключена. ", 4);
                return;
            }
            if (vs.Length == 0) {
                AddTextConsole("Вы должны указать ключ -l или -w", 1);
                return;
            }

            if (vs[1] == "-w" & GameSrv.OS == Server.TypeOSEnum.WinSrv)
            {
                AddTextConsole("Exe -- 01", 2);
                AddTextConsole("FF45 2200 mov", 1);
                AddTextConsole("Started 2202", 3);
                AddTextConsole("--> kernel.dll", 5);
            }
            else if (vs[1] == "-l" & GameSrv.OS != Server.TypeOSEnum.WinSrv)
            {
                AddTextConsole(".logs server ", 1);
                AddTextConsole("open /", 5);
                AddTextConsole(@"open \.logs", 1, false);
            }
            else {
                AddTextConsole("Подключение к логам сервера ...", 5);
                AddTextConsole("Нет доступа или нет ключа", 1);
                return;
            }
            AddTextConsole("Clear Logs | Process...", 3);
            GameSrv.LogSaver = 0;
            AddTextConsole("Clear Logs | End process.", 4);
            AddTextConsole("Exit Server | Выход из сервера", Off_Server, 2);
        }

        public void Cmd_mkdir(string txt) {
            string[] vs = txt.Split(' ');
            if (vs.Length == 0)
            {
                AddTextConsole("Вы должны указать название папки ", 1);
                return;
            }

            GameSrv.CreateDir(Pwd, vs[1], FileServerClass.PremisionEnum.AdminUserGuest);

        }

        public void Cmd_wget(string txt)
        {
            string[] vs = txt.Split(' ');
            if (GameSrv.OS != Server.TypeOSEnum.Linux & GameSrv.OS != Server.TypeOSEnum.Logerhead)
            {
                AddTextConsole("Ъ`Red= \"wget\" не является внутренней или внешней командой, исполняемой программой или пакетным файлом.Ъ", 1);
                return;
            }
            try
            {
                if (App.GameGlobal.OpenUrl.ContainsKey(vs[2]) == false)
                {
                    AddTextConsole("Узел не найден, либо закрыт невозможно скачать", 15);
                    return;
                }
                AddTextConsole("Подключаюсь к узлу", 1);
                FileServerClass file;
                Action action = null;
                // скачка из сетевого ресурса
                if (vs[1] == "-u")
                {
                    file = App.GameGlobal.OpenUrl[vs[2]];

                    // Проверка доступа к файлу
                    if (FileServerClass.CheckAccess(Pwd, GameSrv) == false)
                    {
                        AddTextConsole("Ъ`Red=Нет доступа к файлу: " + Pwd + " Ъ");
                        return;
                    }
                    if (FileServerClass.Exist(Pwd, file.FileName, GameSrv) == false) action = () => GameSrv.CreateFiles(Pwd, file);
                }
                else if (vs[1] == "-d")
                {
                    file = OpenFileEx(vs[2]);
                    if (FileServerClass.CheckAccess(Pwd + "/" + file.FileName, GameSrv) == false)
                    {
                        AddTextConsole("Статус: Доступ к файлу закрыт.");
                        return;
                    }
                    if (FileServerClass.Exist("/user/Hpro4/Download/", file.FileName, GameSrv) == false) action = () =>
                    {
                        App.GameGlobal.MyServer.CreateFiles("/user/Hpro4/Download/", file);
                        App.GameGlobal.EventIntroduce(EventEnum.ФайлСкачан, file.FileName );
                    };
                }
                else
                {
                    AddTextConsole("Неверно указан ключ " + vs[1], 1);
                    return;
                }
                AddTextConsole("Передача данных.", 5, false);
                switch (file.Size.ToString().Length)
                {
                    case 1:
                        AddTextConsole("               13%", 1, false);
                        AddTextConsole("               56%", 7, false);
                        AddTextConsole("               89%", 9, false);
                        AddTextConsole("               96%", 11, false);
                        break;
                    case 2:
                        AddTextConsole("               8 %", 1, false);
                        AddTextConsole("               22%", 8, false);
                        AddTextConsole("               58%", 4, false);
                        AddTextConsole("               71%", 6, false);
                        AddTextConsole("               86%", 7, false);
                        AddTextConsole("               99%", 12, false);
                        break;
                    case 3:
                        AddTextConsole("               11%", 1, false);
                        AddTextConsole("               24%", 8, false);
                        AddTextConsole("               31%", 4, false);
                        AddTextConsole("               45%", 6, false);
                        AddTextConsole("               69%", 7, false);
                        AddTextConsole("               80%", 5, false);
                        AddTextConsole("               93%", 10, false);
                        break;
                    case 4:
                        AddTextConsole("               5 %", 1, false);
                        AddTextConsole("               17%", 8, false);
                        AddTextConsole("               24%", 4, false);
                        AddTextConsole("               39%", 6, false);
                        AddTextConsole("               51%", 7, false);
                        AddTextConsole("               72%", 5, false);
                        AddTextConsole("               86%", 10, false);
                        AddTextConsole("               98%", 6, false);
                        break;
                    case 5:
                        AddTextConsole("               9 %", 1, false);
                        AddTextConsole("               20%", 4, false);
                        AddTextConsole("               37%", 7, false);
                        AddTextConsole("               49%", 5, false);
                        AddTextConsole("               60%", 9, false);
                        AddTextConsole("               70%", 5, false);
                        AddTextConsole("               86%", 5, false);
                        AddTextConsole("               91%", 10, false);
                        AddTextConsole("               98%", 12, false);
                        break;
                    default:
                        AddTextConsole("               10%", 1, false);
                        AddTextConsole("               24%", 4, false);
                        AddTextConsole("               42%", 6, false);
                        AddTextConsole("               57%", 8, false);
                        AddTextConsole("               67%", 11, false);
                        AddTextConsole("               73%", 5, false);
                        AddTextConsole("               84%", 5, false);
                        AddTextConsole("               89%", 7, false);
                        AddTextConsole("               95%", 8, false);
                        AddTextConsole("               99%", 10, false);
                        break;
                }
                AddTextConsole("               100%", 2, false);
                AddTextConsole("Скопировано 1 файл(ов)", action, 1);
            }
            catch
            {
                AddTextConsole("Ъ`Red=Неверно составлена команда, неверно указанные параметры Ъ");
            }
        }

        public void Cmd_download(string txt)
        {
            string[] vs = txt.Split(' ');

            if (GameSrv.OS != Server.TypeOSEnum.WinSrv)
            {
                AddTextConsole("Ъ`Red=\"download\" неверно введенная команда или такой исполнительный файл отсутствуетЪ", 1);
                return;
            }
            if (vs.Length == 1)
            {
                AddTextConsole("Ъ`Red=Синтакстическая ошибка, при указании адреса Ъ", 1);
                return;
            }
            AddTextConsole("$client = new- object System.Net.WebClient", 5);
            AddTextConsole("$client.DownloadFile(" + vs[1] + ", " + Pwd + ")", 5);
            AddTextConsole("===============================(download)============================", 1);
            AddTextConsole("                                   (c)Dmitrev Aleksea 2000", 1);
            AddTextConsole("                                   download files for windows", 1);

            Action action;
            FileServerClass file;

            if (vs[1] == "/d")
            {
                if (vs.Length == 2)
                {
                    AddTextConsole("Ъ`Red=Вы не указали название файла для скачки Ъ", 1);
                    return;
                }
                file = OpenFileEx(vs[2]);
                if (file == null) return;
                if (FileServerClass.CheckAccess(Pwd + "/" + file.FileName, GameSrv) == false)
                {
                    AddTextConsole("Статус: Доступ к файлу закрыт.");
                    return;
                }
                if (FileServerClass.Exist("/user/Hpro4/Download/", file.FileName, GameSrv))
                {
                    AddTextConsole("Статус: Такой файл уже существует " + Pwd + "/" + file.FileName);
                    return;
                }
                action = () =>
                {
                    App.GameGlobal.MyServer.CreateFiles("/user/Hpro4/Download/", file);
                    App.GameGlobal.EventIntroduce(EventEnum.ФайлСкачан, file.FileName);
                };
            } else
            {
                if (App.GameGlobal.OpenUrl.ContainsKey(vs[1]) == false)
                {
                    AddTextConsole("Удалённый адрес [" + vs[1] + "]", 15);
                    AddTextConsole("Ошибка: Файл не найден на сервере.", 4);
                    return;
                }

                file = App.GameGlobal.OpenUrl[vs[1]];

                // Проверка доступа к файлу
                if (FileServerClass.CheckAccess(Pwd, GameSrv) == false)
                {
                    AddTextConsole("Статус: Доступ к файлу закрыт.");
                    return;
                }

                if (FileServerClass.Exist(Pwd, file.FileName, GameSrv))
                {
                    AddTextConsole("Статус: Такой файл уже существует");
                    return;
                }
                action = () => GameSrv.CreateFiles(Pwd, file);
            }
            AddTextConsole("Статус: Переход в состояние закачки.", 5);
            switch (file.Size.ToString().Length)
            {
                case 1:
                    AddTextConsole("( ) ( ) ( )", 1, false);
                    AddTextConsole("(X) ( ) ( )", 7, false);
                    AddTextConsole("(X) (X) ( )", 9, false);
                    AddTextConsole("(X) (X) (X)", 11, false);
                    break;
                case 2:
                    AddTextConsole("( ) ( ) ( ) ( ) ( )", 1, false);
                    AddTextConsole("(X) ( ) ( ) ( ) ( )", 8, false);
                    AddTextConsole("(X) (X) ( ) ( ) ( )", 4, false);
                    AddTextConsole("(X) (X) (X) ( ) ( )", 6, false);
                    AddTextConsole("(X) (X) (X) (X) ( )", 7, false);
                    AddTextConsole("(X) (X) (X) (X) (X)", 12, false);
                    break;
                case 3:
                    AddTextConsole("( ) ( ) ( ) ( ) ( ) ( )", 1, false);
                    AddTextConsole("(X) ( ) ( ) ( ) ( ) ( )", 8, false);
                    AddTextConsole("(X) (X) ( ) ( ) ( ) ( )", 4, false);
                    AddTextConsole("(X) (X) (X) ( ) ( ) ( )", 6, false);
                    AddTextConsole("(X) (X) (X) (X) ( ) ( )", 7, false);
                    AddTextConsole("(X) (X) (X) (X) (X) ( )", 5, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X)", 10, false);
                    break;
                case 4:
                    AddTextConsole("( ) ( ) ( ) ( ) ( ) ( ) ( )", 1, false);
                    AddTextConsole("(X) ( ) ( ) ( ) ( ) ( ) ( )", 8, false);
                    AddTextConsole("(X) (X) ( ) ( ) ( ) ( ) ( )", 4, false);
                    AddTextConsole("(X) (X) (X) ( ) ( ) ( ) ( )", 6, false);
                    AddTextConsole("(X) (X) (X) (X) ( ) ( ) ( )", 7, false);
                    AddTextConsole("(X) (X) (X) (X) (X) ( ) ( )", 5, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) ( )", 10, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) (X)", 6, false);
                    break;
                case 6:
                    AddTextConsole("( ) ( ) ( ) ( ) ( ) ( ) ( ) ( )", 1, false);
                    AddTextConsole("(X) ( ) ( ) ( ) ( ) ( ) ( ) ( )", 4, false);
                    AddTextConsole("(X) (X) ( ) ( ) ( ) ( ) ( ) ( )", 7, false);
                    AddTextConsole("(X) (X) (X) ( ) ( ) ( ) ( ) ( )", 5, false);
                    AddTextConsole("(X) (X) (X) (X) ( ) ( ) ( ) ( )", 9, false);
                    AddTextConsole("(X) (X) (X) (X) (X) ( ) ( ) ( )", 5, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) ( ) ( )", 5, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) (X) ( )", 10, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) (X) (X)", 12, false);
                    break;
                default:
                    AddTextConsole("( ) ( ) ( ) ( ) ( ) ( ) ( ) ( ) ( )", 1, false);
                    AddTextConsole("(X) ( ) ( ) ( ) ( ) ( ) ( ) ( ) ( )", 4, false);
                    AddTextConsole("(X) (X) ( ) ( ) ( ) ( ) ( ) ( ) ( )", 6, false);
                    AddTextConsole("(X) (X) (X) ( ) ( ) ( ) ( ) ( ) ( )", 8, false);
                    AddTextConsole("(X) (X) (X) (X) ( ) ( ) ( ) ( ) ( )", 11, false);
                    AddTextConsole("(X) (X) (X) (X) (X) ( ) ( ) ( ) ( )", 5, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) ( ) ( ) ( )", 5, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) (X) ( ) ( )", 7, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) (X) (X) ( )", 8, false);
                    AddTextConsole("(X) (X) (X) (X) (X) (X) (X) (X) (X)", 10, false);
                    break;
            }
            
            AddTextConsole("Файл скачен. ",action, 1, false);
        }

        public void Cmd_connect(string txt)
        {
            string[] vs = txt.Split(' ');
            try
            {
                //-adress -ip
                Server srv = null;
                switch (vs[1])
                {
                    case "-off":
                        Off_Server();
                        AddTextConsole("Вы отключились от сервера. ", 1);
                        return;
                    case "-ip":
                    case "-adress":
                        srv = App.GameGlobal.FindServer(vs[2]);
                        break;
                    default:
                        AddTextConsole("Вы не указали ключ -adress или -ip", 1);
                        break;
                }
                if (srv == null)
                {
                    AddTextConsole("Ъ`Red= Сервер не найден, проверте название Ъ", 1);
                    return;
                }

                if (vs[3] == "-shell")
                {                   
                   FileServerClass fileServer= OpenFileEx(vs[4]);
                    if (fileServer == null) return;
                    // Тут работа и проверка шелла
                    if (fileServer.FileСontents.TypeInformation == Enums.TypeParam.shell)
                    {
                        if (fileServer.FileСontents.TextCommand.ToLower() == srv.OSName.ToLower() == false)
                        {
                            AddTextConsole("Ъ`CornflowerBlue=error: 451  Object.Module._extensions Ъ", 2);
                            AddTextConsole("Ъ`CornflowerBlue=error: tryModuleLoad (internal/modules/)/ Ъ", 2);
                            AddTextConsole("Ъ`CornflowerBlue=error: myAsyncFunc 0051 x00 Ъ", 2);
                            AddTextConsole("Ъ`CornflowerBlue=error: at Planner.incrementalAdd Ъ", 2);
                            AddTextConsole("Ъ`CornflowerBlue=error: _01x00 Ъ", 2);
                            AddTextConsole(" ! Этот Shell не совместим с это OS  ! ", 2);
                            return;
                        }
                    }
                    else
                    {
                        AddTextConsole("Ъ`Red=Выполнение программы не возможно, файл не соотвествует типу Shell  Ъ");
                        return;
                    }

                    if (srv.Premision == Server.PremissionServerEnum.FullControl | srv.Premision == Server.PremissionServerEnum.UserControl)
                    {
                        AddTextConsole("Ъ`BlueViolet= Ожидание ответа. Ъ", 1);
                    }
                    else
                    {
                        AddTextConsole("! У вас нет доступа к этому серверу !");
                        return;
                    }
                    ConnectServer(srv);
                }
                else if (vs[3] == "-auth")
                {
                    AddTextConsole("Connect server...");
                    if (srv.LoginAndPass == "") srv.CreateLoginPass();
                    AddTextConsole("=ok=", 5, false);
                    TextConsoleOpenBuffer("Login", 1);
                    BufferConsole.WaitCommand = delegate
                    {
                        AddTextConsole("");
                        AddTextConsole("");
                        TextConsoleOpenBuffer("Password", 1);
                        BufferConsole.WaitCommand = delegate
                        {
                            AddTextConsole("");
                            AddTextConsole("");
                            AddTextConsole("Проверка ...", 1);
                            ///dsadsdas
                            if ((BufferConsole.Login + ":" + BufferConsole.Password) != srv.LoginAndPass)
                            {
                                ConnectServer(srv);
                            }
                            else
                            {
                                AddTextConsole("Ъ`Red= Неверный логин или пароль !Ъ");
                            }
                            BashConsole();
                        };
                        BashConsole();
                    };
                    return;
                }
                else if (vs[3] == "-regexpl")
                {
                    AddTextConsole("Connect server: regexpl");
                    if (srv.OS == Server.TypeOSEnum.Linux & srv.Premision == Server.PremissionServerEnum.FullControl)
                    {
                        ConnectServer(srv);
                    }
                    else
                    {
                        AddTextConsole("Err84- F0 * F1 (not sys*file)", 5);
                        AddTextConsole("system halt = 0", 6);
                        AddTextConsole("Не удалось подключиться, данный тип подключения не доступен ", 1);
                    }
                }
            }
            catch
            {
                AddTextConsole("Ъ`Red=Синтаксическая ошибка при написание командыЪ", 1);
            }
        }

        public void Cmd_pl(string txt)
        {
            if (GameSrv.OS != Server.TypeOSEnum.Linux & GameSrv.OS != Server.TypeOSEnum.Logerhead)
            {
                AddTextConsole("Ъ`Red= \"pl\" не является внутренней или внешней командой, исполняемой программой или пакетным файлом.", 1);
                return;
            }

            string[] vs = txt.Split(' ');
            AddTextConsole("exlist.cpp");
            AddTextConsole("EXC interface::statup");
            AddTextConsole(".", 4);
            AddTextConsole("..", 2);
            AddTextConsole("../pl", 1);

            if (vs.Length == 1)
            {
                AddTextConsole("Ъ`Red=Вы не указали путь к файлу или файл Ъ");
                return;
            }
            vs[1] = txt[3..]; // объеденяет все строки в одну
            FileServerClass fileServer = OpenFileEx(vs[1]);
            if (fileServer == null) return;
            // Сплойты или нет 
            if (fileServer.FileСontents.TypeInformation == Enums.TypeParam.exploit)
            {
                Random rnd = new Random();
                int se = rnd.Next(0, 10);

                // тип оформления работы
                if (fileServer.FileСontents.ByteParam == 0)
                {
                    fileServer.FileСontents.ByteParam = (byte)se;
                }
                else
                {
                    se = fileServer.FileСontents.ByteParam;
                }

                UpperTxt(se);
                AddTextConsole("Укажите IP Сервера для атаки ::");
                TextConsoleOpenBuffer("ServerIP", 1);
                BufferConsole.WaitCommand = delegate
                {
                    AddTextConsole("");
                    AddTextConsole("Connect server...");
                    var v = App.GameGlobal.Servers.Find(x => x.IP == BufferConsole.ServerIP & x.ActSrv == true);
                    if (v == null)
                    {
                        AddTextConsole("Ъ`Red=Сервер  не найден с этим IPЪ Ъ`White=" + BufferConsole.ServerIP.ToString() + "Ъ Ъ`Red=или не отвечает на запросы из-за перегрузки.Ъ");
                        BufferConsole.Clear();
                        BashConsole();
                        return;
                    }

                    AddTextConsole("Укажите номер уязвимого порта:");
                    TextConsoleOpenBuffer("NumberPort", 1);
                    BufferConsole.Srv = v;
                    BufferConsole.WaitCommand = ExploidRun;
                    BashConsole();
                };
            }
            else  if (fileServer.FileСontents.TypeInformation == Enums.TypeParam.backdoor)  {
                    string t = App.GameGlobal.MainWindow.Firewall_TXT[fileServer.FileСontents.IntParam];
                    string os = Enum.GetName(typeof(Server.TypeOSEnum), GameSrv.OS);

                    AddTextConsole("exec make " + fileServer.FileName + " clean", 4);
                    AddTextConsole("----- making target " + GameSrv.OSName + " [new preregs] -----", 4);
                    AddTextConsole(".config script/b", 4);
                    AddTextConsole(".config script/a", 1);
                    AddTextConsole(".config param 1", 1);
                    AddTextConsole(".config include/config", 1);

                    // универсальный доступ
                    if (fileServer.FileСontents.TextCommand.Contains("url="))
                    {
                        if (fileServer.FileСontents.TextCommand.Contains(GameSrv.NameSrv))
                        {
                            AddTextConsole("System Configuration....", 4);
                            AddTextConsole("Authentication", 15);
                            AddTextConsole("Вы получили доступ к серверу!", 12);
                            GameSrv.Premision = Server.PremissionServerEnum.FullControl;
                        }
                        else { AddTextConsole("Вам отказанно в доступе !", 12); }
                        goto end_p;
                    }
                    // стандартный доступ
                    if (GameSrv.OS == Server.TypeOSEnum.WinSrv)
                    {
                        AddTextConsole("win.exe script", 1);
                        if (t.Split(',')[2] == os)
                        {
                            if (GameSrv.FireWall == t.Split(',')[1])
                            {
                                // если разница большая между хай теком и популярностью в 10 ьл сервер не получиться взломать 
                                if ((GameSrv.PopularSRV - App.GameGlobal.GamerInfo.HiTecLevel) > 10)
                                {
                                    AddTextConsole("SET Win/ $dir$", 4);
                                    AddTextConsole("NoDriveTypeAutoRun = dword:000000ff", 1);
                                    AddTextConsole("label=Inc_drive", 1);
                                    AddTextConsole(@"DriverPath=drivers\ 0", 1);
                                    AddTextConsole(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\ false", 1);
                                    AddTextConsole("Ъ`OrangeRed= Вы получили полный доступ Ъ", 8);
                                    GameSrv.Premision = Server.PremissionServerEnum.FullControl;
                                }
                                else
                                {
                                    AddTextConsole("_", 1);
                                    AddTextConsole(" ", 3, false);
                                    AddTextConsole("_", 3, false);
                                    AddTextConsole(" ", 3, false);
                                    AddTextConsole("_", 3, false);
                                    AddTextConsole("Ъ`OrangeRed= Администратор смог определить вас он более продвинутый чем ваш текущий скилл Ъ", 8, false);
                                }
                            }
                            else
                            {
                                AddTextConsole("_", 1);
                                AddTextConsole(" ", 3, false);
                                AddTextConsole("_", 3, false);
                                AddTextConsole(" ", 3, false);
                                AddTextConsole("Ъ`OrangeRed= Фаервол заблокировал работу программы Ъ", 8, false);
                            }
                        }
                        else
                        {
                            AddTextConsole("script err #11 Ъ`OrangeRed= Операционная система не соотвествует текущей Ъ", 1);
                        }
                    }
                    // =>> linux
                    else
                    {
                        AddTextConsole("pl script", 1);
                        bool b20 = t.Split(',')[0].IndexOf("20") != -1;
                        if (t.Split(',')[2] == os)
                        {
                            //В зависимости от популярности сервера и версии бекдора будет доступ
                            if (b20 & (GameSrv.PopularSRV - App.GameGlobal.GamerInfo.HiTecLevel) > 10)
                            {
                                AddTextConsole("sudo apt-get install dialog", 4);
                                AddTextConsole("add-apt-repository", 1);
                                AddTextConsole("choices=$Inc_drive", 1);
                                AddTextConsole("apt-get update && sudo apt-get upgrade -y", 1);
                                AddTextConsole("Ъ`OrangeRed= Вы повысили уровень доступа Ъ", 8);
                                GameSrv.Premision = Server.PremissionServerEnum.FullControl;
                            }
                            else if (b20 & (GameSrv.PopularSRV - App.GameGlobal.GamerInfo.HiTecLevel) > 5)
                            {
                                AddTextConsole("DNS=192.168.0.13", 4);
                                AddTextConsole("add-apt-repository", 1);
                                AddTextConsole("apt-key adv --keyserver keyserver.", 1);
                                AddTextConsole("if [[ $EUID -ne 0 ]]; ", 1);
                                AddTextConsole("Ъ`OrangeRed=Вы повысили уровень доступа Ъ", 8);
                                GameSrv.Premision = Server.PremissionServerEnum.FullControl;
                            }
                            else
                            {
                                AddTextConsole("_", 1);
                                AddTextConsole(" ", 3, false);
                                AddTextConsole("_", 3, false);
                                AddTextConsole("Ъ`OrangeRed= Сервер отказал вам в доступе, так как выполнил комманды Ъ", 8, false);
                            }
                        }
                        else
                        {
                            AddTextConsole("script mds1 - error ", 1);
                            AddTextConsole("Ъ`OrangeRed=Несовместимая ОСЪ", 1);
                        }
                    }

                end_p:
                    App.GameGlobal.EventIntroduce(EventEnum.ВходНаСервер,  GameSrv.NameSrv );
                }
            else
            {
                AddTextConsole("Ъ`Red=Undefined subroutine &main::insertUser called at      " + vs[1] + " Ъ", 5);
                AddTextConsole("Ъ`Red=Файл не являеться скриптом    " + vs[1] + " Ъ", 2);
                return;
            }

        }

        public void Cmd_ls(string txt)
        {
            if (GameSrv.OS != Server.TypeOSEnum.Linux & GameSrv.OS != Server.TypeOSEnum.Logerhead)
            {
                AddTextConsole("Ъ`Red= \"ls\" не является внутренней или внешней командой, исполняемой программой или пакетным файлом.Ъ", 1);
                return;
            }
            bool full = false;
            bool num = false;
            string[] vs = txt.Split(' ');
            foreach (var item in vs)
            {
                if (item == "-la") full = true;
                if (item == "-n") num = true;
            }

            if (full) AddTextConsole("hdd0:\\" + Pwd);

            FF00_ls.Clear();
            int i = 0;
            string p = "";
            foreach (var item in FileServerClass.GetInfoFiles(Pwd, GameSrv))
            {
                FF00_ls.Add(item.FileName);
                string col = item.Rights switch
                {
                    FileServerClass.PremisionEnum.OnlyAdmin => "Red",
                    FileServerClass.PremisionEnum.AdminAndUser => "Yellow",
                    _ => StTextColor.ToString(),
                };
                if (num == true) p = i.ToString() + ":  ";

                string txtR;
                string color;
                switch (item.Rights)
                {
                    case FileServerClass.PremisionEnum.none:
                        txtR = "--- ---";
                        color = "Gold";
                        break;
                    case FileServerClass.PremisionEnum.OnlyAdmin:
                        txtR = "adm ---";
                        color = "Red";
                        break;
                    case FileServerClass.PremisionEnum.AdminAndUser:
                        color = "Yellow";
                        txtR = "adm use";
                        break;
                    case FileServerClass.PremisionEnum.AdminUserGuest:
                    default:
                        color = "White";
                        txtR = "all usr";
                        break;
                }

                if (item.Dir == null)
                {
                    if (full == true)
                    {
                        txtR = "- " + txtR;
                        AddTextConsole(p + "Ъ`" + color + "=" + txtR + " " + item.FileName + " " + item.Size + "Ъ Файл");
                    }
                    else { AddTextConsole(p + item.FileName.Replace (" ","%20")); }
                }
                else
                {
                    if (full == true) {
                        txtR = "d " + txtR;
                        AddTextConsole(p + "Ъ`"+ color + "=" + txtR + " " + item.FileName + " 0 Ъ Папка"); 
                    
                    }
                    else
                    {
                        AddTextConsole(p + "Ъ`Gold={" + item.FileName.Replace(" ", "%20") + "}Ъ");
                    }
                }
                i++;
            }
        }

        public void Cmd_dir(string txt)
        {
            if (GameSrv.OS != Server.TypeOSEnum.WinSrv)
            {
                AddTextConsole("Ъ`Red=\"dir\" неверно введенная команда или такой исполнительный файл отсутствуетЪ", 1);
                return;
            }
            string[] vs = txt.Split(' ');

            bool num = false;
            foreach (var item in vs)
            {
                if (item == "-n") num = true;
            }

            AddTextConsole("Том в устройстве hdd0: не имеет метки.", 1);
            AddTextConsole("Серийный номер тома: " + Pwd.GetHashCode(), 1);
            AddTextConsole("");
            AddTextConsole("Содержимое папки" + Pwd, 1);
            FF00_ls.Clear();
            int i = 0;
            foreach (var item in FileServerClass.GetInfoFiles(Pwd, GameSrv))
            {
                FF00_ls.Add(item.FileName);
                string d = "     ";
                if (item.Dir != null) d = "<DIR>";
                if (num == true) d += " `" + i + "`";

                if (item.Rights == FileServerClass.PremisionEnum.OnlyAdmin)
                {
                    AddTextConsole("Ъ`MistyRose=" + item.Size.ToString() + new string(' ', 10 - item.Size.ToString().Length) + " " + d + " " + item.FileName + "Ъ");
                }
                else
                {
                    AddTextConsole("Ъ`White=" + item.Size.ToString() + new string(' ', 10 - item.Size.ToString().Length) + " " + d + " " + item.FileName + "Ъ");
                }
                i++;
            }

        }

        public void Cmd_pwd(string txt)
        {
            if (GameSrv.OS != Server.TypeOSEnum.Linux & GameSrv.OS != Server.TypeOSEnum.Logerhead)
            {
                AddTextConsole("Ъ`Red= \"pwd\" не является внутренней или внешней командой, исполняемой программой или пакетным файлом.Ъ", 1);
                return;
            }
            string[] vs = txt.Split(' ');
            if (vs.Length > 1)
            {
                if (vs[1] == "-l") { AddTextConsole(Pwd); }
            }
            else { AddTextConsole("hdd0:" + Pwd); }
        }

        public void Cmd_id(string txt)
        {
            if (GameSrv.OS != Server.TypeOSEnum.Linux & GameSrv.OS != Server.TypeOSEnum.Logerhead)
            {
                AddTextConsole("Ъ`Red= \"id\" не является внутренней или внешней командой, исполняемой программой или пакетным файлом.Ъ", 1);
                return;
            }
            string[] vs = txt.Split(' ');
            if (vs.Length > 1)
            {
                if (vs[1] == "-uname") { AddTextConsole(GameSrv.OSName + " [" + GameSrv.OS.ToString() + "]"); }
            }
            else { AddTextConsole(GameSrv.OSName); }
        }

        public void Cmd_ver(string txt)
        {
            if (GameSrv.OS != Server.TypeOSEnum.WinSrv)
            {
                AddTextConsole("Ъ`Red=\"ver\" неверно введенная команда или такой исполнительный файл отсутствуетЪ", 1);
                return;
            }
            string[] vs = txt.Split(' ');
            if (vs.Length > 1)
            {
                if (vs[1] == "-os") { AddTextConsole(GameSrv.OSName); }
            }
            else
            {
                AddTextConsole("Версия системы:" + GameSrv.OSName);
                AddTextConsole("Брандмауер:    " + GameSrv.FireWall);
            }
        }
            
        public void Cmd_exist(string txt)
        {
            try
            {
                string[] vs = txt.Split(' ');
                vs[1] = vs[1].Replace('\\', '/');

                if (vs[1].Substring(0, 1) != "/") vs[1] = Pwd + "/" + vs[1];

                FileServerClass file = FileServerClass.GetFile(vs[1], GameSrv);

                if (file == null)
                {
                    AddTextConsole("Нет файла по этому адрес " + vs[1]);
                }
                else
                {
                    AddTextConsole("Имя файла ::  Ъ`White=" + file.FileName + "Ъ");
                    AddTextConsole("Размер ::  Ъ`White=" + file.Size + "Ъ");
                    AddTextConsole("Доступ ::  Ъ`White=" + file.Rights.ToString() + "Ъ");
                    AddTextConsole("Путь ::  Ъ`White=" + Pwd + "Ъ");
                    AddTextConsole("Ъ`GreenYellow=( " + file.FileСontents.TypeInformation.ToString() + " )Ъ");
                }
            }
            catch (Exception)
            {
                AddTextConsole("Ъ`Red=Неверно составлена команда, нужен точный путь к файлу Ъ");
            }

        }

        public void Cmd_ping(string txt)
        {
            string[] vs = txt.Split(' ');
            try
            {
                if (vs.Length == 1)
                {
                    AddTextConsole("Ъ`Red=Вы не указали название сервера Ъ");
                    return;
                }
                AddTextConsole(" Ъ`White= Обмен пакетами по 32 байта: " + vs[1] + " Ъ");

                int act = 5;
                if (vs.Length == 3) { act = int.Parse(vs[2][2..]); }

                var v = App.GameGlobal.Servers.Find(x => x.NameSrv.ToLower() == vs[1].ToLower());

                if (v == null)
                {
                    for (int i = 0; i < act; i++)
                    {
                        AddTextConsole("Ъ`White= Заданный узел недоступен", 8);
                    }
                    return;
                }

                if (v.ActSrv == false)
                {
                    AddTextConsole("Ъ`White= Ответ от " + v.IP + ": число байт=32 время<" + v.Ping.ToString() + "мс TTL=128Ъ", 8);
                    AddTextConsole("Ъ`White= Сервер не отвечает из-за перезагрузки", 8);
                }
                else
                {
                    for (int i = 0; i < act; i++)
                    {
                        AddTextConsole("Ъ`White= Ответ от " + v.IP + ": число байт=32 время<" + v.Ping.ToString() + "мс TTL=128Ъ", 8);
                    }
                }
                ClipBoard = v.IP;
            }
            catch (Exception)
            {
                AddTextConsole("Ъ`Red= Ошибка синтаксиса комманды", 1);
            }
        }

        public void Cmd_mail(string txt)
        {
            string[] vs = txt.Split(' ');
            MailInBox w = null;

            // Проверка на сущ. id 
            Func<bool> chk = () =>
            {
                if (vs.Length < 2) { AddTextConsole("Ошибка синтаксиса комманды, не указан id"); return false; }
                if (int.TryParse(vs[2], out int id) == false) { AddTextConsole("Ошибка синтаксиса комманды, нет значения " + vs[2]); return false; }
                if (GameSrv.Mails.Count - 1 < id || id <= -1) { AddTextConsole("Ошибка синтаксиса комманды, почта с таким id не найден!"); return false; }
                w = GameSrv.Mails[id];
                return true;
            };

            // выборка комманд
            switch (vs[1])
            {
                case "open":
                    FrmSoft.Mail mail = new Mail();
                    mail.ShowForm();
                    break;
                case "ls":
                case "list":
                    int i = 0;
                    string b;
                    foreach (var item in GameSrv.Mails)
                    {
                        b = item.ReadMail ? "V" : " ";
                        AddTextConsole($" id: {i} [{b}] Тема: {item.Title} Дата: {item.DateTo:dd/mm/yy}");
                        i++;
                    }
                    break;
                case "read":
                    if (chk() == false) return;
                    AddTextConsole("Сообщение от : Ъ`CadetBlue=" + w.Mailto + "Ъ");
                    AddTextConsole("Сообщение для : Ъ`CadetBlue=user@local.comЪ");
                    AddTextConsole("Тема письма : Ъ`Green=" + w.Title + "Ъ");
                    string[] body = w.BodyText.Split('\n');
                    foreach (var item in body) AddTextConsole("Ъ`White=" + item.Substring(0, Math.Min(item.Length, MAX_STRING)) + "Ъ");
                    w.ReadMail = true;
                    break;
                case "action":
                case "act":
                    if (chk() == false) return;
                    if (w.CommandList == null) AddTextConsole("Нет прикреплений к этому письму", 5);
                    else w.CommandList.Run();
                    break;
                default:
                    AddTextConsole("Не найден такой параметор!");
                    break;
            }
        }

        public void Cmd_cd(string txt)
        {
            string[] vs = txt.Split(' ');
            try
            {
                vs[1] = vs[1].Replace('\\', '/');
                vs[1] = vs[1].Replace("%20", " ");
                if (vs[1] == "../" | vs[1] == "/../")
                {
                    string[] c = Pwd.Split('/');
                    vs[1] = "/";
                    for (int i = 0; i < c.Length - 1; i++)
                    {
                        vs[1] += c[i] + "/";
                    }
                    vs[1] = vs[1].Replace("//", "/");
                }
                else if (vs[1] == "download" | vs[1] == "Download") vs[1] = "/user/Hpro4/Download/";
                else if (vs[1].Substring(0, 1) != "/") vs[1] = Pwd + vs[1]; // эта строка должна быть всегда в конце этого условия               


                if (FileServerClass.ExistDir(vs[1], GameSrv))
                {
                    AddTextConsole("dir::" + vs[1]);
                    Pwd = vs[1];
                }
                else
                {
                    AddTextConsole("Ъ`Red=Такой каталог не найден : Ъ Ъ`White=" + vs[1] + " Ъ");
                }
            }
            catch (Exception)
            {
                AddTextConsole("Ъ`Red=Неверное написание команды, синтаксиса Ъ");
            }

        }

        public void Cmd_route(string txt)
        {
            string[] vs = txt.Split(' ');
            if (vs.Length > 3)
            {
                AddTextConsole("Ъ`Red=Вы не указали название сервера или серверов Ъ");
                return;
            }

            Server server1 = App.GameGlobal.FindServer(vs[1]);
            Server server2 = App.GameGlobal.FindServer(vs[2]);
            Action action = ()=>{ };

            AddTextConsole(vs[1]+"=>", 2);
            if (server1 == null)
            {
                AddTextConsole("Ъ`Red=Этот сервер не доступен Ъ" + vs[1]);
                return;
            }
            else if (server2 == null)
            {
                AddTextConsole("Ъ`Red=Сервер к которому нужно подключиться не найден  Ъ" + vs[2]);
                return;
            }
            AddTextConsole(vs[1] + "=>"+ vs[2], 4,false);
            AddTextConsole(server1.IP + "port 11" , 4);
            AddTextConsole(server2.IP + "port 11", 4);
            AddTextConsole("Создание маршрута: Ъ`White=[" + server1.IP + "]Ъ >> Ъ`White=[" + server2.IP + "]Ъ", 8);

            if (server1.Premision == Server.PremissionServerEnum.none) {
                AddTextConsole("Невозможно установить соеденение с сервером " + server1.NameSrv,2);
                AddTextConsole("Нет права доступа к настройкам сервера",2);
                AddTextConsole("Cancel Route: Ъ`White=[" + server1.IP + "]Ъ >> Ъ`White=[" + server2.IP + "]Ъ", 2);
                return;
            }
            AddTextConsole("Маршрут ...",5);
            var f = App.GameGlobal.Routers.Find(x => x.FirstServer.NameSrv == server1.NameSrv & x.EndServer.NameSrv == server2.NameSrv);
            if (f == null)
            {                
                AddTextConsole("Маршрут Ъ`Green= создан новый маршрутЪ", 15,false);
               action = ()=> App.GameGlobal.AddRouter(server1, server2);                                 
            }
            else AddTextConsole("Маршрут Ъ`Red= уже существуетЪ", 15, false);
            AddTextConsole("Завершение сценария (0) Exit", action, 1);
        }

        public void Cmd_getmoney(string txt)
        {
            if (App.CheatCode)
            {
                try
                {
                    string[] vs = txt.Split(' ');
                    App.GameGlobal.Bank.DefaultBankAccount.Money += int.Parse(vs[1]);
                    AddTextConsole("Вы читер!!!");
                }
                catch
                {
                    AddTextConsole("");
                }
            }
        }

        public void Cmd_addextrapoint(string txt)
        {
            if (App.CheatCode)
            {
                try
                {
                    string[] vs = txt.Split(' ');
                    byte i = byte.Parse(vs[2]);
                    App.GameGlobal.GamerInfo.ExtraPoint += i;
                    AddTextConsole("Вы читер!!!");
                }
                catch
                {
                    AddTextConsole("");
                }
            }
        }

        public void Cmd_openscen(string txt)
        {
            if (App.CheatCode)
            {
                string[] vs = txt.Split(' ');
                var r = App.GameGlobal.GameScen.GetType().GetMethod("Scen_"+vs[1]);
                if (r == null)
                {
                    MessageBox.Show("Нет такого сценария " + vs[1]);
                }
                else
                {
                    App.GameGlobal.GameScen.ActiveScen = (GameScen.ScenStruct)r.Invoke(App.GameGlobal.GameScen, new object[] { });
                }
                AddTextConsole("Вы читер!!!");
            }
        }

        public void Cmd_servercontrol(string txt)
        {
            if (App.CheatCode)
            {
                string[] vs = txt.Split(' ');
                Server srv = App.GameGlobal.FindServer(vs[1]);
                srv.Premision = Server.PremissionServerEnum.FullControl;
                AddTextConsole("Вы читер!!!");
            }
        }

        public void Cmd_opensrv(string txt)
        {
            if (App.CheatCode)
            {
                string[] vs = txt.Split(' ');
                Server srv = App.GameGlobal.FindServer(vs[1]);
                ConnectServer(srv);
                AddTextConsole("Вы читер!!!");
            }
        }

        public void Cmd_enter(string txt)
        {
            if (App.CheatCode)
            {
                string[] vs = txt.Split(' ');
                App.GameGlobal.MainWindow.ОткрАдминПанел( null,null);
                AddTextConsole("Вы читер!!! " + vs[0]);
            }
        }

        public void Cmd_eventlist(string txt)
        {
            if (App.CheatCode)
            {
                string[] vs = txt.Split(' ');
                foreach (var item in App.GameGlobal.AllEventGame )
                {
                    AddTextConsole(item .ToString () + vs[0]);
                }
               
            }
        }

        private FileServerClass OpenFileEx(string file)
        {
            try
            {               
                if (file.Substring(0, 5) == "~FF00")
                {
                    try
                    {
                        int ii = int.Parse(file[4..]);
                        file = FF00_ls[ii];
                    }
                    catch (Exception)
                    {
                        App.GameGlobal.Msg("Системная ошибка", "Критическая ошибка. Индекс находиться за пределами массива. Приложение будет закрыто ", FrmError.InformEnum.Критическая_ошибка);
                        this.Close();
                    }
                }
                file = file.Replace("%20", " "); // Экранизация
                // Поиск файла по пути или но названию в текущей папке
                if (FileServerClass.Exist(Pwd, FileServerClass.PatchToFileNamePerfix(file), GameSrv) == false)
                {
                    //поиск по названию в этой папке
                    if (FileServerClass.Exist(FileServerClass.PatchOnly(file), FileServerClass.PatchToFileNamePerfix(file), GameSrv) == false)
                    {
                        AddTextConsole("Ъ`Red=Файл указан не верно: нет такого файла по пути " + file + " Ъ");
                        return null;
                    }
                }
                else
                {
                    file = Pwd + file;
                }

                // Проверка доступа к файлу
                if (FileServerClass.CheckAccess(file, GameSrv) == false)
                {
                    AddTextConsole("Ъ`Red=(OpenFileEx) Нет доступа к файлу." + file + " Ъ");
                    return null;
                }
                return FileServerClass.GetFile(file, GameSrv);
            }
            catch
            {
                AddTextConsole("Ъ`Red=Ошибка к обращению файлу " + file + " Ъ");
                return null;
            }
        }
        private void ExploidRun()
        {
            AddTextConsole("");
            AddTextConsole("Find Port...");
            Server.Port? t = BufferConsole.Srv.Ports.Find(x => x.Num == ushort.Parse(BufferConsole.NumberPort));
            if (t.Value.NameTitle == null)
            {
                AddTextConsole("Ъ`Red=Такого порта не существует тутЪ");
            }
            else
            {
                Vulnerabilities v = App.GameGlobal.VulnerabilitiesList.Find(x => x.ID == BufferConsole.FileStart.FileСontents.IntParam);
                if (t.Value.NameTitle == v.CName)
                {
                    // Успех в работе
                    SuccessTxt(BufferConsole.FileStart.FileСontents.ByteParam);
                    BufferConsole.Srv.Premision = v.GrantPremission;
                    App.GameGlobal.LogAdd("Сервер взломан " + BufferConsole.ServerName + " Теперь у вас есть новые привилегии", Enums.LogTypeEnum.Server);
                }
                else
                {
                    // Не удача в работе
                    ErrorTxt(BufferConsole.FileStart.FileСontents.ByteParam);
                }
            }
            BashConsole();
        }
        private void Off_Server() {
            GameSrv = App.GameGlobal.MyServer;
            Pwd = "/";
            TimerAdminLog.Content = "---";
            TimerAdminStr.Content = "";            
        }
        private void ConnectServer(Server srv)
        {
            AddTextConsole("Ъ`BlueViolet= ***************************************************************** Ъ", 1);
            AddTextConsole("Ъ`BlueViolet= ******************       CONNECT      *************************** Ъ", 1);
            AddTextConsole("Ъ`BlueViolet= ***************************************************************** Ъ", 1);

            GameSrv = srv;
            Pwd = "/";
            TimerLog = 10000 - (srv.PopularSRV*100);        // Устанавливает начальный таймер время нахождения на серваке
            AddTextConsole("Ъ`BlueViolet= Вход в сервер выполнен. Ъ", 1);
            AddTextConsole("Ъ`BlueViolet= Важно: Оставаясь подключенным к серверу вам могут вычислить  Выход из сервера connect -off Ъ", 1);
            AddTextConsole("Ъ`BlueViolet=   Выход из сервераЪ Ъ`White=connect -off Ъ", 1);
            App.GameGlobal.EventIntroduce(EventEnum.ВходНаСервер,  GameSrv.NameSrv );
        }
    }
    //***************************************************************************************************************************************************************************
    //***************************************************************************************************************************************************************************
    //***************************************************************************************************************************************************************************
    /// <summary>
    /// Данные контента
    /// </summary>
    public partial class Cmd
    {

        private readonly BufferCmd BufferConsole = new BufferCmd();
        private class BufferCmd
        {
            public FileServerClass FileStart { get; set; }
            public string ServerName { get; set; }
            public string ServerIP { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string NumberPort { get; set; }
            public Server Srv { get; set; }

            public NextMetod WaitCommand;
            public delegate void NextMetod();

            public void Clear()
            {
                FileStart = null;
                ServerName = "";
                ServerIP = "";
                NumberPort = "";
                Login = "";
                Password = "";
                Srv = null;
                WaitCommand = null;
            }
        }

        private void UpperTxt(int i)
        {
            switch (i)
            {
                case 1:
                    AddTextConsole("Virtual disk X: created !", 3);
                    AddTextConsole("", 6);
                    AddTextConsole("perl " + BufferConsole.FileStart.FileName, 2);
                    AddTextConsole("expl.c", 5);
                    AddTextConsole("echo ini.ini>name=" + BufferConsole.ServerName, 2);
                    AddTextConsole("readme.text", 3);
                    AddTextConsole("test.php", 2);
                    AddTextConsole("gcc expl.c", 9);
                    AddTextConsole("----------------------------------------------------------------------", 1);
                    AddTextConsole("|                     Exploit_start 2000                             |", 1);
                    AddTextConsole("----------------------------------------------------------------------", 1);
                    AddTextConsole("loading . . . .", 8);
                    break;
                case 2:
                    AddTextConsole("perl " + BufferConsole.FileStart.FileName + " -configure");
                    AddTextConsole("connect=" + BufferConsole.ServerName + @" // connect server");
                    AddTextConsole("target=" + BufferConsole.ServerIP + "// active");
                    AddTextConsole("interface run", 8);
                    AddTextConsole("Ъ`Blue=SYSTEM cons=" + BufferConsole.FileStart.FileName + ";; 28Ъ", 3);
                    AddTextConsole("perl " + BufferConsole.FileStart.FileName + " -run");
                    AddTextConsole("Ъ`White=go|| progress.t1=1780Ъ", 2);
                    AddTextConsole("Ъ`White=  || progress.t2=445Ъ", 4);
                    AddTextConsole("Ъ`White=  || progress.t3=5566Ъ", 5);
                    AddTextConsole("Ъ`White=  || x=342; z=334Ъ ", 6);
                    AddTextConsole("Ъ`White=  || OS = indeberaЪ", 6);
                    AddTextConsole("Ъ`White=  || R~05432.textЪ", 4);
                    AddTextConsole("Ъ`White=  || R~33344.textЪ", 1);
                    AddTextConsole("Ъ`White=  || endЪ", 1);
                    break;
                case 3:
                    AddTextConsole(" COPY tmp/ " + BufferConsole.FileStart.FileName, 1);
                    AddTextConsole("ok copy");
                    AddTextConsole("perl tmp/" + BufferConsole.FileStart.FileName, 16);
                    AddTextConsole("tmp/expl.pl");
                    AddTextConsole("tmp/team ERR_2560.txt", 4);
                    AddTextConsole("tmp/perl expl.pl", 2);
                    AddTextConsole(" :expl>connect " + BufferConsole.FileStart.FileName, 4);
                    AddTextConsole(" :expl>runing  " + BufferConsole.ServerName + "", 2);
                    break;
                case 4:
                    AddTextConsole("Ъ`Green=   //start poc=SID=128989Ъ", 3);
                    AddTextConsole("Ъ`Green=    //uptime 16:04Ъ", 4);
                    AddTextConsole("Ъ`Green=    //connect::Ъ" + BufferConsole.ServerName, 5);
                    AddTextConsole("Ъ`Green=    //port:1234Ъ", 3);
                    AddTextConsole("Ъ`Green=    //timelimit:3320Ъ", 4);
                    AddTextConsole("Ъ`Green=    //echo:23-29Ъ", 5);
                    AddTextConsole("Ъ`Green=    //ping`s 44Ъ", 6);
                    break;
                case 5:
                    AddTextConsole("perl " + BufferConsole.FileStart.FileName + ">root/temp/", 2);
                    AddTextConsole("expl.php", 4);
                    AddTextConsole("/post/", 6);
                    AddTextConsole("/post/readme.me", 3);
                    AddTextConsole("/post/php.txt", 5);
                    AddTextConsole("/post/~b" + BufferConsole.FileStart.GetHashCode().ToString() + ".txt", 3);
                    AddTextConsole("php.exe -run expl.php?url=" + BufferConsole.ServerName + ";", 3);
                    AddTextConsole("Ъ`Blue=active port 127.0.0.1:80 " + BufferConsole.ServerName + ";Ъ", 1);
                    break;
                case 6:
                    AddTextConsole("open ~9494930.dat", 2);
                    AddTextConsole(" <x44x44x66x23x33x84x40x04x04x54x44x55x09x01", 4);
                    AddTextConsole(" <end", 4);
                    AddTextConsole("send file>~9494930.dat," + BufferConsole.ServerName, 12);
                    AddTextConsole("send . . . .", 3);
                    break;
                case 7:
                    AddTextConsole("MKDIR root/test/");
                    AddTextConsole("perl root/test/ > " + BufferConsole.FileStart.FileName + "!");
                    AddTextConsole("ex.pl");
                    AddTextConsole("!perl root/test/ex.pl -www " + BufferConsole.ServerName, 8);
                    AddTextConsole("", 4);
                    AddTextConsole("$explo='" + BufferConsole.FileStart.FileName + ":" + BufferConsole.ServerName + "'; ", 3);
                    break;
                case 8:
                    AddTextConsole("perl " + BufferConsole.FileStart.FileName + ".pl", 1);
                    AddTextConsole("e.pl", 3);
                    AddTextConsole("perl e.pl -url " + BufferConsole.ServerName + "*", 6);
                    AddTextConsole("             ______________", 1);
                    AddTextConsole(@"            /  /________\  \", 1);
                    AddTextConsole(@"           /  /          \  \", 1);
                    AddTextConsole(@"          /  /     ||     \  \", 1);
                    AddTextConsole(@"         /  /     =\/=     \  \", 1);
                    AddTextConsole(@"        /  /       ||       \  \", 1);
                    AddTextConsole(@"       /  /        ||        \  \", 1);
                    AddTextConsole(@"                   \/", 1);
                    break;
                case 9:
                    AddTextConsole("perl " + BufferConsole.FileStart.FileName + ".pl", 7);
                    AddTextConsole("create expl.exe", 2);
                    AddTextConsole("run expl.exe -host " + BufferConsole.ServerName, 3);
                    AddTextConsole("run expl.exe", 4);
                    AddTextConsole("                         ----------", 1);
                    AddTextConsole(@"                       /            \", 1);
                    AddTextConsole(@"                      |              |", 1);
                    AddTextConsole(@"                      |,  .-.  .-.  ,|", 1);
                    AddTextConsole(@"                      | )(_o/  \o_)( |", 1);
                    AddTextConsole(@"                      |/     /\     \|", 1);
                    AddTextConsole(@"            (@_       (_     ^^     _)", 1);
                    AddTextConsole(@"       _     ) \_______\__|IIIIII|__/_______________________", 1);
                    AddTextConsole(@"      (_)@8@8{}<________|-\IIIIII/-|________________________>", 1);
                    AddTextConsole(@"             )_/        \          /", 1);
                    AddTextConsole("            (@           `--------`", 1);
                    AddTextConsole(" 'exploit of " + BufferConsole.FileStart.FileName.Replace(".pl", "") + " Max Paint (c) 2001-2002 ", 1);
                    break;
                default:
                    AddTextConsole("Start...//", 3);
                    AddTextConsole("///*", 8);
                    break;
            }
        }

        private void ErrorTxt(int i)
        {
            switch (i)
            {
                case 0:
                    AddTextConsole("Ъ`Red= :expl>error   Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 1:
                    AddTextConsole("Ъ`Red= :expl>error   Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 2:
                    AddTextConsole("Ъ`Red=  :" + BufferConsole.ServerIP + ">error      Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 3:
                    AddTextConsole("Ъ`Red= error 354   Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 4:
                    AddTextConsole("Ъ`Red=error:: #41#   Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 5:
                    AddTextConsole("Ъ`Red= :expl>error   Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 6:
                    AddTextConsole("Ъ`Red= Parse error: 541   Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 7:
                    AddTextConsole("Ъ`Red= >error 51FF004-45151  Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 8:
                    AddTextConsole("Ъ`Red=error::: Нет доступа  Ъ", 2);
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
                case 9:
                    AddTextConsole("Программа не работает  ", 2);
                    AddTextConsole("Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена ", 1);
                    break;
                default:
                    AddTextConsole("Ъ`Red=Ошибка: Порт не соответствует данной уязвимости или эта уязвимость была исправлена Ъ", 1);
                    break;
            }
        }

        private void SuccessTxt(int i)
        {
            switch (i)
            {
                case 1:
                    AddTextConsole("OK. Работает доступ к серверу <o0kf>", 3);
                    break;
                case 2:
                    AddTextConsole("Ъ`White=  || fde-auЪ", 4);
                    AddTextConsole("Ъ`White=  || Trance=#EE334Ъ", 3);
                    AddTextConsole("Ъ`White=  || FFA34-W2234Ъ", 2);
                    AddTextConsole("Ъ`White=  || endЪ", 1);
                    AddTextConsole("Ъ`White= Успех в работе программы!Ъ", 1);
                    break;
                case 3:
                    AddTextConsole(" :expl>OK  ", 2);
                    AddTextConsole("DELDIR tmp/", 2);
                    AddTextConsole("Открыт доступ...", 3);
                    break;
                case 4:
                    AddTextConsole("Ъ`Green=   //..09911Ъ", 4);
                    AddTextConsole("Ъ`Green=   //...9922Ъ", 4);
                    AddTextConsole("Завершено", 3);
                    break;
                case 5:
                    AddTextConsole("kill root/temp/expl.php", 2);
                    AddTextConsole("kill root/temp/post/readme.me", 4);
                    AddTextConsole("kill root/temp/post/php.txt", 2);
                    AddTextConsole("kill root/temp/post/~b00045356GFJ4I447G.txt", 7);
                    AddTextConsole("Завершено PHPInfo()", 3);
                    break;
                case 6:
                    AddTextConsole("send ok", 2);
                    AddTextConsole("Вы получили новые права", 3);
                    break;
                case 7:
                    AddTextConsole("DELDIR root/test/", 3);
                    AddTextConsole("Успешно завершено!", 3);
                    break;
                case 8:
                    AddTextConsole("kill e.pl", 3);
                    AddTextConsole("ok прога работает", 3);
                    break;
                case 0:
                    AddTextConsole("       =ok=      Работает... Успешно ... Ошибок (0)", 7);
                    AddTextConsole("------------------------------------------------------------------", 6);
                    AddTextConsole("------------------------------------------------------------------", 4);
                    break;
                case 9:
                    AddTextConsole("kill expl.pl", 1);
                    AddTextConsole("Скрипт выполнен без ошибок", 7);
                    break;
                default:
                    AddTextConsole("OK. ", 3);
                    break;
            }
        }

    }

}
