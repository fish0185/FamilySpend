using MediatR;

namespace FamilySpend.App.AllocateFundingCommand;

public class AllocateFundingCommand : IRequest
{
    public string? FromUserId { get; set; }
    
    public string ToUserEmail { get; set; }
    
    public decimal Amount { get; set; }
}