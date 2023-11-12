using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PH4_WPF.FrmSoft
{  
    public partial class FrmError : Window
    {
        private readonly System.Windows.Threading.DispatcherTimer SetupTimer = new System.Windows.Threading.DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(50) };
      
        public NextMetod WaitCommand;
        public delegate void NextMetod();

        public FrmError(string title, string txt, InformEnum inform )
        {
            InitializeComponent();
            TitleMsg.Content = title;
            MessText.Text = txt;
            this.Show();

            SetupTimer.Tick += new EventHandler(SetupProgress);
            SetupBar.Visibility = Visibility.Hidden;

            switch (inform)
            {
                case InformEnum.Информация:
                    Img.Source = new BitmapImage(new Uri(App.PatchAB + @"msg\inform.png"));
                    break;
                case InformEnum.Критическая_ошибка:
                    Img.Source = new BitmapImage(new Uri(App.PatchAB + @"msg\error.png"));
                    break;
                case InformEnum.УстановкаПрограммы:
                    Img.Source = new BitmapImage(new Uri(App.PatchAB + @"msg\download.png"));
                    this.Height = 21;//208
                    SetupBar.Value = 0;
                    SetupBar.Visibility = Visibility.Visible;
                    SetupTimer.Start();
                    break;
                case InformEnum.СообщениеОтПрограмимы :
                    Img.Source = new BitmapImage(new Uri(App.PatchAB + @"msg\soft.png"));
                    break;
                case InformEnum.ЕстьПроблемы:
                default:
                    Img.Source = new BitmapImage(new Uri(App.PatchAB + @"msg\problems.png"));
                    break;
            }            
        }

        public FrmError(string title, string txt, NextMetod metod )
        {
            InitializeComponent();
            TitleMsg.Content = title;
            MessText.Text = txt;
            this.Show();
            SetupBar.Visibility = Visibility.Hidden;
            Img.Source = new BitmapImage(new Uri(App.PatchAB + @"msg\qest.png"));
            OK_Button.Content = "Отмена";
            WaitCommand = metod;
            RunButton.Visibility = Visibility.Visible;

        }

        private void SetupProgress(object sender, EventArgs e)
        {
            if (SetupBar.Maximum == SetupBar.Value) {
                this.Height = 208;
                SetupBar.Visibility = Visibility.Hidden;
                SetupTimer.Stop();
            }
            SetupBar.Value += 1;           
        }

        public enum InformEnum { 
         Информация,
         Критическая_ошибка,
         УстановкаПрограммы,
         СообщениеОтПрограмимы,
         ЕстьПроблемы
        }

        private void КнопкаОК(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void КнопкаRun(object sender, RoutedEventArgs e)
        {
            WaitCommand.Invoke();
            this.Close();
        }
    }
}
