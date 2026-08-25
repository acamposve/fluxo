using System.Data;
using Fluxo.BuildingBlocks.Domain;

namespace Fluxo.BuildingBlocks.Infrastructure;

/// <summary>
/// Appends domain events raised by an Aggregate to its module's outbox table, inside the
/// same transaction the write-repository uses to persist the aggregate's state — this is
/// what makes publication at-least-once even if the process dies right after commit.
/// Implemented per module once that module has its first Aggregate (see docs/architecture/outbox.md).
/// </summary>
public interface IOutboxWriter
{
    Task AppendAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        IDbConnection connection,
        IDbTransaction transaction,
        CancellationToken cancellationToken = default);
}
