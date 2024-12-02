using Serilog.Context;

namespace CleanArchitectureDDD.WebApi.Middleware;

public class RequestContextLoggingMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", GetHCorrelationId(context)))
        {
            return _next(context);
        }
    }

    private string GetHCorrelationId(HttpContext context)
    {
         context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

         return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}