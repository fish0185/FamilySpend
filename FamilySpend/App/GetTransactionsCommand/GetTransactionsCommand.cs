using MediatR;

namespace FamilySpend.App.GetTransactionsCommand;

public class GetTransactionsCommand : IRequest<IEnumerable<TransactionResponse>>
{
    public string UserId { get; set; }
}