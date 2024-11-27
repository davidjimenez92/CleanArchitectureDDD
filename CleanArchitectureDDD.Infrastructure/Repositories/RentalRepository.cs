using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal sealed class RentalRepository: Repository<Rental, RentalId>, IRentalRepository
{
    private static readonly RentalStatus[] ActiveRentStatuses = { RentalStatus.Reserved, RentalStatus.Confirmed, RentalStatus.Completed };
    
    public RentalRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsOverLappingAsync(Vehicle vehicle, DateRange duration, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Rental>()
            .AnyAsync(
                rent => rent.VehicleId == vehicle.Id &&
                        rent.Duration!.StartDate <= duration.EndDate &&
                        rent.Duration.EndDate >= duration.StartDate &&
                        ActiveRentStatuses.Contains(rent.Status),
                cancellationToken
            );
    }
}