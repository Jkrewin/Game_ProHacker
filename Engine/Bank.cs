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
        /// Курсы валют в долларах и  Акции компании (опционально)
        /// </summary>
        public List<Stock> MonetarySystem;

        public BankClass() {
            MonetarySystem = new List<Stock>();          
            MonetarySystem.Add(new Stock(0.01, Enums.TypeMoneyEnum.Karbovantsy));
            MonetarySystem.Add(new BankClass.Stock(2, Enums.TypeMoneyEnum.Ether));
            MonetarySystem.Add(new BankClass.Stock(10, Enums.TypeMoneyEnum.BitCoin));
        }

        /// <summary>
        /// Система биржи 
        /// </summary>
        public void Monetary() {
            var rnd = new Random();
            foreach (var item in MonetarySystem)
            {
                int k; // 0-падение другая цифра рост
                double m; // сумма роста
                switch (item.Trend)
                {
                    case Stock.TrendEnum.Bearish:
                        k = 7;
                        m = 6;
                        break;
                    case Stock.TrendEnum.Bullish:
                        k = 2;
                        m = 6;
                        break;
                    case Stock.TrendEnum.Flat:
                        k = 5;
                        m = 2;
                        break;
                    default:
                        k = 3;
                        m = 1;
                        break;
                }

                double d = m * rnd.NextDouble();
                k = rnd.Next(0, k);
                if (k == 0)
                {
                    item.Сost = Math.Round(item.Сost - d, 2);
                }
                else
                {
                    item.Сost = Math.Round(item.Сost + d, 2);
                }
                item.ChangeTrend();
            }
        }

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
        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Аккаунт по индексу</returns>
        public BankAccount this[int index] { get => Accounts[index]; }

        /// <summary>
        /// Типы валют и акций (опционально)
        /// </summary>
        [Serializable]
        public sealed class Stock
        {
            private const int MAX_VALUE = 30;
            private readonly List<double> HistoryStock ;
            private int IndexHistory = 0;
            private byte DayTrend = 0;

            /// <summary>
            /// Текущий тренд валюты
            /// </summary>
            public TrendEnum Trend = TrendEnum.None; 

            /// <summary>
            /// Тип валюты
            /// </summary>
            public Enums.TypeMoneyEnum TypeMoney { get; private set; } = Enums.TypeMoneyEnum.none;
            /// <summary>
            /// Цена сейчас
            /// </summary>
            public double Сost { 
                get => HistoryStock[IndexHistory] ; 
                set {                 
                    if (IndexHistory == MAX_VALUE)
                    {
                        HistoryStock.RemoveAt(0);
                    }
                    else { IndexHistory++; }
                    HistoryStock.Add(value);                
                } }
            /// <summary>
            /// Ранее цена
            /// </summary>
            public double OldCost
            {
                get
                {
                    if (IndexHistory == 0) return 0;
                    return HistoryStock[IndexHistory - 1];                    
                }
            }

            public Stock(double cost, Enums.TypeMoneyEnum type, double startCost = 0) {
                HistoryStock = new List<double>(MAX_VALUE);
                HistoryStock.Add(cost);
                Сost = cost;
                TypeMoney = type;
            }

            public void ChangeTrend(TrendEnum trend) {
                var rnd = new Random();
                DayTrend = (byte)rnd.Next(1, 15);
                Trend = trend;
            }
            /// <summary>
            /// Меняет тренд валюты
            /// </summary>
            public void ChangeTrend() {
                if (DayTrend == 0)
                {
                    var rnd = new Random();
                    DayTrend = (byte)rnd.Next(1, 15);
                    byte g = (byte)rnd.Next(1, 6);
                    switch (g)
                    {
                        case 1:
                            Trend = TrendEnum.None;
                            break;
                        case 2:
                        case 3:
                            Trend = TrendEnum.Flat;
                            break;
                        case 4:
                            Trend = TrendEnum.Bearish;
                            break;
                        case 5:
                            Trend = TrendEnum.Bullish;
                            break;
                        default:
                            break;
                    }
                }
                else {
                    DayTrend--;
                }
            }

            /// <summary>
            /// Показывает разницу в падение или росте
            /// </summary>
            /// <returns></returns>
            public string RisingCost()
            {
                if (OldCost > Сost)
                {
                    return "-" + Math.Round(OldCost - Сost, 3).ToString();
                }
                else
                {
                    return "+" + Math.Round(Сost - OldCost, 3).ToString();
                }
            }

            /// <summary>
            /// Типы тренда 
            /// </summary>
            public enum TrendEnum
            {
                /// <summary>
                /// тренд роста
                /// </summary>
                Bearish,
                /// <summary>
                /// тренд падения
                /// </summary>
                Bullish,
                /// <summary>
                /// обычный тренд
                /// </summary>
                Flat,
                /// <summary>
                /// не определен
                /// </summary>
                None
            }
        }

        /// <summary>
        /// Банковский счет
        /// </summary>
        [Serializable]
        public sealed class BankAccount
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
