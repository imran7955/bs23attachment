using Microsoft.EntityFrameworkCore;
using LmsProject.Domain.Entities;

namespace LmsProject.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Instructor> Instructors { get; set; } = null!;
        public DbSet<Material> Materials { get; set; } = null!;
        public DbSet<CourseMaterial> CourseMaterials { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Many-to-Many Join Table for Course and Material with Position tracking
            modelBuilder.Entity<CourseMaterial>()
                .HasKey(cm => new { cm.CourseId, cm.MaterialId });

            modelBuilder.Entity<CourseMaterial>()
                .HasOne(cm => cm.Course)
                .WithMany(c => c.CourseMaterials)
                .HasForeignKey(cm => cm.CourseId);

            modelBuilder.Entity<CourseMaterial>()
                .HasOne(cm => cm.Material)
                .WithMany(m => m.CourseMaterials)
                .HasForeignKey(cm => cm.MaterialId);

            // Configure Implicit Many-to-Many relationship between Course and Instructor
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses);
        }
    }
}