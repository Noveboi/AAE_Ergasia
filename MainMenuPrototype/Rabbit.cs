using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.Windows.Forms;

namespace MainMenuPrototype
{
    internal class Rabbit
    {
        public static Size Size = new Size(90, 90);

        public PictureBox HitBox;
        public Timer HopTimer = new Timer();
        private Timer HitAnimTimer = new Timer() { Interval = 100 };
        private Game gameRef;

        private int moveMs = 600;
        public bool Fake = false;
        public int HitCounter = 0;

        public Rabbit(Game game, bool fake)
        {
            gameRef = game;
            Fake = fake;
        }

        public void Hop()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            if(gameRef.diff == Game.Difficulties.Medium || gameRef.diff == Game.Difficulties.Hard)
            {
                HopTimer.Interval = rnd.Next(moveMs / 3, moveMs);
            }
            int x = rnd.Next(20, gameRef.Width - HitBox.Width - 30);
            int y = rnd.Next(UI.Height + 20, gameRef.Height - HitBox.Height - 30);
            HitBox.Location = new Point(x, y);
        }

        /// <summary>
        /// Starts the rabbit's timer and subscribe to the necessary events
        /// </summary>
        public void GiveTheGiftOfLife()
        {
            HitBox.Click += HitBox_Click;
            HopTimer.Interval = moveMs;
            HopTimer.Tick += HopTimer_Tick;
            HitAnimTimer.Tick += HitAnimTimer_Tick;
            HopTimer.Enabled = true;
        }

        private void HitAnimTimer_Tick(object sender, EventArgs e)
        {
            HitBox.Image = Image.FromFile("../../Images/rabbit.png");
            HitAnimTimer.Stop();
        }

        private void HopTimer_Tick(object sender, EventArgs e)
        {
            Hop();
        }

        private void HitBox_Click(object sender, EventArgs e)
        {
            HitBox.Image = Image.FromFile("../../Images/rabbit_hit.png");

            if (Fake)
            {
                HopTimer.Stop();
                gameRef.Controls.Remove(HitBox);
                return;
            }
            HitCounter++;
            gameRef.ui.HitCounter.Text = HitCounter == 1 ? $"{HitCounter} Hit" : $"{HitCounter} Hits";
            HitAnimTimer.Start();
        }
    }
}
