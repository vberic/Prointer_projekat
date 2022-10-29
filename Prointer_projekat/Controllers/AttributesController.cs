using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Prointer_projekat.Data;
using Prointer_projekat.Helper;

namespace Prointer_projekat.Controllers;
[ApiController]
[Route("attributes")]
public class AttributesController : ControllerBase
{
    private IConfiguration _config;
    private readonly ConnectionManager _context = new();
    public AttributesController(IConfiguration config)
    {
        _config = config;
    }
    [HttpGet]
    public IActionResult Test()
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
       
        
        return Ok("cookie");
    }
}