using System.Diagnostics.CodeAnalysis;

namespace CleanArchitectureDDD.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if(isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Success result can't have an error.");
        }
        if(isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Failed result must have an error.");
        }

        IsIsSuccess = isSuccess;
        Error = error;
    }

    public bool IsIsSuccess { get; }
    public bool IsFailure => !IsIsSuccess;
    public Error Error { get; }
    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    public static Result<TValue> Create<TValue>(TValue value) => value is not null? Success(value): Failure<TValue>(Error.NullValue());


}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    protected internal Result(TValue? value, bool isSuccess, Error error): base(isSuccess, error)
    {
        _value = value;
    }
    
    
    [NotNull]
    public TValue Value => IsIsSuccess ? _value! : throw new InvalidOperationException("Failed result doesn't have a value.");
    
    public static implicit operator Result<TValue>(TValue value) => Create(value);
}