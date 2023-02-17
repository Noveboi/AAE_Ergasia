using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MainMenuPrototype
{
    public class UI
    {
        public Panel Frame;
        public Label TimeDisplay;
        public Label UserDisplay;
        public Label HitCounter;
        public List<Control> Controls;

        public static Color BG_Color = Color.FromArgb(4, 107, 186);
        public static Color FG_Color = Color.Linen;
        private const string FontUsed = "Impact";
        public static int Height = 100;
        public static Font GetFont(float emSize)
        {
            return new Font(FontUsed, emSize);
        }

        public UI()
        {
            Controls = new List<Control>();
        }

        public List<Control> InitializeUI()
        {
            // Frame
            Frame = new Panel();
            Frame.Size = new Size(Game.WindowSize.Width, Height);
            Frame.Location = new Point(0, 0);
            Frame.BackColor = BG_Color;

            // TimeDisplay
            TimeDisplay = new Label();
            TimeDisplay.Text = TimeSpan.FromSeconds(0).ToString();
            TimeDisplay.Size = new Size(Game.WindowSize.Width / 4, 100);
            TimeDisplay.BackColor = BG_Color;
            TimeDisplay.ForeColor = FG_Color;
            TimeDisplay.Location = new Point(0, 0);
            TimeDisplay.Font = GetFont(20);
            TimeDisplay.TextAlign = ContentAlignment.MiddleCenter;
            TimeDisplay.AutoSize = false;

            // UserDisplay
            UserDisplay = new Label();
            UserDisplay.Text = Game.Player.Username;
            UserDisplay.Size = new Size(Game.WindowSize.Width / 2, 100);
            UserDisplay.BackColor = BG_Color;
            UserDisplay.ForeColor = FG_Color;
            UserDisplay.Location = new Point(Game.WindowSize.Width / 4, 0);
            UserDisplay.Font = GetFont(36);
            UserDisplay.TextAlign = ContentAlignment.MiddleCenter;
            UserDisplay.AutoSize = false;

            // HitCounter
            HitCounter = new Label();
            HitCounter.Text = "0 Hits";
            HitCounter.Size = new Size(Game.WindowSize.Width / 4, 100);
            HitCounter.BackColor = BG_Color;
            HitCounter.ForeColor = FG_Color;
            HitCounter.Location = new Point(3*Game.WindowSize.Width/4, 0);
            HitCounter.TextAlign = ContentAlignment.MiddleCenter;
            HitCounter.Font = GetFont(24);
            HitCounter.AutoSize = false;

            // Manage the Frame's Controls
            Frame.Controls.Add(TimeDisplay);
            Frame.Controls.Add(HitCounter);
            Frame.Controls.Add(UserDisplay);

            // Set Z-Index
            Frame.Controls.SetChildIndex(HitCounter, 0);
            Frame.Controls.SetChildIndex(TimeDisplay, 0);
            Frame.Controls.SetChildIndex(UserDisplay, 0);

            // Add Controls
            Controls.Add(Frame);

            return Controls;
        }
    }
}
