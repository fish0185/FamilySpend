using FamilySpend.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.GetCurrentUserCommand;

public class GetCurrentUserCommandHandler(
    FamilySpendDbContext familySpendDbContext, ZipUserDbContext zipUserDbContext) : IRequestHandler<GetCurrentUserCommand, GetCurrentUserResponse>
{
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await zipUserDbContext.Users.FindAsync(request.CurrentUserId, cancellationToken);
        if (!currentUser.IsPrimary)
        {
            return new GetCurrentUserResponse
            {
                Email = currentUser.Email,
                IsPrimaryAccount = false,
                SubAccountEmails = []
            };
        }

        var links = await familySpendDbContext.FamilyLinks.Where(x => x.UserId == request.CurrentUserId)
            .Include(x => x.ZipUser).ToListAsync(cancellationToken);
        return new GetCurrentUserResponse
        {
            Email = currentUser.Email,
            IsPrimaryAccount = true,
            SubAccountEmails = links.Select(x => x.ZipUser.Email).ToArray()
        };
    }
}