using CleanArchitectureDDD.Application.Abstractions.Messaging;
using CleanArchitectureDDD.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace CleanArchitectureDDD.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Handling request {CommandName} ({@Command})", name, request);
            var result = await next();

            if (result.IsIsSuccess)
            {
                _logger.LogInformation("Request {CommandName} handled successfully", name);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error))
                {
                    _logger.LogError("Request {CommandName} failed with errors", name);
                }
            }

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Request {CommandName} failed", name);
            throw;
        }
    }
}