using CleanArchitectureDDD.Domain.Entities.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Reviews;

public sealed record Ratting
{
    public static readonly Error Invalid = new Error("Ratting.Invalid", "Ratting must be between 1 and 5.");
    
    public int Value { get; init; }
    
    private Ratting(int value) => Value = value;
    
    public static Result<Ratting> Create(int value)
    {
        if (value < 1 || value > 5)
        {
            return Result.Failure<Ratting>(Invalid);
        }
        
        return new Ratting(value);
    }
}