namespace Fluxo.BuildingBlocks.Application;

/// <summary>
/// A read operation. Per constitution.md Artículo II.4, query handlers only read —
/// no business logic — and are free to use a denormalized read model (Artículo II.2).
/// </summary>
public interface IQuery<TResponse>;
