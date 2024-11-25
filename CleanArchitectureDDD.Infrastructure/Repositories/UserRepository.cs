using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Infrastructure.Contexts;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal sealed class UserRepository: Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}