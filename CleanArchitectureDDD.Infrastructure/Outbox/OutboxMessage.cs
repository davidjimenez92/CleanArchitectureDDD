namespace CleanArchitectureDDD.Infrastructure.Outbox;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public DateTime OcurredOnUtc { get; init; }
    public string Type { get; init; }
    public string Content { get; init; }
    
    public DateTime? ProcessedOnUtc { get; init; }
    public string? Error { get; init; }

    public OutboxMessage(Guid id, DateTime ocurredOnUtc, string type, string content)
    {
        Id = id;
        OcurredOnUtc = ocurredOnUtc;
        Type = type;
        Content = content;
    }
}