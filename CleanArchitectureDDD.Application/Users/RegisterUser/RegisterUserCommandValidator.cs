using FluentValidation;

namespace CleanArchitectureDDD.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).NotEmpty().MinimumLength(5);
        RuleFor(u => u.Name).NotEmpty();
        RuleFor(u => u.Surname).NotEmpty();
    }
}