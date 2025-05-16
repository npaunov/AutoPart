using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace EntityFramework
{
    /// <summary>
    /// Provides a design-time factory for creating instances of <see cref="AutoPartDbContext"/>.
    /// This is required for Entity Framework Core tools to create the context when running migrations or updating the database.
    /// </summary>
    public class AutoPartDbContextFactory : IDesignTimeDbContextFactory<AutoPartDbContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AutoPartDbContext"/> with the configured options.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
        /// <returns>A configured <see cref="AutoPartDbContext"/> instance.</returns>
        public AutoPartDbContext CreateDbContext(string[] args)
        {
            string folderPath = @"D:\AutoPart\DataBase";
            var dbPath = Path.Combine(folderPath, "AutoPartsDb.mdf");
            var connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30";

            var optionsBuilder = new DbContextOptionsBuilder<AutoPartDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AutoPartDbContext(optionsBuilder.Options);
        }
    }
}
