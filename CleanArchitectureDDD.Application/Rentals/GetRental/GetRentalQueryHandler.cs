using CleanArchitectureDDD.Application.Abstractions.Data;
using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using Dapper;

namespace CleanArchitectureDDD.Application.Rentals.GetRental;

internal sealed class GetRentalQueryHandler : IQueryHandler<GetRentalQuery, RentalResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetRentalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<RentalResponse>> Handle(GetRentalQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        var sqlQuery = """
                       SELECT 
                            id AS Id,
                            vehicle_id AS VehicleId,
                            user_id AS UserId,
                            status AS Status,
                            rental_price AS RentalPrice,
                            rental_currency AS RentalCurrency,
                            maintenance_price AS MaintenancePrice,
                            maintenance_currency AS MaintenanceCurrency,
                            accessory_price AS AccessoryPrice,
                            accessory_currency AS AccessoryCurrency,
                            total_price AS TotalPrice,
                            total_currency AS TotalCurrency,
                            start_date AS StartDate,
                            end_date AS EndDate,
                            date_created AS DateCreated
                       FROM rentals
                       WHERE id = @Id
                       """;

        var rent = await connection.QueryFirstOrDefaultAsync<RentalResponse>(
            sqlQuery,
            new { request.Id });

        return rent!;
    }
}