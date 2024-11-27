using CleanArchitectureDDD.Application.Abstractions.Email;
using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Rentals.Events;
using CleanArchitectureDDD.Domain.Users;
using MediatR;

namespace CleanArchitectureDDD.Application.Rentals.BookRental;

internal sealed class BookRentalDomainEventHandler : INotificationHandler<RentalBookedDomainEvent>
{
    private readonly IRentalRepository _iRentalRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    
    public BookRentalDomainEventHandler(IRentalRepository iRentalRepository, IUserRepository userRepository, IEmailService emailService)
    {
        _iRentalRepository = iRentalRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    
    public async Task Handle(RentalBookedDomainEvent notification, CancellationToken cancellationToken)
    {
        var rent = await _iRentalRepository.GetByIdAsync(notification.Id, cancellationToken);
        if (rent is null)
        {
            return;
        }
        
        var user = await _userRepository.GetByIdAsync(rent.UserId!, cancellationToken);
        if (user is null)
        {
            return;
        }
        
        await _emailService.SendEmailAsync(user.Email!, "Rent created", $"Your rent with id {rent.Id} has been created");
    }
}