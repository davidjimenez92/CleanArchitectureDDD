using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Reviews;

public sealed record Rating
{
    public static readonly Error Invalid = new Error("Ratting.Invalid", "Ratting must be between 1 and 5.");
    
    public int Value { get; init; }
    
    private Rating(int value) => Value = value;
    
    public static Result<Rating> Create(int value)
    {
        if (value < 1 || value > 5)
        {
            return Result.Failure<Rating>(Invalid);
        }
        
        return new Rating(value);
    }
}