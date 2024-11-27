
using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(UserId UserId): IDomainEvent
{
    
}