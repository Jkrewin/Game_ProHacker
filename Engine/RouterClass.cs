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
    public class RouterClass
    {
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
        /// Сама линиии маршрута которая рис
        /// </summary>
        public Line Line
        {
            get
            {
                if (line == null)
                {
                    Line lineNew = new Line
                    {
                        StrokeThickness = LineArgument.StrokeThickness,
                        Stroke = LineArgument.Stroke,
                        Y1 = LineArgument.Y1,
                        X1 = LineArgument.X1,
                        Y2 = LineArgument.Y2,
                        X2 = LineArgument.X2
                    };
                    line = lineNew;
                }
                return line;
            }
        }
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
        /// Необходим для создание линии в качестве свойств
        /// </summary>
        [Serializable]
        public struct LineArgumentStruct
        {
            public double StrokeThickness;
            public Brush Stroke { get => Brushes.GreenYellow; }
            public double Y1;
            public double Y2;
            public double X1;
            public double X2;
        }
    }
}
