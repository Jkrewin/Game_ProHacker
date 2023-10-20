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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PH4_WPF.FrmSoft
{    
    public partial class PortScaner : Window
    {
        private DispatcherTimer DisTimer = new System.Windows.Threading.DispatcherTimer();
        private BitmapImage Img1Port;
        private BitmapImage Img2Port;
        private BitmapImage ImgAlert;
        private Image LastImage;
        private Label LastLabel;
        private ProgressBar LastProgressBar;
        private Button LastButton;
        private int PossPort=0;
        private Server SelectServer;
        

        public PortScaner()
        {
            InitializeComponent();
            DisTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            DisTimer.Interval = TimeSpan.FromMilliseconds (250);
            
            
            Img1Port= new BitmapImage(new Uri(App.PatchAB + @"\soft\scaner_server\PortActive.png"));
            Img2Port = new BitmapImage(new Uri(App.PatchAB  + @"\soft\scaner_server\PortDeactive.png"));
            ImgAlert = new BitmapImage(new Uri(App.PatchAB + @"\soft\scaner_server\Alert.png"));

            Msg_UI.Visibility = Visibility.Hidden;
        }

        public void CreateUnitPort() {
            var bc = new BrushConverter();

            Border border = new Border()
            {
                CornerRadius = new CornerRadius(5),
                Background = Brushes.Black,
                Height = 18,
                Margin = new Thickness(98, 19, 700, 19),
                RenderTransformOrigin = new Point(0.5, 0.5)               
            };
          
            Grid grid = new Grid() { Height = 56, Width = 860 };

            Image image = new Image()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 50,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Width = 51,
                Source = Img1Port,
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.DownOnly
            };

            DropShadowEffect dropShadow = new DropShadowEffect()
            {
                ShadowDepth = 0,
                Color = (Color)ColorConverter.ConvertFromString("LawnGreen"),
                Opacity = 0.6,
                BlurRadius = 20
            };

            ProgressBar progressBar = new ProgressBar()
            {
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 21,
                Margin = new Thickness(86, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Width = 144,
                SmallChange = 1,
                Value = 1,
                Foreground = (Brush)bc.ConvertFrom("#FF1AE03E"),
                BorderBrush = (Brush)bc.ConvertFrom("#FF229A57"),
                Effect = dropShadow,
                OpacityMask = new VisualBrush(border)
            };

            Label label = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(253, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Width = 150,
                FontFamily = new FontFamily("Calibri"),
                FontSize = 14,
                Content = "Search ...",
                Foreground = (Brush)bc.ConvertFrom("#FFB6DCD4")
            };

            Button button = new Button()
            {
                Content = "",
                Width = 30,
                Height = 30,
                Background = (Brush)bc.ConvertFrom("#FF1B2026"),
                Foreground = (Brush)bc.ConvertFrom("#FF7EA1E8"),
                BorderThickness = new Thickness(1),
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                FontSize=24,
                Margin = new Thickness (350, 0, 0, 0),
                Visibility = Visibility.Hidden
            };


            button.Click += ClickBugButton;
            LastButton = button;
            LastImage = image;
            LastProgressBar = progressBar;
            LastLabel = label;
            grid.Children.Add(border);
            grid.Children.Add(label);
            grid.Children.Add(progressBar);
            grid.Children.Add(image);
            grid.Children.Add(button);
            DisTimer.Start();
            MapGm.Children.Add (grid);
        }

        Vulnerabilities SelVulnerabilities;
        private void ClickBugButton(object sender, RoutedEventArgs e)
        {
            SelVulnerabilities = ((Button)sender).Tag as Engine.Vulnerabilities;
            Msg_UI.Visibility = Visibility.Visible;
            L_Port.Content = ((Button)sender).CommandParameter;
            L_App.Content = SelVulnerabilities.CName;
           
            switch (SelVulnerabilities.GrantPremission)
            {           
                case Server.PremissionServerEnum.FullControl:
                    L_Right.Content = "Исполнение произвольного кода";
                    L_Right.Foreground = Brushes.Red;
                    break;
                case Server.PremissionServerEnum.UserControl:
                    L_Right.Content = "Неавторизованный доступ ";
                    L_Right.Foreground = Brushes.DeepSkyBlue ;
                    break;
                case Server.PremissionServerEnum.GuestControl:
                    L_Right.Content = "Доступ к ценной информации ";
                    L_Right.Foreground = Brushes.YellowGreen;
                    break;
                case Server.PremissionServerEnum.Zombies:
                    L_Right.Content = "Zombies";
                    L_Right.Foreground = Brushes.Yellow;
                    break;
                case Server.PremissionServerEnum.none:
                default:
                    L_Right.Foreground = Brushes.Green;
                    L_Right.Content = "<нет>";
                    break;
            }

        }

        bool chImg;
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (LastProgressBar.Value >= LastProgressBar.Maximum)
            {
                if (PossPort == SelectServer.Ports.Count)
                {
                    MapGm.Children.RemoveAt(MapGm.Children.Count-1);
                    DisTimer.Stop();                    
                }
                else
                {                   
                LastProgressBar.Visibility = Visibility.Hidden;
                ((Grid)MapGm.Children[PossPort]).Children[0].Visibility = Visibility.Hidden;

                LastLabel.Content = "Port:" + SelectServer.Ports[PossPort].Num +" " 
                    + SelectServer.Ports[PossPort].NameTitle.Trim() == "" ? "[Заблокирован]" : SelectServer.Ports[PossPort].NameTitle;

                    var v = FindVulnerabilities(SelectServer.Ports[PossPort].NameTitle);
                    if (v != null) { 
                        LastImage.Source = ImgAlert;
                        LastButton.Tag = v;
                        LastButton.CommandParameter = SelectServer.Ports[PossPort].Num;
                        LastButton.Visibility = Visibility.Visible;
                    }

                    PossPort++;
                    CreateUnitPort();
                }

               

            }

            chImg = !chImg;
            LastImage.Source = (chImg) ? Img1Port : Img2Port;
            LastProgressBar.Value++;
        }

        private Engine.Vulnerabilities FindVulnerabilities(string txt) {
            foreach (var item in App.GameGlobal.VulnerabilitiesList )
            {
                if (item.CName == txt) {
                    return item;
                }
            }
            return null;
        }

        private void ФормаЗакрыта(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        private void НачатьПоиск(object sender, RoutedEventArgs e)
        {
            SelectServer = App.GameGlobal.FindServer(Search.Text);
            if (SelectServer == null)
            {
                UIerrorTab.Visibility = Visibility.Visible;
                return;
            }
            if (SelectServer.ActSrv == false) {
                App.GameGlobal.Msg( "Ошибка", "Сервер не отвечает на запросы.", FrmError.InformEnum.Информация );
                return;
            }

            UIerrorTab.Visibility = Visibility.Hidden;
            PossPort = 0;
            CreateUnitPort();
        }

        private void ВыделениеКнопки(object sender, MouseEventArgs e)
        {

        }

        private void КурсорНадЗакрыть(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect()
            {
                ShadowDepth = 0,
                Color = (Color)ColorConverter.ConvertFromString("Red"),
                Opacity = 0.6,
                BlurRadius = 20
            };
            ((Image)sender).Effect = dropShadow;

        }

        private void КурсорУшелЗакрыто(object sender, MouseEventArgs e)
        {
            ((Image)sender).Effect = null;
        }

        private void НажатьЗакрыть(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed ) this.Close();
        }

        private void КурсорНадСвернуть(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect()
            {
                ShadowDepth = 0,
                Color = (Color)ColorConverter.ConvertFromString("LightGreen"),
                Opacity = 0.6,
                BlurRadius = 20
            }; 
            ((Image)sender).Effect = dropShadow;
        }

        private void КурсорУшелСвернуть(object sender, MouseEventArgs e)
        {
            ((Image)sender).Effect = null; 
        }

        private void КурсорНажатСвернуть(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { }
        }

        private void НажатиеПеретаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void КнопкаЕнтер(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) НачатьПоиск(null, null);
        }

        private void ЗакрытьОкно(object sender, RoutedEventArgs e)
        {
            Msg_UI.Visibility = Visibility.Hidden;
        }

        private void СвернутьОкно(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.WindowState = WindowState.Minimized;
        }

        private void СкачатьОписание(object sender, RoutedEventArgs e)
        {
            Browser.FrmBrowser frm = new Browser.FrmBrowser();
            frm.ShowForm();
            frm = App.GameGlobal.ActiveApp[frm.GetType().FullName] as Browser.FrmBrowser;
            var page = new Browser.Page_Explot();
            frm.FrameBrouser.Navigate(page);
            page.Refreh_News(new List<Vulnerabilities>() { SelVulnerabilities });
        }

        private void СкачатьСплойт(object sender, RoutedEventArgs e)
        {
            Browser.FrmBrowser frm = new Browser.FrmBrowser();
            frm.ShowForm();
            frm = App.GameGlobal.ActiveApp[frm.GetType().FullName] as Browser.FrmBrowser;
            var page = new Browser.Page_milw0rm();
            frm.FrameBrouser.Navigate(page);
            page.Refreh_Exploid(new List<Vulnerabilities>() { SelVulnerabilities });
        }
    }
}
