using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace battleships_official2
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            SetupApp();
            SetupMenu();
            SetupDatabaseConnection();
            
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void usernameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            if (usernameBox.Text == string.Empty)
            {
                MessageBox.Show("Please enter a non-empty username");
                return;
            }

            new Game(usernameBox.Text).Show();
            Close();
        }
    }
}
