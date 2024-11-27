namespace CleanArchitectureDDD.Application.Vehicles.SearchVehicles;

public class VehicleResponse
{
    public Guid Id { get; init; }
    public string? Model { get; init; }
    public string? Vin { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public AddressResponse? Address { get; set; }
}