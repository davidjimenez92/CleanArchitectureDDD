using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Reviews;

public static class ReviewErrors
{
    public static Error NotElegible => new Error("Review.NotElegible", "Review is not elegible for this operation.");

}