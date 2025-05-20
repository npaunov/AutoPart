using Microsoft.EntityFrameworkCore;
using AutoPartApp.Models;

namespace AutoPartApp.EntityFramework
{
    public class AutoPartDbContext : DbContext
    {
        public DbSet<Part> PartsInStock { get; set; }
        public DbSet<PartsSalesTotal> PartsSalesTotals { get; set; }
        public DbSet<PartSale> PartSales { get; set; }

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
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Package)
                      .IsRequired();

                entity.Property(e => e.InStore)
                      .IsRequired();
            });

            modelBuilder.Entity<PartsSalesTotal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalSales)
                      .IsRequired();

                // Configure one-to-one relationship
                entity.HasOne(e => e.Part)
                      .WithOne(p => p.SalesTotal)
                      .HasForeignKey<PartsSalesTotal>(e => e.Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}