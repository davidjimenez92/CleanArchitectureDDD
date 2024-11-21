using CleanArchitectureDDD.Domain.Entities.Abstractions;
using CleanArchitectureDDD.Domain.Entities.Users.Events;

namespace CleanArchitectureDDD.Domain.Entities.Users;

public sealed class User: Entity
{
    private User(Guid id, Name? name, LastName? lastName, Email? email) : base(id)
    {
        Name = name;
        LastName = lastName;
        Email = email;
    }

    public Name? Name { get; private set; }
    public LastName? LastName { get; private set; }
    public Email? Email { get; private set; }
    
    public static User Create(Name name, LastName lastName, Email email)
    {
        var user = new User(Guid.NewGuid(), name, lastName, email);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
}