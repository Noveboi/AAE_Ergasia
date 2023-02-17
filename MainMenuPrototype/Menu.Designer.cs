using System.Windows.Forms;
using System.Data.SQLite;
using System;

namespace MainMenuPrototype
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.title = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.playButton = new System.Windows.Forms.Button();
            this.diffSelection = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(12, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(776, 102);
            this.title.TabIndex = 0;
            this.title.Text = "Hit The Rabbit!";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(277, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter your username!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(277, 234);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(234, 22);
            this.usernameBox.TabIndex = 2;
            this.usernameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.usernameBox_KeyDown);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(277, 373);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(234, 65);
            this.playButton.TabIndex = 3;
            this.playButton.Text = "Play!";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // diffSelection
            // 
            this.diffSelection.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.diffSelection.FormattingEnabled = true;
            this.diffSelection.ItemHeight = 16;
            this.diffSelection.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard"});
            this.diffSelection.Location = new System.Drawing.Point(277, 270);
            this.diffSelection.Margin = new System.Windows.Forms.Padding(0);
            this.diffSelection.Name = "diffSelection";
            this.diffSelection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.diffSelection.Size = new System.Drawing.Size(234, 96);
            this.diffSelection.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(595, 373);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 65);
            this.button1.TabIndex = 5;
            this.button1.Text = "Leaderboards";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.diffSelection);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.title);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hit The Rabbit";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;

        // Player Generated Code Below !!!
        private void SetupApp()
        {
            foreach (Control c in Controls)
            {
                c.Font = App.Font;
            } 
        }

        private void SetupMenu()
        {
            this.title.Font = UI.GetFont(36);
            this.title.ForeColor = UI.FG_Color;
            this.label1.ForeColor = UI.FG_Color;
            this.playButton.ForeColor = UI.FG_Color;
            this.BackColor = Game.BG_Color;
            this.playButton.Font = UI.GetFont(16);
            this.playButton.BackColor = UI.BG_Color;
            this.playButton.FlatStyle = FlatStyle.Flat;

            this.button1.Font = UI.GetFont(14);
            this.button1.BackColor = UI.BG_Color;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.ForeColor = UI.FG_Color;


            this.diffSelection.BackColor = Game.BG_Color;
            this.diffSelection.ForeColor = UI.FG_Color;
            this.diffSelection.SelectedItem = this.diffSelection.Items[1];
            this.diffSelection.Font = App.GetFont(12);
        }

        private void SetupDatabaseConnection()
        {
            // Establish connection to database
            connection = new SQLiteConnection(
                "Data Source=htr_database.db;Version=3;");
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private SQLiteConnection connection;
        private Label label1;
        private TextBox usernameBox;
        private Button playButton;
        private ListBox diffSelection;
        private Button button1;
    }
}

