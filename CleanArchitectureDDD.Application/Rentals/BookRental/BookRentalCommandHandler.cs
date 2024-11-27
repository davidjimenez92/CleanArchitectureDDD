using CleanArchitectureDDD.Application.Abstractions.Clock;
using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Application.Exceptions;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Application.Rentals.BookRental;

internal sealed class BookRentalCommandHandler: ICommandHandler<BookRentalCommand, Guid>
{
    
    private readonly IUserRepository _userRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IRentRepository _rentRepository;
    private readonly PriceService _priceService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public BookRentalCommandHandler(IUserRepository userRepository, IVehicleRepository vehicleRepository, IRentRepository rentRepository, PriceService priceService, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _vehicleRepository = vehicleRepository;
        _rentRepository = rentRepository;
        _priceService = priceService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    
    public async Task<Result<Guid>> Handle(BookRentalCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null)
        {
            return Result.Failure<Guid>(VehicleErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);
        
        if(await _rentRepository.IsOverLappingAsync(vehicle, duration, cancellationToken))
        {
            return Result.Failure<Guid>(RentErrors.Overlap);
        }

        try
        {
            var rent = Rent.Create(vehicle, user.Id, duration, _priceService, _dateTimeProvider.currentTime);

            _rentRepository.AddAsync(rent);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(rent.Id);
        }
        catch (ConcurrencyException ex)
        {
            return Result.Failure<Guid>(RentErrors.Overlap);
        }
    }
}
