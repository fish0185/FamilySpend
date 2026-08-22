using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilySpend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FundingController : ControllerBase
{
    [HttpPost("/allocate")]
    public IActionResult AllocateFunding(int amount, int accountId)
    {
        // add funding to sub-account
        return Ok(new[] { "Product 1", "Product 2" });
    }
    
    [HttpPost("/add")]
    public IActionResult AddFunding(int amount)
    {
        // talk to payment gateway
        // add funding to balance
        return Ok(new[] { "Product 1", "Product 2" });
    }
}