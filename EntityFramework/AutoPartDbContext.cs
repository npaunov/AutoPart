using Microsoft.EntityFrameworkCore;
using AutoPartApp.Models;

namespace AutoPartApp.EntityFramework
{
    public class AutoPartDbContext : DbContext
    {
        public DbSet<Part> Parts { get; set; }
        //public DbSet<Warehouse> Warehouses { get; set; }

        public AutoPartDbContext(DbContextOptions<AutoPartDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Part>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .IsRequired()
                      .ValueGeneratedNever();

                entity.Property(e => e.Description)
                      .IsRequired();

                entity.Property(e => e.PriceBGN)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)"); // Explicitly set precision and scale

                entity.Property(e => e.Package)
                      .IsRequired();

                entity.Property(e => e.InStore)
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
