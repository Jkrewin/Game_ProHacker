using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static PH4_WPF.GameScen.ScenStruct;
using System.Linq;
using static PH4_WPF.GameScen.ScenStruct.ICQ;

namespace PH4_WPF.FrmSoft
{

    public partial class FrmICQ : Window
    {
        private readonly Game.GameChatClass ICQ;

        private readonly System.Windows.Threading.DispatcherTimer XTimer = new System.Windows.Threading.DispatcherTimer();

        public FrmICQ()
        {
            InitializeComponent();
            this.Owner = App.GameGlobal.MainWindow;
            ICQ = App.GameGlobal.GameChat;

            if (System.IO.File.Exists(App.PatchAB + @"\chat_avators\" + ICQ.MyChat.Img))
                ImgAva.Source = new BitmapImage(new Uri(App.PatchAB + @"\chat_avators\" + ICQ.MyChat.Img));

            ImgAva.Visibility = Visibility.Hidden;
            L_NameN.Visibility = Visibility.Hidden;
            L_NameN.Content = ICQ.MyChat.Nicke;
            XTimer.Tick += new EventHandler(Q_Chat);
            XTimer.Interval = TimeSpan.FromSeconds(ICQ.MyChat.Messages[ICQ.IndexChat].Sec);
            XTimer.Start();
        }

        private void Q_Chat(object sender, EventArgs e)
        {
            L_AnswerText.Visibility = Visibility.Hidden;
            App.GameGlobal.SoundSignal("iqWav");
            ImgAva.Visibility = Visibility.Visible;
            L_NameN.Visibility = Visibility.Visible;
            App.GameGlobal.MainWindow.MessageIcon.Source = App.UriResImage("/Content/soft/IQNewMess.png");
            TB_BodyText.Text = ICQ.MyChat.Messages[ICQ.IndexChat].Text;
            CBoxAnswer.Items.Clear();
            ICQ.MyChat.Messages[ICQ.IndexChat].Answers.ForEach(x => CBoxAnswer.Items.Add(x.TextAnswer));
            ICQ.ICQ_Win.WindowState = WindowState.Normal;
            ICQ.ICQ_Win.Activate();
            XTimer.Stop();
        }

        private void ПеретаскиваниеФормы(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void ФормаЗакрыта(object sender, EventArgs e)
        {
            App.GameGlobal.ActiveApp.Remove(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        private void ВыбранОтвет(object sender, SelectionChangedEventArgs e)
        {
            int i = CBoxAnswer.SelectedIndex;
            if (i != -1)
            {
                var r = ICQ.MyChat.Messages[ICQ.IndexChat].Answers[i];
                switch (r.CommandAnswer)
                {
                    case Message.Answer.CommandAnswerEnum.Переход:
                        ICQ.IndexChat = r.IntArgument;
                        XTimer.Interval = TimeSpan.FromSeconds(ICQ.MyChat.Messages[ICQ.IndexChat].Sec);
                        L_AnswerText.Visibility = Visibility.Visible;
                        L_AnswerText.Content = "Ваш ответ отправлен: " + r.TextAnswer;
                        XTimer.Start();
                        break;
                    case Message.Answer.CommandAnswerEnum.ВыходЗапуститьСкрипт:
                        App.GameGlobal.GameChat = null;
                       List <Engine.GameEvenClass.IEventGame> script = App.GameGlobal.GameScen.ActiveScen.Script[r.StrArgument];
                        script.ForEach(x => x.Run());
                        App.GameGlobal.MainWindow.MessageIcon.Opacity = 50;                        
                        this.Close();
                        break;
                    case Message.Answer.CommandAnswerEnum.ВыходЗапуститьЧат:
                        App.GameGlobal.GameChat = new Game.GameChatClass(App.GameGlobal.GameScen.ActiveScen.Chat[r.StrArgument]);
                        App.GameGlobal.GameChat.InLoadChat();
                        this.Close();
                        break;
                    case Message.Answer.CommandAnswerEnum.ПростоВыход:
                        App.GameGlobal.GameChat = null;
                        App.GameGlobal.MainWindow.MessageIcon.Opacity = 50;
                        this.Close();
                        break;
                    default:
                        break;
                }

            }
        }

        private void Свернуть(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;


        private void ВыбранаФорма(object sender, RoutedEventArgs e)
        {
            //сообщение прочитано
            App.GameGlobal.MainWindow.MessageIcon.Source = App.UriResImage("/Content/soft/IQ.png");
        }
    }
}
