using MediatR;

namespace FamilySpend.App.DeleteUserOrderCategoryCommand;

public class DeleteUserOrderCategoryCommand : IRequest
{
    public string UserId { get; set; }
    public string SubAccountEmail { get; set; }
    public string CategoryName { get; set; }
}