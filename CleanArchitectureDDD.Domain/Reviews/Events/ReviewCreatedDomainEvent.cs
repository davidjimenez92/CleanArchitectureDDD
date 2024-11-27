using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals;

namespace CleanArchitectureDDD.Domain.Reviews.Events;

public record ReviewCreatedDomainEvent(ReviewId Id): IDomainEvent;