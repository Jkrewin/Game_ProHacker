using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PH4_WPF.FrmSoft
{

    public partial class FrmIdUser : Window
    {
        readonly string[,] DefecerInfo = new string[10,2];
        readonly string[,] CrackerInfo = new string[10,2];
        readonly string[,] VirInfo = new string[10,2];
        readonly string[,] CorderInfo = new string[10,2];

        byte SelectK = 0; 
        const char STARS = '';
        readonly GamerInfoClass GamerInfo;
        public FrmIdUser()
        {
            InitializeComponent();
            GamerInfo = App.GameGlobal.GamerInfo;

            DefecerInfo[0, 0] = "НАВЫК: Перезапуск сервера и отключение";
            DefecerInfo[1, 0] = "НАВЫК: Взлом сайта, сообщить о deface";
            DefecerInfo[2, 0] = "АКТИВНОСТЬ: Повышает скрытность на сервере";
            DefecerInfo[3, 0] = "НАВЫК: Возможность создавать маршруты серверов";
            DefecerInfo[4, 0] = "";
            DefecerInfo[5, 0] = "";
            DefecerInfo[6, 0] = "";
            DefecerInfo[7, 0] = "";
            DefecerInfo[8, 0] = "";
            DefecerInfo[9, 0] = "";

            DefecerInfo[0, 1] = "Теперь вы знаете, как вырубить сервер, после этого он перезапустить через некоторое время. Вы получите опыт. Сервер после этого повысит свою безопасность. Доступно в статусе сервера.  ";
            DefecerInfo[1, 1] = "";
            DefecerInfo[2, 1] = "";
            DefecerInfo[3, 1] = "";
            DefecerInfo[4, 1] = "";
            DefecerInfo[5, 1] = "";
            DefecerInfo[6, 1] = "";
            DefecerInfo[7, 1] = "";
            DefecerInfo[8, 1] = "";
            DefecerInfo[9, 1] = "";

            CrackerInfo[0, 0] = "СОФТ: Brute-force программа";           
            CrackerInfo[1, 0] = "СОФТ: Очистка логов";
            CrackerInfo[2, 0] = "Собирать хеш-пароли в сети и серверов";
            CrackerInfo[3, 0] = "Делать заказы на перебор хешей";
            CrackerInfo[4, 0] = "Торговля хешами (кодами)";
            CrackerInfo[5, 0] = "Доступ к криптовалюте";
            CrackerInfo[6, 0] = "Новая крипта";
            CrackerInfo[7, 0] = "Взлом ";
            CrackerInfo[8, 0] = "+";
            CrackerInfo[9, 0] = "+";

            CrackerInfo[0, 1] = "Термин brute-force обычно используется в контексте хакерских атак на логины/пароли. Перебор паролей для получение доступа к серверу";
            CrackerInfo[1, 1] = "Сервера собирают информацию об активности на сервере, очистка логов повысит ваши шансы быть скрытым.";
            CrackerInfo[2, 1] = "";
            CrackerInfo[3, 1] = "";
            CrackerInfo[4, 1] = "";
            CrackerInfo[5, 1] = "";
            CrackerInfo[6, 1] = "";
            CrackerInfo[7, 1] = "";
            CrackerInfo[8, 1] = "";
            CrackerInfo[9, 1] = "";

            VirInfo[0, 0] = "КОД: Вирус по краже паролей";
            VirInfo[1, 0] = "КОД: Вирус Зомби";
            VirInfo[2, 0] = "КОД: Вирус Червя";
            VirInfo[3, 0] = "КОД: Вирус DDoS";
            VirInfo[4, 0] = "КОД: Вирус Вымогатель";
            VirInfo[5, 0] = "КОД: Вирус для *UNIX Зомби";
            VirInfo[6, 0] = "КОД: Вирус для *UNIX Червя";
            VirInfo[7, 0] = "КОД: Вирус для *UNIX Вымогатель";
            VirInfo[8, 0] = "КОД: Глобальный Вирус";
            VirInfo[9, 0] = "КОД: Вирус кража паролей";

            VirInfo[0, 1] = "";
            VirInfo[1, 1] = "";
            VirInfo[2, 1] = "";
            VirInfo[3, 1] = "";
            VirInfo[4, 1] = "";
            VirInfo[5, 1] = "";
            VirInfo[6, 1] = "";
            VirInfo[7, 1] = "";
            VirInfo[8, 1] = "";
            VirInfo[9, 1] = "";

            CorderInfo[0, 0] = "НАВЫК: Писать код, улучшать программы ";
            CorderInfo[1, 0] = "";
            CorderInfo[2, 0] = "";
            CorderInfo[3, 0] = "";
            CorderInfo[4, 0] = "";
            CorderInfo[5, 0] = "";
            CorderInfo[6, 0] = "";
            CorderInfo[7, 0] = "";
            CorderInfo[8, 0] = "";
            CorderInfo[9, 0] = "";

            CorderInfo[0, 1] = "Вы можете писать программы, это поможет вам обновлять программы дописывая код в Админ Панели.";
            CorderInfo[1, 1] = "";
            CorderInfo[2, 1] = "";
            CorderInfo[3, 1] = "";
            CorderInfo[4, 1] = "";
            CorderInfo[5, 1] = "";
            CorderInfo[6, 1] = "";
            CorderInfo[7, 1] = "";
            CorderInfo[8, 1] = "";
            CorderInfo[9, 1] = "";


        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(App.PatchAB + GamerInfo.Ava))
                ElipsAva.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(App.PatchAB + GamerInfo.Ava)) };

            NickName.Content = GamerInfo.GameName;
            AgeUser.Content = GamerInfo.Age;

            if (GamerInfo.Gender == 1) GenderUser.Content = "Мужской";
            else if (GamerInfo.Gender == 1) GenderUser.Content = "Женский";
            else GenderUser.Content = "n/a";

            UpdateLev();
            InfoLabel.Text = "Выберете необходимый навык чтобы получить подробную информацию";
            Label_point.Content = GamerInfo.ExtraPoint;
            ButtonUPLvl.Visibility = Visibility.Hidden;
            
            AddStars(CoderStars, GamerInfo.CoderLvl);
            AddStars(DefecerStars, GamerInfo.DefecerLvl);
            AddStars(VirStars, GamerInfo.VirLvl);
            AddStars(CrackerStars, GamerInfo.CrackLvl);


        }

        public void UpdateLev() {

            LvlUser.Content = GamerInfo.Level;
            if (GamerInfo.ExpNow == 0)
            {
                ExpUser.Visibility = Visibility.Hidden;
            }
            else
            {
                ExpUser.Visibility = Visibility.Visible;
                ExpUser.Width = 100 / (GamerInfo.ExpNextLv / GamerInfo.ExpNow);
            }
        }

        private void SelectTem(Label sender, string [,] mass, byte lvl) {

            L_Coder.Background = null;
            L_Craker.Background = null;
            L_Defecer.Background = null;
            L_Vir.Background = null;
            L_CoderB.Background = null;
            L_CrakerB.Background = null;
            L_DefecerB.Background = null;
            L_VirB.Background = null;

            sender.Background = Brushes.CornflowerBlue;           
            
                LB_Info.Items.Clear();
                for (int i = 0; i < 9; i++)
                {
                    Brush b = null;
                    if (lvl >= (i+1)) b = Brushes.GreenYellow;
                    Label label = new Label()
                    {
                        Content = mass[i, 0],
                        Padding = new Thickness(5, 0, 5, 0),
                        Background = b,
                        Tag = mass[i, 1],
                        Width = 385
                    };
                    LB_Info.Items.Add(label);
                }
            
        }

        private void AddStars(Label label, byte b)
        {
            label.Content = string.Join("", Enumerable.Repeat(STARS, b));
        }

        private void ЗакрытьОкно(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ЗакрытаФорма(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        private void Перемещение(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void ВыборКодера(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { 
                SelectTem((Label)sender, CorderInfo, App.GameGlobal.GamerInfo.CoderLvl); 
                SelectK = 1;
                if (GamerInfo.ExtraPoint != 0) ButtonUPLvl.Visibility = Visibility.Visible;
            }
        }

        private void ВыборДефейсер(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SelectTem((Label)sender, DefecerInfo, App.GameGlobal.GamerInfo.DefecerLvl);
                SelectK = 2;
                if (GamerInfo.ExtraPoint != 0) ButtonUPLvl.Visibility = Visibility.Visible;
            }
        }

        private void ВыборВирьмейкер(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { 
                SelectTem((Label)sender, VirInfo, App.GameGlobal.GamerInfo.VirLvl);
                SelectK = 3;
                if (GamerInfo.ExtraPoint != 0) ButtonUPLvl.Visibility = Visibility.Visible;
            }
        }

        private void ВыборКрекер(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { 
                SelectTem((Label)sender, CrackerInfo, App.GameGlobal.GamerInfo.CrackLvl);
                SelectK = 4;
                if (GamerInfo.ExtraPoint != 0) ButtonUPLvl.Visibility = Visibility.Visible;
            }
        }        

        private void ВыборНавыкаСписка(object sender, SelectionChangedEventArgs e)
        {
            if (LB_Info.SelectedItem is Label label)
            {
                InfoLabel.Text = label.Tag.ToString();
            }
        }

        private void Повысить_навык(object sender, RoutedEventArgs e)
        {            
            switch (SelectK)
            {
                case 1:
                    if (GamerInfo.CoderLvl != 10) 
                    {
                        GamerInfo.CoderLvl++;
                        GamerInfo.ExtraPoint--;
                    }
                    break;
                case 2:
                    if (GamerInfo.DefecerLvl != 10)
                    {
                        GamerInfo.DefecerLvl++;
                        GamerInfo.ExtraPoint--;
                    }
                    break;
                case 3:
                    if (GamerInfo.VirLvl != 10)
                    {
                        GamerInfo.VirLvl++;
                        GamerInfo.ExtraPoint--;
                    }
                    break;
                case 4:
                    if (GamerInfo.CrackLvl != 10)
                    {
                        GamerInfo.CrackLvl++;
                        GamerInfo.ExtraPoint--;
                    }
                    break;
                default:
                    break;
            }
            Label_point.Content = GamerInfo.ExtraPoint;
            if (GamerInfo.ExtraPoint == 0) ButtonUPLvl.Visibility = Visibility.Hidden;
        }
    }
}
