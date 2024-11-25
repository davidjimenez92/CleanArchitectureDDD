using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal sealed class RentRepository: Repository<Rent>, IRentRepository
{
    private static readonly RentStatus[] ActiveRentStatuses = { RentStatus.Reserved, RentStatus.Confirmed, RentStatus.Completed };
    
    public RentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsOverLappingAsync(Vehicle vehicle, DateRange duration, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Rent>()
            .AnyAsync(
                rent => rent.VehicleId == vehicle.Id &&
                        rent.Duration!.StartDate <= duration.EndDate &&
                        rent.Duration.EndDate >= duration.StartDate &&
                        ActiveRentStatuses.Contains(rent.Status),
                cancellationToken
            );
    }
}