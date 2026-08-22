namespace FamilySpend.Infra.Entities;

public class Loan : BaseEntity
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public string UserId { get; set; }
    public string DependenceUserId { get; set; }
}