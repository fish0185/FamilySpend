using System.ComponentModel.DataAnnotations.Schema;

namespace FamilySpend.Infra.Entities;

public class Loan : BaseEntity
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public ZipUser ZipUser { get; set; }
}