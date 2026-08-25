namespace Fluxo.BuildingBlocks.Domain;

/// <summary>
/// Marks a fact that already happened inside an Aggregate. Raised via AggregateRoot.Raise,
/// dispatched by Infrastructure through the Outbox (constitution.md Artículo I.3 / II.5).
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}
