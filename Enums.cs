namespace PH4_WPF
{
    /// <summary>
    /// Типы перечислений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Типы валют
        /// </summary>
        public enum TypeMoneyEnum
        {
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
            МощностьСервера
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
            // для уникальных серверов с авто сервисов
            WebForumApi,
            MailApi,
            WebChatApi,
            ShopApi,
            BookingApi,
            StreamVideoApi,
            SerWebApi
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
            exploit,
            shell,
            backdoor,
            file,
            goal_file,
            exe,
            dir,
            instructions
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
    }
}
