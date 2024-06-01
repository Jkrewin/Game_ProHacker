using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace PH4_WPF.etc
{
    /// <summary>
    /// Translator of text into other languages
    /// </summary>
    public class Translator 
    {        
        private Dictionary<string, string> DicTxt  = new Dictionary<string, string>();
        private string Sfile = App.PatchAB + @"\Ru-rus.txt";

        public bool Loaded = false;

        public Translator() {
            if (System.IO.File.Exists(Sfile))
            {
                try
                {
                   string [] texts= System.IO.File.ReadAllLinesAsync(Sfile, System.Text.Encoding.Default).Result;
                    foreach (var item in texts)
                    {
                        string[] vs = item.Split('Ъ');
                        DicTxt.Add(vs[0], vs[1]);
                    }
                    Loaded = true;
                }
                catch 
                {
                    Loaded = false;
                }               
            }
        }

        public string this[string text] { get => Lng(text); }

        /// <summary>
        /// The method translates the text that is in the .txt; if not, it suggests adding a translation of the text
        /// </summary>
        /// <param name="words">Selected words</param>
        /// <returns>Translation text if there is no translation then the text will be returned</returns>
        public string Lng(string words) {
            if ( Loaded == false) return words;
            if (DicTxt.TryGetValue(words, out string txt))
            {
                return txt;
            }
            else {
                string value = words;
                if (InputBox("New text", words, ref value) == DialogResult.OK)
                {
                    DicTxt.Add(words , value);
                    #if DEBUG
                        System.IO.File.WriteAllLinesAsync(@"..\..\..\Content\Ru-rus.txt", DicTxt.Select(x => x.Key + "Ъ" + x.Value).ToArray());
                    #endif                    
                }
                return value;
            }
        }

        private static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}
