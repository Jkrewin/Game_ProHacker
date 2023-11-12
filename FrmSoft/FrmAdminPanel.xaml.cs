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
using static PH4_WPF.Engine.Virtualization;

namespace PH4_WPF.FrmSoft
{
    public partial class FrmAdminPanel : Window
    {
        readonly Server MainServer;
        Virtualization VirtualSrv { get => MainServer.VirtualizationServer; }
        Virtualization.InstaceClass SelectedInstance = null;
        Virtualization.InstaceClass TagInstace;
        private readonly System.Windows.Threading.DispatcherTimer EchoCm = new System.Windows.Threading.DispatcherTimer();


        public FrmAdminPanel(Server srv)
        {
            InitializeComponent();

            FormCrateSrev.Visibility = Visibility.Hidden;
            MainServer = srv;
            Label_Url.Content = srv.NameSrv;

            EchoCm.Tick += new EventHandler(АнимацияЗадержки);
            EchoCm.Interval = TimeSpan.FromMilliseconds(2000);
            EchoCm.Stop();           

            if (srv.VirtualizationServer == null)
            {
                srv.VirtualizationServer = new Virtualization();
                InstaceTypeEnum[] defaultTypes = new InstaceTypeEnum[]
                { InstaceTypeEnum.FTP ,
                    InstaceTypeEnum.MySql ,
                    InstaceTypeEnum.WebForum ,
                    InstaceTypeEnum.BanerAD ,
                    InstaceTypeEnum.Mail
                };
                foreach (var item in defaultTypes)
                {
                    MainServer.VirtualizationServer.Instance.Add(MainServer.VirtualizationServer.Role_Templates(item));
                }
                MainServer.VirtualizationServer.MaxPower = MainServer.PopularSRV * 45;

            }
            Refreh_ListVM();
            App.GameGlobal.MainWindow.NewDayEvent += UpdateDayUpgrade; //каждое обновление дня обновляет инфу о сервере
            App.GameGlobal.MainWindow.Event_Completed += EventComplited; //если произошло событие в которм зарешился апгрейд этого сервера
        }

        private void HideAllPage() {
            Page1.Visibility = Visibility.Hidden;
            Page2.Visibility = Visibility.Hidden;
            Page3.Visibility = Visibility.Hidden;
        }

        private void EventComplited(GameEvenStruct.IEventGame eventGame) {
            if (eventGame is GameEvenStruct.UpgradeSoft soft) {
                if (soft.ServerName == MainServer.NameSrv) {                    
                    Refreh_ListVM();                 
                }
            }
        }

        private void АнимацияЗадержки(object sender, EventArgs e) {
            if (TagInstace.StatusInstance != Virtualization.StatusInstanceEnum.Working)
            {

                if ((MainServer.VirtualizationServer.SummarPower + TagInstace.KVT_Ver * TagInstace.VerA) > PowerServer.Maximum)
                {
                    App.GameGlobal.Msg("", "Невозможно запустить так как сервер будет перегружен, высокая нагрузка на сервер.", FrmError.InformEnum.Информация);

                }
                else
                {

                    bool b = MainServer.VirtualizationServer.CheckDependencies(TagInstace);
                    if (b)
                    {
                        TagInstace.StatusInstance = StatusInstanceEnum.Working;
                        StartStop.Content = "";
                        App.GameGlobal.EventIntroduce(GameScen.ScenStruct.ConditionStruct.ConditionEnum.ЗапущенаСлужбаНаСервере,
                            new string[] { MainServer.NameSrv, TagInstace.InstaceType.ToString() });
                        Refreh_ListVM();
                    }
                    else
                    {
                        App.GameGlobal.Msg("", MainServer.VirtualizationServer.MSG_error, FrmError.InformEnum.Информация);
                    }
                }
            }
            else
            {
                TagInstace.StatusInstance = StatusInstanceEnum.Stopping;
                MainServer.VirtualizationServer.ServiceControl();
                Refreh_ListVM();
            }
            PG_InProcess.Visibility = Visibility.Hidden ;
            EchoCm.Stop();           
        }

