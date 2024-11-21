using CleanArchitectureDDD.Domain.Entities.Shared;

namespace CleanArchitectureDDD.Domain.Entities.Rentals;

public record PriceDetail(Currency pricePerPeriod, Currency maintenance, Currency priceWithAccessories, Currency totalPrice);