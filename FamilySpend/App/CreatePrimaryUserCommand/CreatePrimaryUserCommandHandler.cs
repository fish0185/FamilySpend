using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FamilySpend.App.CreatePrimaryUserCommand;

public class CreatePrimaryUserCommandHandler(UserManager<ZipUser> userManager, FamilySpendDbContext dbContext)
    : IRequestHandler<CreatePrimaryUserCommand>
{
    private readonly UserManager<ZipUser> _userManager = userManager;

    public async Task Handle(CreatePrimaryUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ZipUser() { UserName = request.Email, Email = request.Email, IsPrimary = true};
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            Console.WriteLine(result.Errors);
            return;
        }

        var newUser = await userManager.FindByEmailAsync(request.Email);
        
        dbContext.Loans.Add(new Loan
        {
            Balance = 1000,
            UserId = newUser.Id,
            ZipUser = newUser
        });
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}