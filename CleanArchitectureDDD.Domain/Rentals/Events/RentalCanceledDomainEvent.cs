using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals.Events;

public sealed record RentalCanceledDomainEvent(RentalId Id): IDomainEvent;