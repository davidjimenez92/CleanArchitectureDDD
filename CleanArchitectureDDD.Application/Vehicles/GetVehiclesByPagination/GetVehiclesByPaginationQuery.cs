using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Application.Vehicles.GetVehiclesByPagination;

public sealed record GetVehiclesByPaginationQuery: SpecificationEntry, IQuery<PaginationResult<Vehicle, VehicleId>>
{
    public string? Model { get; init; }
}