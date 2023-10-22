using System;
using System.Collections.Generic;
using System.Text;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Профиль игрока
    /// </summary>
    [Serializable]
    public class GamerInfoClass
    {
        private int exp;
        private int expNext = 100;
        private ushort level = 1;

        /// <summary>
        /// Имя игрока
        /// </summary>
        public string GameName { get; set; } = "ProHacker";
        /// <summary>
        /// Возраст
        /// </summary>
        public short Age = 12;
        /// <summary>
        /// Пол
        /// </summary>
        public short Gender = 0;
        /// <summary>
        /// Аватарка
        /// </summary>
        public string Ava;
        /// <summary>
        /// Опыт сейчас
        /// </summary>
        public int ExpNow { get => exp; }
        /// <summary>
        /// Необходимый опыт для слелующего левела
        /// </summary>
        public int ExpNextLv { get => expNext; }
        /// <summary>
        /// Уровень игрока 
        /// </summary>
        public ushort Level { get => level; }
        /// <summary>
        /// Уровень найденных уязвимостей чем выше тем больше появляеться эксплойтов
        /// </summary>
        public byte HiTecLevel { get; set; } = 5;
        /// <summary>
        /// Влияет на цены чем выше тем товар дороже
        /// </summary>
        public double MultiplierPrices { get; set; } = 1.2;

        /// <summary>
        /// Очки навыка
        /// </summary>
        public byte ExtraPoint = 0;
        /// <summary>
        /// кодер завезды
        /// </summary>
        public byte CoderLvl = 0;
        /// <summary>
        /// дефесер лвл
        /// </summary>
        public byte DefecerLvl = 0;
        /// <summary>
        /// вирьмейкер
        /// </summary>
        public byte VirLvl = 0;
        /// <summary>
        /// крекер левл
        /// </summary>
        public byte CrackLvl = 0;

        /// <summary>
        /// Есть ли доступ к крипте у игрока
        /// </summary>
        public bool Cripta { get; set; } = false;
               

        /// <summary>
        /// Добавить опыт к игроку
        /// </summary>
        /// <param name="e"></param>
        public void AddExp(int e)
        {
            exp += e;
            if (exp > expNext)
            {
                expNext *= 2;
                level++;
                ExtraPoint++;
                App.GameGlobal.LogAdd("!Новый левел lvl:" + level );
            }
            // Обновить пункты если окно открыто со статусом левела 
            var s = typeof(FrmSoft.FrmIdUser).FullName;
            if (App.GameGlobal.ActiveApp.ContainsKey(s)) {
                var frm = App.GameGlobal.ActiveApp[s] as FrmSoft.FrmIdUser;
                frm.UpdateLev();
            }
        }

    }
}
