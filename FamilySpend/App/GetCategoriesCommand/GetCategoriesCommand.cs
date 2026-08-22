using MediatR;

namespace FamilySpend.App.GetCategoriesCommand;

public class GetCategoriesCommand : IRequest<GetCategoriesResponse>
{
    public string UserId { get; set; }
}