using System.Data.SQLite;
using System.Windows.Forms;
using System;

namespace MainMenuPrototype
{
    partial class Results
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
            this.SuspendLayout();
            // 
            // Results
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(977, 543);
            this.Name = "Results";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Results";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Results_FormClosed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Results_Paint);
            this.Resize += new System.EventHandler(this.Results_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        // User code below!
        // Same as in Menu.cs
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
    }
}