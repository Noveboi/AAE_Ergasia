using System.Windows.Forms;
using System.Data.SQLite;
using System;
using System.Drawing;

namespace battleships_official2
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
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Comic Sans", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(12, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(776, 57);
            this.title.TabIndex = 0;
            this.title.Text = "BATTLESHIPS";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.title.BackColor = Color.Transparent;
            this.title.ForeColor = Color.White;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(277, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter your username!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.BackColor = Color.Transparent;
            this.label1.ForeColor = Color.White;

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
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.title);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
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
            title.Font = App.GetFont(32);
            BackgroundImage = Image.FromFile("sea.jpg");
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void SetupDatabaseConnection()
        {
            // Establish connection to database
            connection = new SQLiteConnection(
                "Data Source=dummy.db;Version=3;");
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
    }
}

