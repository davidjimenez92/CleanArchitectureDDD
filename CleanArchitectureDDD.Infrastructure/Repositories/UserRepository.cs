using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal sealed class UserRepository: Repository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }
}