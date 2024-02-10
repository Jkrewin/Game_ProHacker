using System;
using System.Collections.Generic;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Профиль игрока
    /// </summary>
    [Serializable]
    public sealed class GamerInfoClass
    {
        private int exp;
        private int expNext = 100;
        private ushort level = 1;
        private readonly HashSet<string> _BonusExtraPoint = new HashSet<string>();

        /// <summary>
        /// Имя игрока
        /// </summary>
        public string GameName { get; set; } = "ProHacker";
        /// <summary>
        /// Возраст
        /// </summary>
        public short Age { get; set; } = 12;
        /// <summary>
        /// Пол
        /// </summary>
        public short Gender { get; set; } = 0;
        /// <summary>
        /// Аватарка
        /// </summary>
        public string Ava { get; set; }
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
        /// Какие бонусы были активированны в игре при чтение файлов или событий
        /// </summary>
        public HashSet<string> BonusExtraPoint { get => _BonusExtraPoint; }
        

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
        /// Способности дефейсера
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool Defecer(Enums.SkillDefecer skill)
        {
            return DefecerLvl >= ((int)skill);
        }
        /// <summary>
        /// Способности кракера
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool Cracker(Enums.SkillCrack skill)
        {
            return CrackLvl >= ((int)skill);
        }
        /// <summary>
        /// Способности вирьмекера
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool Vir(Enums.SkillVir skill)
        {
            return VirLvl >= ((int)skill);
        }
        /// <summary>
        /// Способности кодера
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool Coder(Enums.SkillCoder skill)
        {
            return CoderLvl >= ((int)skill);
        }
        /// <summary>
        /// Внутри игровой рейтинг игрока
        /// </summary>
        /// <returns></returns>
        public int RatingUser()
        {
            int t = 0;
            foreach (var item in App.GameGlobal.Servers)
            {
                t = t + item.RatingSrv();
            }
            return t * 6;
        }
        /// <summary>
        /// Добавить экста поинт для навыков
        /// </summary>
        /// <param name="prof"></param>
        public void Add_Bonus(PH4_WPF.Engine.GameEvenStruct.GetProf.ProfEnum prof ) => _BonusExtraPoint.Add(prof.ToString ());
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
                App.GameGlobal.LogAdd("Новый левел lvl:" + level , Enums.LogTypeEnum.Exp );
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
