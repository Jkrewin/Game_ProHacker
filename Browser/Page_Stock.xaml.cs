using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PH4_WPF.Browser
{   
    public partial class Page_Stock : Page
    {
        private Enums.TypeMoneyEnum _TypeMoney = Enums.TypeMoneyEnum.none; 

        public Page_Stock()
        {
            InitializeComponent();
            if (App.GameGlobal.GamerInfo.Cracker (Enums.SkillCrack.ДоступКрипте) )
            {
                CB_ETH.Visibility = Visibility.Visible;
                CB_BTC.Visibility = Visibility.Visible;
            }
            ListBoxStock.Items.Clear();
            App.GameGlobal.MainWindow.NewDayEvent += AutoRefreh;
        }

        private void AutoRefreh(int days)
        {
            if (_TypeMoney != Enums.TypeMoneyEnum.none) Refreh_Stocks();
        }

        private void Refreh_Stocks() {
            var bc = new BrushConverter();
            ListBoxStock.Items.Clear();
            
            string Sw(Enums.TypeMoneyEnum type ) => type switch
                {
                    Enums.TypeMoneyEnum.none => "",
                    Enums.TypeMoneyEnum.Dollar => "USD",
                    Enums.TypeMoneyEnum.Karbovantsy => "KB",
                    Enums.TypeMoneyEnum.Ether => "ETH",
                    Enums.TypeMoneyEnum.BitCoin => "BTC",
                    _ => "???",
                };
            
            foreach (Enums.TypeMoneyEnum item in typeof(Enums.TypeMoneyEnum).GetEnumValues())  
            {
                if (item == Enums.TypeMoneyEnum.none) continue;
                Engine.BankClass.Stock stock = App.GameGlobal.Bank.MonetarySystem.Find(x => x.TypeMoney == item);

                if (stock == null) continue;

                string colStock ;
                if (stock.RisingCost().StartsWith ('+')) colStock = "#FF11CA84";  //green
                else colStock = "#FFCA1111";  //red

                Grid grid = new Grid()
                {
                    Height = 23,
                    Width = 784
                };

                Label labelTitle = new Label()
                {
                    Content = Sw(item),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = (Brush)bc.ConvertFrom("#FFDADFE3"),
                    Background = null,
                    Width = 35,
                    FontFamily = new FontFamily("Calibri"),
                    Padding = new Thickness(5, 3, 5, 5),
                    FontSize = 14
                };

                Label labelTitleSub = new Label()
                {
                    Content = "/"+ Sw(_TypeMoney),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(28, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = (Brush)bc.ConvertFrom("#FF6E727B"),
                    Background = null,
                    Width = 35,
                    FontFamily = new FontFamily("Calibri"),
                    FontSize = 11,
                    Height = 23,
                    Padding = new Thickness(5, 3, 5, 5)
                };

                Label labelPrice = new Label()
                {
                    Content = stock.Сost,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(110, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment =HorizontalAlignment.Right,
                    Foreground = (Brush)bc.ConvertFrom("#FF3A9A7E"),
                    Background = null,
                    Width = 78,
                    FontFamily = new FontFamily("Calibri"),
                    Padding = new Thickness(5, 3, 5, 5),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                };

                Label labelPriceSub = new Label()
                {
                    Content = stock.OldCost,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(180, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = (Brush)bc.ConvertFrom("#FF6E727B"),
                    Background = null,
                    Width = 48,
                    FontFamily = new FontFamily("Calibri"),
                    FontSize = 11,
                    Height = 23,
                    Padding = new Thickness(5, 3, 5, 5)
                };

                Label labelStock = new Label()
                {
                    Content = stock.RisingCost(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(248, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = (Brush)bc.ConvertFrom("#FF1C2129"), //FFFFFFFF
                    Background = (Brush)bc.ConvertFrom(colStock), 
                    Width = 51,
                    FontFamily = new FontFamily("Calibri"),
                    FontSize = 11,
                    Height = 18,
                    Padding = new Thickness(5, 2, 5, 5),
                    FontWeight = FontWeights.Bold,
                    Clip = new RectangleGeometry() { RadiusX = 2, RadiusY = 2, Rect = new Rect(0, 0, 51, 18) }
                };

                grid.Children.Add(labelTitle);
                grid.Children.Add(labelTitleSub);
                grid.Children.Add(labelPrice);
                grid.Children.Add(labelPriceSub);
                grid.Children.Add(labelStock);

                ListBoxStock.Items.Add(grid);
            }
        }
      
        private void Загруженно(object sender, RoutedEventArgs e)
        {
            //Refreh_Stocks();
        }

        private void ВыборЭлементаВалюты(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();
            Label label = sender as Label;
            Brush brushA = (Brush)bc.ConvertFrom("#FFDBEAE6");
            Brush brushB = (Brush)bc.ConvertFrom("#FFCEC217");

            _TypeMoney = (Enums.TypeMoneyEnum)int.Parse(label.Tag.ToString());
            Refreh_Stocks();           
            CB_BTC.Foreground = brushA;
            CB_ETH.Foreground = brushA;
            CB_KR.Foreground = brushA;
            CB_USD.Foreground = brushA;

            label.Foreground = brushB;
        }

        private void ВыборКонвертацииВалют(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
