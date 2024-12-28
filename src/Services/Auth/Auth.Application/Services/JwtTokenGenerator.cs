using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.DTOs.Auth.Responses;
using Auth.Application.Helpers;
using Auth.Application.Services.IServices;
using Auth.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Services;

public class JwtTokenGenerator(IOptions<JwtOptionsSetting> options) : IJwtTokenGenerator
{
    private readonly JwtOptionsSetting _jwt = options.Value;


    public string GeneratorToken(ApplicationUser user, IEnumerable<string>? roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwt.Secret);


        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim("FullName", user.FullName)
        };
        if (roles is not null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role
            )));    
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwt.Audience,
            Issuer = _jwt.Issuer,
            Expires = DateTime.UtcNow.AddHours(7),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwt.Secret);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwt.Issuer,
            ValidAudience = _jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

   

    public UserDto DecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (!tokenHandler.CanReadToken(token))
        {
            throw new SecurityTokenException("Invalid token format");
        }

        var key = Encoding.UTF8.GetBytes(_jwt.Secret);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwt.Issuer,
            ValidAudience = _jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

        var userId = principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var userName = principal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        var fullName = principal.Claims.First(x => x.Type == "FullName").Value;

        return new UserDto(userId, userName, fullName);
    }


    public string GeneratorTokenByUserId(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwt.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwt.Audience,
            Issuer = _jwt.Issuer,
            Expires = DateTime.UtcNow.AddHours(9),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GeneratorRefreshToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(userId);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}