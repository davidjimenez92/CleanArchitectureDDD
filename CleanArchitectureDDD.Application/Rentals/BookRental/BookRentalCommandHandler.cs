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
    private readonly IRentalRepository _iRentalRepository;
    private readonly PriceService _priceService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public BookRentalCommandHandler(IUserRepository userRepository, IVehicleRepository vehicleRepository, IRentalRepository iRentalRepository, PriceService priceService, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _vehicleRepository = vehicleRepository;
        _iRentalRepository = iRentalRepository;
        _priceService = priceService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    
    public async Task<Result<Guid>> Handle(BookRentalCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(new VehicleId(request.VehicleId), cancellationToken);
        if (vehicle is null)
        {
            return Result.Failure<Guid>(VehicleErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);
        
        if(await _iRentalRepository.IsOverLappingAsync(vehicle, duration, cancellationToken))
        {
            return Result.Failure<Guid>(RentalErrors.Overlap);
        }

        try
        {
            var rent = Rental.Book(vehicle, user.Id!, duration, _priceService, _dateTimeProvider.currentTime);

            _iRentalRepository.AddAsync(rent);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rent.Id!.Value;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(RentalErrors.Overlap);
        }
    }
}
