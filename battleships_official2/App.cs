using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace battleships_official2
{
    /// <summary>
    /// This form is launched first and is the parent of the rest of the forms that are opened.
    /// It also contains some user defined properties
    /// </summary>
    public partial class App : Form
    {
        private const int defaultFontSize = 12;
        public readonly static new Font Font = new Font("Impact", defaultFontSize);
        public static Font GetFont(float emSize)
        {
            return new Font(Font.Name, emSize);
        }

        public App()
        {
            InitializeComponent();
            new Menu().Show();
        }
    }
}
