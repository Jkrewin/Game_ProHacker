using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PH4_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Стандартные размеры для Grid где распологаеться иконка сервера и название 
        public const int W_GRIND= 55;
        public const int H_GRIND = 70;
      
        /// <summary>
        /// Изменяемый каталог игрового контрента 
        /// </summary>
        public static readonly string PatchAB = System.AppDomain.CurrentDomain.BaseDirectory + "\\Content\\";

        /// <summary>
        ///  Oсновной инстанс
        /// </summary>
        public static Game GameGlobal = new Game();

        /// <summary>
        /// Облегчает доступ к ресурсам картинок
        /// </summary>
        /// <param name="patch">пример Content/Desktop/bPanel/spPaneliisel.png</param>
        /// <returns>BitmapImage</returns>
        public static System.Windows.Media.Imaging.BitmapImage UriResImage(string patch) =>
            new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/PH4_WPF;component/" + patch, UriKind.Relative));

    }
}
