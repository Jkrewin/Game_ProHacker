using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;

namespace PH4_WPF.FrmSoft
{
    
    public partial class Mail : Window
    {
        private readonly string PatchAB = App.PatchAB + @"\soft\mail\";

        private ObservableCollection<ListViewItemsData> ListViewItemsCollections = new ObservableCollection<ListViewItemsData>();
     

        public Mail()
        {
            InitializeComponent();
            LsMail.ItemsSource = ListViewItemsCollections;
            Refreh_Mail();



        }

      

       

        public void Refreh_Mail() {
            ListViewItemsCollections.Clear();
            
            foreach (MailInBox item in App.GameGlobal.Servers[0].Mails)
            {
                string mail = item.ReadMail == false ? "mailnew.png" : "Mail.png";
                ListViewItemsCollections.Add(new ListViewItemsData()
                {
                    GridViewColumnName_ImageSource =PatchAB + mail,
                    GridViewColumnName_LabelContent = item.Title,
                     Mail = item                     
                });                   
            }

          
        }

        private  class ListViewItemsData
        {
            public string GridViewColumnName_ImageSource { get; set; }
            public string GridViewColumnName_LabelContent { get; set; }
            public MailInBox Mail;
        }

        private void Закртыта_форма(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        private void Выбор(object sender, MouseButtonEventArgs e)
        {
            if (LsMail.SelectedItem == null) return;
            var w = LsMail.SelectedItem as ListViewItemsData;
            w.Mail.ReadMail = true;
            MailText.Inlines.Clear();
            MailText.Inlines.Add(new Run("Сообщение от : ") { FontWeight = FontWeights.Regular });
            MailText.Inlines.Add(new Run(w.Mail.Mailto + "\r\n") { FontWeight = FontWeights.SemiBold, Foreground = Brushes.Blue });
            MailText.Inlines.Add(new Run("Сообщение для : ") { FontWeight = FontWeights.Regular });
            MailText.Inlines.Add(new Run("user@local.com\r\n") { FontWeight = FontWeights.SemiBold, Foreground = Brushes.Blue });
            MailText.Inlines.Add(new Run("Тема письма : ") { FontWeight = FontWeights.Regular });
            MailText.Inlines.Add(new Run(w.Mail.Title + "\r\n") { FontWeight = FontWeights.SemiBold, Foreground = Brushes.Green });
            MailText.Inlines.Add(new Run("\r\n "));
            MailText.Inlines.Add(new Run(w.Mail.BodyText) { FontWeight = FontWeights.Normal, Foreground = Brushes.DarkSlateGray });
            MailText.Inlines.Add(new Run("\r\n "));
            MailText.Inlines.Add(new Run("\r\n "));
            MailText.Inlines.Add(new Run("                  <Подпись> ") {  FontSize = 10 });
            MailText.Inlines.Add(new Run("Дата отправки: " + w.Mail.DateTo.ToString("dd/MM/yyyy")) { FontWeight = FontWeights.ExtraLight, FontSize =11 });
            w.GridViewColumnName_ImageSource = PatchAB + "Mail.png";
            LsMail.Items.Refresh();

            if (w.Mail.CommandList != null) {
                OpenAtch.Visibility = Visibility.Visible;
                AttachMail = w.Mail;
            }

            // Индикатор показывает что все прочитанно
            var result = from TV in ListViewItemsCollections where TV.Mail.ReadMail == false select TV;
            if (result.Count() == 0) App.GameGlobal.MainWindow.MailIndicator.Source =
            new BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\Mail.png"));

        }




        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
        System.Windows.Media.Brush SaveColor;
        private void КурсорНадКраснымКружком(object sender, MouseEventArgs e)
        {
            if (SaveColor == null) SaveColor = RedButton.Fill;
            RedButton.Fill = System.Windows.Media.Brushes.CornflowerBlue;
        }
        private void КурсорСошелСкрасногоКружка(object sender, MouseEventArgs e)
        {
            RedButton.Fill = SaveColor;
            SaveColor = null;
        }
        private void НажатКрасный(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void УдалениеКнопка(object sender, RoutedEventArgs e)
        {
            if (LsMail.SelectedItem == null) return;
            var w = LsMail.SelectedItem as ListViewItemsData;
            App.GameGlobal.Servers[0].Mails.Remove(w.Mail);
            ListViewItemsCollections.Remove(w);
            MailText.Text = "";
        }

        MailInBox  AttachMail;
        private void ОткрытьПрикрКомманду(object sender, RoutedEventArgs e)
        {
            if (AttachMail.CommandList  is Engine.GameEvenStruct.GetMoney t)
            {
                string txt = "";
                if (t.CheckLogik(ref txt) == false)
                {
                    FrmError frm = new FrmError("Ошибка", txt, FrmError.InformEnum.Информация);                   
                }
                else {
                    App.GameGlobal.SoundSignal("button-sound-14");
                    t.Run();
                    AttachMail.CommandList  = null;
                    OpenAtch.Visibility = Visibility.Hidden ;
                }
            }
        }
    }
}
