using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

// --------------------------------------- ΣΗΜΑΝΤΙΚΟ --------------------------------------------------
//
// Εαν τυχόν η φόρτωση αυτού το παραθύρου (DBViewer) είναι πολύ αργή και ενοχλεί,
// τοτε μπορείτε να φτιαξέτε νέα και λιγότερα έγγραφα με το python script: generateTestSamples.py
// Το script βρίσκεται στο bin/Debug/ και παίρνει δυο command-line arguments.
// Εκτέλεση:
//    python generateTestSamples.py [peopleAmount] [testSamples] 
//    Το 1o argument δίνει το ποσό των ανθρώπων που έχουν κάνει αιτήσεις
//    Το 2ο argument δίνει το ποσό των αιτήσεων που υπάρχουν
// Παράδειγμα χρήσης
//    python generateTestSamples.py 20 50
// Η παραπάνω εντολή ξαναφτιάχνει το πίνακα 'Records' της βάσης και βάζει 20 πελάτες 
// και τα στοιχεία τους και 50 αιτήσεις απο αυτούς τους 20.
//
// ----------------------------------------------------------------------------------------------------

namespace Thema3
{
    /// <summary>
    /// Displays a screen-wide view of the 'Records' table from the database.
    /// Features include: 
    /// Selecting any record and updating/deleting it &
    /// Searching for a value in any column of the table using the TextBoxes given
    /// </summary>
    public partial class DBViewer : Form
    {
        public const float titleFontSize = 9.5f;
        public const float elementFontSize = 8.5f;
        private int DBTableYStart = 0;

        private SQLiteConnection connection;
        private Button resetButton;
        private Button delButton;
        private Button updateButton;

        private int selectedRow = -1;
        private int selectedRowID = -1;
        private Color defaultBG;
        private Color defaultFG;

        private DoubleBufferedTable Table;
        private DoubleBufferedTable HeadRows;

        public DBViewer()
        {
            InitializeComponent();
            CreateTables();
            connection = MainMenu.connection;
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Height + 50);
            SetupButtons();
            InitializeHeadRows();
            SetupTable(Table);
        }

