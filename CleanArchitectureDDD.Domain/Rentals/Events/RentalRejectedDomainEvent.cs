using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals.Events;

public sealed record RentalRejectedDomainEvent(RentalId Id): IDomainEvent;