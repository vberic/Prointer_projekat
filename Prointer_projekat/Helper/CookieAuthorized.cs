

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Prointer_projekat.Helper;


public class CookieAuthorize
{
  private readonly IConfiguration _config;
  public CookieAuthorize(IConfiguration config)
  {
    _config = config;
  }

  public bool ValidateCookie(HttpRequest httpRequest)
  {
    string cookie = httpRequest.Cookies["jwt"] ?? string.Empty;

    if (cookie.IsNullOrEmpty())
      return false;

    return JwtCheck(cookie);
  }

  private bool JwtCheck(string authToken)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var validationParameters = new TokenValidationParameters()
    {
      ValidateLifetime = true, 
      ValidateAudience = true, 
      ValidateIssuer = true,   
      ValidIssuer = _config["Jwt:Issuer"],
      ValidAudience = _config["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
    };

    SecurityToken validatedToken;
    try
    {
      tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
    }
    catch (Exception)
    {
      return false;
    }
    
    return true;
  }
}