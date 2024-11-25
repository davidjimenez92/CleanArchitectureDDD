using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal abstract class Repository<T>
where T : Entity
{
    protected readonly ApplicationDbContext _dbContext;
    
    protected Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
    
    public void AddAsync(T entity)
    {
        _dbContext.AddAsync(entity);
    }
}