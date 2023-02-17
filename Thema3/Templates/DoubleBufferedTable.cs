using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thema3
{
    public class DoubleBufferedTable : TableLayoutPanel
    {
        /// <summary>
        /// https://www.richard-banks.org/2007/09/how-to-create-flicker-free.html
        /// For faster table loading
        /// </summary>
        public DoubleBufferedTable()
        {
            DoubleBuffered = true;
            // Bitwise OR (|) is used to set all 3 ControlStyles to TRUE
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);

            Location = new System.Drawing.Point(13, 13);
        }
    }
}
