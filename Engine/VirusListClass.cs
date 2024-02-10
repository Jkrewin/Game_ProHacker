using System;
using System.Collections.Generic;
using System.Text;

namespace PH4_WPF.Engine
{
   public sealed class VirusListClass
    {
        /// <summary>
        /// Зараженные вирусом системы
        /// </summary>
        public List<InfectedSysClass> InfectedSys { get; set; } = new List<InfectedSysClass>();
        /// <summary>
        /// Антивирусная база 
        /// </summary>
        public List<AntivirusBase> VirusList = new List<AntivirusBase>();

        /// <summary>
        /// Зараженная система
        /// </summary>
        public class InfectedSysClass {
            /// <summary>
            /// Вирус
            /// </summary>
            public VirusStruct Virus;
            /// <summary>
            /// Тип системы
            /// </summary>
            public string SysType;

            public InfectedSysClass(VirusStruct virus) {
                Virus = virus;

            }
        }

        /// <summary>
        /// База данных антивируса
        /// </summary>
        public class AntivirusBase {
            /// <summary>
            /// Находиться в базе антивируса
            /// </summary>
            public VirusStruct Antivirus;
            /// <summary>
            /// Количество зараженных систем
            /// </summary>
            public int InfectStat = 0;
            /// <summary>
            /// Вред причененный  в сети
            /// </summary>
            public float Harm = 0f;
        }

        /// <summary>
        /// Вирус
        /// </summary>
        public struct VirusStruct {
            /// <summary>
            /// Название 
            /// </summary>
            public string NameVirus;
            /// <summary>
            /// Тип вируса
            /// </summary>
            public typeVirusEnum TypeVirus;
            /// <summary>
            /// Качество работы вируса
            /// </summary>
            public byte Rats;

            public enum typeVirusEnum
            {
                StandartWin
            }
        }
    }
}
