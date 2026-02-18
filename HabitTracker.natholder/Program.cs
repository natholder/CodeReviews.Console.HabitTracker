using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{

    public class Runs
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Miles { get; set; }
    }
    class Program
    {
        static string dbFileName = "HabitTracker.db";
        static string connectionString = $"Data Source ={dbFileName}";
        static void Main(string[] args)
        {
            initDB();
            MenuLoop();
        }

        static void initDB()
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                Console.WriteLine($"Database {dbFileName} created or opened successfully.");
                CreateTable(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        static void MenuLoop()
        {
            string menuText = @"
    __  ____ __             ______                __            
   /  |/  (_) /__  _____   /_  __/________ ______/ /_____  _____
  / /|_/ / / / _ \/ ___/    / / / ___/ __ `/ ___/ //_/ _ \/ ___/
 / /  / / / /  __(__  )    / / / /  / /_/ / /__/ ,< /  __/ /    
/_/  /_/_/_/\___/____/    /_/ /_/   \__,_/\___/_/|_|\___/_/";
            bool running = true;
            string? userInput;
            int number;
            Console.WriteLine(menuText);
            while (running)
            {
                ShowMenu();
                userInput = Console.ReadLine();
                if (int.TryParse(userInput, out number) && number > 0 && number <= 6)
                {
                    switch (userInput)
                    {
                        case "1":
                            SelectRecords();
                            break;
                        case "2":
                            Insert();
                            break;
                        case "3":
                            Console.WriteLine("update");
                            break;
                        case "4":
                            Delete();
                            break;
                        default:
                            running = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }
        static void CreateTable(SqliteConnection connection)
        {
            string CreateTableQuery = @"
            CREATE TABLE IF NOT EXISTS Habits (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date DATETIME,
            Miles DOUBLE
            )";

            using var command = new SqliteCommand(CreateTableQuery, connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Table 'Habits' created or already exists.");
        }

        static void Insert()
        {
            DateTime date = GetDate();
            double miles = GetMiles();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var insert = connection.CreateCommand();
            insert.CommandText = $"INSERT INTO Habits (Date, Miles) VALUES ('{date}', {miles})";
            insert.ExecuteNonQuery();
            connection.Close();
        }

        static void SelectRecords()
        {
            Console.Clear();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var select = connection.CreateCommand();
            select.CommandText = $"SELECT * FROM Habits";
            List<Runs> tableData = [];
            SqliteDataReader reader = select.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new Runs
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            Miles = reader.GetDouble(2)
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            connection.Close();

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("   Id    |   Date        |    Miles");
            Console.WriteLine("---------------------------------------");
            foreach (var row in tableData)
            {
                Console.WriteLine($"|   {row.Id}   |   {row.Date.ToString("MM-dd-yyyy")}    |    {row.Miles}      |");
            }
            Console.WriteLine("---------------------------------------");

        }

        static void Delete()
        {
            int id = GetId();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var insert = connection.CreateCommand();
            insert.CommandText = $"DELETE FROM Habits WHERE Id = {id}";
            int rowsAffected = insert.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"Successfully deleted record with ID: {id}");
            }
            else
            {
                Console.WriteLine($"No record found with ID: {id}");
            }

            connection.Close();
        }

        //TODO: Start and finish update method
        static void Update()
        {

        }


        static void ShowMenu()
        {
            Console.WriteLine("1. View");
            Console.WriteLine("2. Insert");
            Console.WriteLine("3. Update");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Quit");
        }

        static double GetMiles()
        {
            double miles;
            string? input;
            Console.WriteLine("How many miles did you run?");
            input = Console.ReadLine();
            if (double.TryParse(input, out miles))
            {
                return miles;
            }
            else
            {
                Console.WriteLine("Please enter a valid number");
                return GetMiles();
            }
        }

        static int GetId()
        {
            int id;
            string? input;
            Console.WriteLine("Enter the id of the record you would like to delete.");
            input = Console.ReadLine();
            if (int.TryParse(input, out id))
            {
                return id;
            }
            else
            {
                Console.WriteLine("Please enter a valid number");
                return GetId();
            }
        }

        static DateTime GetDate()
        {
            string? input;
            DateTime date;
            Console.WriteLine("What day was your run? (type 'td' for today's date, or enter date as MM/DD/YYYY):");
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please enter a valid date or 'td' for today.");
                return GetDate(); // Recursive call for empty input
            }

            if (input?.ToLower() == "td")
            {
                return DateTime.Today;
            }

            if (DateTime.TryParse(input, out date))
            {
                return date;
            }

            Console.WriteLine("Invalid date format. Please try again (e.g., 02/16/2026) or type 'td' for today.");
            return GetDate(); // Recursive call for invalid date
        }
    }
}
