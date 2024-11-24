using System.Security.AccessControl;

namespace CleanArchitectureDDD.Application.Rentals.GetRental;

public sealed class RentalResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid VehicleId { get; init; }
    public int Status { get; init; }
    public decimal RentalPrice { get; init; }
    public string RentalCurrency { get; init; }
    public decimal MaintenacePrice { get; init; }
    public string MaintenanceCurrency { get; init; }
    public decimal AccessoryPrice { get; init; }
    public string AccessoryCurrency { get; init; }
    public decimal TotalPrice { get; init; }
    public string TotalCurrency { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public DateTime DateCreated { get; init; }
}