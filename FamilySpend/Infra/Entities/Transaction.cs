namespace FamilySpend.Infra.Entities;

public class Transaction : BaseEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
}