namespace CleanArchitectureDDD.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    void AddAsync(User user);
    Task<bool> ExistUserByEmailAsync(Email email, CancellationToken cancellationToken = default);
}