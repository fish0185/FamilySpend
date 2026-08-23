using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.AddFundingCommand;

public class AddFundingCommandHandler(UserManager<ZipUser> userManager, FamilySpendDbContext dbContext) : IRequestHandler<AddFundingCommand>
{
    public async Task Handle(AddFundingCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0 )
        {
            Console.WriteLine("Amount must be greater than zero");
            return;
        }
        
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is not { IsPrimary: true })
        {
            throw new InvalidOperationException("User is not found or is not primary");
        }

        var loan = await dbContext.Loans.Where(l => l.UserId == request.UserId).FirstAsync(cancellationToken);
        loan.Balance += request.Amount;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}