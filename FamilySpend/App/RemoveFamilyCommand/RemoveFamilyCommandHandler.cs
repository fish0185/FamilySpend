using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.RemoveFamilyCommand;

public class RemoveFamilyCommandHandler(UserManager<ZipUser> userManager, FamilySpendDbContext dbContext) : IRequestHandler<RemoveFamilyCommand>
{
    public async Task Handle(RemoveFamilyCommand request, CancellationToken cancellationToken)
    {
         // validation
         var currentUser = await userManager.FindByIdAsync(request.UserId);
         if (currentUser is not { IsPrimary: true })
         {
             throw new InvalidOperationException("User not found or not primary user");
         }
         
         var family = await userManager.FindByEmailAsync(request.RemoveUserEmail);
         if (family == null)
         {
             return;
         }
         
         var link = await dbContext.FamilyLinks.Where(x=>x.UserId == request.UserId && family.Id == x.FamilyUserId).FirstOrDefaultAsync(cancellationToken);
         if (link == null)
         {
             return;
         }
         
         // reset balance
         var userLoan = await dbContext.Loans.Where(x => x.UserId == request.UserId).FirstAsync(cancellationToken);
         var familyLoan = await dbContext.Loans.Where(x => x.UserId == family.Id).FirstAsync(cancellationToken);
         userLoan.Balance += familyLoan.Balance;
         
         // remove 
         dbContext.FamilyLinks.Remove(link);
         await dbContext.SaveChangesAsync(cancellationToken);
    }
}