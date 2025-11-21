# How to Connect to SQLite in .NET With C#

Based on [https://www.devart.com/dotconnect/sqlite/connect-sqlite-in-net.html](https://www.devart.com/dotconnect/sqlite/connect-sqlite-in-net.html)

dotConnect for SQLite is a high-performance ADO.NET data provider that enables seamless connectivity between .NET applications and SQLite databases. Built on top of the standard SQLite engine, it offers enhanced functionality including advanced security features, ORM support, and optimized performance for enterprise applications.

This guide demonstrates how to establish connections to SQLite databases in your C# applications, covering everything from basic connectivity to advanced scenarios like encrypted databases and EF Core integration.

## Connect to SQLite using C#

Establishing a connection to SQLite using dotConnect is straightforward with the SQLiteConnection class. This section walks you through creating a basic connection, opening it, and executing your first queries against a SQLite database.

```cs
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
```

## Connect to SQLite using encryption

Security is critical for modern applications. dotConnect for SQLite supports database encryption out of the box, allowing you to protect sensitive data at rest. Learn how to create encrypted databases and connect to them using password-protected connection strings.

```cs
using Devart.Data.SQLite;

namespace SQLiteConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string databasePath = @"path\to\your\sakila.db";

            string connectionString = $"Data Source={databasePath};Encryption=AES256;FailIfMissing=false;Password=best;License Key=**********";

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
```

## Connect to SQLite with EF Core

Entity Framework Core provides a powerful ORM layer for .NET applications. dotConnect for SQLite seamlessly integrates with EF Core, enabling you to work with SQLite databases using LINQ queries and a code-first or database-first approach. This section covers configuration and basic usage patterns.

```cs
using System;
using System.Linq;
using Devart.Data.SQLite.EFCore;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDotConnectSqliteDemo
{

    public class Actor
    {
        public int ActorId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime LastUpdate { get; set; }
    }

    public class SakilaContext : DbContext
    {
        private readonly string _connectionString;

        public SakilaContext(string connectionString) => _connectionString = connectionString;

        public DbSet Actors => Set();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSQLite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity();
            e.ToTable("Actor");
            e.HasKey(a => a.ActorId);
            e.Property(a => a.ActorId).HasColumnName("ActorId");
            e.Property(a => a.FirstName).HasColumnName("FirstName");
            e.Property(a => a.LastName).HasColumnName("LastName");
            e.Property(a => a.LastUpdate).HasColumnName("LastUpdate");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var connString = @"Data Source=path\to\your\sakila.db;License Key=**********";
            using
            var db = new SakilaContext(connString);
            Console.WriteLine("Fetching first 15 actors...\n");
            var actors = db.Actors.OrderBy(a => a.ActorId).Take(15).Select(a => new {
                a.ActorId,
                a.FirstName,
                a.LastName,
            }).ToList();
            Console.WriteLine("ActorId\tFirstName\tLastName");
            Console.WriteLine("-------\t---------\t--------");
            foreach (var a in actors)
            {
                Console.WriteLine($"{a.ActorId}\t{a.FirstName, -10}\t{a.LastName}");
            }
            Console.WriteLine("\nDone. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
```