using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;

namespace Thema3
{
    internal class BasicLabel : Label
    {
        public BasicLabel(float emSize)
        {
            Font = new Font("Segoe UI", emSize);
        }
    }
}
