using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class Program
    {

        static void Main(string[] args)
        {
            string dbFileName = "HabitTracker.db";
            string connectionString = $"Data Source ={dbFileName}";
            string menuText = @"
    __  ____ __             ______                __            
   /  |/  (_) /__  _____   /_  __/________ ______/ /_____  _____
  / /|_/ / / / _ \/ ___/    / / / ___/ __ `/ ___/ //_/ _ \/ ___/
 / /  / / / /  __(__  )    / / / /  / /_/ / /__/ ,< /  __/ /    
/_/  /_/_/_/\___/____/    /_/ /_/   \__,_/\___/_/|_|\___/_/";
            string? userInput;
            bool running = true;
            int number;
            double miles = 0.0;
            DateTime date = new DateTime();
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

            while (running)
            {

                Console.WriteLine(menuText);
                ShowMenu();
                userInput = Console.ReadLine();
                if (int.TryParse(userInput, out number) && number > 0 && number <= 6)
                {
                    switch (userInput)
                    {
                        case "1":
                            Console.WriteLine("view");
                            break;
                        case "2":
                            miles = GetMiles(miles, userInput);
                            date = GetDate(date, userInput);
                            Console.WriteLine(date.Date);
                            break;
                        case "3":
                            Console.WriteLine("update");
                            break;
                        case "4":
                            Console.WriteLine("delete");
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
            Miles INT
            )";

            using (var command = new SqliteCommand(CreateTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table 'Habits' created or already exists.");
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("1. View");
            Console.WriteLine("2. Insert");
            Console.WriteLine("3. Update");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Quit");
        }

        static int Insert()
        {


            string CreateTableQuery = @"
            CREATE TABLE IF NOT EXISTS Habits (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date DATETIME,
            Miles INT
            )";

            return 0;
        }

        static double GetMiles(double miles, string? input)
        {
            Console.WriteLine("How many miles?");
            input = Console.ReadLine();
            return double.TryParse(input, out miles) == false ? -1 : miles;
        }

        static DateTime GetDate(DateTime date, string? input)
        {

            Console.WriteLine("What day was your run? (tyoe td for today's date)");
            input = Console.ReadLine();
            if (input == "td")
            {
                date = DateTime.Today;
                return date;
            }
            else if (DateTime.TryParse(input, out date))
            {
                return date;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}