        private void Refreh_ListVM()
        {
            var bc = new BrushConverter();
            if (Page1.Visibility == Visibility.Visible)
            {
                LsVM.Items.Clear();
                foreach (var item in MainServer.VirtualizationServer.Instance)
                {
                    LsVM.Items.Add(MainServer.VirtualizationServer.CreateListBoxItem(item));
                }
            }
            PowerServer.Maximum = MainServer.VirtualizationServer.MaxPower;           
            if (MainServer.VirtualizationServer.SummarPower>= MainServer.VirtualizationServer.MaxPower ){
                PowerServer.Value = PowerServer.Maximum;
                PowerServer.Foreground = (Brush)bc.ConvertFrom("#FFF92323"); 
            } else {
                PowerServer.Value = MainServer.VirtualizationServer.SummarPower;
                 PowerServer .Foreground = (Brush)bc.ConvertFrom("#FFF2C94D"); //#FFF2C94D
            }

            PowerServer.ToolTip = "Максимальная: " + PowerServer.Maximum + "Квт" + "\nТекущая: "+ VirtualSrv.SummarPower;
            L_ОбщееKVT.Content = PowerServer.Maximum +" KVT";
            // todo
            PB_popular.Maximum = MainServer.PopularSRV * 155;
            PB_popular.Value = VirtualSrv.SummarPopular;

            L_DayPop.Content = "посещаемость в день: " + PB_popular.Value;
        }

        private void UpdateDayUpgrade() {
            if (Page2.Visibility == Visibility.Visible & VirtualSrv.CheckUpgradeNow)
            {
                LabelProdNow.Content = VirtualSrv.ProccesNowUpgade.Value.NewVer.InstaceType.ToString();
                PB_ProccesWork.Maximum = (VirtualSrv.ProccesNowUpgade.Value.EvenLink.DataStart - VirtualSrv.ProccesNowUpgade.Value.DataStart).TotalDays;
                PB_ProccesWork.Value = PB_ProccesWork.Maximum - (VirtualSrv.ProccesNowUpgade.Value.EvenLink.DataStart - App.GameGlobal.DataGM).TotalDays;
            }
            else if (Page2.Visibility == Visibility.Visible & VirtualSrv.CheckUpgradeNow == false)
            {
                PB_ProccesWork.Value = 0;
                LabelProdNow.Content = "Жду разработки....";
            } 
        }

        private void Перетаскивание(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void Выделяет_кнопку_выход(object sender, MouseEventArgs e) => ExitButton.Fill = Brushes.IndianRed;
        private void ПрекращаетВыделение(object sender, MouseEventArgs e) => ExitButton.Fill = Brushes.DarkRed;
        private void УдалениеКнопка(object sender, MouseButtonEventArgs e) => this.Close();
        private void ФормаЗакрыта(object sender, EventArgs e) => App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);

        private void МышкаНадКнопкой(object sender, MouseEventArgs e)
        {
            Selector.Visibility = Visibility.Visible;
            Thickness thickness = ((Label)sender).Margin;
            ((Label)sender).Foreground = Brushes.Black;
            Selector.Margin = new Thickness(thickness.Left - 2, thickness.Top - 2, thickness.Right, thickness.Bottom);
        }

        private void ФормаВыделение(object sender, MouseEventArgs e)
        {
            // Selector.Visibility = Visibility.Hidden;
        }

        private void Выход_выделения(object sender, MouseEventArgs e)
        {
            Selector.Visibility = Visibility.Hidden;
            ((Label)sender).Foreground = Brushes.White;
        }

        private void КликПоРоли(object sender, MouseButtonEventArgs e)
        {
            HideAllPage();
            Page1.Visibility = Visibility.Visible;
            Refreh_ListVM();
        }

        private void ЗапускСервиса(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed & PG_InProcess.Visibility == Visibility.Hidden)
            {
                if (LsVM.SelectedItem is ListBoxItem v)
                {
                    PG_InProcess.Visibility = Visibility.Visible;
                    TagInstace = v.Tag as Virtualization.InstaceClass;
                    EchoCm.Start();
                }
            }
        }

        private void КнопкаЗапускНад(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            ((Label)sender).Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFD1C7CB");
        }

