using CleanArchitectureDDD.Domain.Entities.Abstractions;
using CleanArchitectureDDD.Domain.Entities.Shared;
using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Domain.Entities.Vehicles;

public sealed class Vehicle: Entity
{
    private Vehicle(Guid id, Model? model, Vin? vin, Address? address, Currency? price, Currency? maintenance, DateTime? lastRent, List<Accessory> accessories) : base(id)
    {
        Model = model;
        Vin = vin;
        Address = address;
        Price = price;
        Maintenance = maintenance;
        LastRent = lastRent;
        Accessories = accessories;
    }   

    public Model? Model { get; private set; }
    public Vin? Vin { get; private set; }
    public Address? Address { get; private set; }
    public Currency? Price { get; private set; }
    public Currency? Maintenance { get; private set; }
    public DateTime? LastRent { get; internal set; }
    public List<Accessory> Accessories { get; private set; } = new();
    
    public static Vehicle Create(Model model, Vin vin, Address address, Currency price, Currency maintenance, DateTime lastRent, List<Accessory> accessories)
    {
        return new Vehicle(Guid.NewGuid(), model, vin, address, price, maintenance, lastRent, accessories);
    }
}