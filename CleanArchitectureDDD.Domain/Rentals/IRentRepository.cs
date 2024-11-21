using CleanArchitectureDDD.Domain.Entities.Vehicles;

namespace CleanArchitectureDDD.Domain.Entities.Rentals;

public interface IRentRepository
{
    Task<Rent> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsOverLappingAsync(Vehicle vehicle, DateRange duration, CancellationToken cancellationToken = default);
    Task AddAsync(Rent rent);
}