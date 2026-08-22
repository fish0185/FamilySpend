using System.Security.Claims;
using FamilySpend.App.AddFundingCommand;
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
    
    [HttpPost("add/{amount:int}")]
    public async Task<IActionResult> AddFunding(int amount)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await mediator.Send(new AddFundingCommand() { Amount = amount, UserId = nameIdentifier}, CancellationToken.None);
        return Ok();
    }
}