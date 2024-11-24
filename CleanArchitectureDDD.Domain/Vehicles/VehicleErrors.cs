using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Vehicles;

public static class VehicleErrors
{
    public static Error NotFound => new Error("Vehicle.NotFound", "Vehicle not found");
}