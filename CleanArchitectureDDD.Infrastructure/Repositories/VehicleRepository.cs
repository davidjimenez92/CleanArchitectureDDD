using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Infrastructure.Contexts;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal sealed class VehicleRepository: Repository<Vehicle, VehicleId>, IVehicleRepository
{
    public VehicleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}