        /// <summary>
        /// Add the custom table controls to the form
        /// </summary>
        private void CreateTables()
        {
            Table = new DoubleBufferedTable();
            Table.ColumnCount = 1;
            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1224F));
            Table.Location = new System.Drawing.Point(13, 13);
            Table.Name = "Table";
            Table.RowCount = 1;
            Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            Table.Size = new System.Drawing.Size(1224, 504);
            Table.TabIndex = 0;
            Controls.Add(Table);

            HeadRows = new DoubleBufferedTable();
            HeadRows.ColumnCount = 1;
            HeadRows.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1224F));
            HeadRows.Location = new System.Drawing.Point(13, 13);
            HeadRows.Name = "HeadRows";
            HeadRows.RowCount = 1;
            HeadRows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            HeadRows.Size = new System.Drawing.Size(1224, 504);
            HeadRows.TabIndex = 0;
            Controls.Add(HeadRows);
        }

        /// <summary>
        /// Reset the table's properties after its been reinitialized
        /// </summary>
        /// <param name="dbTable"></param>
        private void SetupProperties(DoubleBufferedTable dbTable) 
        {
            if (dbTable.ColumnCount != 8)
            {
                dbTable.Size = new Size(Width - 40, Height - flowLayoutPanel1.Height - 100 - DBTableYStart);

                dbTable.AutoScroll = true;
                dbTable.ColumnStyles.Clear();
                dbTable.RowStyles.Clear();
                dbTable.ColumnCount = 8;
                dbTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
                dbTable.RowCount = 0;
                float colWidthPercent = (100 / (dbTable.ColumnCount - 1)) + 40 / (float)Width;

                for (int i = 0; i < dbTable.ColumnCount; i++)
                {
                    if (i == 0)
                    {
                        dbTable.ColumnStyles.Add(new ColumnStyle()
                        {
                            SizeType = SizeType.Absolute,
                            Width = 40
                        });
                        continue;
                    }

                    dbTable.ColumnStyles.Add(new ColumnStyle()
                    {
                        SizeType = SizeType.Percent,
                        Width = colWidthPercent
                    });
                }
                dbTable.Top = HeadRows.Location.Y + HeadRows.Height;
            }
        }

        /// <summary>
        /// Sets up table using all SELECT ALL query
        /// </summary>
        private void SetupTable(DoubleBufferedTable dbTable)
        {
            dbTable.SuspendLayout();

            SetupProperties(dbTable);

            //Read from DB and add rows
            AddRowsFromDB(dbTable);

            dbTable.ResumeLayout();
        }
        
        /// <summary>
        /// Sets up table using SELECT WHERE column=value query
        /// </summary>
        /// <param name="column">The column name as specified in the database</param>
        /// <param name="value">The value to search for</param>
        private void SetupTable(DoubleBufferedTable dbTable, string column, string value)
        {
            dbTable.SuspendLayout();

            SetupProperties(dbTable);


            //Read from DB and add rows based on the user's query
            AddRowsFromDB(dbTable, column, value);

            dbTable.ResumeLayout();
        }
        /// <summary>
        /// Add a FlowLayoutPanel containing the necessary buttons
        /// </summary>
        private void SetupButtons()
        {
            flowLayoutPanel1.Height += 50;
            flowLayoutPanel1.Width = Width - 40;
            flowLayoutPanel1.Controls.Add(resetButton = new Button()
            {
                Text = "Επαναφορά",
                Margin = new Padding(50, 0, 50, 0)
            });
            flowLayoutPanel1.Controls.Add(delButton = new Button()
            {
                Text = "Διαγραφή εγγραφής"
            });
            flowLayoutPanel1.Controls.Add(updateButton = new Button()
            {
                Text = "Τροποποίηση εγγραφής"
            });
            FlowPanelTemplate.SetupFlowPanel(flowLayoutPanel1, 16, true);
            resetButton.Click += ResetButton_Click;
            delButton.Click += DelButton_Click;
            updateButton.Click += UpdateButton_Click;

            delButton.Enabled = false;
            updateButton.Enabled = false;
        }
        /// <summary>
        /// Run SQL command to gather ALL rows from the Records table
        /// </summary>
        /// <returns>The SQL query result</returns>
        private SQLiteDataReader SelectDB()
        {
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Records;", connection);
            return cmd.ExecuteReader();
        }
        /// <summary>
        /// Run SQL command to gather SPECIFIC rows from Records table
        /// </summary>
        /// <param name="column">The targeted table column</param>
        /// <param name="value">The value that the targeted column needs to be relate to</param>
        /// <returns></returns>
        private SQLiteDataReader SelectDB(string column, string value)
        {
            SQLiteCommand cmd = new SQLiteCommand(
                $"SELECT * FROM Records WHERE UPPER({column}) LIKE UPPER('%{value}%');", connection);
            return cmd.ExecuteReader();
        }
        private void DeleteRowFromDB()
        {
            string sql = $"DELETE FROM Records WHERE id={selectedRowID};";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }        
        /// <summary>
        /// Create the controls for the first two rows of the DBViewer 
        /// </summary>
        private void InitializeHeadRows()
        {
            // Add title labels
            HeadRows.ColumnStyles.Clear();
            HeadRows.RowStyles.Clear();
            HeadRows.ColumnCount = 8;
            HeadRows.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            HeadRows.RowCount = 2;

            float colWidthPercent = (100 / (HeadRows.ColumnCount - 1)) + 40 / (float)Width;

            for (int i = 0; i < HeadRows.ColumnCount; i++)
            {
                if (i == 0)
                {
                    HeadRows.ColumnStyles.Add(new ColumnStyle()
                    {
                        SizeType = SizeType.Absolute,
                        Width = 40
                    });
                    continue;
                }

                HeadRows.ColumnStyles.Add(new ColumnStyle()
                {
                    SizeType = SizeType.Percent,
                    Width = colWidthPercent
                });
            }

            BasicLabel label0 = new BasicLabel(titleFontSize);
            BasicLabel label1 = new BasicLabel(titleFontSize);
            BasicLabel label2 = new BasicLabel(titleFontSize);
            BasicLabel label3 = new BasicLabel(titleFontSize);
            BasicLabel label4 = new BasicLabel(titleFontSize);
            BasicLabel label5 = new BasicLabel(titleFontSize);
            BasicLabel label6 = new BasicLabel(titleFontSize);
            BasicLabel label7 = new BasicLabel(titleFontSize);
            label0.Text = "ID";
            label1.Text = "Ονομ/νυμο";
            label2.Text = "E-mail";
            label3.Text = "Tηλέφωνο";
            label4.Text = "Ημ. γέννησης";
            label5.Text = "Αίτημα";
            label6.Text = "Διεύθυνση";
            label7.Text = "Ημ. αιτήματος";

            defaultBG = label0.BackColor;
            defaultFG = label0.ForeColor;

            Label[] labels;
            labels = new Label[]
            { label0, label1, label2, label3, label4, label5, label6, label7 };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Font = new Font(labels[i].Font, FontStyle.Bold);
                labels[i].Anchor = AnchorStyles.Left;

                HeadRows.Controls.Add(labels[i], i, 0);
            }

            //Add search boxes
            for (int i = 0; i < HeadRows.ColumnCount; i++)
            {
                TextBox tb;
                HeadRows.Controls.Add(tb = new TextBox()
                {
                    Width = (int)((colWidthPercent / 100) * HeadRows.Width),
                    Name = i.ToString()
                }, i, 1);

                tb.KeyUp += Tb_KeyUp;
            }

            HeadRows.Size = new Size(Width - 50,
                HeadRows.Controls[HeadRows.Controls.Count - 1].Location.Y +
                HeadRows.Controls[HeadRows.Controls.Count - 1].Height);
        }
        private void AddRowsFromDB(DoubleBufferedTable dbTable)
        {
            float colWidthPercent = (100 / (dbTable.ColumnCount - 1)) + 40 / (float)Width;

            using (SQLiteDataReader reader = SelectDB())
            {
                while (reader.Read())
                {
                    InsertRow(dbTable, reader, colWidthPercent);   
                }
            }
            for (int i = 0; i < dbTable.RowCount; i++)
                dbTable.RowStyles.Add(new ColumnStyle()
                {
                    SizeType = SizeType.AutoSize
                });
        }
        private void AddRowsFromDB(DoubleBufferedTable dbTable, string column, string value)
        {
            float colWidthPercent = (100 / (dbTable.ColumnCount - 1)) + 40 / (float)Width;

            using (SQLiteDataReader reader = SelectDB(column, value))
            {
                while (reader.Read())
                {
                    InsertRow(dbTable, reader, colWidthPercent);
                }
            }
            for (int i = 0; i < dbTable.RowCount; i++)
                dbTable.RowStyles.Add(new ColumnStyle()
                {
                    SizeType = SizeType.AutoSize
                });
        }
        /// <summary>
        /// Create the controls for a new row in the Table
        /// </summary>
        private void InsertRow(DoubleBufferedTable dbTable, SQLiteDataReader reader, float colWidthPercent)
        {
            dbTable.RowCount++;
            Label lbl;
            dbTable.Controls.Add(lbl = new Label()
            {
                Text = reader.GetInt16(0).ToString(),
                Anchor = AnchorStyles.Left,
                Name = $"r{dbTable.RowCount - 1}"
            }, 0, dbTable.RowCount - 1);

            lbl.Click += Lbl_Click;

            for (int i = 1; i < dbTable.ColumnCount; i++)
            {
                Label rowLbl;
                dbTable.Controls.Add(rowLbl = new Label()
                {
                    Text = reader.GetString(i),
                    Anchor = AnchorStyles.Left,
                    AutoSize = false,
                    Width = (int)((colWidthPercent / 100) * dbTable.Width),
                    Name = $"r{dbTable.RowCount - 1}"
                }, i, dbTable.RowCount - 1);

                rowLbl.Click += Lbl_Click;
            }
        }
        private string[] GetSelectedRowValues()
        {
            string[] row = new string[8];
            string sql = $"SELECT * FROM Records WHERE id={selectedRowID};";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                row[0] = reader.GetInt16(0).ToString();
                for (int i = 1; i <= 7; i++)
                {
                    row[i] = reader.GetString(i);
                }
            }
            return row;
        }
        /// <summary>
        /// Reinitializes the Table 
        /// </summary>
        private void RefreshTable()
        {
            Controls.Remove(Table);
            DoubleBufferedTable newTable = new DoubleBufferedTable();
            SetupTable(newTable);

            Controls.Add(newTable);
            Table = newTable;
        }
        /// <summary>
        /// Reinitialize the Table using a FILTERED SQL select query
        /// </summary>
        /// <param name="filter"></param>
        private void RefreshTable(TextBox filter)
        {
            Controls.Remove(Table);
            DoubleBufferedTable newTable = new DoubleBufferedTable();
            int columnIdx = int.Parse(filter.Name);
            SetupTable(newTable, CustomerQueryModel.Map[columnIdx], filter.Text);

            Controls.Add(newTable);
            Table = newTable;
        }

        #region Events
        private void Lbl_Click(object sender, EventArgs e)
        {
            delButton.Enabled = true;
            updateButton.Enabled = true;

            Label lbl = sender as Label;

            // Unhighlight previously selected row
            if (selectedRow != -1)
            {
                foreach (Label labelInRow in Table.Controls.Find($"r{selectedRow}", false))
                {
                    labelInRow.BackColor = defaultBG;
                    labelInRow.ForeColor = defaultFG;
                }
            }

            int newSelectedRow = int.Parse(lbl.Name.Substring(1));

            // Highlight new row
            foreach (Label labelInRow in Table.Controls.Find(lbl.Name, false))
            {
                labelInRow.BackColor = Color.MediumBlue;
                labelInRow.ForeColor = Color.White;
                selectedRow = newSelectedRow;
            }

            // Get ID of new row
            Label idLabel = Table.GetControlFromPosition(0, selectedRow) as Label;
            selectedRowID = int.Parse(idLabel.Text);
        }
        private void Tb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                RefreshTable(sender as TextBox);
            }
        }
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Records updateRecordForm = new Records(GetSelectedRowValues());
            updateRecordForm.actionButton.Text = "Τροποποίηση";

            updateRecordForm.Show();
        }
        private void DelButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                $"Θέλετε σίγουρα να διαγράψετε την εγγραφή με ID = {selectedRowID}.", 
                "Είστε σίγουροι;", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Delete result from database 
                DeleteRowFromDB();

                //Refresh the TableLayoutPanel
                RefreshTable();
            }
        }
        private void ResetButton_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }
        #endregion
    }
}
