using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    
    public partial class FrmFile : Window
    {
      


        private ObservableCollection<ListViewItemsData> ListViewItemsCollections = new ObservableCollection<ListViewItemsData>();
        private string MyPath="/";
       
        /// <summary>
        /// Путь к файлам тут назначать 
        /// </summary>
        public string PatchDisplay { get
            {
                return MyPath;
            }
            set {
                MyPath = value;
                SPatch.Text = MyPath;
                }
             }

        private class ListViewItemsData
        {
            public string GridViewColumnName_ImageSource { get; set; }
            public string GridViewColumnName_LabelContent { get; set; }
            public FileServerClass File;
        }

        public FrmFile()
        {
            InitializeComponent();
            LsFile.ItemsSource = ListViewItemsCollections;
            RefFiles();
        }

        
        private void RefFiles() {
            var ls = Engine.FileServerClass.GetInfoFiles(MyPath, App.GameGlobal.MyServer);
            ListViewItemsCollections.Clear();
            foreach (var item in ls  )
            {
                ListViewItemsData data = new ListViewItemsData() { File = item, GridViewColumnName_LabelContent = item.FileName };

                if (item.Dir != null)
                {
                    data.GridViewColumnName_ImageSource = "/Content/soft/FileManager/filesystems.png";
                }
                else if (System.IO.File.Exists(App.PatchAB + @"\soft\fileManager\"+ item.Perfix   +".png")) {
                    data.GridViewColumnName_ImageSource = App.PatchAB + @"\soft\fileManager\" + item.Perfix + ".png";
                }
                else
                {
                    data.GridViewColumnName_ImageSource = App.PatchAB + @"\soft\fileManager\etc.png";
                }

                ListViewItemsCollections.Add(data);
            }   
        }




        private void ФормаЗакрыта(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        private void Выделяет_кнопку_выход(object sender, MouseEventArgs e)
        {
            ExitButton.Fill = Brushes.IndianRed;
        }

        private void ПрекращаетВыделение(object sender, MouseEventArgs e)
        {
            ExitButton.Fill = Brushes.DarkRed;
        }

        private void УдалениеКнопка(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void КликПоЭлементу(object sender, MouseButtonEventArgs e)
        {
            ListViewItemsData l = (ListViewItemsData)LsFile.SelectedItem;
            if (l == null) return;
            if (l.File.Dir == null)
            {
                //это файл
                var f = Engine.FileServerClass.GetFile(MyPath + l.File.FileName, App.GameGlobal.MyServer);

                // Установка программы в App
                if (f.FileСontents.TypeInformation == FileServerClass.ParameterClass.TypeParam.exe)
                {
                    App.GameGlobal.MyServer.CreateFiles("/apps/", f.FileСontents.TextCommand, "Запускает программу", (int)(f.Size * 1.2), FileServerClass.PremisionEnum.AdminAndUser, "exe", false);
                    App.GameGlobal.MainWindow.Refreh_AppDeck();
                    App.GameGlobal.Msg("Установка", "Установка программы " + f.FileName + " завершенно ", FrmError.InformEnum.УстановкаПрограммы);
                }
                // Переместить сполйт
                else if ( f.FileСontents.TypeInformation == FileServerClass.ParameterClass.TypeParam.exploit & MyPath == "/user/Hpro4/Download/")
                {                    
                     var h = Engine.FileServerClass.GetFile("/user/Hpro4/Exploit/", App.GameGlobal.MyServer);
                    if (h.Dir.Find(x => x.FileName == f.FileName) == null) {

                        FrmSoft.FrmError msg = new FrmSoft.FrmError("Перемещение файла", "Переместить файл " + h.FileName + " в папку exploit ?", delegate {
                            h.Dir.Add(f);
                            Engine.FileServerClass.GetFile("/user/Hpro4/Download/", App.GameGlobal.MyServer).Dir.Remove(f);
                            Папкаexploit(null, null);
                        });
                        msg.Show();
                        msg.Activate();
                        msg.Topmost = true;                        
                    }               
                }

            }
            else {
                // это папка                 
                PatchDisplay = PatchDisplay + l.File.FileName + "/";
                RefFiles();
            }
        }

        private void ВерхДиректорияКлик(object sender, RoutedEventArgs e)
        {
            if (SPatch.Text == "/") return;
            string[] p = PatchDisplay.Split('/');
            string deep="";
            for (int i = 0; i < p.Length-2; i++)
            {
                deep += p[i]+"/";
            }
            PatchDisplay = deep;
            RefFiles();
        }

        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        public void ПапкаЗагрузка(object sender, RoutedEventArgs e)
        {
            PatchDisplay = "/user/Hpro4/Download/";
            RefFiles();
        }

        private void УдалитьФайл(object sender, RoutedEventArgs e)
        {
            ListViewItemsData f = (ListViewItemsData)LsFile.SelectedItem;
            if (f == null) return;
            if (f.File.SystemFile == false)
            {
                f.File.FileDel();
                RefFiles();
            }
            else {
                App.GameGlobal.Msg("Ошибка", "Нельзя удалить этот файл", FrmError.InformEnum.Критическая_ошибка);
            }
        }

        private void ВыделенФайл(object sender, SelectionChangedEventArgs e)
        {
            ListViewItemsData f = (ListViewItemsData)LsFile.SelectedItem;
            if (f == null) return;
            foreach (var item in App.GameGlobal.OpenUrl)
            {
                if (item.Value.FileName == f.File.FileName) {
                    var bc = new BrushConverter();
                    SharedFile.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF98A6C2");
                }
            }
            SharedFile.Background = null;
        }

        string oldPatch;
        FileServerClass CutFile;
        private void ВыризатьФайл(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            ListViewItemsData f = (ListViewItemsData)LsFile.SelectedItem;
            if (f == null) return;
            if (f.File.SystemFile == false)
            {
                
                ButtonCut.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF95A2BC");
                CutFile = f.File;
                oldPatch = MyPath;
            }
            else
            {
                oldPatch = "";
                CutFile = null;
                ButtonCut.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF2E3440");
                App.GameGlobal.Msg("Ошибка", "Нельзя перемещать системные файлы", FrmError.InformEnum.Критическая_ошибка);
            }
        }

        private void ВставитьФайл(object sender, RoutedEventArgs e)
        {
            var ls = Engine.FileServerClass.GetInfoFiles(MyPath, App.GameGlobal.MyServer);
            if (ls.Find(x => x.FileName == CutFile.FileName) != null)
            {
                App.GameGlobal.Msg("Ошибка", "Такой файл тут уже существует", FrmError.InformEnum.Критическая_ошибка);
            }
            else {
                var bc = new BrushConverter();
                ButtonCut.Background=(System.Windows.Media.Brush)bc.ConvertFrom("#FF2E3440");

                string[] sf = MyPath.Split('/');
                var sel= App.GameGlobal.MyServer.FileSys.Dir ;
                foreach (var item in sf)
                {
                    foreach (var tv in sel)
                    {
                        if (item == tv.FileName) { sel = tv.Dir; }
                    }
                    
                }
                
                sel.Add(CutFile);
                RefFiles();

                sf = oldPatch.Split('/');
                foreach (var item in sf)
                {
                    foreach (var tv in sel)
                    {
                        if (item == tv.FileName) { sel = tv.Dir; }
                    }

                }
                sel.Remove(CutFile);
            }            
        }

        private void МенюФайлов(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ListViewItemsData f = (ListViewItemsData)LsFile.SelectedItem;
                if (f == null) return;

            }
        }

        private void Папкаexploit(object sender, RoutedEventArgs e)
        {
            PatchDisplay = "/user/Hpro4/Exploit/";
            RefFiles();           
        }

        private void Расшарить(object sender, RoutedEventArgs e)
        {
            ListViewItemsData f = (ListViewItemsData)LsFile.SelectedItem;
            if (f == null) return;
            var bc = new BrushConverter();
            foreach (var item in App.GameGlobal.OpenUrl)
            {
                if (item.Value.FileName == f.File.FileName)
                {
                    App.GameGlobal.Msg("", "Общий доступ к файлу отключен", FrmError.InformEnum.Информация);
                    SharedFile.Background = null;
                    App.GameGlobal.OpenUrl.Remove(item.Key);
                    return;
                }
            }    

            if (f.File.SystemFile)
            {
                App.GameGlobal.Msg("Ошибка", "Системные файлы нельзя сделать общедоступными", FrmError.InformEnum.Информация);
                return;
            }

            string url = "localhost/" + f.File.FileName;
            if (App.GameGlobal.OpenUrl.ContainsKey(url))
            {
                App.GameGlobal.Msg("Ошибка", "Этот файл с таким названием уже ранее был добавлен в общедоступный список", FrmError.InformEnum.Критическая_ошибка);
                return;
            }

            App.GameGlobal.Msg("Папка", "Этот файл доступен по адресу " + url, FrmError.InformEnum.Информация);
            App.GameGlobal.OpenUrl.Add(url,f.File) ;
            SharedFile.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF98A6C2");

        }
    }
}
