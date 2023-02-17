using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace battleships_official2
{
    internal class Block : Label
    {
        public enum BlockStates
        {
            Water,
            Ship,
            Hit,
            Miss
        }

        public int BlockSize = 30;
        public Block() 
        {
            this.AutoSize = false;
            this.Text = string.Empty;
            this.Size = new Size(BlockSize, BlockSize);
            this.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
