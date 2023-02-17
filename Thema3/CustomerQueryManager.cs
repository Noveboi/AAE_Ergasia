using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thema3
{
    internal class CustomerQueryManager
    {
        SQLiteConnection connection;
        public CustomerQueryManager()
        {
            connection = MainMenu.connection;
        }
        public void InsertIntoDB(CustomerQueryModel customerQuery)
        {
            string sql = "INSERT INTO Records (" +
            "FullName, Email, Phone, DOB, Type, Address, Time_Of_Query) " +
            "VALUES (@fullname, @email, @phone, @dob, @type, @address, @toq)";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            List<SQLiteParameter> parameters = new List<SQLiteParameter>()
            {
                new SQLiteParameter("fullname", customerQuery.FullName),
                new SQLiteParameter("email", customerQuery.Email),
                new SQLiteParameter("phone", customerQuery.Phone),
                new SQLiteParameter("dob", customerQuery.DOB.ToString("dd/MM/yyyy")),
                new SQLiteParameter("type", customerQuery.QueryType),
                new SQLiteParameter("address", customerQuery.Address),
                new SQLiteParameter("toq", customerQuery.TimeOfQuery.ToString("dd/MM/yyyy HH:mm:ss tt")),
            };
            parameters.ForEach(param => cmd.Parameters.Add(param));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateRecord(int id, CustomerQueryModel customerQuery)
        {
            string sql = "UPDATE Records " +
                "SET FullName=@fullname, Email=@email, Phone=@phone, DOB=@dob, Type=@type," +
                "Address=@address, Time_Of_Query=@toq " +
                $"WHERE id={id};";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            List<SQLiteParameter> parameters = new List<SQLiteParameter>()
            {
                new SQLiteParameter("fullname", customerQuery.FullName),
                new SQLiteParameter("email", customerQuery.Email),
                new SQLiteParameter("phone", customerQuery.Phone),
                new SQLiteParameter("dob", customerQuery.DOB.ToString("dd/MM/yyyy")),
                new SQLiteParameter("type", customerQuery.QueryType),
                new SQLiteParameter("address", customerQuery.Address),
                new SQLiteParameter("toq", customerQuery.TimeOfQuery.ToString("dd/MM/yyyy HH:mm:ss tt")),
            };
            parameters.ForEach(param => cmd.Parameters.Add(param));
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
