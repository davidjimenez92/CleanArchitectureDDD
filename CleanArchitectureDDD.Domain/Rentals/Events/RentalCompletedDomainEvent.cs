using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals.Events;

public sealed record RentalCompletedDomainEvent(RentalId Id): IDomainEvent;