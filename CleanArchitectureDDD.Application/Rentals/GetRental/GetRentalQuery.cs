using CleanArchitectureDDD.Application.Abstractions.Messaging;

namespace CleanArchitectureDDD.Application.Rentals.GetRental;

public sealed record GetRentalQuery(Guid Id) : IQuery<RentalResponse>;
