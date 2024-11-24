using CleanArchitectureDDD.Domain.Abstractions;
using MediatR;

namespace CleanArchitectureDDD.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
    
}