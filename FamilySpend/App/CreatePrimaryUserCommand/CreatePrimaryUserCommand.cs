using MediatR;

namespace FamilySpend.App.CreatePrimaryUserCommand;

public class CreatePrimaryUserCommand : IRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}