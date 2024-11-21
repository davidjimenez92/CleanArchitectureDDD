using CleanArchitectureDDD.Domain.Shared;

namespace CleanArchitectureDDD.Domain.Rentals;

public record PriceDetail(Currency pricePerPeriod, Currency maintenance, Currency priceWithAccessories, Currency totalPrice);