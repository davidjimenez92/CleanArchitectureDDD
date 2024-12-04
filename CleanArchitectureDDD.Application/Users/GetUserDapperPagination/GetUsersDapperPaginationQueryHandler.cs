using System.Text;
using CleanArchitectureDDD.Application.Abstractions.Data;
using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Application.Users.GetUsersPagination;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users;
using Dapper;

namespace CleanArchitectureDDD.Application.Users.GetUserDapperPagination;

internal sealed class GetUsersDapperPaginationQueryHandler: IQueryHandler<GetUsersDapperPaginationQuery, PagedDapperResults<UserPaginationData>>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetUsersDapperPaginationQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<PagedDapperResults<UserPaginationData>>> Handle(GetUsersDapperPaginationQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        var builder = new StringBuilder("""
            SELECT 
              usr.email, rl.name as role, pe.name as permission
            FROM users usr
                LEFT JOIN user_role usrl 
                    ON usrl.user_id = usr.id
                LEFT JOIN roles rl 
                    ON rl.id = usrl.role_id
                LEFT JOIN role_permissions rlpe 
                    ON rl.id = rlpe.role_id
                LEFT JOIN permissions pe
                    ON pe.id = rlpe.permission_id
           """);
        var search = string.Empty;
        var whereStatement = string.Empty;
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            search = "%" + EncodeForLike(request.Search) + "%";
            whereStatement = " WHERE rl.name LIKE @Search ";
            builder.AppendLine(whereStatement);
        }
        
        var orderBy = request.OrderBy;
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            var orderStatement = string.Empty;
            var orderAsc = request.OrderAsc? "ASC" : "DESC";
            switch (orderBy)
            {
                case "email": orderStatement = $" ORDER BY usr.email {orderAsc}"; break;
                case "role": orderStatement = $" ORDER BY rl.name {orderAsc}"; break;
                default: orderStatement = $" ORDER BY pe.name {orderAsc}"; break;
            }
            builder.AppendLine(orderStatement);
        }
        builder.AppendLine(" LIMIT @PageSize OFFSET @Offset;");

        builder.AppendLine("""
                           SELECT 
                             COUNT(*)
                           FROM users usr
                               LEFT JOIN user_role usrl 
                                   ON usrl.user_id = usr.id
                               LEFT JOIN roles rl 
                                   ON rl.id = usrl.role_id
                               LEFT JOIN role_permissions rlpe 
                                   ON rl.id = rlpe.role_id
                               LEFT JOIN permissions pe
                                   ON pe.id = rlpe.permission_id
                           """);
        builder.AppendLine(whereStatement);
        builder.AppendLine(";");

        var offset = request.PageSize * (request.PageIndex - 1);
        using var multi = await connection.QueryMultipleAsync(builder.ToString(), 
            new
            {
                PageSize = request.PageSize,
                Offset = offset,
                Search = search,
            });

        var items = await multi.ReadAsync<UserPaginationData>().ConfigureAwait(false);
        var totalItems = await multi.ReadFirstAsync<int>().ConfigureAwait(false);

        var result = new PagedDapperResults<UserPaginationData>(totalItems, request.PageIndex, request.PageSize)
        {
            Items = items
        };

        return result;
    }

    private string EncodeForLike(string search)
    {
        return search.Replace("[", "[]]").Replace("%", "[%]");
    }
}