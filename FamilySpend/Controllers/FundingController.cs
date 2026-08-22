using System.Security.Claims;
using FamilySpend.App.AllocateFundingCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FundingController(IMediator mediator) : ControllerBase
{
    [HttpPost("allocate")]
    public async Task<IActionResult> AllocateFunding(AllocateFundingCommand  command,  CancellationToken cancellationToken)
    {
        // add funding to sub-account
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        command.FromUserId = nameIdentifier;
        await mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost("add")]
    public IActionResult AddFunding(int amount)
    {
        // talk to payment gateway
        // add funding to balance
        return Ok(new[] { "Product 1", "Product 2" });
    }
}