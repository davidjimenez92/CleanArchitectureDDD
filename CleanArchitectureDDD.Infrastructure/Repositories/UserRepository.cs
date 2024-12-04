using System.Linq.Expressions;
using CleanArchitectureDDD.Application.Paginations;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal sealed class UserRepository
    : Repository<User, UserId>, IUserRepository, IPaginationRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<bool> ExistUserByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>().AnyAsync(user => user.Email == email, cancellationToken);
    }
}