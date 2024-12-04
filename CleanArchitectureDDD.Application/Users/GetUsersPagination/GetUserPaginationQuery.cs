using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Users;

namespace CleanArchitectureDDD.Application.Users.GetUsersPagination;

public record GetUserPaginationQuery : PaginationParams, IQuery<PagedResults<User, UserId>>
{
    
}