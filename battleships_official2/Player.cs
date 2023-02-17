using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace battleships_official2
{
    internal class Player
    {
        Board board;
        public Fleet fleet;
        public bool isAI;
        Game form;

        public string Name;

        /// <summary>
        /// Each player has one board and one fleet, along with the ability to attack the opponent.
        /// On initialization, set up the player instance's Board and Fleet
        /// </summary>
        /// <param name="AI">Specify if this player instance is user controlled or not</param>
        /// <param name="Form">In order to modify the Controls of the Game game</param>
        /// 
        public Player(bool AI, Game Form, string name)
        {
            form = Form;
            isAI = AI;
            Name = name;

            //Set up player's board
            board = !isAI 
                ? new Board(50, form.Height / 3, form, this) 
                : new Board(form.Width - new Block().BlockSize * 10 - 50, form.Height / 3, form, this);

            //Set up player's fleet
            fleet = new Fleet(board,this);

            board.Update();

        }
        //i,j already in correct game!!!!!
        public void PewPew(Player opponent, int i, int j)
        {
            PerformPewPew(opponent, i, j);
        }

        private void PerformPewPew(Player opponent, int i, int j)
        {
            //block to be attacked: opponent.board.Array[i, j];
            if (opponent.board.Array[i, j] == Block.BlockStates.Water)
                opponent.board.Array[i, j] = Block.BlockStates.Miss;
            else if (opponent.board.Array[i, j] == Block.BlockStates.Ship)
            {
                opponent.board.Array[i, j] = Block.BlockStates.Hit;
                opponent.fleet.HitShip(i, j);
            }
            opponent.board.Update();
        } 

        public void PewPew(Player opponent)
        {
            Random rnd = new Random();
            if(!isAI)
            {
                MessageBox.Show("ΦΥΓΕ!!!"); //1
                return;
            }
            int x = rnd.Next(10);
            int y = rnd.Next(10);
            while (board.Array[x,y]==Block.BlockStates.Hit || board.Array[x,y]==Block.BlockStates.Miss)
            {
                x = rnd.Next(10);
                y = rnd.Next(10);

            }
            PerformPewPew(opponent, x, y);
        }
    }
}
