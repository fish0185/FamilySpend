using System.Security.Claims;
using FamilySpend.App.CreateTransactionCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] { "Product 1", "Product 2" });
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransaction(CreateTransactionCommand  command, CancellationToken cancellationToken)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        command.UserId = nameIdentifier;
        await mediator.Send(command, cancellationToken);
        return Ok();
    }
}