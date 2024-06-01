using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace PH4_WPF.FrmSoft
{
  
    public partial class FrmApp : Window
    {
        private short _cluster =1;
        private short SelectedHash=-1;       
        private Engine.Server SelectedSrv;
        private readonly System.Windows.Threading.DispatcherTimer BruteTimer = new System.Windows.Threading.DispatcherTimer();
        private short HashWorker;
        private bool ResultPwd;
        private bool _isWork  = false;
        private bool IsWork { get => _isWork; 
            set {
                _isWork = value;
                if (_isWork)
                {
                    BruteTimer.Start();
                    ButtonStart.Background = Brushes.ForestGreen;
                    ButtonStart.Content = "Стоп";
                }
                else {
                    BruteTimer.Stop();
                    ButtonStart.Background = null;
                    ButtonStart.Content = "Старт";
                } } }
        private short Сluster { 
            get => _cluster;
            set {
                _cluster = value;
                LabelСluster.Content = value + "m/в день";            
            }
        }

        public FrmApp()
        {
            InitializeComponent();
            App.GameGlobal.MainWindow.NewDayEvent += StepHash;
        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {

            //Настройка таймера анимации
            BruteTimer.Tick += new EventHandler(BruteTick);
            BruteTimer.Interval = TimeSpan.FromMilliseconds(150);
            BruteTimer.Stop();

            Сluster = 1;
        }
         
        private void StepHash(int days) {        
            if (_isWork == false) return;

            switch (App.GameGlobal.GameSpeed)
            {
                case Game.GameSpeedEnum.Speed1X:
                    HashWorker--;
                    break;
                case Game.GameSpeedEnum.Speed2X:
                    HashWorker -= 2;
                    break;
                case Game.GameSpeedEnum.Speed4X:
                    HashWorker -= 4;
                    break;
                default:
                    break;
            }

            // процесс завершен подбора
            if (HashWorker < 0)
            {
                string s;
                if (ResultPwd)
                {
                    // готовый пароль тут 
                    s = "Успех найден [Логин:Пароль] " + SelectedSrv.LoginAndPass + " для сервера " + SelectedSrv.NameSrv;
                    App.GameGlobal.EventIntroduce(Enums.ConditionEnum.ПодборПароляЗавершен, SelectedSrv.NameSrv, SelectedSrv.LoginAndPass);
                }
                else
                {
                    // пароль не найден из-за сложности 
                    s = "Логин и пароль невозможно подобрать из-за сложности или защиты";
                    App.GameGlobal.EventIntroduce(Enums.ConditionEnum.ПодборПароляЗавершен, SelectedSrv.NameSrv, "");
                }
                App.GameGlobal.LogAdd(s, Enums.LogTypeEnum.Server);
                InfoProcess.Content = s;
                IsWork = false;
               
            }
        }

        private void BruteTick(object sender, EventArgs e) {
            if (App.GameGlobal.GameSpeed == Game.GameSpeedEnum.Pause) {
                LabelResBrute.Content = "[ПАУЗА] Запустите таймер. ";
                return;
            }
            
            var rnd = new Random();
            char[] cr = new char[30];

            for (int i = 0; i < cr.Length; i++)           
                cr[i] = Convert.ToChar(rnd.Next(60, 126));

            LabelResBrute.Content = new string  (cr);
        }

        private void ФормаЗакрыта(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            App.GameGlobal.MainWindow.NewDayEvent -= StepHash;
        }
        private void Выделяет_кнопку_выход(object sender, MouseEventArgs e) => ExitButton.Fill = Brushes.IndianRed;
        private void ПрекращаетВыделение(object sender, MouseEventArgs e)=> ExitButton.Fill = Brushes.DarkRed;
        private void УдалениеКнопка(object sender, MouseButtonEventArgs e)=> this.Close();

        private void КнопкаТипаХешей(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            Button[] buttons = new Button[3];
            buttons[0] = ButtonA;
            buttons[1] = ButtonB;
            buttons[2] = ButtonC;

            foreach (var item in buttons) item.Background = (Brush)bc.ConvertFrom("#FF2F2E2E");

            var b = sender as Button;
            SelectedHash = short.Parse ( b.Tag.ToString ());
            switch (SelectedHash)
            {
                case 1:
                    // BTC

                    break;
                case 2:
                    // ETH


                    break;
                default:
                    // hash
                   if (T_Search.Text=="") T_Search.Background = (Brush)bc.ConvertFrom("#FF743737");
                    break;
            }
            buttons[SelectedHash].Background = Brushes.ForestGreen;            
        }

        private  void ПоискСвоихРигов(object sender, RoutedEventArgs e)
        {
            if (ButtonFinder.Background == Brushes.Red) return;

            var result = (from tv in App.GameGlobal.Servers 
                         where tv.Premision == Engine.Server.PremissionServerEnum.FullControl || tv.Premision == Engine.Server.PremissionServerEnum.UserControl 
                         select tv).ToArray();                        
            ButtonFinder.Background = Brushes.Red;

            Task task = new Task(() => {
                short s = 0;
                foreach (var item in result)
                {
                    if (item.VirtualizationServer != null)
                    {
                        Thread.Sleep(1000);
                        if (item.VirtualizationServer.Instance.Any(x => x.InstaceType == Enums.InstaceTypeEnum.RigFrame))
                        {
                            s++;
                            this.Dispatcher.Invoke(() => ListBoxServer.Items.Add(item.NameSrv));
                        }

                    }
                }
                this.Dispatcher.Invoke(() => 
                { 
                    if (s == 0) InfoProcess.Content = "Только сервера с установленной службой перебора хещей (RigFrame) может быть объединён в общую сеть "; 
                    ButtonFinder.Background = null; 
                    Сluster = (short)(s + 1); 
                });
                
            });

            task.Start();
        }

     

        private void МышкаУшлаКнопки(object sender, MouseEventArgs e)
        {
            LabelError.Visibility = Visibility.Hidden;
        }

        public void ФокусПотерян(object sender, RoutedEventArgs e)
        {
            SelectedSrv = App.GameGlobal.FindServer(T_Search.Text);
            if (SelectedSrv == null) T_Search.Background = Brushes.DarkRed;
            else T_Search.Background = null;
        }

        private void ЗапускПереборщика(object sender, RoutedEventArgs e)
        {
            if (IsWork)
            {
                // тут поиск остановлен
                IsWork = false;
            }
            else
            {
                // Тут поиск запущен
                if (SelectedHash == 0)
                {
                    if (SelectedSrv == null)
                    {
                        LabelError.Visibility = Visibility.Visible;
                        LabelError.Content = "Сервер не найден или вы не указали как целевой";
                        return;
                    }
                    if (SelectedSrv.LoginAndPass == "") SelectedSrv.CreateLoginPass();
                    HashWorker = (short)((SelectedSrv.PopularSRV * 8) - Сluster); // количество дней для подбора 
                    ResultPwd = App.GameGlobal.GamerInfo.HiTecLevel >= SelectedSrv.PopularSRV;//будет найден пароль или нет
                    IsWork = true;
                }
                else
                {
                    LabelError.Visibility = Visibility.Visible;
                    LabelError.Content = "Вы не указали тип хешей!";
                }
            }
        }

        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void ЗапускПереборщика(object sender, KeyEventArgs e)
        {

        }
    }
}
