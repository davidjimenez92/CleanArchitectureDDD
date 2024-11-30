using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Users;

public static class UserErrors
{
    public static Error NotFound => new Error("User.Found", "User not found");
    public static Error InvalidCredentials => new Error("User.InvalidCredentials", "Invalid credentials");
    public static Error AlreadyExists => new Error("User.AlreadyExists", "User already exists");
}