using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MainMenuPrototype
{
    /// <summary>
    /// Simple data model that mirrors the 'Games' table in the database
    /// </summary>
    public class GameResults
    {
        public Player Player { get; set; }
        public int Score { get; set; }
        public int Time { get; set; }

        private Game.Difficulties DiffPlayed;

        public GameResults(Player p, int s, int t, Game.Difficulties d)
        {
            Player = p;
            Time = t;
            DiffPlayed = d;
            Score = CalculateScore(s);
        }

        private int CalculateScore(int initialScore)
        {
            // If diff = Easy | Score * 0.5
            // If diff = Normal | Score * 1
            // If diff = Hard | Score * 1.5
            return (int)(initialScore * (((int)DiffPlayed + 1) * 0.5)); 
        }
    }
}
