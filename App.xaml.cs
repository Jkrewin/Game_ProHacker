using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PH4_WPF
{
    public partial class App : Application
    {
        private static readonly string FileConfig = AppDomain.CurrentDomain.BaseDirectory + @"Save\config.cnf";

        public static GameStart GameStart;

        //Стандартные размеры для Grid где распологаеться иконка сервера и название 
        public const int W_GRIND= 55;
        public const int H_GRIND = 70;
      
        /// <summary>
        /// Изменяемый каталог игрового контрента 
        /// </summary>
        public static readonly string PatchAB = AppDomain.CurrentDomain.BaseDirectory + @"Content\";

        /// <summary>
        ///  Oсновной инстанс
        /// </summary>
        public static Game GameGlobal = new Game();

        /// <summary>
        /// Test
        /// <code> FrmSoft.FrmError msg = new FrmSoft.FrmError(Tr[Title], Tr[Text], Inform);</code>
        /// </summary>
        public static etc.Translator TranslatorTest = new etc.Translator();

        /// <summary>
        /// Облегчает доступ к ресурсам картинок <b>через не прямой адрес PH4_WPF;component</b>
        /// </summary>
        /// <param name="patch">пример Content/Desktop/bPanel/spPaneliisel.png</param>
        /// <returns>BitmapImage</returns>
        public static System.Windows.Media.Imaging.BitmapImage UriResImage(string patch) =>
            new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/PH4_WPF;component/" + patch, UriKind.Relative));

        /// <summary>
        /// Uses BrushConverter to convert text to color. 
        /// </summary>
        /// <param name="col">Format ex = #FF4B566A</param>
        /// <returns>Media.Brush</returns>
        public static System.Windows.Media.Brush BrushConv(string col) {
            var bc = new System.Windows.Media.BrushConverter();
            return (System.Windows.Media.Brush)bc.ConvertFrom(col);
        }

        #region "Для файла config"
        private static bool _CheatCode = false;
        private static bool _DebugMode = false;
        private static bool _SoundDisable = false;
        private static bool _StartUP = true;

        /// <summary>
        /// Использовать режим отладки
        /// </summary>
        public static bool DebugMode
        {
            get => _DebugMode;
            set
            {
                _CheatCode = value;
                SaveConfig("DebugMode", _DebugMode);
            }
        }
        /// <summary>
        /// Использовать чит коды
        /// </summary>
        public static bool CheatCode
        {
            get => _CheatCode;
            set
            {
                _CheatCode = value;
                SaveConfig("CheatCode", _CheatCode);
            }
        }
        /// <summary>
        /// Отключить звук
        /// </summary>
        public static bool SoundDisable
        {
            get => _SoundDisable;
            set
            {
                _CheatCode = value;
                SaveConfig("SoundDisable", _SoundDisable);
            }
        }
        /// <summary>
        /// Бытро запускает игру для отладки и тестирования
        /// </summary>
        public static bool StartUP
        {
            get => _StartUP;
            set
            {
                _StartUP = value;
                SaveConfig("SoundDisable", _SoundDisable);
            }
        }

        /// <summary>
        /// Сохраняет настройки конфигурации в игре
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение для сохранения</param>
        private static void SaveConfig(string key, object value)
        {
            if (System.IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "Save") == false) return;
            string[] txt = System.IO.File.ReadAllLines(FileConfig, System.Text.Encoding.Default);

            for (int i = 0; i < txt.Length; i++)
            {
                if (key == txt[i].Split('=')[0])
                {
                    txt[i] = key + "" + value.ToString();
                    goto end;
                }
            }
            Array.Resize(ref txt, txt.Length + 1);
            txt[^1] = key + "" + value.ToString();
        end:;
            System.IO.File.WriteAllLines(FileConfig, txt);
        }
        public static void LoadConfig()
        {
            if (System.IO.File.Exists(FileConfig) == false) return;
            string[] txt = System.IO.File.ReadAllLines(FileConfig, System.Text.Encoding.Default);

            foreach (var item in txt)
            {
                switch (item.Split('=')[0])
                {
                    case "CheatCode":
                        _CheatCode = bool.Parse(item.Split('=')[1]);
                        break;
                    case "DebugMode":
                        _DebugMode = bool.Parse(item.Split('=')[1]);
                        break;
                    case "SoundDisable":
                        _SoundDisable = bool.Parse(item.Split('=')[1]);
                        break;
                    case "StartUP":
                        _StartUP = bool.Parse(item.Split('=')[1]);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion


    }
}
