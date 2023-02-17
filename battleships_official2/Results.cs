using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace battleships_official2
{
    public partial class Results : Form
    {
        Player player;
        int laps;
        int turns;
        WinTracker winTracker;
        bool w;

        internal Results(GameResults gameResults, Player p, WinTracker winTracker)
        {
            InitializeComponent();
            this.winTracker = winTracker;
            laps = gameResults.GameTime;
            turns = gameResults.Turns;
            player = p;
            w = gameResults.Winner == p.Name;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = TimeSpan.FromSeconds(laps).ToString();
            label3.Text = turns.ToString();
           
            if (w)
            {
                label1.Text = "YOU WON :)";
            }
            else
            {
                label1.Text = "YOU GOT SUNK :(";
            }

            MessageBox.Show($"Wins: {winTracker.Wins}, Loses: {winTracker.Loses}");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game game = new Game(player.Name, winTracker);
            game.Show();
            Close();
        }
    }
}
