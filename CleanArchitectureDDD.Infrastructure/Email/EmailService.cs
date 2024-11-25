using CleanArchitectureDDD.Application.Abstractions.Email;

namespace CleanArchitectureDDD.Infrastructure.Email;

internal sealed class EmailService: IEmailService
{
    public Task SendEmailAsync(Domain.Users.Email recipient, string subject, string body)
    {
        //TODO Implement email sending
        //TODO implemt rabbitMQ to push info into a new queue for email sending.
        return Task.CompletedTask;
    }
}