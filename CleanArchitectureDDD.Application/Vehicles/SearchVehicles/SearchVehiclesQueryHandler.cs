using CleanArchitectureDDD.Application.Abstractions.Data;
using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Vehicles;
using Dapper;

namespace CleanArchitectureDDD.Application.Vehicles.SearchVehicles;

internal sealed class SearchVehiclesQueryHandler: IQueryHandler<SearchVehiclesQuery, IReadOnlyList<VehicleResponse>>
{
    private static readonly int[] ActiveRentalStatuses = { (int)RentStatus.Confirmed, (int)RentStatus.Reserved, (int) RentStatus.Completed };
    
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public SearchVehiclesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<IReadOnlyList<VehicleResponse>>> Handle(SearchVehiclesQuery request, CancellationToken cancellationToken)
    {
        if(request.StartDate > request.EndDate)
        {
            return new List<VehicleResponse>();
        }
        
        using var connection = _sqlConnectionFactory.CreateConnection();
        var sqlQuery = """
                        SELECT 
                             a.id AS Id,
                             a.model AS Model,
                             a.vin AS Vin,
                             a.price AS Price,
                             a.currency AS Currency,
                             a.address_street AS Street,
                             a.address_city AS City,
                             a.address_zip_code AS ZipCode,
                             a.address_country AS Country,
                        FROM vehicles as a
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM rentals as b
                            WHERE b.vehicle_id = a.id
                            AND b.start_date <= @EndDate
                            AND b.end_date >= @StartDate
                            AND b.status = ANY(@ActiveRentalStatuses)
                        )
                        """;

        var vehicles = await connection.QueryAsync<VehicleResponse, AddressResponse, VehicleResponse>(
            sqlQuery,
            (vehicle, address) =>
            {
                vehicle.Address = address;
                return vehicle;
            },
            new
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ActiveRentalStatuses = ActiveRentalStatuses
            },
            splitOn: "Street"
        );
        
        return vehicles.ToList();
    }
}