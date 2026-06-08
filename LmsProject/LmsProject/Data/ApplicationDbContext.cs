using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LmsProject.Models;

namespace LmsProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Your LMS Tables
        public DbSet<Instructor> Instructors { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Material> Materials { get; set; } = null!;
        public DbSet<CourseMaterial> CourseMaterials { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Keeps the Identity user login system working
            base.OnModelCreating(modelBuilder);

            // 1. Configure the Composite Primary Key for the CourseMaterial join table
            modelBuilder.Entity<CourseMaterial>()
                .HasKey(cm => new { cm.CourseId, cm.MaterialId });

            // 2. Configure the Course -> CourseMaterial (One-to-Many) relationship
            modelBuilder.Entity<CourseMaterial>()
                .HasOne(cm => cm.Course)
                .WithMany(c => c.CourseMaterials)
                .HasForeignKey(cm => cm.CourseId);

            // 3. Configure the Material -> CourseMaterial (One-to-Many) relationship
            modelBuilder.Entity<CourseMaterial>()
                .HasOne(cm => cm.Material)
                .WithMany(m => m.CourseMaterials)
                .HasForeignKey(cm => cm.MaterialId);
        }
    }
}