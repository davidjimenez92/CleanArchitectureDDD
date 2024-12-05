using CleanArchitectureDDD.Application.Abstractions.Email;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Users.Events;
using MediatR;

namespace CleanArchitectureDDD.Application.Users.RegisterUser;

internal sealed class UserCreateDomainEventHandler: INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public UserCreateDomainEventHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }
        
        await _emailService.SendEmailAsync(user.Email!, "New User", "New User in CleanArchitectureDDD");
    }
}