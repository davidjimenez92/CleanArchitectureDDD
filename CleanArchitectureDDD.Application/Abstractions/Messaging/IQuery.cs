using CleanArchitectureDDD.Domain.Abstractions;
using MediatR;

namespace CleanArchitectureDDD.Application.Abstractions.Messaging;

public interface IQuery<TResponse>: IRequest<Result<TResponse>>
{
    
}