using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.GetTransactionsCommand;

public class GetTransactionsCommandHandler(UserManager<ZipUser> userManager, FamilySpendDbContext dbContext) : IRequestHandler<GetTransactionsCommand, IEnumerable<TransactionResponse>>
{
    public async Task<IEnumerable<TransactionResponse>> Handle(GetTransactionsCommand request, CancellationToken cancellationToken)
    {
        // var user = await userManager.FindByIdAsync(request.UserId);
        var transactions = await dbContext.Transactions.Include(x=>x.Order).Where(x => x.UserId == request.UserId).ToListAsync(cancellationToken);
        return transactions.Select(x => new TransactionResponse
        {
            Id = x.Id,
            OrderId = x.OrderId,
            Amount = x.Amount,
            TransactionType = x.TransactionType,
            ItemDescription = x.Order.ItemDescription,
            MerchantName = x.Order.MerchantName,
        });
    }
}