        private void КнопкаПотеряФокус(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            ((Label)sender).Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFD4D8F1");
        }

        private void ВыборЭлемента(object sender, SelectionChangedEventArgs e)
        {
            if (LsVM.SelectedItem is ListBoxItem v)
            {
                Virtualization.InstaceClass Tag = v.Tag as Virtualization.InstaceClass;

                if (Tag.StatusInstance == Virtualization.StatusInstanceEnum.Working) { StartStop.Content = ""; }
                else { StartStop.Content = ""; }

            }
        }
       
        private void НовыеСервисы(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed & PG_InProcess.Visibility == Visibility.Hidden)
            {
                FormCrateSrev.Visibility = Visibility.Visible;

                CB_Serv.Items.Clear();
                foreach (InstaceTypeEnum item in Enum.GetValues(typeof(InstaceTypeEnum )).Cast<InstaceTypeEnum>().ToList())
                {
                    if (MainServer.VirtualizationServer.Instance.FindIndex (x => x.InstaceType == item) == -1) CB_Serv.Items.Add(item.ToString()); 
                }

                
            }
        }

        private void УбратьСписок(object sender, RoutedEventArgs e)
        {
            FormCrateSrev.Visibility = Visibility.Hidden;
        }

        private void СоздатьРоль(object sender, RoutedEventArgs e)
        {
            string str = CB_Serv.Text;
            if (str != "")
            {
                MainServer.VirtualizationServer.Instance.Add(MainServer.VirtualizationServer.Role_Templates((InstaceTypeEnum)Enum.Parse(typeof(InstaceTypeEnum), str, true)));
                Refreh_ListVM();
                FormCrateSrev.Visibility = Visibility.Hidden;
            }

        }

        private void ВыборСервисаНового(object sender, SelectionChangedEventArgs e)
        {
            string str = CB_Serv.Text;
            if (str != "")
            {
                InfoServ.Content= MainServer.VirtualizationServer.GetTextName((InstaceTypeEnum)Enum.Parse(typeof(InstaceTypeEnum), str, true));
            }
        }

        private void ПоднятьНовуюВерсию(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed & PG_InProcess.Visibility == Visibility.Hidden)
            {
                if (LsVM.SelectedItem is ListBoxItem v)
                {
                    HideAllPage();
                    SelectedInstance = v.Tag as InstaceClass;
                    StartUpProg.Visibility = SelectedInstance.UpdateSoft ? Visibility.Hidden : Visibility.Visible;
                    Page2.Visibility = Visibility.Visible;
                    LabelNameProgramm.Content  = SelectedInstance.InstaceType.ToString();

                    L_old_kvt.Content = SelectedInstance.KVT;
                    L_old_popular.Content = SelectedInstance.Popular;
                    L_new_kvt.Content = SelectedInstance.KVT;
                    L_new_polular.Content = (SelectedInstance.VerB + 1) * SelectedInstance.Popular_Ver;

                }

            }
        }

        private void UgProg(object sender, MouseButtonEventArgs e)
        {
            if (MainServer.VirtualizationServer.ProccesNowUpgade == null) return;

            SelectedInstance.UpdateSoft = true;
            DateTime date_end = App.GameGlobal.DataGM;
            date_end=date_end.AddDays (40);

            byte verb = SelectedInstance.VerB ;
            verb++;

            Virtualization.InstaceClass instace = new InstaceClass()
            {
                InstaceType = SelectedInstance.InstaceType,
                VerA = SelectedInstance.VerA,
                VerB = verb,
                StatusInstance = StatusInstanceEnum.Stopping,
                UpdateSoft= false 
            };

            MainServer.VirtualizationServer.UpgadeSoft(instace, date_end, Label_Url.Content.ToString());
            if (App.GameGlobal.GameSpeed == Game.GameSpeedEnum.Pause )  LabelProdNow.Content = "Запустите время игры"; 
            StartUpProg.Visibility = Visibility.Hidden;
        }

        private void КликПоВерсиям(object sender, MouseButtonEventArgs e)
        {
            HideAllPage();           
            Page3.Visibility = Visibility.Visible;            
        }

        private void Маштабирование(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
