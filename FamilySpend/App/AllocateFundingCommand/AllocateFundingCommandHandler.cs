using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.AllocateFundingCommand;

public class AllocateFundingCommandHandler(UserManager<ZipUser> userManager, FamilySpendDbContext dbContext) : IRequestHandler<AllocateFundingCommand>
{
    public async Task Handle(AllocateFundingCommand request, CancellationToken cancellationToken)
    {
        // validate
        var currentUser = await userManager.FindByIdAsync(request.FromUserId);
        if (currentUser is not { IsPrimary: true })
        {
            throw new InvalidOperationException("User not found or not primary user");
        }
        
        var toUser = await userManager.FindByEmailAsync(request.ToUserEmail);
        if (toUser == null)
        {
            return;
        }
        
        var links = await dbContext.FamilyLinks.Where( x=> x.UserId == request.FromUserId).ToListAsync(cancellationToken);
        var link = links.FirstOrDefault( x => x.FamilyUserId == toUser.Id);
        if (link == null)
        {
            Console.WriteLine("no link found");
            return;
        }
        
        var fromLoan = await dbContext.Loans.Where(x => x.UserId == request.FromUserId).FirstAsync(cancellationToken);
        var toLoan = await dbContext.Loans.Where(x => x.UserId == toUser.Id).FirstAsync(cancellationToken);
        
        switch (request.Amount)
        {
            case > 0 when fromLoan.Balance < request.Amount:
                Console.WriteLine("not enough money main account");
                return;
            case < 0 when toLoan.Balance < -request.Amount:
                Console.WriteLine("not enough money main sub-account");
                return;
        }

        fromLoan.Balance -= request.Amount;
        toLoan.Balance += request.Amount;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}