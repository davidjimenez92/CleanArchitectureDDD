using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Application.Users.GetUserDapperPagination;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Users;

namespace CleanArchitectureDDD.Application.Users.GetUsersPagination;

public sealed record GetUserPaginationQuery : PaginationParams, IQuery<PagedResults<User, UserId>>, IQuery<PagedDapperResults<UserPaginationData>>
{
    
}