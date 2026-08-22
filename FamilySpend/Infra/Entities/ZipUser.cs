using Microsoft.AspNetCore.Identity;

namespace FamilySpend.Infra.Entities;

public class ZipUser : IdentityUser
{
    public bool IsPrimary { get; set; }
    public Loan Loan { get; set; }
}