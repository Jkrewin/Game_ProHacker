using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PH4_WPF.FrmSoft
{    
    public partial class FrmIDE : Window
    {
        private const double DAY_DEV = 45;  //Количество дней на разработку 
        private Grid Field_Pattern;         // Save шаблон списка строку

        private bool IsWork { get =>  App.GameGlobal.VirusList.EventCreateVirus != null;   }
        private Enums.SkillVir SelectTypeVir
        {
            get
            {
                if (CB_ListVirus.Text == "") throw new ArgumentException("Не выбранно ничего в поле выбора вируса");
                return (Enums.SkillVir)int.Parse(CB_ListVirus.Text[0..1]);
            }
        }

        public FrmIDE()
        {
            InitializeComponent();
            App.GameGlobal.MainWindow.NewDayEvent += Working;
        }       

        private void Загруженно(object sender, RoutedEventArgs e)
        {
            if (IsWork) PB_Process.Visibility = Visibility.Visible;
            Field_Pattern = StackP_main.Children[0] as Grid;

            Refreh_List();

        }
        private void Refreh_List()
        {
            StackP_main.Children.Clear();
            foreach (var item in App.GameGlobal.VirusList.InfectedSys)
            {
                string[] vs = item.Info;
                Grid grid = new Grid() { Width = Field_Pattern.Width, Height = Field_Pattern.Height, Background = Field_Pattern.Background };
                for (int i = 0; i < 3; i++)
                {                    
                    Label obj = Field_Pattern.Children[i] as Label;
                    grid.Children.Add(new Label()
                    {
                        Content = vs[i],
                        HorizontalAlignment = obj.HorizontalAlignment,
                        Height = obj.Height,
                        VerticalAlignment = obj.VerticalAlignment,
                        Width = obj.Width,
                        FontFamily = obj.FontFamily,
                        Padding = obj.Padding,
                        Foreground = obj.Foreground,
                        Margin = obj.Margin
                    });                    
                }
                StackP_main.Children.Add(grid);
            }
        }
        private void Working(int days)
        {
            if (IsWork)
            {
                var d = App.GameGlobal.VirusList.EventCreateVirus.DataStart;
                int i = (d - App.GameGlobal.DataGM).Days; 
                if (i == -1)
                {
                    PB_Process.Value = 0;
                    PB_Process.Visibility = Visibility.Hidden;
                    App.GameGlobal.VirusList.EventCreateVirus = null;
                    App.GameGlobal.Msg("Завершено", "Файл доступен по месту " + FrmFile.PatchEnviron.HDoc, FrmError.InformEnum.СообщениеОтПрограмимы);
                }
                else if (i <= DAY_DEV)
                {
                    PB_Process.Value = DAY_DEV - i;
                }
            }
        }
        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
        private void МенюОткрыто(object sender, EventArgs e)
        {
            CB_ListVirus.Items.Clear();
            for (int i = 1; i < 10; i++)
            {
                Enums.SkillVir skill = (Enums.SkillVir)i;
                if (App.GameGlobal.GamerInfo.Vir(skill)) {
                    switch (skill)
                    {
                        case Enums.SkillVir.ВирусыWin:
                            CB_ListVirus.Items.Add("1. вирус для *win систем");
                            break;
                        case Enums.SkillVir.ЗомбиВирусWin:
                            CB_ListVirus.Items.Add("2. вирус зомби - получение контроля (*win)");
                            break;
                        case Enums.SkillVir.ЧервьWin:
                            CB_ListVirus.Items.Add("3. червь - самостоятельно развиваеться (*win)");
                            break;
                        case Enums.SkillVir.ВирусыNix:
                            CB_ListVirus.Items.Add("4. вирус для *nix систем");
                            break;
                        case Enums.SkillVir.ВирусВымогательWin:
                            CB_ListVirus.Items.Add("5. вирус вымогатель для *win");
                            break;
                        case Enums.SkillVir.ВирусВымогательUnix:
                            CB_ListVirus.Items.Add("6. вирус вымогатель для *nix");
                            break;
                        case Enums.SkillVir.AAA:
                            break;
                        case Enums.SkillVir.AAAA:
                            break;
                        case Enums.SkillVir.AAAAA:
                            break;
                        default:
                            break;
                    }                   
                }               
            }
        }
        private void КурсорНадКружком(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Ellipse ellipse = sender as Ellipse;
            ellipse.Stroke  = (Brush)bc.ConvertFrom("#FFD9CECE");
        }
        private void КурсорУшелКружок(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Ellipse ellipse = sender as Ellipse;
            ellipse.Stroke = (Brush)bc.ConvertFrom("#FF4F68EE");
        }
        private void НажатСиний(object sender, MouseButtonEventArgs e) => this.WindowState = WindowState.Minimized;
        private void НажатКрасный(object sender, MouseButtonEventArgs e)=> this.Close();
        private void ЗакрытаФорма(object sender, EventArgs e) {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            App.GameGlobal.MainWindow.NewDayEvent -= Working;
        }
        private void ЗапускПроцесса(object sender, RoutedEventArgs e)
        {
            if (IsWork)
            {
                App.GameGlobal.Msg("Процесс создания", "В настоящий момент процесс идет создания вируса.", FrmError.InformEnum.СообщениеОтПрограмимы);
            }
            else
            {
                Engine.FileServerClass.ParameterClass parameter = new Engine.FileServerClass.ParameterClass()
                {
                    TextCommand = "",
                    IntParam = (int)SelectTypeVir,
                    TypeInformation = Enums.TypeParam.virus_win
                };

                DateTime date_end = App.GameGlobal.DataGM;
                date_end = date_end.AddDays(DAY_DEV);

                Engine.GameEvenClass.CreateFile file = new Engine.GameEvenClass.CreateFile()
                {
                    NameFile = "Virus",
                    Path = FrmFile.PatchEnviron.HDoc,
                    Perfix = "exe",
                    Size = 541,
                    Premision = Engine.FileServerClass.PremisionEnum.AdminAndUser,
                    SystemFile = false,
                    Comment = parameter
                };

                Engine.GameEvenClass gameEven = new Engine.GameEvenClass(date_end, file);
                App.GameGlobal.VirusList.EventCreateVirus = gameEven;
                App.GameGlobal.AllEventGame.Add(gameEven);
                PB_Process.Visibility = Visibility.Visible;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Refreh_List();
    }
}
