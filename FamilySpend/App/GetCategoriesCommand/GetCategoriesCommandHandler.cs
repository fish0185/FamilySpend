using FamilySpend.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.GetCategoriesCommand;

public class GetCategoriesCommandHandler(FamilySpendDbContext dbContext) : IRequestHandler<GetCategoriesCommand, GetCategoriesResponse>
{
    public async Task<GetCategoriesResponse> Handle(GetCategoriesCommand request, CancellationToken cancellationToken)
    {
        var categories = await dbContext.OrderCategories.ToListAsync(cancellationToken);
        return new GetCategoriesResponse
        {
            Categories = categories.Select(x => x.Name).ToArray()
        };
    }
}