using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Domain.Shared;

namespace CleanArchitectureDDD.Domain.Rentals;

public class PriceService
{
    public PriceDetail CalculatePrice(Vehicle vehicle, DateRange period)
    {
        var currencyType = vehicle.Price!.CurrencyType;
        var pricePerPeriod = new Currency(period.TotalDays * vehicle.Price.Price, currencyType);
        decimal percentageChage = 0;
        vehicle.Accessories.ForEach(a =>
        {
            percentageChage += a switch
            {
                Accessory.AndroidAuto or Accessory.AppleCarPlay => 0.05m,
                Accessory.Ac => 0.01m,
                Accessory.Gps => 0.01m,
                Accessory.Wifi => 0.02m,
                _ => 0
            };
        });
        var accessoriesCharge = Currency.Zero(currencyType);
        if (percentageChage > 0)
        {
            accessoriesCharge = new Currency(pricePerPeriod.Price * percentageChage, currencyType);
        }
        var totalPrice = new Currency(pricePerPeriod.Price + accessoriesCharge.Price + vehicle.Maintenance!.Price, currencyType);
        
        return new PriceDetail(pricePerPeriod, vehicle.Maintenance, accessoriesCharge, totalPrice);
    }
}