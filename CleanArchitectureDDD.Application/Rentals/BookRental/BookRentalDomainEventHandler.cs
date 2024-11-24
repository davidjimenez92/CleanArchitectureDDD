using CleanArchitectureDDD.Application.Abstractions.Email;
using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Rentals.Events;
using CleanArchitectureDDD.Domain.Users;
using MediatR;

namespace CleanArchitectureDDD.Application.Rentals.BookRental;

internal sealed class BookRentalDomainEventHandler : INotificationHandler<RentCreatedDomainEvent>
{
    private readonly IRentRepository _rentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    
    public BookRentalDomainEventHandler(IRentRepository rentRepository, IUserRepository userRepository, IEmailService emailService)
    {
        _rentRepository = rentRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    
    public async Task Handle(RentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var rent = await _rentRepository.GetByIdAsync(notification.Id, cancellationToken);
        if (rent is null)
        {
            return;
        }
        
        var user = await _userRepository.GetByIdAsync(rent.UserId, cancellationToken);
        if (user is null)
        {
            return;
        }
        
        await _emailService.SendEmailAsync(user.Email, "Rent created", $"Your rent with id {rent.Id} has been created");
    }
}