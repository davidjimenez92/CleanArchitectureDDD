namespace CleanArchitectureDDD.Domain.Rentals;

public record RentalId(Guid Value)
{
    public static RentalId New() => new(Guid.NewGuid());
}