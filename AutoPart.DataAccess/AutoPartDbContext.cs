using Microsoft.EntityFrameworkCore;
using AutoPart.Models;
using AutoPart.Models.Orders;

namespace AutoPart.DataAccess
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the AutoPart application.
    /// Manages entity sets and configures the database schema.
    /// </summary>
    public class AutoPartDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the parts currently in stock.
        /// </summary>
        public DbSet<Part> PartsInStock { get; set; }

        /// <summary>
        /// Gets or sets the sales records for parts.
        /// </summary>
        public DbSet<PartSale> PartSales { get; set; }

        /// <summary>
        /// Gets or sets the orders placed for parts.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the items included in each order.
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPartDbContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public AutoPartDbContext(DbContextOptions<AutoPartDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the entity model and relationships using the Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Part entity
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

            // Configure the Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.TotalSumBGN)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TotalSumEuro)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Date)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .IsRequired();

                entity.Property(e => e.ModifiedAt)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .IsRequired();
            });

            // Configure the OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);

                entity.Property(e => e.Description)
                      .IsRequired();

                entity.Property(e => e.Package)
                      .IsRequired();

                entity.Property(e => e.PriceBGN)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.PriceEuro)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SubtotalBGN)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SubtotalEuro)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.QuantityOrdered)
                      .IsRequired();

                entity.Property(e => e.QuantityReceived)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .IsRequired();

                // Relationships are handled by EF Core conventions and attributes
            });

            // Configure the PartSale entity (if you want to be explicit)
            modelBuilder.Entity<PartSale>(entity =>
            {
                entity.HasKey(e => e.SaleId);

                entity.Property(e => e.PartId)
                      .IsRequired();

                entity.Property(e => e.SaleDate)
                      .IsRequired();

                entity.Property(e => e.Quantity)
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
