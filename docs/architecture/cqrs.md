# Convención CQRS

Implementado en `Fluxo.BuildingBlocks.Application`. Ver ADR-0001 para la razón de no usar MediatR.

## Contratos

- `ICommand` / `ICommand<TResponse>` — una escritura. `TResponse`, cuando existe, es un identificador (ej. el `Guid` de la entidad creada), nunca un modelo de lectura (`constitution.md` Artículo II.3).
- `IQuery<TResponse>` — una lectura. `TResponse` es el modelo de lectura que necesite la UI; puede ser una proyección desnormalizada (Artículo II.2).
- `ICommandHandler<TCommand>` / `ICommandHandler<TCommand, TResponse>` / `IQueryHandler<TQuery, TResponse>` — un handler por Command/Query. Nunca un handler que resuelve varios.
- `Result` / `Result<TValue>` — todo handler devuelve éxito/fracaso explícito; nada de excepciones para flujo de control esperado (validación de negocio, no encontrado, etc.).

## Dispatcher

`IDispatcher` (implementado por `Dispatcher`) es el único punto de entrada desde `API/Presentation` hacia `Application`. Resuelve el handler correspondiente vía el `IServiceProvider` de DI, usando el tipo en tiempo de ejecución del Command/Query — sin un mediador de terceros (ADR-0001).

Cada módulo registra sus propios handlers con un llamado a `services.AddHandlersFrom(typeof(<Módulo>Module).Assembly)` desde `Fluxo.Api` (el único lugar autorizado a conocer todos los módulos a la vez). Cada módulo expone una clase marcadora vacía (ej. `IdentityAccessModule`) solo para apuntar el assembly scan — no tiene otro propósito.

## Convención de organización por módulo

```
{Modulo}.Application/
  Commands/
    {Verbo}{Entidad}/
      {Verbo}{Entidad}Command.cs
      {Verbo}{Entidad}CommandHandler.cs
  Queries/
    {Verbo}{Entidad}/
      {Verbo}{Entidad}Query.cs
      {Verbo}{Entidad}QueryHandler.cs
```

Un Command/Query, su Handler y (si aplica) su validador viven en la misma carpeta — se navega por feature, no por tipo de archivo.
