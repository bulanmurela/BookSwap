using System;
using System.Configuration;
using System.Data;
using Npgsql;

namespace BookSwapApp.Helpers
{
    public class DatabaseHelpers
    {
        private readonly string connectionString;

        public DatabaseHelpers()
        {
            // Mengambil connection string dari app.config
            connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
        }

        public NpgsqlConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void ExecuteNonQuery(string query)
        {
            using (var connection = OpenConnection())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            var dataTable = new DataTable();
            using (var connection = OpenConnection())
            using (var command = new NpgsqlCommand(query, connection))
            using (var adapter = new NpgsqlDataAdapter(command))
            {
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = OpenConnection())
                {
                    Console.WriteLine("Database Connected!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect database: " + ex.Message);
                return false;
            }
        }

    }
}
