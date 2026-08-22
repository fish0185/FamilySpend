using System.Security.Claims;
using FamilySpend.App.AddUserOrderCategoryCommand;
using FamilySpend.App.DeleteUserOrderCategoryCommand;
using FamilySpend.App.GetCategoriesCommand;
using FamilySpend.App.GetTransactionsCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderCategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var response = await mediator.Send(new GetCategoriesCommand
        {
            UserId = nameIdentifier
        }, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("user")]
    public async Task<IActionResult> SetUserOrderCategories(AddUserOrderCategoryCommand command, CancellationToken cancellationToken)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        command.UserId = nameIdentifier;
        await mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [HttpDelete("user")]
    public async Task<IActionResult> RemoveUserOrderCategories([FromQuery]string email, [FromQuery] string category, CancellationToken cancellationToken)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await mediator.Send(new DeleteUserOrderCategoryCommand
        {
            UserId = nameIdentifier,
            SubAccountEmail = email,
            CategoryName = category
        }, cancellationToken);
        return Ok();
    }
}