using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FamilySpend.App.InvitationCommand;

public class InvitationCommandHandler(UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
    : IRequestHandler<InvitationCommand>
{
    public async Task Handle(InvitationCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, "Test.1234");
        if (!result.Succeeded)
        {
            Console.WriteLine(result.Errors);
            return;
        }

        var newUser = await userManager.FindByEmailAsync(request.Email);
        
        // link account
        await applicationDbContext.FamilyLinks.AddAsync(new FamilyLink
        {
            UserId = request.Id,
            FamilyUserId = newUser.Id,
            IsPrimary = false
        }, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}