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