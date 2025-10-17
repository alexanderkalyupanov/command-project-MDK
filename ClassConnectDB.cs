using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandProject
{
    class ClassConnectDB
    {
        private const string ConnectionString = "Server=HaverlandSuslovPC\\MSSQLSERVER02;Database=OnlineLibraryDB;Integrated Security=True;";

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Ошибка при открытии соединения с базой данных: {ex.Message}");
            }
        }

        public static void CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }
    }
}
