using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Vehicles;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(VehicleId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vehicle>> GetAllWithSpecificationAsync(ISpecification<Vehicle, VehicleId> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<Vehicle, VehicleId> specification, CancellationToken cancellationToken = default);
}