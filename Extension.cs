using System.Windows;

namespace PH4_WPF
{
    /// <summary>
    /// Методы расширения
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Открывает формы внутри основной формы
        /// </summary>
        /// <param name="win"></param>
        public static void ShowForm(this Window win)
        {
            string s = win.GetType().FullName;
            if (App.GameGlobal.ActiveApp.ContainsKey(s) == false)
            {
                App.GameGlobal.ActiveApp.Add(s, win);
                win.Owner = App.GameGlobal.MainWindow;
                win.Show();
            }
            else
            {
                App.GameGlobal.ActiveApp[s].Show();
                App.GameGlobal.ActiveApp[s].Activate();
                App.GameGlobal.ActiveApp[s].WindowState  = WindowState.Normal;
            }
        }
    }    
}
