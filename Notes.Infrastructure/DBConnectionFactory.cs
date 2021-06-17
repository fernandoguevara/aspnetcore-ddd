using Microsoft.Data.SqlClient;
using Npgsql;
using System.Data.Common;

namespace Notes.Infrastructure
{
    public static class DBConnectionFactory
    {
        public static DbConnection MakeDBConnection(string connectionString)
        {
            if(connectionString.Contains("Server"))
            {
                return new SqlConnection(connectionString);
            }
            else
            {
                return new NpgsqlConnection(connectionString);
            }
        }
    }
}
