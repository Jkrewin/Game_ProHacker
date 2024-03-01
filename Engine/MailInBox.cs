using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Почтовое сообщение
    /// </summary>
    [Serializable]
    public sealed class MailInBox
    {
        /// <summary>
        /// Заголовок письма
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Текст письма
        /// </summary>
        public string BodyText { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string Mailto { get; set; }
        /// <summary>
        /// Комманды действия совершаемые этим письмом
        /// </summary>
        public Engine.GameEvenClass.IEventGame  CommandList { get; set; }
        /// <summary>
        /// Прочитано или нет 
        /// </summary>
        public bool ReadMail { get; set; }
        /// <summary>
        /// Дата когда пришло
        /// </summary>
        public DateTime DateTo { get; set; }

        #region "Тут статика"
        /// <summary>
        /// Создает почтовое сообщение с учетом оповещения
        /// </summary>
        public static void NewMail(string mailto, string title, string bodytext, Engine.GameEvenClass.IEventGame command)
        {
            ref var ls = ref App.GameGlobal.Servers[0].Mails;
            if (ls == null) ls = new List<MailInBox>();
            ls.Add(new MailInBox()
            {
                BodyText = bodytext,
                CommandList = command,
                DateTo = App.GameGlobal.DataGM,
                Mailto = mailto,
                ReadMail = false,
                Title = title
            });
            RfMail();
        }
        /// <summary>
        /// Создает почтовое сообщение с учетом оповещения
        /// </summary>
        public static void NewMail(MailInBox mail)
        {
            ref var ls = ref App.GameGlobal.Servers[0].Mails;
            if (ls == null) ls = new List<MailInBox>();
            if (mail.DateTo == null) mail.DateTo = App.GameGlobal.DataGM;
            ls.Add(mail);
            RfMail();
        }
        /// <summary>
        /// Проверяет если новая почта или нет и показывает иконку на раб. столе
        /// </summary>
        public static void MailNotification() {
            if (App.GameGlobal.MyServer.Mails.FindAll(x => x.ReadMail == false).Count == 0)
            {
                App.GameGlobal.MainWindow.MailIndicator.Source =
            new BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\Mail.png"));
            }
            else
            {
                App.GameGlobal.MainWindow.MailIndicator.Source =
                new BitmapImage(new Uri(App.PatchAB + @"\Desktop\bPanel\sel mail.png"));
            }
        }
        /// <summary>
        /// Нужен для обновления нотификации
        /// </summary>
        private static void RfMail() { 
         // обновляет список почты
            if (App.GameGlobal.ActiveApp.ContainsKey(typeof(FrmSoft.Mail).FullName))
            {
                var frm = App.GameGlobal.ActiveApp[typeof(FrmSoft.Mail).FullName] as FrmSoft.Mail;
                frm.Refreh_Mail();
            }
            // нотификация прихода почты
            MailNotification();
            App.GameGlobal.SoundSignal("newMail");
        }
        #endregion

    }
}
