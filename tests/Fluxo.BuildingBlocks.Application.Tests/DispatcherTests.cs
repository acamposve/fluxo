using Fluxo.BuildingBlocks.Application;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fluxo.BuildingBlocks.Application.Tests;

public sealed record PingCommand(string Message) : ICommand<string>;

public sealed class PingCommandHandler : ICommandHandler<PingCommand, string>
{
    public Task<Result<string>> Handle(PingCommand command, CancellationToken cancellationToken)
        => Task.FromResult(Result<string>.Success($"pong:{command.Message}"));
}

public sealed record FailingCommand : ICommand;

public sealed class FailingCommandHandler : ICommandHandler<FailingCommand>
{
    public Task<Result> Handle(FailingCommand command, CancellationToken cancellationToken)
        => Task.FromResult(Result.Failure("nope"));
}

public class DispatcherTests
{
    private static IDispatcher BuildDispatcher()
    {
        var services = new ServiceCollection();
        services.AddDispatcher();
        services.AddScoped<ICommandHandler<PingCommand, string>, PingCommandHandler>();
        services.AddScoped<ICommandHandler<FailingCommand>, FailingCommandHandler>();
        return services.BuildServiceProvider().GetRequiredService<IDispatcher>();
    }

    [Fact]
    public async Task Send_WithResponse_ResolvesHandlerAndReturnsItsResult()
    {
        var dispatcher = BuildDispatcher();

        var result = await dispatcher.Send(new PingCommand("hi"));

        Assert.True(result.IsSuccess);
        Assert.Equal("pong:hi", result.Value);
    }

    [Fact]
    public async Task Send_WithoutResponse_PropagatesHandlerFailure()
    {
        var dispatcher = BuildDispatcher();

        var result = await dispatcher.Send(new FailingCommand());

        Assert.True(result.IsFailure);
        Assert.Equal("nope", result.Error);
    }

    [Fact]
    public async Task Send_WithoutRegisteredHandler_Throws()
    {
        var services = new ServiceCollection();
        services.AddDispatcher();
        var dispatcher = services.BuildServiceProvider().GetRequiredService<IDispatcher>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => dispatcher.Send(new FailingCommand()));
    }
}
