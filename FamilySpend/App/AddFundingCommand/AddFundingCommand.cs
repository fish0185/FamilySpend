using MediatR;

namespace FamilySpend.App.AddFundingCommand;

public class AddFundingCommand : IRequest
{
    public decimal Amount { get; set; }
    public string UserId { get; set; }
}