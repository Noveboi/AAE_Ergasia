using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace battleships_official2
{
    internal class Fleet
    {
        public Ship Carrier;
        public Ship Battleship;
        public Ship Cruiser;
        public Ship Submarine;
        public List<Ship> Ships;
        private Board boardRef;

        private Player captain;
        public Fleet(Board board, Player Captain)
        {
            Carrier = new Ship(5, "Carrier");
            Battleship = new Ship(4, "Battleship");
            Cruiser = new Ship(3, "Cruiser");
            Submarine = new Ship(2, "Submarine");

            Ships = new List<Ship>
            {
                Carrier, Battleship, Cruiser, Submarine
            };

            boardRef = board;
            captain = Captain;
            SpawnShips();
        }

        public bool Destroyed()
        {
            return Carrier.IsDestroyed() &&
                   Battleship.IsDestroyed() &&
                   Cruiser.IsDestroyed() &&
                   Submarine.IsDestroyed();
        }

        public void HitShip(int x, int y)
        {
            foreach(Ship ship in Ships)
            {
                for (int i = 0; i < ship.States.Count; i++)
                {
                    int[] currentShipState = ship.States[i];
                    if (currentShipState[0] == x && currentShipState[1] == y)
                    {
                        ship.States[i] = new int[] { x, y, 1 };
                        if (ship.IsDestroyed())
                            MessageBox.Show($"{captain.Name}'s {ship.Name} has been destroyed!!");
                    }
                }
            }
        }

        /// <summary>
        /// Place the 4 ships upon the board with random locations.
        /// </summary>
        private void SpawnShips()
        {
/***********************************************************************************************************
 * For each ship do the following:                                                                         *
 *      1 -> Get random X,Y coordinates and a random orientation (horizontal or vertical)                  *
 *                                                                                                         *
 *      2 -> Based on the orientation, offset the X or Y coordinate and check the following conditions:    *
 *           - The offset coordinate is less than 10 (avoid array access violation)                        *
 *           - The board's array at the offset coordinate is 0 (water block)                               *
 *                                                                                                         *
 *      3 -> If the conditions from step 2 are met, append to the state list the current offset            *
 *           coordinates and continue looping.                                                             *
 *           If the conditions from step 2 are met, and it's the final block being checked,                *
 *           then proceed to step 4, else go back to step 1.                                               *
 *                                                                                                         *
 *      4 -> If canPlace = true, do the following:                                                         *
 *           - Put the ship on the board's array and                                                       *
 *           - Set the initial State list of the ship.                                                     *
 *      DONE!                                                                                              *
 ***********************************************************************************************************/
            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int shipLength = 2; shipLength <= 5; shipLength++)
            {
                bool shipPlaced = false;
                while (!shipPlaced)
                {
                    bool canPlace = false;
                    int x = rnd.Next(0, 10);
                    int y = rnd.Next(0, 10);
                    List<int[]> states = new List<int[]>();
                    // 0 - Horizontal 
                    // 1 - Vertical
                    int orientation = rnd.Next(0, 2);
                    for (int i = 0; i < shipLength; i++)
                    {
                        bool validHorizontal = orientation == 0 && 
                            x + i < 10 && 
                            boardRef.Array[x + i, y] == 0;
                        bool validVertical = orientation == 1 && 
                            y + i < 10 && 
                            boardRef.Array[x, y + i] == 0;

                        //Check if block placement is valid AND its not the final block being placed
                        if (validHorizontal && i != shipLength - 1)
                        {
                            states.Add(new int[] { x + i, y, 0 });
                            continue;
                        }
                        else if (validVertical && i != shipLength - 1)
                        {
                            states.Add(new int[] { x, y + i, 0 });
                            continue;
                        }
                        //Check if block placement is valid AND its the final block being placed
                        //if its valid, set canPlace = true and break from loop!!!!!!!!!!!!!!!!
                        if (validHorizontal && i == shipLength - 1)
                        {
                            states.Add(new int[] { x + i, y, 0 });
                            canPlace = true;
                        }
                        else if (validVertical && i == shipLength - 1)
                        {
                            states.Add(new int[] { x, y + i, 0 });
                            canPlace = true;
                        }
                        break;
                    }

                    if (canPlace)
                    {
                        foreach(var state in states)
                        {
                            //Put ship on board
                            int coordX = state[0];
                            int coordY = state[1];

                            boardRef.Array[coordX, coordY] = Block.BlockStates.Ship;

                            //Set ship states
                            switch (shipLength)
                            {
                                case 2:     // Is Submarine
                                    Submarine.States.Add(state);
                                    break;
                                case 3:     // Is Cruiser
                                    Cruiser.States.Add(state);
                                    break;
                                case 4:     // Is Battleship
                                    Battleship.States.Add(state);
                                    break;
                                case 5:     // Is Carrier
                                    Carrier.States.Add(state);
                                    break;
                            }
                        }
                        shipPlaced = true;
                    }
                }
            }
        }
    }
}
