namespace CleanArchitectureDDD.Domain.Users;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void AddAsync(User user);
}