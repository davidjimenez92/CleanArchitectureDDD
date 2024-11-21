using CleanArchitectureDDD.Domain.Entities.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Rentals.Events;

public sealed record RentCreatedDomainEvent(Guid Id): IDomainEvent;