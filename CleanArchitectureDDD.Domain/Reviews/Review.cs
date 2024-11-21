using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Reviews.Events;

namespace CleanArchitectureDDD.Domain.Reviews;

public sealed class Review: Entity
{
    private Review(Guid id, Guid vehicleId, Guid rentId, Guid userId, Ratting rating, Comment? comment, DateTime? createdAt) : base(id)
    {
        VehicleId = vehicleId;
        RentId = rentId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedAt = createdAt;
    }

    public Guid VehicleId { get; private set; }
    public Guid RentId { get; private set; }
    public Guid UserId { get; private set; }
    public Ratting Rating { get; private set; }
    public Comment? Comment { get; private set; }
    public DateTime? CreatedAt { get; private set; }
    
    public static Result<Review> Create(Rent rent, Ratting rating, Comment? comment, DateTime? createdAt)
    {
        if (rent.Status != RentStatus.Completed)
        {
            return Result.Failure<Review>(ReviewErrors.NotElegible);
        }
        var review =  new Review(Guid.NewGuid(), rent.VehicleId, rent.Id, rent.UserId, rating, comment, createdAt);

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id!));
        return review;
    }
}