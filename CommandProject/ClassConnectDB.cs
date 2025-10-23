using System;
using System.Data.SqlClient;

namespace CommandProject
{
    class ClassConnectDB
    {
        // Жёстко заданная строка подключения для указанного сервера и базы данных
        private static readonly string ConnectionString =
            "Server=DESKTOP-JUDO2JQ\\MSSQLSERVERDEZGA;Database=OnlineLibraryDB;Integrated Security=True;TrustServerCertificate=True;";

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

        // Возвращает заданную строку подключения, чтобы вызывающие стороны могли создавать помощники без открытия соединения
        public static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
