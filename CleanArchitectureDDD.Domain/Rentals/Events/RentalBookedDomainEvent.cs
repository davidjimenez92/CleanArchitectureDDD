using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals.Events;

public sealed record RentalBookedDomainEvent(RentalId Id): IDomainEvent;