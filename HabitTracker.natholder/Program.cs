using System;
using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class Program
    {

        static void Main(string[] args)
        {
            string dbFileName = "HabitTracker.db";
            string connectionString = $"Data Source ={dbFileName}";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine($"Database {dbFileName} created or opened successfully.");
                    CreateTable(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        static void CreateTable(SqliteConnection connection)
        {
            string CreateTableQuery = @"
            CREATE TABLE IF NOT EXISTS Habits (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date DATETIME,
            Miles INT
            )";

            using (var command = new SqliteCommand(CreateTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table 'Habits' created or already exists.");
            }
        }
    }
}