using Club.Domain;
using Club.Models; // Make sure this namespace is included
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Club.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add this line to register your new table:
    public DbSet<CommitteeMember> CommitteeMembers { get; set; }

    // for student and course
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
}