namespace CleanArchitectureDDD.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    void AddAsync(User user);
}