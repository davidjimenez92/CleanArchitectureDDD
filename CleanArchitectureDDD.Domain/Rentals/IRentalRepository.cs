using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Domain.Rentals;

public interface IRentalRepository
{
    Task<Rental?> GetByIdAsync(RentalId id, CancellationToken cancellationToken = default);
    Task<bool> IsOverLappingAsync(Vehicle vehicle, DateRange duration, CancellationToken cancellationToken = default);
    void AddAsync(Rental rental);
}