using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals.Events;
using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Domain.Rentals;

public sealed class Rent: Entity
{
    private Rent(Guid id, RentStatus status, DateRange? duration, Guid vehicleId, Guid userId, Currency? price, Currency? maintenance, Currency? accessoriesPrice, Currency? totalPrice, DateTime? dateCreated) : base(id)
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

    public RentStatus Status { get; private set; }
    public DateRange? Duration { get; private set; }
    public Guid VehicleId { get; private set; }
    public Guid UserId { get; private set; }
    public Currency? Price { get; private set; }
    public Currency? Maintenance { get; private set; }
    public Currency? AccessoriesPrice { get; private set; }
    public Currency? TotalPrice { get; private set; }
    public DateTime? DateCreated { get; private set; }
    public DateTime? DateConfirmation { get; private set; }
    public DateTime? DateRejection { get; private set; }
    public DateTime? DateCopletation { get; private set; }
    public DateTime? DateCancellation { get; private set; }
    
    public static Rent Create(Vehicle vehicle, Guid userId, DateRange duration, PriceService priceService, DateTime dateCreated)
    {
        var priceDetail = priceService.CalculatePrice(vehicle, duration);
        var rent = new Rent(
            Guid.NewGuid(), 
            RentStatus.Reserved, 
            duration, 
            vehicle.Id, 
            userId, 
            priceDetail.PricePerPeriod, 
            priceDetail.Maintenance, 
            priceDetail.AccessoriesCharge, 
            priceDetail.TotalPrice, 
            dateCreated
            );
        
        rent.RaiseDomainEvent(new RentCreatedDomainEvent(rent.Id!));

        vehicle.LastRent = dateCreated;
        
        return rent;
    }
    
    public Result Confirm(DateTime utcNow)
    {
        if (Status != RentStatus.Reserved)
        {
            return Result.Failure(RentErrors.NotReserved);
        }

        Status = RentStatus.Confirmed;
        DateConfirmation = utcNow;
        
        RaiseDomainEvent(new RentConfirmedDomainEvent(Id!));
        return Result.Success();
    }
    
    public Result Reject(DateTime utcNow)
    {
        if (Status != RentStatus.Reserved)
        {
            return Result.Failure(RentErrors.NotReserved);
        }

        Status = RentStatus.Rejected;
        DateRejection = utcNow;
        
        RaiseDomainEvent(new RentRejectedDomainEvent(Id!));
        return Result.Success();
    }
    
    public Result Cancel(DateTime utcNow)
    {
        if (Status != RentStatus.Confirmed)
        {
            return Result.Failure(RentErrors.NotConfirmed);
        }

        if (Duration!.StartDate < DateOnly.FromDateTime(utcNow))
        {
            return Result.Failure(RentErrors.AlreadyStarted);
        }
        
        Status = RentStatus.Canceled;
        DateCancellation = utcNow;
        
        RaiseDomainEvent(new RentCanceledDomainEvent(Id!));
        return Result.Success();
    }
    
    public Result Complete(DateTime utcNow)
    {
        if (Status != RentStatus.Confirmed)
        {
            return Result.Failure(RentErrors.NotConfirmed);
        }

        if (Duration!.EndDate < DateOnly.FromDateTime(utcNow))
        {
            return Result.Failure(RentErrors.Overlap);
        }
        
        Status = RentStatus.Completed;
        DateCopletation = utcNow;
        
        RaiseDomainEvent(new RentCompletedDomainEvent(Id!));
        return Result.Success();
    }
}