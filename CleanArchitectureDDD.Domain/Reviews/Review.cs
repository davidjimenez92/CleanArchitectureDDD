using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Reviews.Events;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Vehicles;

namespace CleanArchitectureDDD.Domain.Reviews;

public sealed class Review: Entity<ReviewId>
{
    private Review()
    {}
    private Review(ReviewId id, VehicleId vehicleId, RentalId rentalId, UserId userId, Rating rating, Comment? comment, DateTime? createdAt) : base(id)
    {
        VehicleId = vehicleId;
        RentalId = rentalId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedAt = createdAt;
    }

    public VehicleId? VehicleId { get; private set; }
    public RentalId? RentalId { get; private set; }
    public UserId? UserId { get; private set; }
    public Rating? Rating { get; private set; }
    public Comment? Comment { get; private set; }
    public DateTime? CreatedAt { get; private set; }
    
    public static Result<Review> Create(Rental rental, Rating rating, Comment? comment, DateTime? createdAt)
    {
        if (rental.Status != RentalStatus.Completed)
        {
            return Result.Failure<Review>(ReviewErrors.NotElegible);
        }
        var review =  new Review(ReviewId.New(), rental.VehicleId, rental.Id, rental.UserId, rating, comment, createdAt);

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id!));
        return review;
    }
}