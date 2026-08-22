using FamilySpend.Infra.Entities;

namespace FamilySpend.App.GetTransactionsCommand;

public class TransactionResponse
{
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public int OrderId { get; set; }
    public string ItemDescription { get; set; }
    public string MerchantName { get; set; }
    public int Id { get; set; }
}