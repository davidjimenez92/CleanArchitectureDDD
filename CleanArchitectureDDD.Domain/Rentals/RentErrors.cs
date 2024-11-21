using System.Runtime.InteropServices.JavaScript;
using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals;

public static class RentErrors
{
    public static Error NotFound = new Error("Rent.Found", "Rent not found");
    public static Error Overlap = new Error("Rent.Overlap", "Rent dates overlap");
    public static Error NotReserved = new Error("Rent.NotReserved", "Rent is not reserved");
    public static Error NotConfirmed = new Error("Rent.NotConfirmed", "Rent is not confirmed");
    public static Error AlreadyStarted = new Error("Rent.AlreadyStarted", "Rent already started");
}