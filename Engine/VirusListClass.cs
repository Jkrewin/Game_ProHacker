using System;
using System.Collections.Generic;
using System.Linq;

namespace PH4_WPF.Engine
{
    [Serializable]
    public sealed class VirusListClass
    {
        /// <summary>
        /// Антивирусная база 
        /// </summary>
        private readonly List<AntivirusBase> VirusList = new List<AntivirusBase>();
        /// <summary>
        /// Создает агоритм работо вируса червь
        /// </summary>
        private readonly Dictionary<VirusStruct, Stack<InfectedSysClass>> Worms = new Dictionary<VirusStruct, Stack<InfectedSysClass>>();

        /// <summary>
        /// Привязка с событию создания вируса чтобы отслеживать его 
        /// </summary>
        public GameEvenClass EventCreateVirus {get; set;}
        /// <summary>
        /// Зараженные вирусом системы
        /// </summary>
        public List<InfectedSysClass> InfectedSys { get; set; } = new List<InfectedSysClass>();
        
        
        /// <summary>
        /// Обновляет базу вирусов
        /// </summary>
        public void Update_AntivirusBase()
        {
            VirusList.ForEach(x => x.IsWarn = true);
        }

        /// <summary>
        /// Вирус заражает сервера любые с самым минимальным типом допуска с авто поиском
        /// </summary>
        public void InfectedZom(FileServerClass file, bool forUnix = false) {
            var param = file.FileСontents;
            //сборка вируса
            VirusStruct virus = new VirusStruct("Вирус Zombie v" + param.IntParam,
                forUnix == false ? VirusStruct.TypeVirusEnum.ZombieWin : VirusStruct.TypeVirusEnum.ZombieUnix,
                param.IntParam);
            AddVirus(virus);

            foreach (var item in App.GameGlobal.Servers  )
            {
                if (item.NameSrv != App.GameGlobal.MyServer.NameSrv || item.Premision != Server.PremissionServerEnum.none) { 
                   if (virus .Rats >=  item.PopularSRV ){
                        item.Premision = Server.PremissionServerEnum.Zombies;
                    }
                }
            }
        }

        /// <summary>
        /// Вирус автом. распространяемый 
        /// </summary>
        /// <param name="file"></param>
        public void InfectedWorm(FileServerClass file)
        {
            //сборка вируса
            var param = file.FileСontents;
            VirusStruct virus = new VirusStruct("Вирус Worm v" + param.IntParam, VirusStruct.TypeVirusEnum.Worms, param.IntParam);
            AddVirus(virus);

            if (Worms.ContainsKey(virus))
            {
                //Создание алгоритма 
                Stack<InfectedSysClass> classes = new Stack<InfectedSysClass>();
                for (int i = 0; i < param.IntParam * 10; i++)
                {
                    InfectedSysClass infected = new InfectedSysClass(virus, "CompUser");
                    classes.Push(infected);
                }
                Worms[virus] = classes;
            }
        }

        /// <summary>
        /// Вымогатель
        /// </summary>
        /// <param name="file"></param>
        /// <param name="forUnix"></param>
        public void InfectedRansomware(FileServerClass file, bool forUnix = false) { 
        
        
        }

        /// <summary>
        /// Запуск вируса первого типа 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="file"></param>
        public void InfectedVirA(Server server, ref FileServerClass file, bool forUnix = false  ) {
            var param = file.FileСontents;

            //сборка вируса
            VirusStruct virus;
            if (forUnix == false) virus = new VirusStruct("Вирус Export v" + param.IntParam,
                forUnix != false ? VirusStruct.TypeVirusEnum.StandartWin: VirusStruct.TypeVirusEnum.StandartUnix,
                param.IntParam);
            else virus = new VirusStruct("Вирус Export v" + param.IntParam, VirusStruct.TypeVirusEnum.StandartUnix, param.IntParam);
            AddVirus(virus);

            // попытка запуска вируса           
            if (!VirusAct(virus))
            {
                // Успешный запуск вируса
                InfectedSys.Add(new InfectedSysClass(virus, server));
                server.Premision = Server.PremissionServerEnum.FullControl;
            }
            else
            {
                // неудача запуска
                file.FileDel();
                App.GameGlobal.LogAdd("Вирус был обнаружен админом " + server.NameSrv, Enums.LogTypeEnum.Server);
                server.AdministratorWarning();
            }
        }

