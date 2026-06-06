using EfCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EfCore
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        public ApplicationDbContext()
        {
            _connectionString = "Server=.\\SQLEXPRESS;Database=EfCore; User Id=imran; Password=pass; Trust Server Certificate = True ";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }
    }
}