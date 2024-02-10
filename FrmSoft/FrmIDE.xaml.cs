using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PH4_WPF.FrmSoft
{    
    public partial class FrmIDE : Window
    {
        public FrmIDE()
        {
            InitializeComponent();
        }

       

        private void Загруженно(object sender, RoutedEventArgs e)
        {

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
                        case Enums.SkillVir.AA:
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
        private void ЗакрытаФорма(object sender, EventArgs e) => App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

    }
}
