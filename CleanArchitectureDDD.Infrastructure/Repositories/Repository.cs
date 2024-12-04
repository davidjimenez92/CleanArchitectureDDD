using System.Linq.Expressions;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Infrastructure.Contexts;
using CleanArchitectureDDD.Infrastructure.Extensions;
using CleanArchitectureDDD.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
    
    public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<int> CountAsync(
        ISpecification<TEntity, TEntityId> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }
    public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TEntityId> specification)
    {
        return SpecificationEvaluator<TEntity, TEntityId>
            .GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification);
    }

    public async Task<PagedResults<TEntity, TEntityId>> GetPaginationAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity,  object>>? includes,
        int page,
        int pageSize,
        string orderBy,
        bool ascending,
        bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (disableTracking) query = query.AsNoTracking();
        if(predicate is not null) query = query.Where(predicate);
        if(includes is not null) query = includes(query);
        
        var skipAmount = pageSize * (page - 1);
        var totalNumberOfRecords = await query.CountAsync(cancellationToken);
        var records = new List<TEntity>();

        if (string.IsNullOrWhiteSpace(orderBy))
        {
            records = await query.Skip(skipAmount).Take(pageSize).ToListAsync(cancellationToken);
        }
        else
        {
            records = await query
                .OrderByPropertyOrField(orderBy, ascending)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        var mod = totalNumberOfRecords % pageSize;
        var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

        return new PagedResults<TEntity, TEntityId>()
        {
            PageNumber = page,
            PageSize = pageSize,
            TotalNumberOfPages = totalPageCount,
            TotalNUmberOfRecords = totalNumberOfRecords,
            Records = records
        };
    }
}