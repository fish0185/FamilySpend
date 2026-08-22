using MediatR;

namespace FamilySpend.App.InvitationCommand;

public class InvitationCommand : IRequest
{
    public string? Id { get; set; }
    public string Email { get; set; }
}