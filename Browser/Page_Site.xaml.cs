using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PH4_WPF.Browser
{   
    public partial class Page_Site : Page
    {
        readonly Engine.Server Server; 

        public Page_Site(Engine.Server server  )
        {
            InitializeComponent();
            Server = server;
            if (Server.VirtualizationServer == null) Server.CreateVirtualAuto();            
        }

        // запускаеться при завершение Initialize этого page
        private void ЗагрузитьСайт(object sender, RoutedEventArgs e)
        {
           
            StackP_Main.Children.Clear();
            var bc = new BrushConverter();

            LabelUrl.Content = Server.NameSrv;

            if (string.IsNullOrEmpty(Server.VirtualizationServer.DefeceString))
            {
                LabelMsgHack.Visibility = Visibility.Hidden;
            }
            else
            {
                LabelMsgHack.Content = Server.VirtualizationServer.DefeceString;
                return;
            }

            foreach (var item in Server.VirtualizationServer.Instance)
            {
                Grid grid = new Grid()
                {
                    Height = 65,
                    Background = (Brush)bc.ConvertFrom("#FFD9E992")
                };

                Label labelName = new Label()
                {
                    Content = Server.VirtualizationServer.GetTextName(item.InstaceType),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(68, 10, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 300,
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 12,
                    FontWeight = FontWeights.Normal
                };

                Label labelPopular = new Label()
                {
                    Content = item.Popular + "/per",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(68, 32, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 106,
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };

                Label labelVer = new Label()
                {
                    Content = item.Version,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(380, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 55,
                    FontFamily = new FontFamily("Candara"),
                    FontSize = 14,
                    FontWeight = FontWeights.Normal
                };

                Rectangle rectangle = new Rectangle()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 64,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 18
                };

                if (item.StatusInstance == Enums.StatusInstanceEnum.Working) rectangle.Fill = (Brush)bc.ConvertFrom("#FF57BB89");
                else if (item.StatusInstance == Enums.StatusInstanceEnum.Stopping) rectangle.Fill = (Brush)bc.ConvertFrom("#FFBB5757");
                else if (item.StatusInstance == Enums.StatusInstanceEnum.ErrorCritical) rectangle.Fill = (Brush)bc.ConvertFrom("#FFFF0101");

                grid.Children.Add(labelName);
                grid.Children.Add(labelPopular);
                grid.Children.Add(labelVer);
                grid.Children.Add(rectangle);

                StackP_Main.Children.Add(grid);
            }
        }
    }
}
