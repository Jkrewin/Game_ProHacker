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

namespace PH4_WPF
{
    /// <summary>
    /// Логика взаимодействия для GameOver.xaml
    /// </summary>
    public partial class GameOver : Window
    {
        public GameOver()
        {
            InitializeComponent();
        }

        public GameOver(string msg)
        {
            InitializeComponent();
            L_Msg.Content = msg;           
        }
    }
}
