using PH4_WPF.Engine;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PH4_WPF.FrmSoft
{

    public partial class FrmIdUser : Window
    {
        readonly SkillInfo[] DefecerInfo;
        readonly SkillInfo[] CrackerInfo;
        readonly SkillInfo[] VirInfo;
        readonly SkillInfo[] CorderInfo;

        byte SelectK = 0; 
        const char STARS = '';
        readonly GamerInfoClass GamerInfo;

        readonly struct SkillInfo
        {
            public readonly string Title { get; }
            public readonly string Info { get; }
            public readonly byte Level { get; }

            public SkillInfo(byte level, string title, string info)
            {
                Title = title;
                Info = info;
                Level = level;
            }
        }
        
        public FrmIdUser()
        {
            InitializeComponent();
            GamerInfo = App.GameGlobal.GamerInfo;

            SkillInfo[] skill;

            // перевое значение level это значение  SkillDefecer 
            skill = new SkillInfo[] {
                new SkillInfo ( 1, "[Н] Перезапуск сервера и отключение", "Теперь вы знаете, как вырубить сервер, после этого он перезапустить через некоторое время. Вы получите опыт. Сервер после этого повысит свою безопасность. Доступно в статусе сервера. "),
                new SkillInfo ( 2, "[Н] Взлом сайта, сообщить о deface", " "),
                new SkillInfo ( 4, "[A] Повышает скрытность на сервере", " "),
                new SkillInfo ( 3, "[Н] Возможность создавать маршруты серверов", " "),
                new SkillInfo ( 5, "[S] Очистка логов", " "),
                new SkillInfo ( 6, "Открыть шкалу рейтинга", " "),
                new SkillInfo ( 7, "Возможность избежать обнаружения", " "),
                new SkillInfo ( 8, "", " "),
                new SkillInfo ( 10, "Финальное задание", " "),
                new SkillInfo ( 9, "Доступ к уязвимостям 0 day", " ")               
            };

            DefecerInfo =  skill.OrderBy(x => x.Level ).ToArray();

            skill = new SkillInfo[] {
                new SkillInfo ( 1, "Собирать хеш-пароли в сети и серверов", "Термин brute-force обычно используется в контексте хакерских атак на логины/пароли. Перебор паролей для получение доступа к серверу"),
                new SkillInfo ( 2, "Скидка на все программы", "Сервера собирают информацию об активности на сервере, очистка логов повысит ваши шансы быть скрытым."),
                new SkillInfo ( 3, "Торговля хешами (кодами)", " "),
                new SkillInfo ( 4, "Делать заказы на перебор хешей", " "),
                new SkillInfo ( 5, "Доступ к криптовалюте", " "),
                new SkillInfo ( 6, "Новая крипта", " "),
                new SkillInfo ( 7, "", " "),
                new SkillInfo ( 8, "", " "),
                new SkillInfo ( 9, "Снижает стоимость железа", " "),
                new SkillInfo ( 10, "Большая скидка на программы", " ")
            };

            CrackerInfo = skill.OrderBy(x => x.Level).ToArray();

            skill = new SkillInfo[] {
                new SkillInfo ( 1, "[C] Тело Вируса для Win систем", " "),
                new SkillInfo ( 2, "[C] Вирус Зомби", " "),
                new SkillInfo ( 3, "[C] Вирус Червя", " "),
                new SkillInfo ( 4, "[C] Тело Вируса для *nix систем", " "),
                new SkillInfo ( 5, "[C] Вирус Вымогатель", " "),
                new SkillInfo ( 6, "[C] Вирус для *UNIX Зомби", " "),
                new SkillInfo ( 7, "[C] Вирус для *UNIX Червя", " "),
                new SkillInfo ( 8, "[C] Вирус для *UNIX Вымогатель", " "),
                new SkillInfo ( 9, "[C] Глобальный Вирус", " "),
                new SkillInfo ( 10, "[C] Вирус кража паролей", " ")
            };

            VirInfo = skill.OrderBy(x => x.Level).ToArray();

            skill = new SkillInfo[] {
                new SkillInfo ( 1, "[Н] Писать код, улучшать программы", "Вы можете писать программы, это поможет вам обновлять программы дописывая код в Админ Панели."),
                new SkillInfo ( 2, "[S] Исходник для поиска данных о банковских счетах", "Помогает искать банковские данные на сервере, скрип доступен в браузере на стартовой странице в разделе Мои Скрипты."),
                new SkillInfo ( 3, "[S] Улучшает Brute-force", " "),
                new SkillInfo ( 4, "[S] Улучшает сканер", " "),
                new SkillInfo ( 5, "[S] Код для поиска важных файлов на сервере", " "),
                new SkillInfo ( 6, "[Н] Писать код как профи", "Повышает качество кода, увеличивает полезность кода"),
                new SkillInfo ( 7, "[Н] Ускоряет майнинг крипты", " "),
                new SkillInfo ( 8, "Падение ваших серверов не влияет на результат", " "),
                new SkillInfo ( 9, "Ошибки на серверах не случаються", " "),
                new SkillInfo ( 10, "Продвитуный софт оптимизирует сервере", " ")
            };

            CorderInfo = skill.OrderBy(x => x.Level).ToArray();

        }

        private void Загруженно(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(App.PatchAB + GamerInfo.Ava))
                ElipsAva.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(App.PatchAB  + GamerInfo.Ava)) };

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

            L_MissionInfo.Text = App.GameGlobal.GameScen.ActiveScen.Title;

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


        private void SelectTem(Label sender, SkillInfo[] mass, byte lvl) {

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
                        Content = mass[i].Title,
                        Padding = new Thickness(5, 0, 5, 0),
                        Background = b,
                        Tag = mass[i].Info,
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
