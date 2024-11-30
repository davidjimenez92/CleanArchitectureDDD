using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitectureDDD.Application.Abstractions.Authentication;
using CleanArchitectureDDD.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitectureDDD.Infrastructure.Authentication;

public sealed class JwtProvider: IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public Task<string> GenerateEncodedToken(User user, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.Value)
        };

        var signIngCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret!)),
            SecurityAlgorithms.HmacSha256Signature
            );
        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddDays(365),
            signIngCredentials
        );
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenValue);
    }
}