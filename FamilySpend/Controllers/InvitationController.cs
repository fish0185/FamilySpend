using System.Security.Claims;
using FamilySpend.App.InvitationCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    
    [HttpDelete]
    public IActionResult RemoveFamily()
    {
        // remove account link
        return Ok(new[] { "Product 1", "Product 2" });
    }
}