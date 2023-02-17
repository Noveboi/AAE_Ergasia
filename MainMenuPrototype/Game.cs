using System;
using System.Drawing;
using System.Windows.Forms;

namespace MainMenuPrototype
{
    public partial class Game : Form
    {
        public enum Difficulties
        {
            Easy,
            Medium,
            Hard
        }
        #region Variables
        public const int GameTime = 10;

        public static Player Player;

        /// <summary>
        /// The game's timer
        /// </summary>
        private Timer timer = new Timer() { Interval = 1000 };
        /// <summary>
        /// Special timer which is activated when a Blackout or Game Over event happens
        /// </summary>
        private Timer waitTimer = new Timer() { Interval = 1000 };
        public PictureBox fakeRabbitPBox = new PictureBox()
        {
            Size = Rabbit.Size,
            Location = new Point(400,400),
            Visible = false
        };
        public PictureBox realRabbitPBox = new PictureBox()
        {
            Size = Rabbit.Size,
            Location = new Point(400,400)
        };

        private int waitSeconds = 3;
        private int Cooldown = 7;
        private Label Darkness;

        public static Color BG_Color = Color.FromArgb(1, 145, 40);
        public static Size WindowSize;

        private Rabbit EvilRabbit;
        private Rabbit EvilFakeRabbit;
        public UI ui;

        public Difficulties diff;

        public int elapsed = 0;
        #endregion
        public Game(Player p, Difficulties d)
        {
            InitializeComponent();

            Player = p;
            diff = d;

            SetupControls();

            FormClosing += App.Child_Closing;

        }

        /// <summary>
        /// Creates the rabbit entity (control)
        /// </summary>
        private Rabbit SpawnRabbit(bool isFake)
        {
            Rabbit rabbit;
            rabbit = new Rabbit(this, isFake);

            rabbit.HitBox = isFake ? fakeRabbitPBox : realRabbitPBox;
            rabbit.HitBox.Visible = true;
            rabbit.HitBox.BackColor = BG_Color;
            rabbit.HitBox.SizeMode = PictureBoxSizeMode.StretchImage;
            rabbit.HitBox.Image = Image.FromFile("../../Images/rabbit.png");

            rabbit.GiveTheGiftOfLife();
            return rabbit;
        }

        private void SetupControls()
        {
            WindowSize = ClientSize;

            Controls.Add(realRabbitPBox);
            Controls.Add(fakeRabbitPBox);

            EvilRabbit = SpawnRabbit(false);

            ui = new UI();
            Controls.AddRange(ui.InitializeUI().ToArray());

            timer.Tick += Timer_Tick;
            waitTimer.Tick += WaitTimer_Tick;
            timer.Enabled = true;

            Darkness = new Label();
            Darkness.Text = $"BLACKOUT!{Environment.NewLine}{waitSeconds}";
            Darkness.ForeColor = Color.White;
            Darkness.Font = new Font("Garamond", 72, FontStyle.Bold);
            Darkness.TextAlign = ContentAlignment.MiddleCenter;
            Darkness.Size = Size;
            Darkness.BackColor = Color.Black;
            Darkness.Location = new Point(0, 0);
            Darkness.AutoSize = false;
        }

        /// <summary>
        /// Cover the entire window with the 'Darkness' label 
        /// </summary>
        /// <param name="text"></param>
        private void CoverWithDarkness(string text)
        {
            Darkness.Text = text;
            Controls.Add(Darkness);
            Controls.SetChildIndex(Darkness, 0);
        }

        /// <summary>
        /// Freeze the game and rabbit timers and wait a specified amount of time before continuing
        /// </summary>
        private void Blackout()
        {
            timer.Stop();
            EvilRabbit.HopTimer.Stop();
            waitTimer.Start();
            CoverWithDarkness($"BLACKOUT!{Environment.NewLine}{waitSeconds}");

            EvilFakeRabbit = SpawnRabbit(true);
        }

        private void WaitTimer_Tick(object sender, EventArgs e)
        {
            waitSeconds--;

            if (waitSeconds == 0 && elapsed >= GameTime)
            {
                waitTimer.Stop();
                GameResults results =
                    new GameResults(Player, EvilRabbit.HitCounter, elapsed, diff);
                new Results(results).Show();
                Close();
                return;
            }

            if (elapsed < GameTime) 
                Darkness.Text = $"BLACKOUT!{Environment.NewLine}{waitSeconds}";

            if (waitSeconds == 0)
            {
                Controls.Remove(Darkness);
                timer.Start();
                EvilRabbit.HopTimer.Start();
                waitTimer.Stop();
                waitSeconds = 3;
                Cooldown = 7;
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            if (elapsed >= GameTime)
            {
                timer.Stop();
                CoverWithDarkness($"GAME{Environment.NewLine}OVER");
                Darkness.BackColor = UI.BG_Color;
                waitSeconds = 2;
                waitTimer.Start();
                return;
            }

            elapsed++;
            ui.TimeDisplay.Text = TimeSpan.FromSeconds(elapsed).ToString();
            Cooldown--;

            if (rnd.Next(0, 10) == 0 && Cooldown <= 0 && diff == Difficulties.Hard)
            {
                Blackout();
            }
        }
    }
}
