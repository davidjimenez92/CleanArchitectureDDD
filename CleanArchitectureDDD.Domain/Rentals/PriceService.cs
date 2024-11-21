using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Domain.Rentals;

public class PriceService
{
    public PriceDetail CalculatePrice(Vehicle vehicle, DateRange period)
    {
        var currencyType = vehicle.Price!.currencyType;
        var pricePerPeriod = new Currency(period.TotalDays * vehicle.Price.price, currencyType);
        decimal percentageChage = 0;
        vehicle.Accessories.ForEach(a =>
        {
            percentageChage += a switch
            {
                Accessory.AndroidAuto or Accessory.AppleCarPlay => 0.05m,
                Accessory.AC => 0.01m,
                Accessory.GPS => 0.01m,
                _ => 0
            };
        });
        var priceWithAccessories = new Currency(pricePerPeriod.price * percentageChage, currencyType);
        var totalPrice = new Currency(pricePerPeriod.price + priceWithAccessories.price + vehicle.Maintenance.price, currencyType);
        
        return new PriceDetail(pricePerPeriod, vehicle.Maintenance, priceWithAccessories, totalPrice);
    }
}