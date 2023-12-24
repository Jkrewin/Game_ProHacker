using System;
using System.Collections.Generic;
using System.Text;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Банковская система
    /// </summary>
    [Serializable]
    public sealed class BankClass
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
        public int ConvertMoney(Enums.TypeMoneyEnum inMoney, int sum, Enums.TypeMoneyEnum toMoney) {
            switch (inMoney)
            {
                case Enums.TypeMoneyEnum.Karbovantsy:
                    sum =(int)( Convert.ToDouble ( sum) * Karbovantsy);
                    break;
                case Enums.TypeMoneyEnum.Ether:
                    sum = (int)(Convert.ToDouble(sum) * Ether);
                    break;
                case Enums.TypeMoneyEnum.BitCoin:
                    sum = (int)(Convert.ToDouble(sum) * BitCoin);
                    break;
                case Enums.TypeMoneyEnum.Dollar:
                default:                   
                    break;
            }

            var f = toMoney switch
            {
                Enums.TypeMoneyEnum.Karbovantsy => Karbovantsy,
                Enums.TypeMoneyEnum.Ether => Ether,
                Enums.TypeMoneyEnum.BitCoin => BitCoin,
                _ => 1,
            };
            return (int)(sum / f);
        }

        public BankAccount this[int index] { get => Accounts[index]; }

        /// <summary>
        /// Банковский счет
        /// </summary>
        [Serializable]
        public class BankAccount
        {
            public string Rs;
            public string Login;
            public string Pass;
            public Enums.TypeMoneyEnum TypeMoney;
            public int Money;
            /// <summary>
            /// Счет игрока вход без логина и пароля
            /// </summary>
            public bool MyCash = false;

           
        }

    }
}
