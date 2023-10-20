using System;
using System.Collections.Generic;
using System.Text;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Банковская система
    /// </summary>
    [Serializable]
    public class BankClass
    {
        /// <summary>
        /// Счета банков
        /// </summary>
        public List<BankAccount> Accounts = new List<BankAccount>();
        /// <summary>
        /// Счет по умолчанию для списания средств
        /// </summary>
        public BankAccount DefaultBankAccount { get; set; }

        // Курсы валют в долларах 
        public double BitCoin = 10;
        public double Karbovantsy = 0.01;
        public double Ether = 2;

        /// <summary>
        /// Конвертатор валют
        /// </summary>
        /// <param name="inMoney">Эти деньги</param>
        /// <param name="sum">Суииа этих денег</param>
        /// <param name="toMoney">В эту валюту конвертируем</param>
        /// <returns>Сумма в валюте <i>toMoney</i></returns>
        public int ConvertMoney(BankAccount.TypeMoneyEnum inMoney, int sum, BankAccount.TypeMoneyEnum toMoney) {
            switch (inMoney)
            {
                case BankAccount.TypeMoneyEnum.Karbovantsy:
                    sum =(int)( Convert.ToDouble ( sum) * Karbovantsy);
                    break;
                case BankAccount.TypeMoneyEnum.Ether:
                    sum = (int)(Convert.ToDouble(sum) * Ether);
                    break;
                case BankAccount.TypeMoneyEnum.BitCoin:
                    sum = (int)(Convert.ToDouble(sum) * BitCoin);
                    break;
                case BankAccount.TypeMoneyEnum.Dollar:
                default:                   
                    break;
            }

            double f;
            switch (toMoney)
            {
                case BankAccount.TypeMoneyEnum.Karbovantsy:
                    f = Karbovantsy;
                    break;
                case BankAccount.TypeMoneyEnum.Ether:
                    f = Ether;
                    break;
                case BankAccount.TypeMoneyEnum.BitCoin:
                    f = BitCoin;
                    break;
                case BankAccount.TypeMoneyEnum.Dollar:
                default:
                    f = 1;
                    break;
            }           
            return (int)(sum / f);
        }

        /// <summary>
        /// Банковский счет
        /// </summary>
        [Serializable]
        public class BankAccount
        {
            public string Rs;
            public string Login;
            public string Pass;
            public TypeMoneyEnum TypeMoney;
            public int Money;
            /// <summary>
            /// Счет игрока вход без логина и пароля
            /// </summary>
            public bool MyCash = false;

            public enum TypeMoneyEnum
            {
                Dollar = 2,
                Karbovantsy = 1,
                Ether = 4,
                BitCoin = 3
            }
        }

    }
}
