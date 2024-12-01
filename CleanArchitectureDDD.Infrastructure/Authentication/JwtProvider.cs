using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitectureDDD.Application.Abstractions.Authentication;
using CleanArchitectureDDD.Application.Abstractions.Data;
using CleanArchitectureDDD.Domain.Users;
using Dapper;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitectureDDD.Infrastructure.Authentication;

public sealed class JwtProvider: IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JwtProvider(IOptions<JwtOptions> options, ISqlConnectionFactory sqlConnectionFactory)
    {
        _jwtOptions = options.Value;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<string> GenerateEncodedToken(User user, CancellationToken cancellationToken = default)
    {
        const string sql = """
                           SELECT ps.name
                           FROM users usr
                           LEFT JOIN user_role usrl ON usr.id = usrl.user_id
                           LEFT JOIN roles ro ON usrl.role_id = ro.id
                           LEFT JOIN role_permissions ropl ON ro.id = ropl.role_id
                           LEFT JOIN permissions ps ON ropl.permission_id = ps.id
                           WHERE usr.id=@UserId
                           """;
        using var connection = _sqlConnectionFactory.CreateConnection();
        var permissions = await connection.QueryAsync<string>(sql, new { UserId = user.Id!.Value} );
        var permissionCollection = permissions.ToHashSet();
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.Value)
        };

        foreach (var permission in permissionCollection)
        {
            claims.Add(new(CustomClaims.Permissions, permission));
        }

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

        return tokenValue;
    }
}