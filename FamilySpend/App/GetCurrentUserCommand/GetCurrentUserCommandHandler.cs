using FamilySpend.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.GetCurrentUserCommand;

public class GetCurrentUserCommandHandler(
    FamilySpendDbContext familySpendDbContext) : IRequestHandler<GetCurrentUserCommand, GetCurrentUserResponse>
{
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await familySpendDbContext.Users.Include(x => x.Loan)
            .Where(x => x.Id == request.CurrentUserId)
            .FirstAsync(cancellationToken);
        if (!currentUser.IsPrimary)
        {
            return new GetCurrentUserResponse
            {
                Email = currentUser.Email,
                Balance = currentUser.Loan.Balance,
                IsPrimaryAccount = false,
                SubAccounts = Enumerable.Empty<SubAccountResponse>()
            };
        }

        var links = (await familySpendDbContext.FamilyLinks.Where(x => x.UserId == request.CurrentUserId)
            .ToListAsync(cancellationToken)).Select(x => x.FamilyUserId);
        var users = await familySpendDbContext.Users.Include(x=>x.Loan).Where(x=> links.Contains(x.Id)).ToListAsync(cancellationToken);
        return new GetCurrentUserResponse
        {
            Email = currentUser.Email,
            IsPrimaryAccount = true,
            Balance = currentUser.Loan.Balance,
            SubAccounts = users.Select(x => new SubAccountResponse
            {
                Balance = x.Loan.Balance,
                Email = x.Email,
            }).ToArray()
        };
    }
}