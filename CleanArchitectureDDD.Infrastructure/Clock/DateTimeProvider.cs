using CleanArchitectureDDD.Application.Abstractions.Clock;

namespace CleanArchitectureDDD.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime currentTime => DateTime.UtcNow;
}