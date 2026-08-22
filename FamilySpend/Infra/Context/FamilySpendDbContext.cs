using FamilySpend.Infra.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.Infra.Context;

public class FamilySpendDbContext : IdentityDbContext<ZipUser>
{
    // The constructor accepts options configuration (like connection strings)
    public FamilySpendDbContext(DbContextOptions<FamilySpendDbContext> options) : base(options)
    {
    }

    // Each DbSet represents a table in the database
    public DbSet<FamilyLink> FamilyLinks { get; set; }
    
    public DbSet<Loan> Loans { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderCategory> OrderCategories { get; set; }
    
    public DbSet<UserOrderCategory> UserOrderCategories { get; set; }
    
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