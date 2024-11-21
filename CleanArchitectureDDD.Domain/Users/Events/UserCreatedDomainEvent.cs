
using CleanArchitectureDDD.Domain.Entities.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId): IDomainEvent
{
    
}