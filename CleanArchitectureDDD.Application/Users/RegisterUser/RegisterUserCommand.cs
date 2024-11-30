using CleanArchitectureDDD.Application.Abstractions.Messaging;

namespace CleanArchitectureDDD.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Name, string Surname, string Password):ICommand<Guid>;