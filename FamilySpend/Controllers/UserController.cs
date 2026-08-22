using System.Security.Claims;
using FamilySpend.App.CreatePrimaryUserCommand;
using FamilySpend.App.GetCurrentUserCommand;
using FamilySpend.Infra.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePrimaryUser(CreatePrimaryUserCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser(GetCurrentUserCommand command, CancellationToken cancellationToken)
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        command.CurrentUserId = nameIdentifier;
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}