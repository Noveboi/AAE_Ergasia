using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thema3
{
    public partial class MainMenu : Form
    {
        public static SQLiteConnection connection = 
            new SQLiteConnection("Data Source=kep.db;Version=3");
        public static RichTextBox ResultsTB;

        public MainMenu()
        {
            connection.Open();
            SetupDatabase();
            InitializeComponent();
            SetupFlowPanel();
            Controls.Add(new Banner(Width, 200));

            ResultsTB = richTextBox1;
        }

        private void SetupDatabase()
        {
            string sql = "CREATE Table IF NOT EXISTS Records (" +
                "id INTEGER," +
                "FullName VARCHAR(255)," +
                "Email VARCHAR(255)," +
                "Phone VARCHAR(16)," +
                "DOB VARCHAR(16)," +
                "Type VARCHAR(255)," +
                "Address VARCHAR(255)," +
                "Time_Of_Query VARCHAR(255)," +
                "PRIMARY KEY (id AUTOINCREMENT));";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }

        private void SetupFlowPanel()
        {
            FlowPanelTemplate.SetupFlowPanel(flowLayoutPanel1,16,true);
            button1.Text = "Καταγραφή Αιτήματος";
            button2.Text = "Αναζήτηση Αιτήματος";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new DBViewer().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Records().Show();
        }
    }
}
