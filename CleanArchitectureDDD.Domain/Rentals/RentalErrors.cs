using System.Runtime.InteropServices.JavaScript;
using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Rentals;

public static class RentalErrors
{
    public static Error NotFound = new Error("Rental.Found", "Rental not found");
    public static Error Overlap = new Error("Rental.Overlap", "Rental dates overlap");
    public static Error NotReserved = new Error("Rental.NotReserved", "Rental is not reserved");
    public static Error NotConfirmed = new Error("Rental.NotConfirmed", "Rental is not confirmed");
    public static Error AlreadyStarted = new Error("Rental.AlreadyStarted", "Rental already started");
}