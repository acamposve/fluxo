using System.Data;

namespace Fluxo.BuildingBlocks.Infrastructure;

/// <summary>
/// The one seam Dapper repositories (write-side) and Query Handlers (read-side) go through
/// to get a connection — constitution.md Artículo III/IV. Never exposed outside Infrastructure.
/// </summary>
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
