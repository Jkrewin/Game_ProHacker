using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace PH4_WPF.Browser
{
    public partial class StartPage : Page
    {
        public StartPage() => InitializeComponent();
        private void OpenFile(string s) {
            if (File.Exists(App.PatchAB + @"\rtf\" + s + ".rtf"))
            {
                TextRange doc = new TextRange(Rtf.Document.ContentStart, Rtf.Document.ContentEnd);
                using (FileStream fs = new FileStream(App.PatchAB + @"\rtf\" + s + ".rtf", FileMode.Open)) doc.Load(fs, DataFormats.Rtf);
            }
        }

        private void ГайдШеллы(object sender, MouseButtonEventArgs e) => OpenFile("shell");
        private void Эксплойты(object sender, MouseButtonEventArgs e) => OpenFile("exploit");
        private void ГайдНовичкаПоКонсоли(object sender, MouseButtonEventArgs e) => OpenFile("cmd");

        private void ОткрытьМоиСкрипты(object sender, MouseButtonEventArgs e)
        {
            string str1 = "Мои Файлы Скрипты";
            string str2 = "Скрыть Список";
            if (LinkMyScr.Text == str1)
            {
                LinkMyScr.Text = str2;
                Rtf.Visibility = Visibility.Hidden;

                if (App.GameGlobal.GamerInfo.Coder(Enums.SkillCoder.ПоискБанковскойИнформации) == false) ScriptOneBank.Visibility =  Visibility.Hidden ;
                if (App.GameGlobal.GamerInfo.Defecer(Enums.SkillDefecer.СообщитьДефейсе) == false) ScriptOneDeface.Visibility = Visibility.Hidden;
            }
            else {
                LinkMyScr.Text = str1;
                Rtf.Visibility = Visibility.Visible;
            }
        }

        private void Загруженна(object sender, RoutedEventArgs e)
        {
            Rtf.Visibility = Visibility.Visible;
        }

        private void СкачатьСкрипт(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Tag.ToString ())
            {
                case "sbank":
                    ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).StartDownload("sbank",
                            new Engine.FileServerClass.ParameterClass() { 
                                    TypeInformation = Enums.TypeParam.sbank, 
                                    TextCommand = "sbank"
                            },
                            650,  "pl");
                    break;
                case "script_deface":
                    ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).StartDownload("script_deface",
                            new Engine.FileServerClass.ParameterClass()
                            {
                                TypeInformation = Enums.TypeParam.script_deface,
                                TextCommand = "sbank"
                            },
                            750, "pl");
                    break;
                default:
                    break;
            }            
        }
    }
}
