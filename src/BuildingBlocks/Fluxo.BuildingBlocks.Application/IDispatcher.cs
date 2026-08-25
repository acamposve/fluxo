namespace Fluxo.BuildingBlocks.Application;

/// <summary>
/// Single entry point from API/Presentation into Application (constitution.md Artículo IV).
/// Resolves the matching handler via DI — see Dispatcher for the resolution mechanism.
/// </summary>
public interface IDispatcher
{
    Task<Result> Send(ICommand command, CancellationToken cancellationToken = default);

    Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);

    Task<Result<TResponse>> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}
