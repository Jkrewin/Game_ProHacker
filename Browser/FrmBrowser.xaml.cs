using PH4_WPF.Engine;
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

namespace PH4_WPF.Browser
{

    public partial class FrmBrowser : Window
    {
        private readonly System.Windows.Threading.DispatcherTimer DownloadFile = new System.Windows.Threading.DispatcherTimer();
        private FileServerClass DownloadFl;

        public FrmBrowser()
        {
            InitializeComponent();

            DownloadFile.Tick += new EventHandler(Download);

            
            FrameBrouser.Navigate(new StartPage());
            PanelDownload.Visibility = Visibility.Hidden;

           
        }

        public void StartDownload(string fileName, FileServerClass.ParameterClass exe, int size, string perfix) {
            bool b = FileServerClass.Exist("/user/Hpro4/Download/", fileName, perfix , App.GameGlobal .MyServer );

            if (b) {
                FrmSoft.FrmError msg = new FrmSoft.FrmError(Title, "Этот файл был скачен ранее", FrmSoft.FrmError.InformEnum.УстановкаПрограммы );
                return;
            }                      

            DownloadFl = new FileServerClass();
            DownloadFl.FileСontents = exe;
            DownloadFl.FileName = fileName;
            DownloadFl.Size = size;
            DownloadFl.Perfix = perfix;
            DownloadFile.Interval = TimeSpan.FromMilliseconds(500);
            ProgressDownload.Value = 0;
            ProgressDownload.Maximum = DownloadFl.Size / 100;
            PanelDownload.Visibility = Visibility.Visible;
            DownloadFile.Start();
        } 


        private void Download(object sender, EventArgs e) {            
            ProgressDownload.Value++;
            if (ProgressDownload.Value == ProgressDownload.Maximum)
            {
                App.GameGlobal.MyServer.CreateFiles("/user/Hpro4/Download/", DownloadFl.FileName, DownloadFl.FileСontents, (int)DownloadFl.Size, FileServerClass.PremisionEnum.AdminUserGuest,  false, false);
                PanelDownload.Visibility = Visibility.Hidden;
                DownloadFile.Stop ();
            }
        }


        private void ФормаЗакрыта(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

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

        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void ПереходБанк(object sender, MouseButtonEventArgs e)
        {
            
            FrameBrouser.Navigate(new BankPage());
        }

        private void Переход_explot(object sender, MouseButtonEventArgs e)
        {
            FrameBrouser.Navigate(new Page_Explot());
        }

        private void ОткрытьПапкуЗагрузки(object sender, RoutedEventArgs e)
        {
            FrmSoft.FrmFile frm = new FrmSoft.FrmFile();
            frm.ShowForm();
            ((FrmSoft.FrmFile )App.GameGlobal.ActiveApp["PH4_WPF.FrmSoft.FrmFile"]).ПапкаЗагрузка (null,null);
        }

        private void Переход_milw0rm(object sender, MouseButtonEventArgs e)
        {
            FrameBrouser.Navigate(new Page_milw0rm());
        }
    }
}
