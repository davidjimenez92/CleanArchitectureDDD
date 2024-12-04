using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Shared;

namespace CleanArchitectureDDD.Application.Users.GetUserDapperPagination;

public sealed record GetUsersDapperPaginationQuery: PaginationParams, IQuery<PagedDapperResults<UserPaginationData>>;