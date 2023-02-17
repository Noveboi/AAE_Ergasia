using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Thema3
{
    internal class CustomerQueryModel
    {
        public string FullName { get; }
        public string Email { get; }
        public string Phone { get; }
        public string QueryType { get; }
        public string Address { get; }
        public DateTime DOB { get; }
        public DateTime TimeOfQuery { get; }

        public static string[] Map = new string[]
        {
            "id",
            "FullName",
            "Email",
            "Phone",
            "DOB",
            "Type",
            "Address",
            "Time_Of_Query"
        };

        public CustomerQueryModel(string name, string email, 
            string phone, DateTime dob, string qt, string addr, DateTime toq)
        {
            FullName = name;
            Email = email;
            Phone = phone;
            DOB = dob;
            QueryType = qt;
            Address = addr;
            TimeOfQuery = toq;
        }

        public override string ToString()
        {
            return $"Ονοματεπώνυμο: {FullName}{Environment.NewLine}" +
                $"E-Mail: {Email}{Environment.NewLine}" +
                $"Τηλέφωνο Επικοινωνίας: {Phone}{Environment.NewLine}" +
                $"Ημερομηνία Γέννησης: {DOB.ToString("dd/MM/yyyy")}{Environment.NewLine}" +
                $"Είδος Αιτήματος: {QueryType}{Environment.NewLine}" +
                $"Διεύθηνση: {Address}{Environment.NewLine}" +
                $"Ημερομηνία Αιτήματος: {TimeOfQuery.ToString("dd/MM/yyyy HH:mm:ss tt")}{Environment.NewLine}";
        }
    }
}
