using Microsoft.AspNetCore.Mvc;
using Prointer_projekat.Data;
using Prointer_projekat.DTOs;
using Prointer_projekat.Helper;
using Prointer_projekat.Models;

namespace Prointer_projekat.Controllers;

[ApiController]

public class ArticleController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ConnectionManager _context = new();
    public ArticleController(IConfiguration config)
    {
        _config = config;
    }
    
    [HttpPost]
    [Route("addArticle")]
    public IActionResult AddNewArticle(NewArticle newArticle)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        
        var test = _context.Articles.FirstOrDefault(art =>
            art.Name.Equals(newArticle.Name) || art.ArticleId.Equals(newArticle.ArticleId));
        if (test != null)
        {
            return BadRequest(test.Name.Equals(newArticle.Name) ? new { message = "Article with that name already exists" } : new { message = "Article code already in use" });
        }

        Article article = new()
        {
            ArticleId = newArticle.ArticleId,
            Name = newArticle.Name,
            Unit = newArticle.Unit,
            Manufacturer = newArticle.Manufacturer
        };
        
        _context.Articles.Add(article);
        _context.SaveChanges();
        
        return Ok(new { message = "Article successfully added" });
    }
    
    [HttpPost]
    [Route("addAttributeToArticle")]
    public IActionResult AddArticleAttribute(AddAttribute add)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        
        var article = _context.Articles.FirstOrDefault(ar => ar.Name.Equals(add.ArticleName));
        if (article == null)
            return BadRequest(new { message = "Article not found" });
        
        var attribute = _context.Attributes.FirstOrDefault(ar => ar.Name.Equals(add.AttributeName));
        if (attribute == null)
            return BadRequest(new { message = "Attribute not found" });
        
        switch (attribute.IsNumerical)
        {
            case true when !double.TryParse(add.Value, out _):
                return BadRequest(new { message = "Attribute value should be a number" });
            case false when double.TryParse(add.Value, out _):
                return BadRequest(new { message = "Attribute value should be alphanumeric" });
        }

        if(attribute.IsUnique && _context.Relations.FirstOrDefault(rel => rel.ArticleId == article.ArticleId && rel.AttributeId == attribute.AttributeId) != null)
            return BadRequest(new { message = "Cannot add another attribute marked as unique" });

        if (_context.Relations.FirstOrDefault(rel =>
                rel.ArticleId == article.ArticleId && rel.AttributeId == attribute.AttributeId &&
                rel.Value.ToLower().Equals(add.Value.ToLower())) != null)
            
            return BadRequest(new { message = "Attribute with same value already added to the article" });
        

        Relation relation = new()
        {
            ArticleId = article.ArticleId,
            AttributeId = attribute.AttributeId,
            Value = add.Value
        };

        _context.Relations.Add(relation);
        _context.SaveChanges();
        
        return Ok("Attribute " + add.AttributeName + " successfully added to " + add.ArticleName);
    }
    
    [HttpPost]
    [Route("deleteArticle")]
    public IActionResult DeleteArticle(string articleName)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        
        var article = _context.Articles.FirstOrDefault(art => art.Name == articleName);
        if(article == null)
            return BadRequest(new { message = "Article not found" });

        var relations = _context.Relations.Where(rel => rel.ArticleId == article.ArticleId);
        
        // Cascade delete
        foreach (var relation in relations)
            _context.Relations.Remove(relation);

        _context.Articles.Remove(article);
        _context.SaveChanges();
        
        return Ok(new {message = "Deleted article"});
    }
    [HttpPost]
    [Route("updateArticle")]
    public IActionResult UpdateArticle(UpdateArticle update)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        
        var article = _context.Articles.FirstOrDefault(art => art.ArticleId == update.ArticleId);
        if(article == null)
            return BadRequest(new { message = "Article not found" });

        if (update.Name != null)
        {
            if(!article.Name.Equals(update.Name) && _context.Articles.FirstOrDefault(art => art.Name.ToLower().Equals(update.Name.ToLower())) != null)
                return BadRequest(new { message = "New name already exists" });
            article.Name = update.Name;
        }

        if (update.Unit != null)
            article.Unit = update.Unit;
        if (update.Manufacturer != null)
            article.Manufacturer = update.Manufacturer;

        _context.SaveChanges();
        
        return Ok(new { message = "Update successful" });
    }
    [HttpPost]
    [Route("removeAttributeFromArticle")]
    public IActionResult DeleteAttribute(RemoveAttribute remove)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        
        var article = _context.Articles.FirstOrDefault(ar => ar.Name.Equals(remove.ArticleName));
        if (article == null)
            return BadRequest(new { message = "Article not found" });
        
        var attribute = _context.Attributes.FirstOrDefault(ar => ar.Name.Equals(remove.AttributeName));
        if (attribute == null)
            return BadRequest(new { message = "Attribute not found" });
        var relations = _context.Relations.Where(rel => rel.ArticleId == article.ArticleId && rel.AttributeId == attribute.AttributeId);
        if(!relations.Any())
            return BadRequest(new { message = "Attribute not associated with article" });

        if (remove.Value != null)
        {
            var relation = _context.Relations.FirstOrDefault(rel =>
                rel.ArticleId == article.ArticleId && rel.AttributeId == attribute.AttributeId &&
                rel.Value.Equals(remove.Value));
            if( relation == null)
                return BadRequest(new { message = "Attribute with given value not found" });
            
            _context.Relations.Remove(relation);
            _context.SaveChanges();
            return Ok(new {message = "Deleted attribute"}); 
        }
        
        foreach (var relation in relations)
            _context.Relations.Remove(relation);
        _context.SaveChanges();
        return Ok(new {message = "Deleted attributes"});
    }
    [HttpPost]
    [Route("updateAttribute")]
    public IActionResult UpdateAttribute(UpdateAttributes update)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        var article = _context.Articles.FirstOrDefault(ar => ar.Name.Equals(update.ArticleName));
        if (article == null)
            return BadRequest(new { message = "Article not found" });
        
        var attribute = _context.Attributes.FirstOrDefault(ar => ar.Name.Equals(update.AttributeName));
        if (attribute == null)
            return BadRequest(new { message = "Attribute not found" });
        
        switch (attribute.IsNumerical)
        {
            case true when !double.TryParse(update.NewValue, out _):
                return BadRequest(new { message = "Attribute value should be a number" });
            case false when double.TryParse(update.NewValue, out _):
                return BadRequest(new { message = "Attribute value should be alphanumeric" });
        }

        var relation = _context.Relations.FirstOrDefault(rel =>
            rel.ArticleId == article.ArticleId && rel.AttributeId == attribute.AttributeId &&
            rel.Value.Equals(update.OldValue));
        if(relation == null)
            return BadRequest(new { message = "Attribute with given value not found" });

        relation.Value = update.NewValue;
        _context.SaveChanges();
        
        return Ok(new { message = "Update successful" });
    }
    [HttpGet]
    [Route("getAllArticles")]
    public IActionResult GetAll(string? order)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        List<Article> articles;
        if(order == null)
            articles = _context.Articles.Where(art => true).ToList();
        else
            articles = order.ToLower() switch
            {
                "name" => _context.Articles.Where(att => true).OrderBy(art => art.Name).ToList(),
                "unit" => _context.Articles.Where(att => true).OrderBy(art => art.Unit).ToList(),
                "manufacturer" => _context.Articles.Where(att => true).OrderBy(art => art.Manufacturer).ToList(),
                _ => _context.Articles.Where(art => true).ToList()
            };

        List<ArticleOut> result = new();
        foreach (var article in articles)
        {
            ArticleOut res = new()
            {
                ArticleId = article.ArticleId,
                Name = article.Name,
                Unit = article.Unit,
                Manufacturer = article.Manufacturer,
                Attributes = new List<KeyValuePair<string, string>>()
            };
            var attributes = (from r in _context.Relations
                join at in _context.Attributes on r.AttributeId equals at.AttributeId
                join ar in _context.Articles on r.ArticleId equals ar.ArticleId
                where r.ArticleId == article.ArticleId
                select new
                {
                    AttributeName = at.Name, r.Value
                });
            foreach (var attribute in attributes)
            {
                res.Attributes.Add(new KeyValuePair<string, string>(attribute.AttributeName, attribute.Value));
            }
            
            result.Add(res);
        }

        return Ok(result);
    }
    [HttpGet]
    [Route("getWithAttribute")]
    public IActionResult ArticleWithAttribute(string attributeName, string? value)
    {
        if (!new CookieAuthorize(_config).ValidateCookie(Request))
            return Unauthorized();
        List<ArticleOut> result = new();
        if (value != null)
        {
            var articles = (from r in _context.Relations
                join at in _context.Attributes on r.AttributeId equals at.AttributeId
                join ar in _context.Articles on r.ArticleId equals ar.ArticleId
                where at.Name == attributeName && r.Value == value
                select new
                {
                    ar.ArticleId,
                    ar.Name,
                    ar.Unit,
                    ar.Manufacturer
                }).ToList();
            
            foreach (var article in articles)
            {
                ArticleOut res = new()
                {
                    ArticleId = article.ArticleId,
                    Name = article.Name,
                    Unit = article.Unit,
                    Manufacturer = article.Manufacturer,
                    Attributes = new List<KeyValuePair<string, string>>()
                };
                var attributes = (from r in _context.Relations
                    join at in _context.Attributes on r.AttributeId equals at.AttributeId
                    join ar in _context.Articles on r.ArticleId equals ar.ArticleId
                    where r.ArticleId == article.ArticleId
                    select new
                    {
                        AttributeName = at.Name, r.Value
                    });
                foreach (var attribute in attributes)
                {
                    res.Attributes.Add(new KeyValuePair<string, string>(attribute.AttributeName, attribute.Value));
                }
            
                result.Add(res);
            }
        }
        else
        {
            var articles = (from r in _context.Relations
                join at in _context.Attributes on r.AttributeId equals at.AttributeId
                join ar in _context.Articles on r.ArticleId equals ar.ArticleId
                where at.Name == attributeName  orderby r.Value  
                select new
                {
                    ar.ArticleId,
                    ar.Name,
                    ar.Unit,
                    ar.Manufacturer
                } into x group x by new {  x.ArticleId, x.Name, x.Unit, x.Manufacturer } into g 
                    select new
                    {
                        g.Key.ArticleId,
                        g.Key.Name,
                        g.Key.Unit,
                        g.Key.Manufacturer
                    }
                ).ToList();
            
            foreach (var article in articles)
            {
                ArticleOut res = new()
                {
                    ArticleId = article.ArticleId,
                    Name = article.Name,
                    Unit = article.Unit,
                    Manufacturer = article.Manufacturer,
                    Attributes = new List<KeyValuePair<string, string>>()
                };
                var attributes = (from r in _context.Relations
                    join at in _context.Attributes on r.AttributeId equals at.AttributeId
                    join ar in _context.Articles on r.ArticleId equals ar.ArticleId
                    where r.ArticleId == article.ArticleId
                    select new
                    {
                        AttributeName = at.Name, r.Value
                    });
                foreach (var attribute in attributes)
                {
                    res.Attributes.Add(new KeyValuePair<string, string>(attribute.AttributeName, attribute.Value));
                }
            
                result.Add(res);
            }
            
        }
        
        return Ok(result);
    }
}