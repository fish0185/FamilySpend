using MediatR;

namespace FamilySpend.App.RemoveFamilyCommand;

public class RemoveFamilyCommand : IRequest
{
    public string? UserId { get; set; }
    public string RemoveUserEmail { get; set; }
}