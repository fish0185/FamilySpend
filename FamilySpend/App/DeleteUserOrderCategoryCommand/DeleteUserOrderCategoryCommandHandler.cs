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
            Console.WriteLine("User is not primary");
            return;
        }
        
        var subUser = await userManager.FindByEmailAsync(request.SubAccountEmail);
        if (subUser is not { IsPrimary: false })
        {
            Console.WriteLine("User is not found");
            return;
        }
        
        var category = await dbContext.OrderCategories.Where(x => x.Name == request.CategoryName).FirstOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            Console.WriteLine("Category is not found");
            return;
        }

        var userOrderCategory = dbContext.UserOrderCategories.FirstOrDefault(x => x.OrderCategoryId == category.Id && x.UserId == subUser.Id);
        if (userOrderCategory is null)
        {
            Console.WriteLine("UserOrderCategory is not found");
            return;
        }
        
        dbContext.UserOrderCategories.Remove(userOrderCategory);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}