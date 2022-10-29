using Microsoft.AspNetCore.Mvc;

namespace Prointer_projekat.Controllers;

[ApiController]
[Route("logout")]
public class LogoutController : ControllerBase
{
    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok(new{message ="Logout successful" });
    }
}