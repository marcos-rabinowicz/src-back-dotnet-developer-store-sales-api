using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Implementation of JWT (JSON Web Token) generator.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(IUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var secret = _configuration["Jwt:SecretKey"];
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("Configuration 'Jwt:SecretKey' is missing or empty.");

        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var signingKey = new SymmetricSecurityKey(keyBytes);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
