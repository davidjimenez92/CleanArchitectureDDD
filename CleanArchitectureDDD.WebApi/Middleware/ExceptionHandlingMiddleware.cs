using CleanArchitectureDDD.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has been occured: {message}", ex.Message);
            var exceptionDetails = GetExceptionDetails(ex);
            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Tittle,
                Detail = exceptionDetails.Detail
            };

            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions.Add("errors", exceptionDetails.Errors);
            }
            
            context.Response.StatusCode = exceptionDetails.Status;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "Validation.Failure",
                "Validation Error",
                "Validation Failure",
                validationException.Errors
            ),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "Server Error",
                "Server Error",
                null
            )
        };
    }

    private record ExceptionDetails(int Status, string Type, string Tittle, string Detail, IEnumerable<object>? Errors);
}