﻿// <auto-generated />
using AutoPart.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AutoPart.DataAccess.Migrations
{
    [DbContext(typeof(AutoPartDbContext))]
    [Migration("20250520173446_AddPartSalesTotalRelation")]
    partial class AddPartSalesTotalRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AutoPartApp.Models.Part", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InStore")
                        .HasColumnType("int");

                    b.Property<int>("Package")
                        .HasColumnType("int");

                    b.Property<decimal>("PriceBGN")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("PartsInStock");
                });

            modelBuilder.Entity("AutoPartApp.Models.PartsSalesTotal", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TotalSales")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PartsSalesTotals");
                });

            modelBuilder.Entity("AutoPartApp.Models.PartsSalesTotal", b =>
                {
                    b.HasOne("AutoPartApp.Models.Part", "Part")
                        .WithOne("SalesTotal")
                        .HasForeignKey("AutoPartApp.Models.PartsSalesTotal", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Part");
                });

            modelBuilder.Entity("AutoPartApp.Models.Part", b =>
                {
                    b.Navigation("SalesTotal")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
