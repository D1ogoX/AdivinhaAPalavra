using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;
using System.Data;

namespace AdivinhaAPalavra.API.Models
{
    public class DBConnection
    {
        public DBConnection()
        {
            Server = "";
            DatabaseName = "";
            UserName = "";
            Password = "";
            Port = "";
        }

        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        public MySqlConnection Connection { get; set; }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;


                string connstring = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", Server, Port, UserName, Password, DatabaseName);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }

            return true;
        }

        public DataTable ExecuteSelectQuery(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, Connection);
            MySqlDataAdapter returnVal = new MySqlDataAdapter(query, Connection);
            DataTable dt = new DataTable();
            returnVal.Fill(dt);
            return dt;
        }

        public bool InsertQuery (string query)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Connection;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                return true;
            }

            catch
            {
                return false;
            }
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}
