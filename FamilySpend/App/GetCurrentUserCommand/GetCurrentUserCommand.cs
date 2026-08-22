using MediatR;

namespace FamilySpend.App.GetCurrentUserCommand;

public class GetCurrentUserCommand : IRequest<GetCurrentUserResponse>
{
    public string? CurrentUserId { get; set; }
}