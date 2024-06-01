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
using PH4_WPF.Engine;
using static PH4_WPF.Engine.BankClass;

namespace PH4_WPF.Browser
{    
    public partial class BankPage : Page
    {

        BankAccount AccountAct;


        public BankPage()
        {
            InitializeComponent();

            RegRS.Items.Add("<нет>");
            foreach (var item in App.GameGlobal.Bank.Accounts )
            {
                if (item.MyCash) RegRS.Items.Add(item.Rs);
            }

           
        }

        private void КликРегистрация(object sender, RoutedEventArgs e)
        {
            RegP.Visibility = Visibility.Visible;
            StartPG.Visibility = Visibility.Hidden;


            CBTypeMoney.Items.Clear();
            CBTypeMoney.Items.Add("1. Карбованци - открыть счет в валюте");
            CBTypeMoney.Items.Add("2. Счет Доллар - Основная валюта $$$");

            if(App.GameGlobal.GamerInfo.Cracker (Enums.SkillCrack.ДоступКрипте)) CBTypeMoney.Items.Add("3. BitCoin Крипто кошелек");
            
            if (App.GameGlobal.GamerInfo.Gender == 0) { Gender.Text = "Female"; }
            else if (App.GameGlobal.GamerInfo.Gender == 1) { Gender.Text = "Male"; }
            else { Gender.Text = "---"; }
            GameName.Text = App.GameGlobal.GamerInfo.GameName ;
            Age.Text = App.GameGlobal.GamerInfo.Age.ToString ();
        }

        private void КонпкаОтмена(object sender, RoutedEventArgs e)
        {
            RegP.Visibility = Visibility.Hidden;
            StartPG.Visibility = Visibility.Visible;
            ErrorText.Visibility = Visibility.Hidden;
        }

