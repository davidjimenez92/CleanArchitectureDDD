using CleanArchitectureDDD.Domain.Entities.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Rentals.Events;

public sealed record RentRejectedDomainEvent(Guid id): IDomainEvent;