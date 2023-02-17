using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thema3
{
    internal class FlowPanelTemplate
    {
        public static void SetupFlowPanel(FlowLayoutPanel flp, int emSize, bool horizontal)
        {
            int elementAmount = flp.Controls.Count;

            foreach (Control control in flp.Controls)
            {
                control.Size = horizontal 
                    ? 
                    new Size(flp.Width / elementAmount - 5, flp.Height - 5)
                    : 
                    new Size(flp.Width - 5, flp.Height / elementAmount - 5);
                control.Font = new Font("Segoe UI", emSize);
                control.Margin = new Padding(2);
            }
        }
    }
}
