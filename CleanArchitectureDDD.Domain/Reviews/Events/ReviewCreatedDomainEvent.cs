using CleanArchitectureDDD.Domain.Entities.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Reviews.Events;

public record ReviewCreatedDomainEvent(Guid Id): IDomainEvent;