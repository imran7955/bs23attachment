using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Club.Models; // Make sure this namespace is included

namespace Club.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add this line to register your new table:
    public DbSet<CommitteeMember> CommitteeMembers { get; set; }
}