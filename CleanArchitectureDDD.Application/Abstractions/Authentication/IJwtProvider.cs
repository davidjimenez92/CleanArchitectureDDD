using System.Security.Claims;
using CleanArchitectureDDD.Domain.Users;

namespace CleanArchitectureDDD.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<string> GenerateEncodedToken(User user, CancellationToken cancellationToken = default);
}