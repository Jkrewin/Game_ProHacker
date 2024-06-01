using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PH4_WPF.etc
{
    
    public partial class FrmSettings : Window
    {
        private const string COLOR_S = "#FF59A714"; // Color of the selected line

        private readonly System.Windows.Threading.DispatcherTimer AnimationAV = new System.Windows.Threading.DispatcherTimer();
        private readonly System.Windows.Threading.DispatcherTimer ScanHDDTimer = new System.Windows.Threading.DispatcherTimer();
        private bool InProcess = false;
        private Stack<FileServerClass> StackFile;
        private List<string> Virus;


        public FrmSettings()
        {
            InitializeComponent();
        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {
            ВыборРежимПК(PC_Work, null);
            AnimationAV.Tick += new EventHandler(АнимацияАнтивируса);
            AnimationAV.Interval = TimeSpan.FromMilliseconds(200);
            AnimationAV.Stop();

            ScanHDDTimer.Tick += new EventHandler(АнимацияСканированияДиск);
            ScanHDDTimer.Interval = TimeSpan.FromMilliseconds(200);
            ScanHDDTimer.Stop();

            FinderAV.Visibility = Visibility.Hidden;

            Virus = new List<string>
            {
                "TestVirus"
            };



        }

        private void ListFile(List<FileServerClass> ls)
        {
            foreach (var item in ls)
            {
                if (item.IsDir)
                {
                    StackFile.Push(item);
                    ListFile(item.Dir);
                }
                else StackFile.Push(item);
            }
        }

        private void SelectLabel(Label lab) {
            foreach (var item in Grid_RightPanel.Children )
            {
                if (item is Label l) l.Background = null;
            }            
            lab.Background = App.BrushConv(COLOR_S);
        }

        private void HideAll() {
            GameProcess.Visibility = Visibility.Hidden;
            HDD_Test.Visibility = Visibility.Hidden;
            Link_Uplink.Visibility = Visibility.Hidden;
            AntiVirus.Visibility = Visibility.Hidden;
        }

        private void ВыделитьКружок(object sender, MouseEventArgs e) => ((Ellipse)sender).Fill = Brushes.Red;

        private void ВыходИзКружка(object sender, MouseEventArgs e) => ((Ellipse)sender).Fill = Brushes.OrangeRed;

        private void ВыборРежимПК(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            SelectLabel((Label)sender);
            GameProcess.Visibility = Visibility.Visible;            
        }

        private void Выполнить(object sender, RoutedEventArgs e)
        {
            if (T1.IsChecked == true)
            {
                string str = App.StartUP ? "debuger" : App.GameGlobal.GamerInfo.GameName;
                App.GameGlobal.MainWindow.SaveGame(AppDomain.CurrentDomain.BaseDirectory + @"Save\" + str + ".sav");
                App.GameGlobal.SoundSignal(Enums.Sounds.saveok);
                Close();
            }
            else if (T2.IsChecked == true)
            {
                App.GameGlobal.MainWindow.NewGame();
                Close();
            }
            else if (T3.IsChecked == true)
            {
                Application.Current.Shutdown();
            }
            else if (T4.IsChecked == true)
            {
                // загрузить профиль 
                App.GameGlobal.ActiveApp.AsParallel().ForAll(x => x.Value.Close());
                App.GameStart.Show();
                App.GameStart.СписокИгры(null,null);
                Close();
            }
        }

        private void ПроверкаНавирусы(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            SelectLabel((Label)sender);
            AntiVirus.Visibility = Visibility.Visible;            
        }

        private void ЗакрытьОкно(object sender, MouseButtonEventArgs e) => Close();


        private void ВыполнитьАнтиВирус(object sender, RoutedEventArgs e)
        {
            if (InProcess)
            {
                if (AnimationAV.IsEnabled)
                {
                    ButtonAV.Content = "Продолжить";
                    AnimationAV.Start();
                    AnimationAV.IsEnabled = false;
                }
                else
                {
                    ButtonAV.Content = "Пауза";
                    AnimationAV.Stop();
                    AnimationAV.IsEnabled = true;
                }

            }
            else
            {
                ListVirus.Items.Clear();
                StackFile = new Stack<FileServerClass>();
                ButtonAV.Content = "Пауза";
                ListFile(App.GameGlobal.MyServer.FileSys.Dir);
                InProcess = true;
                PG_InProcess.Visibility = Visibility.Visible;
                AnimationAV.Start();
            }
        }

        private void АнимацияСканированияДиск(object sender, EventArgs e) {
            
            PG_HDD.Value++;

            if (PG_HDD.Value == PG_HDD.Maximum) {

                ScanHDDTimer.Stop();
            }
        }

        private void АнимацияАнтивируса(object sender, EventArgs e)
        {
            if (StackFile.TryPop(out FileServerClass f) == false)
            {
                TextAV.Content = "...";
                InProcess = false;
                AnimationAV.Stop();
                PG_InProcess.Visibility =  Visibility.Hidden;
                ButtonAV.Content = "Начать проверку";
                return;
            }


            TextAV.Content = f.FileName;
            if (Virus.All (x=> x == f.FileСontents.TextCommand )) {
                FinderAV.Visibility = Visibility;
                ListVirus.Items.Add( "Файл заражен: " + f.FileName + " ("+ f.FileСontents.TextCommand +") - удален");
                f.FileDel();
            }
            
        }

        private void ПроверкаHDD(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            SelectLabel((Label)sender);
            HDD_Test.Visibility = Visibility.Visible;
            TV_HDD.Visibility = Visibility.Visible;
        }

        private void HDD_Scan(object sender, RoutedEventArgs e)
        {
            TV_HDD.Visibility = Visibility.Hidden;
            PG_HDD.Maximum = 100;
            PG_HDD.Minimum = 0;
            PG_HDD.Value = 0;
            ScanHDDTimer.Start();
        }

       

        private void ПереходLink(object sender, MouseButtonEventArgs e)
        {
            HideAll();
            SelectLabel((Label)sender);
            Link_Uplink .Visibility = Visibility.Visible;
        }
    }
}
