using Devart.Data.SQLite;

namespace SQLiteConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string databasePath = @"path\to\your\sakila.db;";

            string connectionString = $"Data Source={databasePath};License Key=**********";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

}