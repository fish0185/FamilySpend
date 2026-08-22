using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePrimaryUser()
    {
        return Ok(new[] { "Product 1", "Product 2" });
    }
}