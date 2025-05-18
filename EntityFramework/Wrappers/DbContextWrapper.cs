using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoPartApp.Models;

namespace AutoPartApp.EntityFramework
{
    /// <summary>
    /// Provides a wrapper around the AutoPartDbContext for database operations.
    /// </summary>
    public class DbContextWrapper : IDisposable
    {
        private readonly AutoPartDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextWrapper"/> class.
        /// </summary>
        public DbContextWrapper()
        {
            var options = BuildOptions();
            _context = new AutoPartDbContext(options);
            // Do not call EnsureCreated here; let the user trigger creation explicitly.
        }

        /// <summary>
        /// Creates a new database file and schema. Overwrites any existing database.
        /// </summary>
        /// <returns>A status message indicating success or failure.</returns>
        public static string CreateNewDatabase()
        {
            try
            {
                var options = BuildOptions();
                using var context = new AutoPartDbContext(options);
                context.Database.EnsureDeleted(); // Optional
                context.Database.EnsureCreated();
                return "Database created successfully!";
            }
            catch (Exception ex)
            {
                return $"Failed to create database: {ex.Message}";
            }
        }

        /// <summary>
        /// Builds the DbContextOptions for connecting to the local database file.
        /// </summary>
        /// <returns>The configured DbContextOptions.</returns>
        private static DbContextOptions<AutoPartDbContext> BuildOptions()
        {
            //string folderPath = @"..\..\..\..\DataBase\";
            string folderPath = @"D:\AutoPart\DataBase";
            var dbPath = Path.Combine(folderPath, "AutoPartsDb.mdf");

            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=AutoPartsDb;Integrated Security=True;Connect Timeout=30";

            return new DbContextOptionsBuilder<AutoPartDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        /// <summary>
        /// Retrieves all parts from the database.
        /// </summary>
        /// <returns>An enumerable collection of all parts.</returns>
        public IEnumerable<Part> GetAllParts() => _context.Parts.ToList();

        /// <summary>
        /// Adds a new part to the database and saves changes.
        /// </summary>
        /// <param name="part">The part to add.</param>
        public void AddPart(Part part)
        {
            _context.Parts.Add(part);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes a part from the database and saves changes.
        /// </summary>
        /// <param name="part">The part to remove.</param>
        public void RemovePart(Part part)
        {
            _context.Parts.Remove(part);
            _context.SaveChanges();
        }

        /// <summary>
        /// Disposes the underlying database context.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}