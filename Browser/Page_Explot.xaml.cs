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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PH4_WPF.Browser
{
   
    public partial class Page_Explot : Page
    {       
        public Page_Explot()
        {
            InitializeComponent();                     
            Refreh_News(App.GameGlobal.VulnerabilitiesList);
        }

        private void СкачатьПрограммуF(object sender, MouseButtonEventArgs e)
        {
            ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).StartDownload("PortScaner",
                new FileServerClass.ParameterClass() { TypeInformation = Enums.TypeParam.exe, TextCommand = "portScaner" },
                540, "exe");
        }

        public void Refreh_News(List<Vulnerabilities> ls) {
            SP_ListNews.Children.Clear();

            for (int i = 0; i < Math.Min (ls.Count ,15); i++)
            {
                var t = ls[i];
                string v= "Уязвимость предоставляет полный доступ к системе  (Critical)";
                if (t.GrantPremission == Engine.Server.PremissionServerEnum.UserControl) v = "Выполнение произвольного кода на целевой системе с правами пользователя";
                else if (t.GrantPremission == Engine.Server.PremissionServerEnum.GuestControl) v = "Получение ограниченного доступа к системе (Low)";

                SP_ListNews.Children.Add(NewsElement("Уязвимая программа: " + t.CName +
                    "\nУязвимая версия программы:  "+ t.VerA + "." + t.VerB + "\n"+v+
                    "", t.ID , t.NameBug        ));

           }        
        }

        private Grid NewsElement(string txt, int id, string bug) {
            var bc = new BrushConverter();
            Grid grid = new Grid() { Height =100 };

            Rectangle rec1 = new Rectangle() { 
            Margin = new Thickness (0, 1, 0, 77),
                Stroke= (System.Windows.Media.Brush)bc.ConvertFrom("#FF3D3D3D"),
                Fill = new LinearGradientBrush() { 
                EndPoint = new Point (0.5, 1),
                StartPoint = new Point (0.5, 0),
                GradientStops = new GradientStopCollection() { new GradientStop() { Color= (Color)ColorConverter.ConvertFromString("#FFA8A8A8"), Offset = 0.639 }, new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#FFCACACA"), Offset = 0 } } 
                }
            };
            
            TextBlock textBlock = new TextBlock()
            {
                TextWrapping= TextWrapping.WrapWithOverflow,
                Margin = new Thickness (0, 28, 0, 0),
                FontSize =14,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height=62,
                Width=584,
                Text = txt,
                Foreground = (Brush)bc.ConvertFrom("#FF292929")
            };

            Border border = new Border()
            {
                BorderThickness = new Thickness (1),
                BorderBrush = Brushes.Black
            };

               Label label = new Label()
            {
                Content= "Описание уязвимости",
                HorizontalAlignment = HorizontalAlignment.Left ,
                Margin = new Thickness (16, -2, 0, 0),
                VerticalAlignment= VerticalAlignment.Top ,
                Foreground = (Brush)bc.ConvertFrom("#FF606060"),
                FontWeight = FontWeights.Bold
            };

            
            Button button = new Button() {
             Content = "",
                HorizontalAlignment= HorizontalAlignment.Left,
                Margin =new Thickness (9, 26, 544, 37),
                VerticalAlignment =  VerticalAlignment.Top,
                Height =30,
                FontFamily = new FontFamily ("Segoe MDL2 Assets"),
                Width=30,
                FontSize=16,
                CommandParameter =bug+"Ъ"+id
            };

            button.Click += ButtonDownLoad;

            border.Child = button;
            grid.Children.Add(rec1);
            grid.Children.Add(textBlock);
            grid.Children.Add(border);
            grid.Children.Add(label);
           
            return grid;
        }

        private void ButtonDownLoad(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            string v = (string)((Button)sender).CommandParameter;
            ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).StartDownload(v.Split('Ъ')[0] + ".txt", 
                new FileServerClass.ParameterClass() { TypeInformation = Enums.TypeParam.instructions , IntParam = int.Parse ( v.Split('Ъ')[1]) }              , 
                rand.Next(150, 800), "txt");
        }

        private void ПоисковаяПанель(object sender, TextChangedEventArgs e)
        {
            List<Vulnerabilities> vulnerabilities = new List<Vulnerabilities>();
            var t = sender as TextBox;
            if (t.Text.Length >= 2) {
                foreach (var item in App.GameGlobal.VulnerabilitiesList)
                {
                    if (item.CName.Contains(t.Text)) {
                        vulnerabilities.Add(item);
                    }
                }
            }
            Refreh_News(vulnerabilities);
        }
    }
}
