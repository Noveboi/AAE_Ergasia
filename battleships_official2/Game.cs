using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace battleships_official2
{
    public partial class Game : Form
    {
        const int GameTime = 90;
        public Font gameFont = new Font("Comic Sans", 14);
        WinTracker winTracker;
        Player user;
        Player enemy;
        int laps;
        int turns;
        bool userWon;
        bool gameOver = false;

        SQLiteConnection connection;

        internal Game(string username, WinTracker winTracker = null)
        {
            connection = new SQLiteConnection("Data Source=battleships.db;Version=3;");
            SetupDatabase();
            InitializeComponent();
            label1.Text = TimeSpan.FromSeconds(laps).ToString();
            user = new Player(false, this, username);
            enemy = new Player(true, this, "Enemyyy");
            if (winTracker == null)
                winTracker = new WinTracker();
            this.winTracker = winTracker;

            //Center textBox
            coordsTextBox.Size = new Size(100, 58);
            coordsTextBox.TextAlign = HorizontalAlignment.Center;
            coordsTextBox.Font = new Font("Garamond", 32);
            coordsTextBox.Location = new Point(Width / 2 - coordsTextBox.Width / 2, 50);
        }

        public void Block_Click(object sender, EventArgs e)
        {
            turns++;
            Block clickedBlock = sender as Block;
            int i = int.Parse(clickedBlock.Name[1].ToString());
            int j = int.Parse(clickedBlock.Name[2].ToString());
            user.PewPew(enemy, i, j);
            enemy.PewPew(user);
            if (user.fleet.Destroyed())
            {
                winTracker.Loses++;
                userWon = false;              
                gameOver = true;
            }
            if (enemy.fleet.Destroyed())
            {
                winTracker.Wins++;
                userWon = true;
                gameOver = true;

            }
        }
        private void coordsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter
                && (coordsTextBox.Text.Length == 2 || coordsTextBox.Text.Length == 3)
                && (coordsTextBox.Text[0] >= 65 && coordsTextBox.Text[0] <= 74)
                && char.IsDigit(coordsTextBox.Text[1])
                && int.Parse(coordsTextBox.Text.Substring(1)) <= 10)
            {
                int i = coordsTextBox.Text[0] - 65;
                int j = int.Parse(coordsTextBox.Text.Substring(1)) - 1;
                user.PewPew(enemy, i, j);
                coordsTextBox.Text = string.Empty;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            laps++;
            label1.Text = TimeSpan.FromSeconds(laps).ToString();
            if (laps == GameTime || gameOver)
            {
                timer1.Stop();
                GameResults gameResults = new GameResults()
                {
                    Player1 = user.Name,
                    Player2 = enemy.Name,
                    Winner = userWon ? user.Name : enemy.Name,
                    GameTime = laps,
                    Turns = turns
                };
                SendGameResultsToDatabase(gameResults);
                
                Results results = new Results(gameResults, user, winTracker);
                Close();
                results.Show();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void coordsTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void SetupDatabase()
        {
            connection.Open();
            string sql = "CREATE TABLE IF NOT EXISTS Games (" +
                "id INTEGER," +
                "Player1 VARCHAR(64) NOT NULL," +
                "Player2 VARCHAR(64) NOT NULL," +
                "Winner VARCHAR(64) NOT NULL," +
                "GameTime INTEGER NOT NULL," +
                "Turns INTERGER NOT NULL," +
                "PRIMARY KEY(id AUTOINCREMENT));";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }

        private void SendGameResultsToDatabase(GameResults gr)
        {
            string sql = "INSERT INTO Games (Player1, Player2, Winner, GameTime, Turns) " +
                "VALUES " +
                $"('{gr.Player1}', '{gr.Player2}', '{gr.Winner}', {gr.GameTime}, {gr.Turns});";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
