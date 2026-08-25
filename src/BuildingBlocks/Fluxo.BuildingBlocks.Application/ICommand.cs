namespace Fluxo.BuildingBlocks.Application;

/// <summary>
/// A write operation. Per constitution.md Artículo II.3, commands return only
/// confirmation/identifier — never business data — so handlers only ever hand back
/// a Result or a Result&lt;TResponse&gt; where TResponse is an identifier, not a read model.
/// </summary>
public interface ICommand;

public interface ICommand<TResponse>;
