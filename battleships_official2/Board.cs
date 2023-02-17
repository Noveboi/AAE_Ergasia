using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace battleships_official2
{
    internal class Board
    {
        /// <summary>
        /// The X, Y coordinates of the whole board.
        /// </summary>
        public Point location;
        /// <summary>
        /// Contains the data about each block's status (water, ship, hit, miss).
        /// </summary>
        public Block.BlockStates[,] Array;

        private Game game;
        
        public int BoardSize = new Block().BlockSize * 10;

        Player owner;

        public Board(int x, int y, Game Form, Player Owner)
        {
            owner = Owner;


            location = new Point(x, y);
            game = Form;
            Array = new Block.BlockStates[10, 10];
            
            FillBoard();
            VisualiseBoard();

        }

        /// <summary>
        /// Fill the board with water blocks.
        /// </summary>
        public void FillBoard()
        {
            for (int i=0; i<10; i++)
            {
                for(int j=0; j<10; j++)
                {
                    Array[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Visually refresh the board in order to accurately display data.
        /// </summary>
        public void Update()
        {
            //player type
            char pType = owner.isAI ? 'e' : 'p';

            //Assign the (i,j)th block the corresponding BackColor, according to the 
            //array's value at (i,j)
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch(Array[i,j])
                    {
                        case Block.BlockStates.Ship:  
                            if (pType == 'e')
                            {
                                game.Controls[$"{pType}{i}{j}"].BackColor = Color.FromArgb(61, 168, 255);
                                break;
                            }
                            game.Controls[$"{pType}{i}{j}"].BackColor = Color.Gray;
                            break;
                        case Block.BlockStates.Hit:    
                            game.Controls[$"{pType}{i}{j}"].BackColor = Color.DarkRed;
                            break;
                        case Block.BlockStates.Miss:    
                            game.Controls[$"{pType}{i}{j}"].BackColor = Color.DarkGreen;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Create the controls that make up the board, those being
        /// Blocks, 
        /// Column Labels and
        /// Row Labels.
        /// </summary>
        public void VisualiseBoard()
        {
            for (int i=0; i < 10; i++)
            {
                int bSize = 0;
                for (int j=0; j<10; j++)
                {
                    Block block = new Block();
                    bSize = block.BlockSize;
                    block.Name = owner.isAI ? $"e{i}{j}" : $"p{i}{j}";
                    block.Location = new Point(location.X + j * block.BlockSize, location.Y + i * block.BlockSize);
                    block.BackColor = Color.FromArgb(61, 168, 255);
                    game.Controls.Add(block);

                    block.Click += game.Block_Click;

                    if (i == 0)
                    {
                        Label columnLabel = new Label();
                        columnLabel.Text = (j + 1).ToString();
                        columnLabel.TextAlign = ContentAlignment.MiddleCenter;
                        columnLabel.Size = block.Size;
                        columnLabel.Location = new Point(block.Location.X, block.Location.Y - block.BlockSize);
                        columnLabel.Font = game.gameFont;
                        game.Controls.Add(columnLabel);
                    }
                }
                Label rowLabel = new Label();
                rowLabel.Text = ((char)(65 + i)).ToString();
                rowLabel.TextAlign = ContentAlignment.MiddleCenter;
                rowLabel.Font = game.gameFont;
                rowLabel.Size = new Size(bSize,bSize);
                rowLabel.Location = new Point(location.X - bSize, location.Y+bSize*i);
                game.Controls.Add(rowLabel);
            }
        }
    }
}
