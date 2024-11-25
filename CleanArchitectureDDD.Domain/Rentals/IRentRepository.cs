using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Domain.Rentals;

public interface IRentRepository
{
    Task<Rent> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsOverLappingAsync(Vehicle vehicle, DateRange duration, CancellationToken cancellationToken = default);
    void AddAsync(Rent rent);
}