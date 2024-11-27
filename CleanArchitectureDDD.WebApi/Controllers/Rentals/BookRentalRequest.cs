namespace CleanArchitectureDDD.WebApi.Controllers.Rentals;

public record BookRentalRequest(Guid VehicleId, Guid UserId, DateOnly StartDate, DateOnly EndDate);