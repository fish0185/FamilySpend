using System.Security.Claims;
using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FamilySpend.App.CreatePrimaryUserCommand;

public class CreatePrimaryUserCommandHandler(UserManager<ZipUser> userManager, RoleManager<IdentityRole> roleManager, FamilySpendDbContext dbContext)
    : IRequestHandler<CreatePrimaryUserCommand>
{
    private readonly UserManager<ZipUser> _userManager = userManager;

    public async Task Handle(CreatePrimaryUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ZipUser() { UserName = request.Email, Email = request.Email, IsPrimary = true};
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(",", result.Errors.Select(error => error.Description)));
        }
        
        bool roleExists = await roleManager.RoleExistsAsync("PrimaryUser");
        
        if (!roleExists)
        {
            // Create the new role instance and persist it to the store
            var identityResult = await roleManager.CreateAsync(new IdentityRole("PrimaryUser"));

            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(",", identityResult.Errors.Select(error => error.Description)));
            }
        }
        
        var roleResult = await _userManager.AddToRoleAsync(user, "PrimaryUser");

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(string.Join(",", roleResult.Errors.Select(error => error.Description)));
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