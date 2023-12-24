using PH4_WPF.Engine;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace PH4_WPF
{
   
    public partial class GameStart : Window
    {       
        private readonly string DirSaveGame = System.AppDomain.CurrentDomain.BaseDirectory + @"Save\";
        int MyIndex = 0;
        string[] Ava;
        int My_char = 0;
        readonly char[] All_char = new[] {'%','^','0','+','@','~','3' };
        readonly string[] TextHack_Text = new string  [55];
        int X_inf=0;
        Bitmap grayScaleImage;
        private readonly System.Windows.Threading.DispatcherTimer AnimationTimer = new System.Windows.Threading.DispatcherTimer();

        public GameStart()
        {
            InitializeComponent();

            LabelVer.Content = "Версия игры: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
          // this.Topmost = true;
            AnimationTimer.Tick += new EventHandler(AnimationTick);
            AnimationTimer.Interval = TimeSpan.FromMilliseconds(50);
            AnimationTimer.Stop();

            StartPanel.Visibility = Visibility.Hidden;
                        
            if (System.IO.Directory.Exists(DirSaveGame) == false) System.IO.Directory.CreateDirectory(DirSaveGame);
                       
            App.GameGlobal.MainWindow = new MainWindow ();
            App.LoadConfig();
                       
        }
          

        private void SelectAva() {
            if (MyIndex == -1) MyIndex = Ava.Length - 1;
            if (MyIndex == Ava.Length) MyIndex =0;
            ImageAva.Source = new BitmapImage(new Uri(Ava [MyIndex]));
        }

        private void Refreh_FileSav() {
            string[] ls = System.IO.Directory.GetFiles(DirSaveGame, "*.sav");
            ListFile.Items.Clear();
            foreach (var item in ls)
            {
                FileInfo f = new FileInfo(item);
                ListFile.Items.Add(f.LastAccessTime.ToString("dd/mm/yy") + " - " + f.Name.Substring(0, f.Name.Length - 4));
            }
        }

        private  Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {          
                Bitmap b = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;            
            
        }

        private void ЗакрытьИгру(object sender, RoutedEventArgs e)=> System.Windows.Application.Current.Shutdown();

        private void AnimationTick(object sender, EventArgs e) {

            if (X_inf == grayScaleImage.Width) {
                X_inf = 0;
                My_char++;
                if (My_char == 5) My_char = 0;
            }
                string asciiArt = "";
            for (int y = 0; y < grayScaleImage.Height; y++)
            {
                Color pixelColor = grayScaleImage.GetPixel(X_inf, y);
                int grayScaleValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                char character;
                if (grayScaleValue > 200)
                    character = '.';
                else if (grayScaleValue > 150)
                    character = '-';
                else if (grayScaleValue > 100)
                    character = ':';
                else if (grayScaleValue > 50)
                    character = '=';
                else
                    character = All_char [My_char];

                asciiArt += character;
            }
            TextHack_Text[X_inf] = asciiArt + "\n";           
            X_inf++;

            TextHack.Text = "";
            foreach (var item in TextHack_Text)
            {
                TextHack.Text += item;
            }
        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {
            if (App.StartUP)
            {
                App.GameGlobal.MainWindow.Show();
                App.GameGlobal.MainWindow.LoadGame(AppDomain.CurrentDomain.BaseDirectory + @"Save\1.sav");
                this.Hide();
                return;
            }            
            
            if (this.Height< 970)  TextHack.FontSize = 13; 
            Bitmap sourceImage = new Bitmap(App.PatchAB + @"Desktop\1696525559492.png");
            sourceImage = ResizeImage(sourceImage, new Size(55, 180));
            grayScaleImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    System.Drawing.Color pixelColor = sourceImage.GetPixel(x, y);
                    int grayScaleValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    Color grayScaleColor = Color.FromArgb(grayScaleValue, grayScaleValue, grayScaleValue);
                    grayScaleImage.SetPixel(x, y, grayScaleColor);
                }
            }

            AnimationTimer.Start();


        }

        private void СписокИгры(object sender, RoutedEventArgs e)
        {
            LoadList.Visibility = Visibility.Visible;
            StartPanel.Visibility = Visibility.Hidden;
            Refreh_FileSav();

        }

        private void НоваяИгра(object sender, RoutedEventArgs e)
        {
            LoadList.Visibility = Visibility.Hidden;
            StartPanel.Visibility = Visibility.Visible;
            Ava = System.IO.Directory.GetFiles(App.PatchAB + @"face\", "*.gif");
            SelectAva();
        }

        private void ЗагрузитьИгру(object sender, RoutedEventArgs e)
        {
            string sfile= ListFile.SelectedItem.ToString().Split (" - ")[1] + ".sav";
            App.GameGlobal.MainWindow.Show();
            App.GameGlobal.MainWindow.LoadGame(DirSaveGame + sfile);            
            this.Hide();
        }

        private void УдалитьИгру(object sender, RoutedEventArgs e)
        {
            string sfile = ListFile.SelectedItem.ToString().Split(" - ")[1] + ".sav";
           if (System.IO.File.Exists (DirSaveGame + sfile)) System.IO.File.Delete(DirSaveGame + sfile);
            Refreh_FileSav();
        }

        private void ПереместитьВПраво(object sender, RoutedEventArgs e)
        {
            MyIndex++;
            SelectAva();
        }

        private void ПереместитьВЛево(object sender, RoutedEventArgs e)
        {
            MyIndex--;
            SelectAva();
        }

        private void НачатьИгру(object sender, RoutedEventArgs e)
        {
            FileInfo f = new FileInfo(Ava [MyIndex]);
            GamerInfoClass gamer = new GamerInfoClass
            {
                Ava = @"face\" + f.Name,
                Age = short.Parse(AgeGamer.Text),
                GameName = Nick.Text
            };

            if (RB1.IsChecked == true) gamer.Gender = 1;
            else if (RB2.IsChecked == true) gamer.Gender = 0;
            else gamer.Gender = 2;
                        
            App.GameGlobal.MainWindow.Show();
            App.GameGlobal.MainWindow.NewGame();
            App.GameGlobal.GamerInfo = gamer;
            this.Hide();

        }
    }
}
