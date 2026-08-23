using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FamilySpend.App.InvitationCommand;

public class InvitationCommandHandler(
    UserManager<ZipUser> userManager, 
    FamilySpendDbContext familySpendDbContext)
    : IRequestHandler<InvitationCommand>
{
    public async Task Handle(InvitationCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await userManager.FindByIdAsync(request.Id);
        if (currentUser is not { IsPrimary: true })
        {
            throw new InvalidOperationException("User not found or not primary user");
        }
        
        var user = new ZipUser() { UserName = request.Email, Email = request.Email, IsPrimary = false};
        var result = await userManager.CreateAsync(user, "Test.1234");
        if (!result.Succeeded)
        {
            Console.WriteLine(result.Errors);
            return;
        }
        
        var newUser = await userManager.FindByEmailAsync(request.Email);
        
        // link account
        familySpendDbContext.FamilyLinks.Add(new FamilyLink
        {
            UserId = request.Id,
            FamilyUserId = newUser.Id,
        });
        
        familySpendDbContext.Loans.Add(new Loan()
        {
            UserId = newUser.Id,
            Balance = 0,
            ZipUser = newUser
        });
        
        await familySpendDbContext.SaveChangesAsync(cancellationToken);
    }
}