using MediatR;

namespace FamilySpend.App.AddUserOrderCategoryCommand;

public class AddUserOrderCategoryCommand : IRequest
{
    public string? UserId { get; set; }
    public string SubAccountEmail { get; set; }
    public string CategoryName { get; set; }
}