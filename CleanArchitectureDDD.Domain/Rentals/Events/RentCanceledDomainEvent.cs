using CleanArchitectureDDD.Domain.Entities.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Rentals.Events;

public sealed record RentCanceledDomainEvent(Guid id): IDomainEvent;