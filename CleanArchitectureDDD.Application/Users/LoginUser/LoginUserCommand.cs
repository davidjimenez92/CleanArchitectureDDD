using CleanArchitectureDDD.Application.Abstractions.Messaging;

namespace CleanArchitectureDDD.Application.Users.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand<string>;
