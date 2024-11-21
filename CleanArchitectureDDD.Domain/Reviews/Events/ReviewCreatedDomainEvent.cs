using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Reviews.Events;

public record ReviewCreatedDomainEvent(Guid Id): IDomainEvent;