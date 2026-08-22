using System.Security.Claims;
using FamilySpend.App.CreateTransactionCommand;
using FamilySpend.App.GetTransactionsCommand;
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
    public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var response = await mediator.Send(new GetTransactionsCommand
        {
            UserId = nameIdentifier
        }, cancellationToken);
        return Ok(response);
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