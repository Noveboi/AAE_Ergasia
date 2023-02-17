using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenuPrototype
{
    /// <summary>
    /// Compactly displays all columns from the "Players" Table in the app's database. 
    /// Features ordering rows (ascending or descending) based on the column the user picks.
    /// </summary>
    public partial class Leaderboard : Form
    {
        private Label PlayerNames;
        private Label BestScores;
        private Label TotalScores;
        private SQLiteConnection connection;
        private List<Tuple<List<Control>, int>> Rows = new List<Tuple<List<Control>, int>>();
        private bool isAsc = false;

        int rowLabelHeight = 40;

        public Leaderboard(SQLiteConnection conn)
        {
            connection = conn;

            InitializeComponent();
            SetupControls();
            AlignControls();
            FillLeaderboard("BestScore", isAsc);
        }

        /// <summary>
        /// Perfoms an SQL Query on the Players table and orders (sorts) it by the Ascending and 
        /// orderElement parameters.
        /// The controls displaying that query information are then created and positioned
        /// </summary>
        /// <param name="orderElement">What column of the Players table to sort by?</param>
        /// <param name="Ascending">Sort by ascending or descending?</param>
        private void FillLeaderboard(string orderElement, bool Ascending)
        {
            // 1 -> Gather data from Players
            SQLiteCommand cmd = new SQLiteCommand(connection);
            cmd.CommandText = Ascending 
                ?
                "SELECT * " +
                "FROM Players " +
                $"ORDER BY {orderElement} ASC;"
                :
                "SELECT * " +
                "FROM Players " +
                $"ORDER BY {orderElement} DESC;";
            // 2 -> For each row read, create the controls for that row
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                int i = 0;
                while(reader.Read())
                {
                    string username = reader.GetString(1);
                    int bestscore = reader.GetInt32(2);
                    int totalscore = reader.GetInt32(3);
                    CreateDataControlsForRow(username,bestscore,totalscore,i);
                    i++;
                }
            }

            // 3 -> Position and resize the controls properly
            AlignRows();

            // 4 -> Reverse the ORDER for the next FillLeaderboard() call
            isAsc = !isAsc;
        }

        private void SetupControls()
        {
            BackColor = UI.BG_Color;
            ForeColor = UI.FG_Color;

            PlayerNames = new Label()
            {
                Text = "Player",
                Cursor = Cursors.Hand,
                Name = "Username",
                AutoSize = false,
                Font = UI.GetFont(24),
                TextAlign = ContentAlignment.MiddleCenter
            };
            BestScores = new Label()
            {
                Text = "Best Score",
                Cursor = Cursors.Hand,
                Name = "BestScore",
                AutoSize = false,
                Font = UI.GetFont(24),
                TextAlign = ContentAlignment.MiddleCenter
            };
            TotalScores = new Label()
            {
                Text = "Total Score",
                Cursor = Cursors.Hand,
                Name = "TotalScore",
                AutoSize = false,
                Font = UI.GetFont(24),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(PlayerNames);
            Controls.Add(BestScores);
            Controls.Add(TotalScores);

            PlayerNames.Click += MainLabel_Click;
            BestScores.Click += MainLabel_Click;
            TotalScores.Click += MainLabel_Click;
        }

        /// <summary>
        /// Align the first row of the leaderboard containing the names of the columns
        /// </summary>
        private void AlignControls()
        {
            PlayerNames.Size = new Size(Width / 3, 100);
            BestScores.Size = new Size(Width / 3, 100);
            TotalScores.Size = new Size(Width / 3, 100);

            PlayerNames.Location = new Point(0, 0);
            BestScores.Location = new Point(PlayerNames.Width, 0);
            TotalScores.Location = new Point(PlayerNames.Width + BestScores.Width, 0);

            // Force the Paint event to fire
            Invalidate();
        }

        /// <summary>
        /// Positions and resizes the controls in each row to fit the window's current size
        /// </summary>
        private void AlignRows()
        {
            foreach(var row in Rows)
            {
                List<Control> labels = row.Item1;
                int i = row.Item2;

                labels[0].Location = new Point(0, 105 + rowLabelHeight * i);
                labels[1].Location = new Point(Width / 3 + 5, 105 + rowLabelHeight * i);
                labels[2].Location = new Point(2 * Width / 3 + 5, 105 + rowLabelHeight * i);

                labels[0].Size = new Size(Width / 3 - 5, rowLabelHeight);
                labels[1].Size = new Size(Width / 3 - 10, rowLabelHeight);
                labels[2].Size = new Size(Width / 3 - 5, rowLabelHeight);
            }
        }

        /// <summary>
        /// Creates the necessary controls for a new row in the leaderboard 
        /// (without positioning or resizing)
        /// </summary>
        /// <param name="i">The row's zero-based index</param>
        /// <returns>The list containg the controls for the row, and the row's index</returns>
        private Tuple<List<Control>,int> CreateDataControlsForRow(string username, int bestscore, int totalscore, int i)
        {
            Color color = UI.FG_Color;
            if (i % 2 == 1)
                color = Color.Violet;
            Label un = new Label()
            {
                Text = username,
                ForeColor = color,
                Font = App.GetFont(16),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Label bs = new Label()
            {
                Text = bestscore.ToString(),
                ForeColor = color,
                Font = App.GetFont(16),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Label ts = new Label()
            {
                Text = totalscore.ToString(),
                ForeColor = color,
                Font = App.GetFont(16),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(un);
            Controls.Add(bs);
            Controls.Add(ts);

            var row = new Tuple<List<Control>, int>(new List<Control>() { un, bs, ts }, i);
            Rows.Add(row);
            return row;
        }

        /// <summary>
        /// Called when one of the column labels are clicked, the leaderboard rows are then 
        /// recreated to display the newly ordered rows correctly
        /// </summary>
        private void MainLabel_Click(object sender, EventArgs e)
        {
            Label s = sender as Label;
            // Remove all row labels
            foreach (var row in Rows)
            {
                foreach (var lbl in row.Item1)
                {
                    Controls.Remove(lbl);
                }
            }

            // Add new row labels
            FillLeaderboard(s.Name, isAsc);
        }

        private void Leaderboard_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Brushes.Linen, 5);
            e.Graphics.DrawLine(p, 0, 100, Width, 100);
            e.Graphics.DrawLine(p, Width / 3, 0, Width / 3, Height);
            e.Graphics.DrawLine(p, 2*Width / 3, 0, 2*Width / 3, Height);
        }

        /// <summary>
        /// Dynamically resizes all necessary controls of the window and thus makes it resizable
        /// </summary>
        private void Leaderboard_Resize(object sender, EventArgs e)
        {
            if (!Created) return;
            AlignControls();
            AlignRows();
        }
    }
}
