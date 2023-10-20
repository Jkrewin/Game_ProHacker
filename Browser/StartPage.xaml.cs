using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PH4_WPF.Browser
{
    public partial class StartPage : Page
    {
        public StartPage() => InitializeComponent();
        private void OpenFile(string s) {
            TextRange doc = new TextRange(Rtf.Document.ContentStart, Rtf.Document.ContentEnd);
            using (FileStream fs = new FileStream(App.PatchAB + @"\rtf\"+s+".rtf", FileMode.Open)) doc.Load(fs, DataFormats.Rtf);
        }

        private void ГайдШеллы(object sender, MouseButtonEventArgs e) => OpenFile("shell");
        private void Эксплойты(object sender, MouseButtonEventArgs e) => OpenFile("exploit");
    }
}
