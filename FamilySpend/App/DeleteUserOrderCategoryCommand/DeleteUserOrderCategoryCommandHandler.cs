using FamilySpend.Infra.Context;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilySpend.App.DeleteUserOrderCategoryCommand;

public class DeleteUserOrderCategoryCommandHandler(FamilySpendDbContext dbContext, UserManager<ZipUser> userManager) : IRequestHandler<DeleteUserOrderCategoryCommand>
{
    public async Task Handle(DeleteUserOrderCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is not { IsPrimary: true })
        {
            throw new InvalidOperationException("User is not found or is not primary");
        }
        
        var subUser = await userManager.FindByEmailAsync(request.SubAccountEmail);
        if (subUser is not { IsPrimary: false })
        {
            throw new InvalidOperationException("User is not found or is primary");
        }
        
        var category = await dbContext.OrderCategories.Where(x => x.Name == request.CategoryName).FirstOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            throw new InvalidOperationException("Category is not found");
        }

        var userOrderCategory = dbContext.UserOrderCategories.FirstOrDefault(x => x.OrderCategoryId == category.Id && x.UserId == subUser.Id);
        if (userOrderCategory is null)
        {
            throw new InvalidOperationException("UserOrderCategory is not found");
        }
        
        dbContext.UserOrderCategories.Remove(userOrderCategory);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}