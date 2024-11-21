using CleanArchitectureDDD.Domain.Entities.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Reviews;

public static class ReviewErrors
{
    public static Error NotElegible => new Error("Review.NotElegible", "Review is not elegible for this operation.");

}