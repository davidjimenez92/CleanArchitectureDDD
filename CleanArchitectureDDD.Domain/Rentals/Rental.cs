using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals.Events;
using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Domain.Rentals;

public sealed class Rental: Entity<RentalId>
{
    private Rental()
    {
    }
    private Rental(RentalId id, RentalStatus status, DateRange? duration, VehicleId vehicleId, UserId userId, Currency? price, Currency? maintenance, Currency? accessoriesPrice, Currency? totalPrice, DateTime? dateCreated) : base(id)
    {
        Status = status;
        Duration = duration;
        VehicleId = vehicleId;
        UserId = userId;
        Price = price;
        Maintenance = maintenance;
        AccessoriesPrice = accessoriesPrice;
        TotalPrice = totalPrice;
        DateCreated = dateCreated;
    }

    public RentalStatus Status { get; private set; }
    public DateRange? Duration { get; private set; }
    public VehicleId? VehicleId { get; private set; }
    public UserId? UserId { get; private set; }
    public Currency? Price { get; private set; }
    public Currency? Maintenance { get; private set; }
    public Currency? AccessoriesPrice { get; private set; }
    public Currency? TotalPrice { get; private set; }
    public DateTime? DateCreated { get; private set; }
    public DateTime? DateConfirmation { get; private set; }
    public DateTime? DateRejection { get; private set; }
    public DateTime? DateCopletation { get; private set; }
    public DateTime? DateCancellation { get; private set; }
    
    public static Rental Book(Vehicle vehicle, UserId userId, DateRange duration, PriceService priceService, DateTime dateCreated)
    {
        var priceDetail = priceService.CalculatePrice(vehicle, duration);
        var rent = new Rental(
            RentalId.New(), 
            RentalStatus.Reserved, 
            duration, 
            vehicle.Id!, 
            userId, 
            priceDetail.PricePerPeriod, 
            priceDetail.Maintenance, 
            priceDetail.AccessoriesCharge, 
            priceDetail.TotalPrice, 
            dateCreated
            );
        
        rent.RaiseDomainEvent(new RentalBookedDomainEvent(rent.Id!));

        vehicle.LastRent = dateCreated;
        
        return rent;
    }
    
    public Result Confirm(DateTime utcNow)
    {
        if (Status != RentalStatus.Reserved)
        {
            return Result.Failure(RentalErrors.NotReserved);
        }

        Status = RentalStatus.Confirmed;
        DateConfirmation = utcNow;
        
        RaiseDomainEvent(new RentalConfirmedDomainEvent(Id!));
        return Result.Success();
    }
    
    public Result Reject(DateTime utcNow)
    {
        if (Status != RentalStatus.Reserved)
        {
            return Result.Failure(RentalErrors.NotReserved);
        }

        Status = RentalStatus.Rejected;
        DateRejection = utcNow;
        
        RaiseDomainEvent(new RentalRejectedDomainEvent(Id!));
        return Result.Success();
    }
    
    public Result Cancel(DateTime utcNow)
    {
        if (Status != RentalStatus.Confirmed)
        {
            return Result.Failure(RentalErrors.NotConfirmed);
        }

        if (Duration!.StartDate < DateOnly.FromDateTime(utcNow))
        {
            return Result.Failure(RentalErrors.AlreadyStarted);
        }
        
        Status = RentalStatus.Canceled;
        DateCancellation = utcNow;
        
        RaiseDomainEvent(new RentalCanceledDomainEvent(Id!));
        return Result.Success();
    }
    
    public Result Complete(DateTime utcNow)
    {
        if (Status != RentalStatus.Confirmed)
        {
            return Result.Failure(RentalErrors.NotConfirmed);
        }

        if (Duration!.EndDate < DateOnly.FromDateTime(utcNow))
        {
            return Result.Failure(RentalErrors.Overlap);
        }
        
        Status = RentalStatus.Completed;
        DateCopletation = utcNow;
        
        RaiseDomainEvent(new RentalCompletedDomainEvent(Id!));
        return Result.Success();
    }
}