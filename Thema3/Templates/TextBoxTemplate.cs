using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thema3
{
    internal class TextBoxTemplate
    {
        public static void SetupTextBox(TextBox tb)
        {
            tb.Multiline = true;
        }

        public static void SetupTextBoxes(Control.ControlCollection tbs)
        {
            foreach (TextBox tb in tbs)
            {
                SetupTextBox(tb);
            }
        }

        public static void SetupTextBoxes(List<TextBox> tbs)
        {
            foreach (TextBox tb in tbs)
            {
                SetupTextBox(tb);
            }
        }
    }
}
