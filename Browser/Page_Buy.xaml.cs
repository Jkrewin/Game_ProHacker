﻿using PH4_WPF.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PH4_WPF.Browser
{
    
    public partial class Page_Buy : Page
    {
        readonly DownloadText DT;
        readonly Page PageOld; 
        public Page_Buy(DownloadText downloadText , Page oldPage )
        {
            InitializeComponent();
            DT = downloadText;
            PageOld = oldPage;
            L_NameText.Content = DT.NameBug;
            L_Type.Content = DT.TypeProg.ToString ();
            L_Price.Content = DT.Price+"$";
        }

        public  struct DownloadText
        {
            public string NameBug;
            public Enums.TypeParam  TypeProg;
            public string ID;
            public string Price;
            public string ValueString;
        }

        private void Отмена(object sender, RoutedEventArgs e)
        {
            ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).FrameBrouser.Navigate(PageOld);
        }

        private void Покупка(object sender, RoutedEventArgs e)
        {
            L_ErrorText.Visibility = Visibility.Hidden;
            var rand = new Random();
            // Если нужно купить программу 
            string dt = DT.ID == "" ? "0" : DT.ID;
            if (L_Price.Content.ToString () !="0$")
            {
                // Это платный софт нужно купить 
                if (App.GameGlobal.Bank.DefaultBankAccount == null)
                {
                    ErrorText("У вас нет аккаунта, вы должны создать и установить по умолчанию");
                    return;
                }
                if (App.GameGlobal.Bank.DefaultBankAccount.TypeMoney != Enums.TypeMoneyEnum.Dollar)
                {
                    ErrorText("Счет должен быть в $$$");
                    return;
                }
                if (App.GameGlobal.Bank.DefaultBankAccount.Money <= int.Parse(DT.Price))
                {
                    ErrorText("У вас недостаточно средств на счете");
                    return;
                }
                if (FileServerClass.Exist("/user/Hpro4/Download/", DT.NameBug, "pl", App.GameGlobal.MyServer))
                {
                    ErrorText("Этот файл был скачен ранее");
                    return;
                }
                // Тут покупка
                App.GameGlobal.Bank.DefaultBankAccount.Money = App.GameGlobal.Bank.DefaultBankAccount.Money - int.Parse(DT.Price);
                App.GameGlobal.LogAdd("Вы купили программу", Enums.LogTypeEnum.Money);
                ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).StartDownload(DT.NameBug,
                    new FileServerClass.ParameterClass()
                    {
                        TypeInformation = DT.TypeProg,
                        IntParam = int.Parse(dt),
                        TextCommand = DT.ValueString
                    }, rand.Next(150, 800), "pl");
                App.GameGlobal.SoundSignal("buy");
                Отмена(null, null);
            }
            else
            {
                if (FileServerClass.Exist("/user/Hpro4/Download/", DT.NameBug, "pl", App.GameGlobal.MyServer))
                {
                    ErrorText("Этот файл был скачен ранее");
                }
                else
                {
                    // тут бесплатная скачка
                    ((FrmBrowser)App.GameGlobal.ActiveApp["PH4_WPF.Browser.FrmBrowser"]).StartDownload(DT.NameBug,
                        new FileServerClass.ParameterClass() { TypeInformation = DT.TypeProg, IntParam = int.Parse(dt), TextCommand = DT.ValueString  },
                        rand.Next(150, 800), "pl");  
                    Отмена(null, null);
                }
                
            }

        }

        private void ErrorText(string txt) {
            L_ErrorText.Visibility = Visibility.Visible;
            L_ErrorText.Content = txt;
        }
    }
}
