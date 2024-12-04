using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Application.Paginations;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Application.Users.GetUsersPagination;

internal sealed class GetUserPaginationQueryHandler: IQueryHandler<GetUserPaginationQuery, PagedResults<User, UserId>>
{
    private readonly IPaginationRepository _paginationRepository;

    public GetUserPaginationQueryHandler(IPaginationRepository paginationRepository)
    {
        _paginationRepository = paginationRepository;
    }

    public async Task<Result<PagedResults<User, UserId>>> Handle(GetUserPaginationQuery request, CancellationToken cancellationToken)
    {
        var predicateb = PredicateBuilder.New<User>(true);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            predicateb = predicateb.Or(p => p.Name == new Name(request.Search));
            predicateb = predicateb.Or(p => p.Email == new Email(request.Search));
        }

        var users = await _paginationRepository.GetPaginationAsync(predicateb,
            p => p.Include(x => x.Roles!).ThenInclude(y => y.Permissions!),
            request.PageNumber,
            request.PageSize,
            request.OrderBy!,
            request.OrderAsc);

        return users;
    }
}