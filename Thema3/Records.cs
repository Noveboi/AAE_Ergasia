using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Thema3
{
    public partial class Records : Form
    {
        private SQLiteConnection connection;
        private int updateID = -1;
        private string[] RowToUpdate;

        public Records(string[] Row)
        {
            updateID = int.Parse(Row[0]);
            RowToUpdate = Row;

            Top = 0;
            InitializeComponent();
            Controls.Add(new Banner(Width, 200));
            SetupFlowPanels();

            connection = MainMenu.connection;

        }
        public Records()
        {
            Top = 0;
            InitializeComponent();
            Controls.Add(new Banner(Width, 200));
            SetupFlowPanels();

            connection = MainMenu.connection;
        }

        private void SetupFlowPanels()
        {
            FlowPanelTemplate.SetupFlowPanel(buttonPanel, 13, true);
            FlowPanelTemplate.SetupFlowPanel(labelPanel, 10, false);
            FlowPanelTemplate.SetupFlowPanel(entryPanel, 10, false);

            //button Panel text
            actionButton.Text = "Εισάγωγη";

            //label panel text
            label1.Text = "Ονοματεπώνυμο";
            label2.Text = "E-mail";
            label3.Text = "Tηλέφωνο επικοινωνίας";
            label4.Text = "Ημερομηνία γέννησης";
            label5.Text = "Είδος αιτήματος";
            label6.Text = "Διεύθυνση κατοικίας";
            label7.Text = "Ημερομηνία αιτήματος";

            //comboBox values
            ValidRequests.GetRequests().ForEach(request => qTypeBox.Items.Add(request));

            LabelTemplate.SetupLabels(labelPanel.Controls);
            TextBoxTemplate.SetupTextBoxes(entryPanel.Controls.OfType<TextBox>().ToList());

            float icr = 3.5f;
            dobPicker.Font = new Font(dobPicker.Font.FontFamily, dobPicker.Font.Size + icr);
            toqPicker.Font = new Font(toqPicker.Font.FontFamily, toqPicker.Font.Size + icr);
            qTypeBox.Font = new Font(qTypeBox.Font.FontFamily, qTypeBox.Font.Size + icr);

            dobPicker.Value = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy",null);

            // Ιf this form is opened for UPDATING record 
            if (updateID != -1)
            {
                int i = 1;
                foreach (Control control in entryPanel.Controls)
                {
                    if (i == 5)
                    {
                        ComboBox box = entryPanel.Controls.OfType<ComboBox>().First();
                        box.SelectedItem = RowToUpdate[i];
                        i++;
                        continue;
                    }
                    if (!(i == 4) && !(i == 7))
                        control.Text = RowToUpdate[i];
                    i++;
                }

                DateTimePicker dob = entryPanel.Controls.OfType<DateTimePicker>().First();
                DateTimePicker toq = entryPanel.Controls.OfType<DateTimePicker>().Last();
                dob.Value = DateTime.ParseExact(RowToUpdate[4], "dd/MM/yyyy", null);
                toq.Value = DateTime.ParseExact(RowToUpdate[7], "dd/MM/yyyy HH:mm:ss tt", null);
            }
        }

        private bool IsValidCustomerQuery()
        {
            string validEmailCharacters = @"[A-z0-9!#\$%&'\*\+\-/=\?^_`{|}~\.]";
            string validDomainCharacters = @"[A-z0-9\-]";
            Regex emailRgx = new Regex($@"{validEmailCharacters}+@{validDomainCharacters}+\.[A-z]+");
            Regex phoneRgx = new Regex("[0-9]{10}");

            int emptyFields = 0;
            foreach (TextBox tb in entryPanel.Controls.OfType<TextBox>())
            {
                if (tb.Text == string.Empty)
                    emptyFields++;
            }

            if (emptyFields != 0)
            {
                MainMenu.ResultsTB.Text = "Παρακαλώ συμπληρώστε όλα τα πεδία";
                return false;
            }

            if (!emailRgx.IsMatch(emailBox.Text))
            {
                MainMenu.ResultsTB.Text = "Παρακαλώ συμπληρώστε ορθή διεύθηνση e-mail (π.Χ. john.doe@example.com)";
                return false;
            }    

            if (!phoneRgx.IsMatch(phoneBox.Text))
            {
                MainMenu.ResultsTB.Text = "Παρακαλώ συμπληρώστε ορθό αριθμό τηλεφώνου (10 ψηφία)";
                return false;
            }

            if (qTypeBox.SelectedItem == null)
            {
                MainMenu.ResultsTB.Text = "Παρακαλώ επιλέξτε είδος αιτήματος.";
                return false;
            }

            return true;
        }

        //Insert
        private void button1_Click(object sender, EventArgs e)
        {
            //MainMenu is always the 1st form opened
            Application.OpenForms[0].Focus();

            if (!IsValidCustomerQuery()) return;

            //Write To DB
            CustomerQueryManager manager = new CustomerQueryManager();
            CustomerQueryModel newCustomerQuery = new CustomerQueryModel(
                fullNameBox.Text,
                emailBox.Text,
                phoneBox.Text,
                dobPicker.Value,
                qTypeBox.SelectedItem.ToString(),
                adrBox.Text,
                toqPicker.Value);

            if (updateID != -1)
                manager.UpdateRecord(updateID, newCustomerQuery);
            else
                manager.InsertIntoDB(newCustomerQuery);

            //Write output to results
            MainMenu.ResultsTB.Text = updateID == -1
                ?
                $"Προσθήκη νέου αιτήματος πελάτη: {Environment.NewLine}{Environment.NewLine}"
                :
                $"Τροποποίηση αιτήματος πελάτη: {Environment.NewLine}{Environment.NewLine}";
            MainMenu.ResultsTB.Text += newCustomerQuery.ToString();
        }

    }
}
