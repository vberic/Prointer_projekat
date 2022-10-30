using Microsoft.AspNetCore.Mvc;
using Prointer_projekat.Data;
using Prointer_projekat.DTOs;
using Prointer_projekat.Helper;
using Prointer_projekat.Models;
using Attribute = Prointer_projekat.Models.Attribute;

namespace Prointer_projekat.Controllers;
[ApiController]

public class AttributesController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ConnectionManager _context = new();
    public AttributesController(IConfiguration config)
    {
        _config = config;
    }
    [HttpPost]
    [Route("addAttribute")]
    public IActionResult AddNewAttribute(NewAttribute newAttribute)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        
        if (_context.Attributes.FirstOrDefault(at => at.Name.Equals(newAttribute.Name)) != null)
            return BadRequest(new { message = "Attribute with that name already exists" });

        Attribute attribute = new()
        {
            Name = newAttribute.Name,
            IsNumerical = newAttribute.IsNumerical,
            IsUnique = newAttribute.IsUnique
        };
        
        _context.Attributes.Add(attribute);
        _context.SaveChanges();
        
        return Ok("New attribute successfully added");
    }
    [HttpGet]
    [Route("getAttributes")]
    public IActionResult GetAll(string? order)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        IQueryable<Attribute> attributes = null!;
        if(order == null)
             attributes = _context.Attributes.Where(att => true);
        else
            attributes = order.ToLower() switch
            {
                "name" => _context.Attributes.Where(att => true).OrderBy(att => att.Name),
                "isnumerical" => _context.Attributes.Where(att => true).OrderBy(att => att.IsNumerical),
                "isunique" => _context.Attributes.Where(att => true).OrderBy(att => att.IsUnique),
                _ => _context.Attributes.Where(att => true)
            };

        List<AttributesOut> result = new ();
        foreach (var attribute in attributes)
        {
            result.Add(new AttributesOut()
            {
                Name = attribute.Name,
                AttributeId = attribute.AttributeId,
                IsNumerical = attribute.IsNumerical,
                IsUnique = attribute.IsUnique
            });
        }
        return Ok(result);
    }
}