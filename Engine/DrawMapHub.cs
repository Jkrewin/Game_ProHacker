using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Необходим для отображения серверов на экране <b>Без Сериализации</b>
    /// </summary>
    public class DrawingHubClass
    {
        private int top;
        private int left;

        /// <summary>
        /// Управляет картинкой для этого сервера
        /// </summary>
        public Image TexturaSrv;
        /// <summary>
        /// Управляет подписью
        /// </summary>
        public Label LabelName;
        /// <summary>
        /// Контейнер
        /// </summary>
        public Canvas Canvas;
        /// <summary>
        /// Кружок за серевером
        /// </summary>
        public Ellipse Ellipse;
        /// <summary>
        /// Положение от верхнего экрана
        /// </summary>
        public int Top
        {
            get => top; set
            {
                Canvas.SetTop(Canvas, value);
                top = value;
            }
        }
        /// <summary>
        /// Положение от левой части экрана
        /// </summary>
        public int Left
        {
            get => left; set
            {
                Canvas.SetLeft(Canvas, value);
                left = value;
            }
        }
        /// <summary>
        /// Квадрат текстуры
        /// </summary>
        public System.Drawing.Rectangle GetLocateRec { get => new System.Drawing.Rectangle(Left, Top, App.W_GRIND, App.H_GRIND); }

        public delegate void OnClick(object sender, MouseButtonEventArgs e);
        public event OnClick OnClickMouse;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="srv"></param>
        /// <param name="top"> Top = x, </param>
        /// <param name="left"> Left = y </param>
        public DrawingHubClass(Server srv, int top = 0, int left = 0)
        {

            TexturaSrv = new Image()
            {
                Width = 48,
                Height = 48,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Stretch = Stretch.Fill,
                VerticalAlignment = System.Windows.VerticalAlignment.Top
            };
            TexturaSrv.MouseDown += new MouseButtonEventHandler((object sender, MouseButtonEventArgs e) => OnClickMouse(srv.NameSrv, null)); ;

            LabelName = new Label()
            {
                Content = srv.NameSrv,
                FontSize = 12,
                Margin = new Thickness(-30, 40, -30, 0),
                Foreground = Brushes.White
            };

            Canvas = new Canvas()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Height = App.W_GRIND,
                Width = App.H_GRIND,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,

            };

            Ellipse = new Ellipse()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Height = 50,
                Width = 50,
                Fill = System.Windows.Media.Brushes.GreenYellow,
                Stroke = System.Windows.Media.Brushes.GreenYellow,
                Margin = new Thickness(-2, 0, 0, 0)
            };

             if (App.GameGlobal.MainWindow.DebugMode) {

                LabelName = new Label()
                {
                    Content = "R: " + srv.PopularSRV + " P: " + srv.Ports.FindAll(x => x.Rationo < 10 ).Count,
                    FontSize = 12,
                    Margin = new Thickness(-30, 40, 0, 0),
                    Foreground = Brushes.White,
                    Background = Brushes.Black
                };

            }

        
            Canvas.Children.Add(Ellipse);
            Canvas.Children.Add(TexturaSrv);
            Canvas.Children.Add(LabelName);

            OnClickMouse += new DrawingHubClass.OnClick(App.GameGlobal.MainWindow.OpenBackPanel);
            App.GameGlobal.MainWindow.MyCanvas.Children.Add(Canvas);
            Canvas.SetZIndex(Canvas, 1);   
            Top = top;
            Left = left;

        }

    }
}
