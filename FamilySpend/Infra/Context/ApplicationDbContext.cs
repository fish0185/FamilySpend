using FamilySpend.Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.Infra.Context;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }
    
    public DbSet<FamilyLink> FamilyLinks => Set<FamilyLink>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<FamilyLink>().HasIndex(x => x.UserId);

        modelBuilder.Entity<FamilyLink>().HasIndex(p => new
            {
                p.UserId,
                p.FamilyUserId
            })
            .IsUnique();
    }
}