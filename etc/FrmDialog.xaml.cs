using PH4_WPF.Engine;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static PH4_WPF.FrmSoft.FrmFile;

namespace PH4_WPF.etc
{
    /// <summary>
    /// Выбор файла для программы
    /// </summary>
    public partial class FrmDialog : Window
    {
        private readonly Action<FileServerClass> _action;
        private FileServerClass SFile;
        private string PatchDisplay = PatchEnviron.Download;

        /// <summary>
        /// Открывает форму
        /// </summary>
        /// <param name="act">Действия будут выполнены после выбора нужного файла</param>
        public FrmDialog(Action<FileServerClass> act, string title)
        {
            InitializeComponent();
            _action = act;
            InfoSelector.Content = title;
        }

        private void ВыборФайла(object sender, RoutedEventArgs e)
        {
            //тут проверка
            if (SFile == null) return;
            //выполнение действий 
            _action(SFile);
            ЗакрытиеФормы(null, null);
        }

        private void RefrehListFile() {           
            FileList.Items.Clear();
            FileList.Items.Add(ListBoxItem(null));

            foreach (var item in FileServerClass.GetInfoFiles(PatchDisplay, App.GameGlobal.MyServer))
            {
                FileList.Items.Add(ListBoxItem(item));
            }
        }

        private void ВыборЭлемента(object sender, SelectionChangedEventArgs e)
        {
           var pi= FileList.SelectedItem as Grid;
            if (pi == null) return;
            else if (pi.Tag == null)
            {
                string[] p = PatchDisplay.Split('/');
                string deep = "";
                for (int i = 0; i < p.Length - 2; i++)
                {
                    deep += p[i] + "/";
                }
                PatchDisplay = deep;
                RefrehListFile();
            }
            else if (((FileServerClass)pi.Tag).IsDir == true) {
                PatchDisplay = PatchDisplay + ((FileServerClass)pi.Tag).FileName + "/";
                RefrehListFile();
            }
            else SFile = (FileServerClass)pi.Tag;
        }

        private Grid ListBoxItem(FileServerClass fileServer)
        {
            var bc = new BrushConverter();
            string f ;

            if (fileServer == null) f = App.PatchAB + @"soft\fileManager\filesystemsup.png";
            else if (fileServer.IsDir) f = App.PatchAB + @"soft\fileManager\filesystems.png";
            else f = App.PatchAB + @"soft\fileManager\" + fileServer.Perfix + ".png";

            var img = new BitmapImage(new Uri(System.IO.File.Exists(f) ? f : App.PatchAB + @"\soft\fileManager\etc.png"));

            Grid grid = new Grid()
            {
                Height = 18,
                Width = 205,
                Tag = fileServer
            };

            Image image = new Image()
            {
                Margin = new Thickness(0, 0, 185, 0),
                Source = img,
                Stretch = Stretch.Uniform
            };

            Label label = new Label()
            {
                Content = fileServer == null ? " . . . " : fileServer.FileName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 172,
                Margin = new Thickness(23, 0, 0, 0),
                Padding = new Thickness(5, 0, 5, 0),
                Foreground = (Brush)bc.ConvertFrom("#FF0A422A")
            };

            grid.Children.Add(image);
            grid.Children.Add(label);
            return grid;
        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {
            RefrehListFile();
        }

        private void ЗакрытиеФормы(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
