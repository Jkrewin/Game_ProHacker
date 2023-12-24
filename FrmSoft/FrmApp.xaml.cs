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
  
    public partial class FrmApp : Window
    {
        private readonly System.Windows.Threading.DispatcherTimer BruteTimer = new System.Windows.Threading.DispatcherTimer();
        public FrmApp()
        {
            InitializeComponent();
        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {

            //Настройка таймера анимации
            BruteTimer.Tick += new EventHandler(BruteTick);
            BruteTimer.Interval = TimeSpan.FromMilliseconds(150);
            BruteTimer.Stop();

                       




        }

        private void BruteTick(object sender, EventArgs e) { 
            var rnd = new Random();
            char[] cr = new char[30];

            for (int i = 0; i < cr.Length; i++)           
                cr[i] = Convert.ToChar(rnd.Next(60, 126));

            LabelResBrute.Content = new string  (cr);
        }

    }
}
