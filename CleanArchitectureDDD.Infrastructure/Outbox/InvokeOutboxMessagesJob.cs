using System.Data;
using CleanArchitectureDDD.Application.Abstractions.Clock;
using CleanArchitectureDDD.Application.Abstractions.Data;
using CleanArchitectureDDD.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace CleanArchitectureDDD.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class InvokeOutboxMessagesJob: IJob
{
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.All
    };
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<InvokeOutboxMessagesJob> _logger;

    public InvokeOutboxMessagesJob(ISqlConnectionFactory sqlConnectionFactory, IPublisher publisher, IDateTimeProvider dateTimeProvider, IOptions<OutboxOptions> outboxOptions, ILogger<InvokeOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Invoking outbox messages");

        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var sql = $@"
                    SELECT 
                    id, content
                    FROM outbox_messages
                    WHERE processed_on_utc IS NULL
                    ORDER BY ocurred_on_utc DESC
                    LIMIT {_outboxOptions.BatchSize}
                    FOR UPDATE
                  ";
        var records = (await connection.QueryAsync<OutboxMessageData>(sql, transaction)).ToList();

        foreach (var record in records)
        {
            Exception? exception = null;
            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(record.Content, _jsonSerializerSettings);
                await _publisher.Publish(domainEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to invoke outbox messages");
                exception = ex;
                
            }

            await UpdateOutboxMessage(connection, transaction, record, exception);
        }
        
        transaction.Commit();
        _logger.LogInformation("Outbox messages updated");
    }

    private async Task UpdateOutboxMessage(IDbConnection connection, IDbTransaction transaction, OutboxMessageData record,
        Exception? exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc, error = @Error 
            WHERE id=@Id";
        
        await connection.ExecuteAsync(sql, new
        {
            Id = record.Id,
            ProcessedOnUtc = _dateTimeProvider.currentTime,
            Error = exception?.ToString()
        }, transaction);
    }
}

public record OutboxMessageData(Guid Id, string Content);