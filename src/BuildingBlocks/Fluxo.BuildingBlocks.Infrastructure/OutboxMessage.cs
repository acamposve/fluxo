namespace Fluxo.BuildingBlocks.Infrastructure;

/// <summary>
/// Row shape for the outbox table (one per module's schema — see docs/architecture/outbox.md).
/// Written in the same transaction as the aggregate it originated from.
/// </summary>
public sealed class OutboxMessage
{
    public required Guid Id { get; init; }
    public required string Type { get; init; }
    public required string Content { get; init; }
    public required DateTimeOffset OccurredOn { get; init; }
    public DateTimeOffset? ProcessedOn { get; init; }
    public string? Error { get; init; }
}
