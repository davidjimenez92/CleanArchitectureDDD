using CleanArchitectureDDD.Application.Abstractions.Messaging;

namespace CleanArchitectureDDD.Application.Vehicles.SearchVehicles;

public sealed record SearchVehiclesQuery(DateOnly StartDate, DateOnly EndDate): IQuery<IReadOnlyList<VehicleResponse>>;