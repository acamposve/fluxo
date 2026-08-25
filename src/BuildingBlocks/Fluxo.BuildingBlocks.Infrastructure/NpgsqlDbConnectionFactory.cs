using System.Data;
using Npgsql;

namespace Fluxo.BuildingBlocks.Infrastructure;

public sealed class NpgsqlDbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new NpgsqlConnection(connectionString);
}
