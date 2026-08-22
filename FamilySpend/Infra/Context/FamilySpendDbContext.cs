using FamilySpend.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.Infra.Context;

public class FamilySpendDbContext :  DbContext
{
    // The constructor accepts options configuration (like connection strings)
    public FamilySpendDbContext(DbContextOptions<FamilySpendDbContext> options) : base(options)
    {
    }

    // Each DbSet represents a table in the database
    public DbSet<FamilyLink> FamilyLinks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<FamilyLink>().HasKey(x => x.Id);
        
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