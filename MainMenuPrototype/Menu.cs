using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenuPrototype
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            // The functions below are found in Menu.Designer.cs
            SetupApp();
            SetupMenu();
            SetupDatabaseConnection();

            FormClosing += App.Child_Closing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // On load, if the table "Players" does not exist, create it 
            string sql1 = "CREATE TABLE IF NOT EXISTS Players (" +
                "id INTEGER," +
                "Username VARCHAR(255) NOT NULL," +
                "BestScore INTEGER NOT NULL," +
                "TotalScore INTEGER NOT NULL," +
                "PRIMARY KEY (id AUTOINCREMENT));";
            // Also create table "Games"
            string sql2 = "CREATE TABLE IF NOT EXISTS Games (" +
                "id INTEGER," +
                "Player_Username VARCHAR(255) NOT NULL," +
                "Score INTEGER NOT NULL," +
                "Time INTEGER NOT NULL," +
                "PRIMARY KEY (id AUTOINCREMENT))";
            SQLiteCommand cmd = new SQLiteCommand(connection);
            cmd.CommandText = sql1;
            cmd.ExecuteNonQuery();
            cmd.CommandText = sql2;
            cmd.ExecuteNonQuery();
        }

        private void Login()
        {
            if (usernameBox.Text == string.Empty)
            {
                MessageBox.Show("Please enter a non-empty username");
                return;
            }
            else if (diffSelection.SelectedItem == null)
            {
                MessageBox.Show("Please select a difficulty");
                return;
            }

            PlayerManager manager = new PlayerManager(connection);
            Player player = manager.FindOrCreatePlayer(usernameBox.Text);
            Game.Difficulties diff = (Game.Difficulties)diffSelection.SelectedIndex;

            new Game(player, diff).Show();
            connection.Close();
            Close();
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
    
        private void button1_Click(object sender, EventArgs e)
        {
            new Leaderboard(connection).Show();
        }
    }
}
