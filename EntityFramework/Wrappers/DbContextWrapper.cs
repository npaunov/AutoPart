using AutoPartApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoPartApp.EntityFramework
{
    /// <summary>
    /// Provides a wrapper around the AutoPartDbContext for database operations.
    /// </summary>
    public class DbContextWrapper : IDisposable
    {
        private readonly AutoPartDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextWrapper"/> class using DI-managed context.
        /// </summary>
        public DbContextWrapper(AutoPartDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new database file and schema. Overwrites any existing database.
        /// </summary>
        /// <returns>A status message indicating success or failure.</returns>
        public string CreateNewDatabase()
        {
            try
            {
                _context.Database.EnsureDeleted(); // Optional
                _context.Database.Migrate(); // not EnsureCreated()
                return "Database created successfully!";
            }
            catch (Exception ex)
            {
                return $"Failed to create database: {ex.Message}";
            }
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
            // The DI container will dispose the context, so this is optional.
            // _context?.Dispose();
        }
    }
}