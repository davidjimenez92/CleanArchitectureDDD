using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Users.Events;

namespace CleanArchitectureDDD.Domain.Users;

public sealed class User: Entity<UserId>
{
    private User()
    {
    }
    private User(UserId id, Name? name, LastName? lastName, Email? email) : base(id)
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
        var user = new User(UserId.New(), name, lastName, email);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
}