using MySqlConnector;
using System.Data;
using System.Data.SqlClient;

namespace Global5.Infra.Data.DataBase
{
    public static class ConnectionManager
    {
        public static IDbConnection GetConnection(string connectionString, DataBaseService dataBaseService)
        {
            var connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        public static void CloseConnection(IDbConnection conn)
        {
            if (conn.State == ConnectionState.Open || conn.State == ConnectionState.Broken)
            {
                conn.Close();
            }
        }
    }
    public enum DataBaseService
    {
        SqlServer,
        Oracle,
        MySql
    }
}
