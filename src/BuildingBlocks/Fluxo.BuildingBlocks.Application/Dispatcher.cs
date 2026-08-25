using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxo.BuildingBlocks.Application;

/// <summary>
/// Resolves ICommandHandler{TCommand}/ICommandHandler{TCommand,TResponse}/IQueryHandler{TQuery,TResponse}
/// from DI by reflecting over the runtime type of the command/query. Deliberately not MediatR — see
/// ADR-0001 (in-process dispatcher instead of a third-party mediator library).
/// </summary>
public sealed class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public Task<Result> Send(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        return Invoke<Result>(handlerType, command, cancellationToken);
    }

    public Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        return Invoke<Result<TResponse>>(handlerType, command, cancellationToken);
    }

    public Task<Result<TResponse>> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        return Invoke<Result<TResponse>>(handlerType, query, cancellationToken);
    }

    private Task<TResult> Invoke<TResult>(Type handlerType, object request, CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No handler registered for '{request.GetType().Name}' (expected {handlerType}).");

        var method = handlerType.GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance)
            ?? throw new InvalidOperationException($"'{handlerType}' has no Handle method.");

        return (Task<TResult>)method.Invoke(handler, [request, cancellationToken])!;
    }
}
