using System;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Уязвимость для серверов
    /// </summary>
    [Serializable]
    public sealed class Vulnerabilities
    {
        private readonly int id;
        /// <summary>
        /// ID для быстрого поиска так как есть много совпадений по названию
        /// </summary>
        public int ID { get => id; }
        /// <summary>
        /// Соотвествует название программы из втрого стобца списка potrs.txt
        /// </summary>
        public string CName;
        /// <summary>
        /// Название уязвимости 
        /// </summary>
        public string NameBug;
        /// <summary>
        /// True - это платная программа 
        /// </summary>
        public bool Shareware = false;

        public ushort VerA = 0;
        public int VerB = 0;
        /// <summary>
        /// Изучена уязвимость (true) значит она используеться 
        /// </summary>
        public bool Studied = false;
        /// <summary>
        /// Какой уровень доступа дает эта уязвимость
        /// </summary>
        public Server.PremissionServerEnum GrantPremission = Server.PremissionServerEnum.none;
        /// <summary>
        /// Указывает что есть эксплойт по этой уязвимости
        /// </summary>
        public bool Exploid = false;

        public Vulnerabilities()
        {
            while (App.GameGlobal.VulnerabilitiesList.Find(x => x.ID == id) != null)
            {
                id = App.GameGlobal.VulnerabilitiesList.Count + 1;
            }
        }

    }
}
