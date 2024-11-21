using MediatR;

namespace CleanArchitectureDDD.Domain.Events;

public interface IDomainEvent :INotification
{
    public void Add<T>(T domainEvent) where T : IDomainEvent;
    public void Remove<T>(T domainEvent) where T : IDomainEvent;
    public IEnumerable<T> Get<T>() where T : IDomainEvent;
    public void Clear();
    
}