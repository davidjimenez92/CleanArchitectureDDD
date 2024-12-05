using System.Text.Json.Nodes;
using CleanArchitectureDDD.Application.Abstractions.Clock;
using CleanArchitectureDDD.Application.Exceptions;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CleanArchitectureDDD.Infrastructure.Contexts;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.All
    };
    public ApplicationDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsToOutboxMessages();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("An error occurred while saving changes", ex);
        }
    }

    private void AddDomainEventsToOutboxMessages()
    {
        var outboxMessages = ChangeTracker.Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(), _dateTimeProvider.currentTime, domainEvent.GetType().Name, JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings)
                )).ToList();
        
        AddRange(outboxMessages);
    }
}