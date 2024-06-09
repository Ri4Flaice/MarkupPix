using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using MarkupPix.Data.Entities;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MarkupPix.Business.Infrastructure;

/// <summary>
/// JWT provider.
/// </summary>
public class JwtProvider
{
    private readonly JwtOptions _options;

    /// <summary>
    /// Initializes a new instance of the class <see cref="JwtProvider"/>.
    /// </summary>
    /// <param name="options">JWT options.</param>
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Generate a new token.
    /// </summary>
    /// <param name="user">User, which generates a new token.</param>
    /// <returns>Token.</returns>
    public string GenerateToken(UserEntity user)
    {
        Claim[] claims =
            [new Claim("emailAddress", user.EmailAddress ?? throw new Exception("User email address empty."))];

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey ?? throw new Exception("Secret key empty."))),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}