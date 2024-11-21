using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals.Events;

public sealed record RentConfirmedDomainEvent(Guid id) : IDomainEvent
{
    
}