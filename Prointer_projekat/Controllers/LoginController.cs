using System.IdentityModel.Tokens.Jwt;
using System.Net.Security;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Prointer_projekat.Data;
using Prointer_projekat.DTOs;
using Prointer_projekat.Models;

namespace Prointer_projekat.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    private IConfiguration _config;
    private readonly ConnectionManager _context = new();
    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login(UserLogin userLogin)
    {
        var user = Authenticate(userLogin);

        if (user != null)
        {
            var token = Generate(user);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true
            });
            return Ok(new {message = "Login successful"});
        }

        return Unauthorized();
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"], 
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User Authenticate(UserLogin userLogin)
    {
        return _context.Users.FirstOrDefault(u =>
                u.Email == userLogin.Email.ToLower() && u.Password == userLogin.Password)!;
    }
}