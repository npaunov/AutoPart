using Microsoft.EntityFrameworkCore;
using Models;

namespace EntityFramework
{
    public class AutoPartDbContext : DbContext
    {
        public DbSet<Part> Parts { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public AutoPartDbContext(DbContextOptions<AutoPartDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships and constraints here if needed
            base.OnModelCreating(modelBuilder);
        }
    }
}
