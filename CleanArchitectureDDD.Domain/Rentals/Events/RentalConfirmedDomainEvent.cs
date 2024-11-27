using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals.Events;

public sealed record RentalConfirmedDomainEvent(RentalId Id) : IDomainEvent
{
    
}