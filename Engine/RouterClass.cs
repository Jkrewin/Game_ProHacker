using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Отображает линии маршрута
    /// </summary>
    [Serializable]
    public sealed class RouterClass
    {
        private bool _MyRoute =false;
        private int top;
        private int left;
        [NonSerialized] private Line line;


        /// <summary>
        /// Сервер начальный
        /// </summary>
        public Server FirstServer;
        /// <summary>
        /// Сервер конец
        /// </summary>
        public Server EndServer;               
        /// <summary>
        /// Параметры линии для рисовки
        /// </summary>
        public LineArgumentStruct LineArgument;
        /// <summary>
        /// Координаты слева
        /// </summary>
        public int Left
        {
            get => left; set
            {
                Canvas.SetLeft(Line, value);
                left = value;
            }
        }
        /// <summary>
        /// Координаты сверху
        /// </summary>
        public int Top
        {
            get => top; set
            {
                Canvas.SetTop(Line, value);
                top = value;
            }
        }
        /// <summary>
        /// Созданный маршрут вам или нет <b>true- это ваш маршрут</b>
        /// </summary>
        public bool MyRoute
        {
            get => _MyRoute;
            set
            {
                _MyRoute = value;
                Line.Stroke = _MyRoute ? Brushes.OrangeRed : Brushes.GreenYellow;
            }
        }
        /// <summary>
        /// Сама линиии маршрута которая рисуется
        /// </summary>
        public Line Line
        {
            get
            {
                line ??= new Line
                {
                    StrokeThickness = LineArgument. StrokeThickness,
                    Stroke = _MyRoute ? Brushes.OrangeRed : Brushes.GreenYellow,
                    Y1 = LineArgument. Y1,
                    X1 = LineArgument. X1,
                    Y2 = LineArgument. Y2,
                    X2 = LineArgument.X2
                };
                return line;
            }
        }


        /// <summary>
        /// Необходим для создание линии в качестве свойств
        /// </summary>
        [Serializable]
        public struct LineArgumentStruct
        {
            public double StrokeThickness;            
            public double Y1;
            public double Y2;
            public double X1;
            public double X2;

            public LineArgumentStruct(double strokeThickness, double y1, double y2, double x1, double x2)
            {
                StrokeThickness = strokeThickness;
                Y1 = y1;
                Y2 = y2;
                X1 = x1;
                X2 = x2;                    
            }           
           
        }
    }
}
