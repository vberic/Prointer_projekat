using Microsoft.AspNetCore.Mvc;
using Prointer_projekat.Data;
using Prointer_projekat.DTOs;
using Prointer_projekat.Models;

namespace Prointer_projekat.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly ConnectionManager _context = new();
        
        
    [HttpPost]
    public IActionResult Registration(NewUser newUser)
    {
        //provjera da li se lozinke poklapaju
        if (!newUser.Password.Equals(newUser.RepeatedPass))
            return BadRequest(new { 
                message = "Passwords are not same" 
            });
            
            //provjera da li je mejl vec u upotrebi
        if (_context.Users.Count(k=> k.Email.Equals(newUser.Email)) != 0)
            return BadRequest(new {
                message = "User with this email already exists"
            });

        User user = new()
        {
            Email = newUser.Email.ToLower(),
            Name = newUser.Name,
            Surname = newUser.Surname,
            PhoneNumber = newUser.PhoneNumber,
            Password = newUser.Password
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(new {
            message = "User with email: " + user.Email + " successfully created"
        });
        }
    }
