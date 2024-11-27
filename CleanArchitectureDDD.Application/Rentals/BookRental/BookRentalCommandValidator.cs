using FluentValidation;

namespace CleanArchitectureDDD.Application.Rentals.BookRental;

public class BookRentalCommandValidator: AbstractValidator<BookRentalCommand>
{
    public BookRentalCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.VehicleId).NotEmpty();
        RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
    }
}