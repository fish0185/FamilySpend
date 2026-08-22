using FamilySpend.Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.Infra.Context;

public class ZipUserDbContext : IdentityDbContext<ZipUser>
{
    public ZipUserDbContext(DbContextOptions<ZipUserDbContext> options) :
        base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}