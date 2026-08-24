using System.Security.Claims;
using FamilySpend.App.InvitationCommand;
using FamilySpend.App.RemoveFamilyCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "PrimaryUser")]
public class InvitationController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvitationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddFamily(InvitationCommand invitationCommand, CancellationToken cancellationToken)
    {
        // create account 
        // link account
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        invitationCommand.Id = nameIdentifier;
        await _mediator.Send(invitationCommand, cancellationToken);
        
        return Ok();
    }
    
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveFamily([FromBody]RemoveFamilyCommand command, CancellationToken cancellationToken)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        command.UserId = nameIdentifier;
        // remove account link
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}