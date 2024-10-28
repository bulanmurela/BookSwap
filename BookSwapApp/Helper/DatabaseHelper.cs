using System;
using System.Data;
using Npgsql;

namespace BookSwapApp.Helpers
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = "Host=localhost;Port=5432;Username=postgre;Password=1234;Database=bookswap;";
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
    }
}