using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MainMenuPrototype
{
    /// <summary>
    /// This form is launched first and is the parent of the rest of the forms that are opened.
    /// It also contains some user defined properties
    /// </summary>
    public partial class App : Form
    {
        private const int defaultFontSize = 12;
        public readonly static new Font Font = new Font("Segoe UI", defaultFontSize);
        public static Font GetFont(float emSize)
        {
            return new Font(Font.Name, emSize);
        }

        public App()
        {
            InitializeComponent();
            new Menu().Show();
        }

        /// <summary>
        /// If the user closes a form ('X' Button, ALT+F4, etc.) and the form is subscribed to this event,
        /// issue an Application.Exit() call and close the entire process
        /// </summary>
        public static void Child_Closing(object sender, FormClosingEventArgs e)
        {
            //Check the Call Stack of the program, if at any point the function Close() has 
            //  been called, then do NOT exit
            //This check is done because the Close() function is classified under UserClosing
            if (new StackTrace().GetFrames().Any(frame => frame.GetMethod().Name == "Close"))
                return;
            if (e.CloseReason == CloseReason.UserClosing)
                Application.Exit();
        }
    }
}
