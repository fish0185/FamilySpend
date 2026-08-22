using System.Transactions;
using FamilySpend.Infra.Entities;
using MediatR;

namespace FamilySpend.App.CreateTransactionCommand;

public class CreateTransactionCommand : IRequest
{
    public string? UserId { get; set; }
    public int OrderCategoryId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public string ItemDescription { get; set; }
    public string MerchantName { get; set; }
}