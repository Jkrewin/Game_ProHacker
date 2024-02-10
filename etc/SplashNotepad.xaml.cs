using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PH4_WPF.etc
{
    public partial class SplashNotepad : Window
    {
        readonly Engine.Server _server;

        public SplashNotepad(Engine.FileServerClass file , Engine.Server server )
        {
            InitializeComponent();
            Rtf.AppendText(server.FileTextInfo [file.FileName]);
            LabelFileName.Content = file.FileName;
            _server = server;
        }

        private void ЗакрытаФорма(object sender, EventArgs e) {
            var txt = new System.Windows.Documents.TextRange(Rtf.Document.ContentStart, Rtf.Document.ContentEnd);
           _server.FileTextInfo[LabelFileName.Content.ToString()] = txt.Text;
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName); 
        }

        private void ButtonChColor(ref object button)
        {
            var b = button as Label;
            var bc = new BrushConverter();
            Brush color = (Brush)bc.ConvertFrom("#FF5C82DC");
            if (b.Background != color) b.Background = color;
        }

        private void ButtonBackcolor(ref object button)
        {
            var b = button as Label;
            var bc = new BrushConverter();
            Brush color = (Brush)bc.ConvertFrom("#FF39A4DA");
            b.Background = color;
        }

        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void МышкаНадОб(object sender, MouseEventArgs e)
        {
            ButtonChColor(ref sender);
        }

        private void МышкаПокинула(object sender, MouseEventArgs e)
        {
            ButtonBackcolor(ref sender);
        }

        private void ВыбратьСвернуть(object sender, MouseButtonEventArgs e)
        {
            this.Height = this.Height > 27 ? 26 : 700;
        }

        private void КнопкаЗакрыть(object sender, MouseButtonEventArgs e) => this.Close();
    }
}
