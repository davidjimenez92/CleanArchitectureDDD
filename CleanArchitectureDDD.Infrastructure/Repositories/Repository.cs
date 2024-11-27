using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal abstract class Repository<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    protected readonly ApplicationDbContext _dbContext;
    
    protected Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
    
    public void AddAsync(TEntity entity)
    {
        _dbContext.AddAsync(entity);
    }
}