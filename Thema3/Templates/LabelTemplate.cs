using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace Thema3
{
    internal class LabelTemplate
    {
        public static void SetupLabel(Label label)
        {
            label.TextAlign = ContentAlignment.MiddleRight;
            label.AutoSize = false;
        }

        public static void SetupLabels(Control.ControlCollection labels)
        {
            foreach(Label label in labels)
            {
                SetupLabel(label);
            }
        }
    }
}
