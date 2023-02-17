using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Management.Instrumentation;
using System.Deployment.Application;

namespace MainMenuPrototype
{
    public partial class Results : Form
    {
        private GameResults GameResults;
        private Label Title;
        private Label PlacementInfo;
        private Label GameInfo;
        private PictureBox VeryEvilRabbit;
        private int TitleBarHeight = 100;
        private int TitleBarSeparatorHeight = 5;
        private int ResultsAreaY;
        public Results(GameResults results)
        {
            GameResults = results;
            InitializeComponent();
            Setup();
            SetupDatabaseConnection();
            WriteResultsToDB();
            ReadFromDB();
        }

        private void Setup()
        {
            ResultsAreaY = TitleBarHeight + TitleBarSeparatorHeight;
            BackColor = Game.BG_Color;
            ForeColor = UI.FG_Color;

            // Title
            Title = new Label();
            Title.AutoSize = false;
            Title.BackColor = UI.BG_Color;
            Title.Text = "Game Results";
            Title.Font = UI.GetFont(48);
            Title.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(Title);

            // PlacementInfo
            PlacementInfo = new Label();
            PlacementInfo.AutoSize = false;
            PlacementInfo.Text =
                $"You placed n-th in your score rankings!{Environment.NewLine}" +
                $"You placed k-th in the global score rankings!";
            PlacementInfo.Font = App.GetFont(20);
            PlacementInfo.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(PlacementInfo);

            // GameInfo
            GameInfo = new Label();
            GameInfo.AutoSize = false;
            GameInfo.Font = UI.GetFont(24);
            GameInfo.Text =
                $"Score: {GameResults.Score}{Environment.NewLine}" +
                $"Time Played: {TimeSpan.FromSeconds(GameResults.Time)}";
            GameInfo.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(GameInfo);

            // Extremely evil rabbit peeking ominously
            VeryEvilRabbit = new PictureBox();
            VeryEvilRabbit.Image = Image.FromFile("../../Images/rabbit_peek.png");
            VeryEvilRabbit.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(VeryEvilRabbit);
            Controls.SetChildIndex(VeryEvilRabbit, 0);

            AlignControls();
        }

        private void WriteResultsToDB()
        {
            SQLiteCommand cmd = new SQLiteCommand(connection);

            // 0 -> Determine whether player has new best score
            int gameScore = GameResults.Score;
            int totalScore = GameResults.Player.TotalScore;
            bool hasNewBestScore = GameResults.Player.BestScore < GameResults.Score;

            // 1 -> Update the Players table
            cmd.CommandText = hasNewBestScore 
                ?
                "UPDATE Players " +
                $"SET BestScore = {gameScore}, TotalScore = {totalScore+gameScore} " +
                $"WHERE Username = '{GameResults.Player.Username}';" 
                :
                "UPDATE Players " +
                $"SET TotalScore = {totalScore+gameScore} " +
                $"WHERE Username = '{GameResults.Player.Username}';";
            cmd.ExecuteNonQuery();

            // 2 -> Insert new record into Games table
            cmd.CommandText =
                "INSERT INTO Games (Player_Username, Score, Time) " +
                $"VALUES ('{GameResults.Player.Username}',{gameScore},{GameResults.Time});";
            cmd.ExecuteNonQuery();
        }

        private void ReadFromDB()
        {
            //Do the following:
            //  From the Games table, select all scores (with usernames) and sort them, 
            //  finally, find the GLOBAL placement of the player
            SQLiteCommand cmd = new SQLiteCommand(connection);

            // List of (Username, Score) from Games table
            List<Tuple<string, int>> scores = new List<Tuple<string, int>>();

            cmd.CommandText =
                "SELECT Player_Username, Score " +
                "FROM Games " +
                "ORDER BY Score DESC;";

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // Get the data
                while (reader.Read())
                    scores.Add(new Tuple<string, int>(reader.GetString(0), reader.GetInt32(1)));
            }

            // 1 -> Get the GLOBAL placement
            // Item1 = Username | Item2 = Score
            int globalPlace = scores.FindIndex(score =>
            score.Item1 == GameResults.Player.Username && score.Item2 == GameResults.Score) + 1;

            // 2 -> Get the LOCAL placement
            var onlyPlayerScores = scores.Where(score => score.Item1 == GameResults.Player.Username).ToList();
            int localPlace = onlyPlayerScores.FindIndex(score => score.Item2 == GameResults.Score) + 1;

            // Change the text to display the data
            PlacementInfo.Text =
                $"You placed #{localPlace} in your score rankings!{Environment.NewLine}" +
                $"You placed #{globalPlace} in the global score rankings!";
        }

        private void AlignControls()
        {
            Title.Location = new Point(0, 0);
            Title.Size = new Size(Width, TitleBarHeight);
            PlacementInfo.Size = new Size(Width, 100);
            PlacementInfo.Location = new Point(0, ResultsAreaY);
            GameInfo.Size = new Size(Width, 100);
            GameInfo.Location = new Point(0, Height - GameInfo.Height - 50);
            VeryEvilRabbit.Size = new Size(100, 100);
            VeryEvilRabbit.Location = new Point(Width - VeryEvilRabbit.Width, Height - VeryEvilRabbit.Height - 20);
        }

        private void Results_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Brushes.Linen, TitleBarSeparatorHeight), 0, 100, Width, 100);
        }

        private void Results_Resize(object sender, EventArgs e)
        {
            AlignControls();
        }

        private void Results_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                new Menu().Show();
            }
        }
    }
}
