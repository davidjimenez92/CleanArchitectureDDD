namespace CleanArchitectureDDD.WebApi.Controllers.Rentals.V1;

public record BookRentalRequest(Guid VehicleId, Guid UserId, DateOnly StartDate, DateOnly EndDate);