        private void КнопкаЗарегиться(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Hidden ;

            if (CBTypeMoney.Text == "") {
                ErrorText.Content = "Вы не указали тип валюты. Проверте заполнение";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (RSNew.Text == "")
            {
                ErrorText.Content = "Вы не указали номер счета. Проверте заполнение";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }


            if (App.GameGlobal.Bank.Accounts.Find(x => x.Rs == RSNew.Text) == null)
            {
                Engine.BankClass .BankAccount  account = new Engine.BankClass .BankAccount()
                {
                    Login = App.GameGlobal.GamerInfo.GameName,
                    Money = 0,
                    MyCash = true,
                    Pass = "XXXXXXXXXXXXXXX",
                    Rs = RSNew.Text ,
                    TypeMoney = (Enums.TypeMoneyEnum)Enum .Parse (typeof(Enums.TypeMoneyEnum ), CBTypeMoney.Text.Substring (0,1) )
                };
                App.GameGlobal.Bank.Accounts.Add(account);
                RegRS.Items.Clear();
                RegRS.Items.Add("<нет>");
                foreach (var item in App.GameGlobal.Bank.Accounts)
                {
                    if (item.MyCash) RegRS.Items.Add(item.Rs );
                }
                КонпкаОтмена(null, null);
            }
            else {
                ErrorText.Content = "Такой номер счета уже есть, напишите новый номер";
                ErrorText.Visibility = Visibility.Visible;
            }

           

        }


        private void ИзменениеВСписке(object sender, SelectionChangedEventArgs e)
        {
            if (RegRS.SelectedItem  != null) {
                Login.Text = App.GameGlobal.GamerInfo.GameName;
                Pass.Password  = "XXXXXXXXXXXXXXXXX";
            }
        }

        
        private void Кнопка_вход(object sender, RoutedEventArgs e)
        {
            BankAccount account;
            if (RegRS.Text != "")
            {
                // Расчетный счет игрока выбран
                account = App.GameGlobal.Bank.Accounts.Find(x => x.Rs == RegRS.Text);
            }
            else
            {
                account = App.GameGlobal.Bank.Accounts.Find(x => x.Login == Login.Text & x.Pass == Pass.Password);
            }

            if (account == null)
            {
                ErrorText.Content = "Неверный логин или пароль !";
                ErrorText.Visibility = Visibility.Visible;
            }
            else
            {
                //Успешный вход
                ButtonFine.Visibility = Visibility.Visible;
                AccountAct = account;
                NumRS.Content = account.Rs;
                AllMoney.Text = account.Money.ToString();
                LinkCnv.Maximum = account.Money;
                LinkCnv.Value = 0;
                Acc.Visibility = Visibility.Visible;
                StartPG.Visibility = Visibility.Hidden;
                foreach (var item in App.GameGlobal.Bank.Accounts)
                {
                    if (item.Rs != account.Rs) AllRS.Items.Add(item.Rs);
                }

                if (App.GameGlobal.Bank.DefaultBankAccount == AccountAct)
                {
                    CB_ПоУмолчанию.IsChecked = true;
                }
            }

        }

        private void Изменение_сумма(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MoneyC.Text =((int)LinkCnv.Value).ToString ();
            InfConvert.Content = App.GameGlobal.Bank.ConvertMoney(AccountAct.TypeMoney , (int)LinkCnv.Value, accountEx.TypeMoney );
        }

        private void Кнопка_входИзСчета(object sender, RoutedEventArgs e)
        {
            Acc.Visibility = Visibility.Hidden ;
            StartPG.Visibility = Visibility.Visible;
            ErrorText.Visibility = Visibility.Hidden;
            CB_ПоУмолчанию.IsChecked = false;
            App.GameGlobal.Bank.DefaultBankAccount = AccountAct;
            AccountAct = null;
        }

        BankAccount accountEx;
        private void ИзменениеСпискеСчетаБанка(object sender, SelectionChangedEventArgs e)
        {
            accountEx = App.GameGlobal.Bank.Accounts.Find(x => x.Rs == (string)AllRS.SelectedValue);
            InfoMoney.Content = accountEx.TypeMoney.ToString();
        }

        private void Кнопка_Удалить_счет(object sender, RoutedEventArgs e)
        {
            if (AccountAct.Money != 0) {
                ErrorText.Content = "Нельзя удалить этот счет так как на нем есть деньги";
                ErrorText.Visibility = Visibility.Visible ;
                return;
            }

            App.GameGlobal.Bank.Accounts.Remove(AccountAct);
            Кнопка_входИзСчета(null,null);
        }

        private void Кнопка_ВыполнитьПеревод(object sender, RoutedEventArgs e)
        {
            AccountAct.Money -= int.Parse(MoneyC.Text);
            AllMoney.Text = AccountAct.Money.ToString ();
            LinkCnv.Value = 0;
            accountEx.Money += int.Parse(InfConvert.Content.ToString () );
            LinkCnv.Maximum = AccountAct.Money;
        }

        private void УстановкаПоУмолчанию(object sender, RoutedEventArgs e)
        {
            if (CB_ПоУмолчанию.IsChecked == true)
            {
                App.GameGlobal.Bank.DefaultBankAccount = AccountAct;
            }
            else {
                App.GameGlobal.Bank.DefaultBankAccount = null;
            }
        }

        private void ПроверкаСчетаПоУмолчанию(object sender, MouseEventArgs e)
        {
            DefLab.Visibility = Visibility.Visible;
            if (App.GameGlobal.Bank.DefaultBankAccount == null)
            {
                DefLab.Content = "У вас отсутствует счет по умолчанию. Важно указать для прямых переводах.";
            }
            else {
                DefLab.Content = "Ваш счет по умолчанию " + App.GameGlobal.Bank.DefaultBankAccount.Rs ;
            }


           
        }

        private void ПокинулКурсор(object sender, MouseEventArgs e)
        {
            DefLab.Visibility = Visibility.Hidden;
        }

        private void Кнопка_Штрафы(object sender, RoutedEventArgs e)
        {
            ButtonFine.Visibility = Visibility.Hidden;
            string str;

            if (App.GameGlobal.FineSum == 0) str = "У вас нет не уплаченных штрафов";
            else if (AccountAct.TypeMoney == Enums.TypeMoneyEnum.Dollar) str = "Транзакция отменена. Счет должен быть только в $ для снятия штрафа";
            else
            {
                if (App.GameGlobal.FineSum > AccountAct.Money)
                {
                    App.GameGlobal.FineSum -= AccountAct.Money;
                    AccountAct.Money = 0;
                    str = "Транзакция частично успешная. У вас нехватило денег на полное погашение штрафа, погашена только его часть. Теперь осталось оплатить всего $ " + App.GameGlobal.FineSum;
                }
                else
                {
                    AccountAct.Money -= App.GameGlobal.FineSum;
                    App.GameGlobal.FineSum = 0;
                    str = "Транзакция успешная. Вы оплатили штраф полностью теперь у вас на счету (" + AccountAct.Rs + ") осталось " + AccountAct.Money + "$";
                }
                AllMoney.Text = AccountAct.Money.ToString();
            }

            MailInBox.NewMail("a1@bank.com", "Сообщение от банка", str, null);
        }
    }
}
