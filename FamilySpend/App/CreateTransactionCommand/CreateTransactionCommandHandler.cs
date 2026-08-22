using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.CreateTransactionCommand;

public class CreateTransactionCommandHandler(FamilySpendDbContext dbContext) : IRequestHandler<CreateTransactionCommand>
{
    public async Task Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // validate category
        
        
        if (request.Amount <= 0)
        {
            Console.WriteLine("Amount must be greater than or equal to zero");
            return;
        }
        
        // validate balance
        var loan = await dbContext.Loans.Where(l => l.UserId == request.UserId).FirstAsync(cancellationToken);

        switch (request.TransactionType)
        {
            case TransactionType.Debit:
                if (request.Amount > loan.Balance )
                {
                    throw new InvalidOperationException("Insufficient balance");
                }
                loan.Balance -= request.Amount;
                break;
            case TransactionType.Credit:
                loan.Balance += request.Amount;
                break;
        }
        
        loan.LastModified = DateTimeOffset.UtcNow;

        var order = new Order
        {
            ItemDescription = request.ItemDescription,
            MerchantName = request.MerchantName,
        };
        
        dbContext.Orders.Add(order);
        
        dbContext.Transactions.Add(new Transaction
        {
            UserId = request.UserId,
            Amount = request.Amount,
            TransactionType = request.TransactionType,
            Order =  order
        });
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}