        private void AddVirus(VirusStruct virus)
        {
            if (virus.TypeVirus == VirusStruct.TypeVirusEnum.Worms)
            {
                if (!Worms.ContainsKey(virus)) VirusList.Add(new AntivirusBase(virus));
            }
            else
            {
                if (!VirusList.Any(x => x.Virus.Equals(virus))) VirusList.Add(new AntivirusBase(virus));
            }
        }

        private bool VirusAct(VirusStruct virus) {
            // Выявленный вирус антивирусом или нет 
            var result = VirusList.Find(x => x.Virus.Equals(virus));
            if (result == null) return false;
            else return result.IsWarn;
        }

        /// <summary>
        /// Проверяет работу вирусов
        /// </summary>
        /// <param name="days"></param>
        public void Worms_Work(int days)
        {
            foreach (var item in Worms)
            {
                if (item.Value.Count == 0)
                {
                    Worms.Remove(item.Key);
                    break;
                }

                InfectedSys.Add(item.Value.Pop());
            }
        }

        /// <summary>
        /// Зараженная система
        /// </summary>
        [Serializable]
        public class InfectedSysClass {
            /// <summary>
            /// Вирус
            /// </summary>
            public VirusStruct Virus;
            /// <summary>
            /// Тип системы
            /// </summary>
            public string[] Info { get => new string[] { CompUser =="" ?  Server.NameSrv + "/" + Server.OSName : CompUser,
                Virus.TypeVirus.ToString(), Virus.NameVirus }; }
            /// <summary>
            /// Сервер
            /// </summary>
            public Server Server;
            /// <summary>
            /// Если был заражен простой пользователь
            /// </summary>
            public string CompUser = "";

            public InfectedSysClass(VirusStruct virus, Server srv) {
                Virus = virus;
                Server = srv;                
            }

            public InfectedSysClass(VirusStruct virus, string compUser)
            {
                Virus = virus;
                CompUser = compUser;
            }
        }

        /// <summary>
        /// База данных антивируса
        /// </summary>
        [Serializable]
        public class AntivirusBase {
            /// <summary>
            /// Находиться в базе антивируса
            /// </summary>
            public VirusStruct Virus;
            /// <summary>
            /// Количество зараженных систем
            /// </summary>
            public int InfectStat = 0;
            /// <summary>
            /// Вред причененный  в сети
            /// </summary>
            public float Harm = 0f;
            /// <summary>
            /// Обнаружен вирус или нет 
            /// </summary>
            public bool IsWarn = false;

            public AntivirusBase(VirusStruct virus) {
                Virus = virus;
            }
        }

        /// <summary>
        /// Вирус
        /// </summary>
        [Serializable]
        public readonly struct VirusStruct {
            readonly DateTime _DateCreate;
            /// <summary>
            /// Название 
            /// </summary>
            public readonly string NameVirus;
            /// <summary>
            /// Тип вируса
            /// </summary>
            public readonly TypeVirusEnum TypeVirus;
            /// <summary>
            /// Качество работы вируса
            /// </summary>
            public readonly int Rats;
            /// <summary>
            /// Дата создания вируса
            /// </summary>
            public DateTime DateCreate { get => _DateCreate; }

            public VirusStruct(string nameVirus, TypeVirusEnum typeVirus, int rats) {
                _DateCreate = App.GameGlobal.DataGM;
                NameVirus = nameVirus;
                TypeVirus = typeVirus;
                Rats = rats;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                var p = (VirusStruct)obj;
                return TypeVirus == p.TypeVirus & Rats== p.Rats;
            }

            public override int GetHashCode()
            {
                return TypeVirus.GetHashCode() ^ Rats.GetHashCode();
            }

            public enum TypeVirusEnum
            {
                StandartWin, StandartUnix,
                ZombieWin, ZombieUnix,
                Worms

            }
        }
    }
}
