using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSHVM
{
    internal class DB
    {
        MySqlConnection connection = new MySqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Works;Integrated Security=True");
        //DESKTOP-BI4MF53//
        public static string conqwe = "qwe";
        public void openConnection()
        {
            if(connection.State==System.Data.ConnectionState.Closed)
                connection.Open();
        }
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
