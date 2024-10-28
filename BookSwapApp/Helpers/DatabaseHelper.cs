using System;
using System.Data;
using System.Security.Policy;
using System.Windows;
using Npgsql;

namespace BookSwapApp.Helpers
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = "Host=localhost;Port=5432;Username=postgres;Password=wonwoorideul;Database=bookswap;";
        }

        public NpgsqlConnection OpenConnection()
        {
            try
            {
                var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                MessageBox.Show("Database terkoneksi", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Koneksi gagal: {ex.Message}", "Kesalahan", MessageBoxButton.OK, MessageBoxImage.Error);
                throw; 
            }
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