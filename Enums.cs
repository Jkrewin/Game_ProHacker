namespace PH4_WPF
{
    /// <summary>
    /// Типы перечислений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Способности класса дефейсера
        /// </summary>
        public enum SkillDefecer : byte
        {
            ПерезапускСервера = 1,
            СообщитьДефейсе = 2,            
            СоздаватьМаршруты = 3,
            УскоренныйПоиск = 4, 
            ЧисткаЛогов = 5,
            AA = 6,//0
            AAA = 7,//0
            AAAA = 8,//0
            ДоступКоВсемУязвимостям = 9,//0
            МаксимальноеДостижение = 10
        }
        /// <summary>
        /// Способности кракера
        /// </summary>
        public enum SkillCrack : byte
        {
            СобиратьХешиПаролей = 1,
            СкидкаНаСофт20 = 2,
            ТорговатьХешами = 3,
            ИИИ = 4,//0
            ДоступКрипте = 5,
            AA = 6,//0
            AAA = 7,//0
            AAAA = 8,//0
            AAAAA = 9,//0
            МаксимальноеДостижение = 10
        }
        /// <summary>
        /// Способности вирусмейкера
        /// </summary>
        public enum SkillVir : byte
        {
            ВирусыWin = 1,
            ЗомбиВирусWin = 2,
            ЧервьWin = 3,
            ВирусыNix = 4,
            ВирусВымогательWin = 5,
            ВирусВымогательUnix = 6,
            AAA = 7,
            AAAA = 8,
            AAAAA = 9,
            МаксимальноеДостижение = 10
        }
        /// <summary>
        /// Способности кодера
        /// </summary>
        public enum SkillCoder : byte
        {
            ПисатьКод = 1,
            ПоискБанковскойИнформации = 2,
            МаскировкаКода = 3,//0
            ИИИ = 4,//0
            ммм = 5,//0
            AA = 6,//0
            AAA = 7,//0
            AAAA = 8,//0
            AAAAA = 9,//0
            МаксимальноеДостижение = 10
        }
        /// <summary>
        /// Типы валют
        /// </summary>
        public enum TypeMoneyEnum : int
        {
            none = 0,
            Dollar = 2,
            Karbovantsy = 1,
            Ether = 4,
            BitCoin = 3
        }
        /// <summary>
        /// Событие при котором случаеться действие
        /// </summary>
        public enum ConditionEnum
        {
            ФайлСкачан,
            ВходНаСервер,
            ЗапущенаСлужбаНаСервере,
            СерверОтключен,
            АдминПанельДоступ,
            ИзменениеПравДоступа,
            ПосещениеСервера,
            ПроизошлоСобытие,
            МощностьСервераУвеличина,
            СофтОбновлен,
            ПодборПароляЗавершен
        }
        /// <summary>
        /// Доступные инстансы
        /// </summary>
        public enum InstaceTypeEnum
        {
            FTP,
            MySql,
            WebForum,
            BanerAD,
            Mail,
            Coordinator,
            WebChat,
            Shop,
            Booking,
            StreamVideo,
            TranceSrv,
            RigFrame,
            // для уникальных серверов с авто сервисов
            WebForumApi,
            MailApi,
            WebChatApi,
            ShopApi,
            BookingApi,
            StreamVideoApi,
            SerWebApi,
            SecServerSorce
        }
        /// <summary>
        /// Статус работы инстанса
        /// </summary>
        public enum StatusInstanceEnum
        {
            Working,
            Stopping,
            ErrorCritical
        }
        /// <summary>
        /// Тип запуска файла
        /// </summary>
        public enum TypeParam
        {
            text,
            exploit,
            shell,
            backdoor,
            file,
            goal_file,
            exe,
            dir,
            instructions,
            script_deface,
            sbank,
            virus_win,
            sorce_file
        }
        /// <summary>
        /// Типы логов для обозначения типа 
        /// </summary>
        public enum LogTypeEnum
        {
            Money,
            Error,
            Server,
            Problem,
            Exp
        }
        /// <summary>
        /// Типы игровых новостей
        /// </summary>
        public enum TopicEnum
        {
            Общие_Новости = 0,
            Найдены_Баги = 1,
            Новости_Шлак = 2,
            Разное = 3,
            Важное = 4,
            НовостиКасательноИгрока = 5
        }
        /// <summary>
        /// Sounds in the game, the name is the name enum of the file in the Content/sound folder
        /// </summary>
        public enum Sounds
        {
            /// <summary>
            /// new message in chat
            /// </summary>
            idWav,
            /// <summary>
            /// new mail
            /// </summary>
            newMail,
            /// <summary>
            /// gameover
            /// </summary>
            gameover,
            /// <summary>
            /// purchasing software
            /// </summary>
            buy,
            /// <summary>
            /// open attachment in mail
            /// </summary>
            attachMail,
            /// <summary>
            /// beep
            /// </summary>
            beep,
            /// <summary>
            /// game save ok
            /// </summary>
            saveok
        }
    }
}
