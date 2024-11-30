namespace CleanArchitectureDDD.Application.Users.RegisterUser;

public record RegisterUserRequest(string Email, string Name, string Surname, string Password);