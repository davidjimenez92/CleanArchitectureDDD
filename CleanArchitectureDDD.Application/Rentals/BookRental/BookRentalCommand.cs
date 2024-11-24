using CleanArchitectureDDD.Application.Abstractions.Messaging;

namespace CleanArchitectureDDD.Application.Rentals.BookRental;

public record BookRentalCommand(
    Guid VehicleId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate
    ): ICommand<Guid>, ICommand;