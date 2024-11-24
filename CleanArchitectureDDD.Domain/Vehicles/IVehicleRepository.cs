namespace CleanArchitectureDDD.Domain.Vehicles;

public interface IVehicleRepository
{
    Task<Vehicle> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    //Task<IEnumerable<Vehicle>> GetAllAsync();
    //Task AddAsync(Vehicle vehicle);
    //Task UpdateAsync(Vehicle vehicle);
    //Task DeleteAsync(Vehicle vehicle);
}