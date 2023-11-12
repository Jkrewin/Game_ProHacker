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
using static PH4_WPF.Browser.Page_Buy;

namespace PH4_WPF.Browser
{
    
    public partial class Page_milw0rm : Page
    {
        public Page_milw0rm()
        {
            InitializeComponent();
            ВыбранныЭксплойты(null, null);
            ImgTitle.Source = new BitmapImage(new Uri(App.PatchAB + @"site\milw0rm.jpg"));
        }


        public void Refreh_Exploid(List<Engine.Vulnerabilities> ls) {
            var bc = new BrushConverter();
            LsGoods.Items.Clear();
            foreach (var item in ls )
            {
                if (item.Exploid)
                {
                    int price = 0;
                    switch (item.GrantPremission)
                    {
                        case Engine.Server.PremissionServerEnum.FullControl:
                            price = 100;
                            break;
                        case Engine.Server.PremissionServerEnum.GuestControl:
                            price = 25;
                            break;
                        case Engine.Server.PremissionServerEnum.UserControl:
                            price = 50;
                            break;
                        default:
                            break;
                    }
                    price = (int)((price + item.VerB) * App.GameGlobal.GamerInfo.MultiplierPrices); // тут множитель цены

                    string str = item.Shareware == true ? price.ToString() : "0";
                    Label label = new Label()
                    {
                        Content = item.NameBug + " Для программы: " + item.CName + "v." + item.VerA + "." + item.VerB + "\r    Цена: " + str + "$",
                        Height = 30,
                        Width = 519,
                        Padding = new Thickness(5, 0, 5, 0),
                        Tag = new DownloadText()
                        {
                            ID = item.ID.ToString(),
                            NameBug = item.NameBug,
                            Price = str,
                            TypeProg = Engine.FileServerClass.ParameterClass.TypeParam.exploit
                        },
                        Foreground = (Brush)bc.ConvertFrom("#FF31EE41")
                    };
                    LsGoods.Items.Add(label);
                }
            }

        }

        private void ВыбранныЭксплойты(object sender, RoutedEventArgs e)
        {
            Refreh_Exploid(App.GameGlobal.VulnerabilitiesList);
        }    


        
       
        private void ВыборЭлементаПокупки(object sender, MouseButtonEventArgs e)
        {
            var j = LsGoods.SelectedItem ;
            if (j == null) return;
            DownloadText v = (DownloadText)((Label)j).Tag;
            ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).FrameBrouser.Navigate(new Page_Buy (v, this));           
        }

        private void Бекдоры(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            LsGoods.Items.Clear();
            string[] txt = App.GameGlobal.MainWindow.Firewall_TXT;
            DownloadText download;
            int i = -1;
            foreach (var tv in txt)
            {
                string[] vs = tv.Split(',');              

                    if (tv != "")
                    {
                        int s = (int)(100 * App.GameGlobal.GamerInfo.MultiplierPrices);
                            i++;
                        download = new DownloadText
                        {
                            ID = i.ToString (),
                            NameBug = tv.Split (',')[0].Replace (" + ", "_"),
                            Price = s.ToString(),
                            TypeProg = Engine.FileServerClass.ParameterClass.TypeParam.backdoor
                        };

                        Label label = new Label()
                        {
                            Content = download.NameBug + "" + "\r    Цена: " + download.Price + "$",
                            Height = 30,
                            Width = 519,
                            Padding = new Thickness(5, 0, 5, 0),
                            Tag = download,
                            Foreground = (Brush)bc.ConvertFrom("#FF31EE41")
                        };
                        LsGoods.Items.Add(label);
                    }
               
            }

        }

        private void Шеллкоды(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            LsGoods.Items.Clear();
            string[] txt = App.GameGlobal.MainWindow.OS_TXT;
            DownloadText download;

            foreach (var item in txt)
            {
                if (item.Contains("="))
                {
                    string t = item.Split('=')[1];
                    string[] tt = t.Split(',');
                    foreach (var item2 in tt)
                    {
                        int s = 100;
                        if (item.Contains("Low")) s = 50;

                        download = new DownloadText
                        {
                            ID = "",
                            NameBug = item2,
                            Price = s.ToString(),
                            TypeProg = Engine.FileServerClass.ParameterClass.TypeParam.shell
                        };

                        Label label = new Label()
                        {
                            Content = download.NameBug + "" + "\r    Цена: " + download.Price + "$",
                            Height = 30,
                            Width = 519,
                            Padding = new Thickness(5, 0, 5, 0),
                            Tag = download,
                            Foreground = (Brush)bc.ConvertFrom("#FF31EE41")
                        };
                        LsGoods.Items.Add(label);

                    }
                }
            }

            download = new DownloadText
            {
                ID = "",
                NameBug = "Unknown",
                Price = (150 * App.GameGlobal.GamerInfo.MultiplierPrices).ToString(),
                TypeProg = Engine.FileServerClass.ParameterClass.TypeParam.shell
            };
            Label label2 = new Label()
            {
                Content = download.NameBug + "" + "\r    Цена: " + download.Price + "$",
                Height = 30,
                Width = 519,
                Padding = new Thickness(5, 0, 5, 0),
                Tag = download,
                Foreground = (Brush)bc.ConvertFrom("#FF31EE41")
            };
            LsGoods.Items.Add(label2);

           
        }
    }
}
