using CleanArchitectureDDD.Domain.Shared;

namespace CleanArchitectureDDD.Domain.Rentals;

public record PriceDetail(Currency PricePerPeriod, Currency Maintenance, Currency AccessoriesCharge, Currency TotalPrice);