using System.Linq.Expressions;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitectureDDD.Application.Paginations;

public interface IPaginationRepository
{
    Task<PagedResults<User, UserId>> GetPaginationAsync(Expression<Func<User, bool>>? predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>> includes,
        int page,
        int pageSize,
        string orderBy,
        bool ascending,
        bool disableTracking = true,
        CancellationToken cancellationToken = default);
}