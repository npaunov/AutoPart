using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;

namespace EntityFramework
{
    public class DbContextWrapper : IDisposable
    {
        private readonly AutoPartDbContext _context;

        public DbContextWrapper()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dbFolder = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\DataBase\"));
            Directory.CreateDirectory(dbFolder);
            var dbPath = Path.Combine(dbFolder, "AutoPartsDb.mdf");

            var connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30";

            var options = new DbContextOptionsBuilder<AutoPartDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new AutoPartDbContext(options);
            _context.Database.EnsureCreated();
        }

        public IEnumerable<Part> GetAllParts() => _context.Parts.ToList();

        public void AddPart(Part part)
        {
            _context.Parts.Add(part);
            _context.SaveChanges();
        }

        public void RemovePart(Part part)
        {
            _context.Parts.Remove(part);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
