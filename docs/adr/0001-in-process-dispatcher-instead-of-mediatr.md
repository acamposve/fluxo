# ADR-0001: Dispatcher propio en vez de MediatR

**Estado:** Propuesto (pendiente de aprobación del Tech Lead — ver `constitution.md` Artículo VII.3).
**Fecha:** 2026-08-25
**Fase:** 0 (Workstream 2 — Arquitectura Base)

## Contexto

`constitution.md` Artículo II exige CQRS puro: un Command o Query por operación, manejado por un único Handler. La forma más común de resolver "de qué Command llegó, a qué Handler despachar" en .NET es un mediador in-process, y la librería de facto para eso es **MediatR**.

MediatR pasó a requerir licencia comercial para versiones recientes (a partir de MediatR v13, con costo por desarrollador/organización según el tamaño). Para un proyecto explícitamente vendido al cliente como de bajo costo total de propiedad y sin dependencias de licenciamiento recurrente (`plan.md`, "Diferenciador clave frente a Magaya"), pagar por una librería de infraestructura interna —que además resuelve algo simple— es inconsistente con esa propuesta de valor.

## Decisión

Implementar un dispatcher propio y mínimo (`Fluxo.BuildingBlocks.Application.Dispatcher`): resuelve `ICommandHandler<TCommand>` / `ICommandHandler<TCommand, TResponse>` / `IQueryHandler<TQuery, TResponse>` desde el `IServiceProvider` de DI usando el tipo en tiempo de ejecución del Command/Query, vía reflection simple (sin cachear `MethodInfo` todavía — optimización a futuro si el profiling lo justifica, no una preocupación de Fase 0).

## Consecuencias

- Cero dependencias de terceros de pago para algo que en este proyecto no necesita más que resolver-e-invocar un handler.
- Sin pipeline behaviors (logging/validación/transacciones cross-cutting) out-of-the-box como los de MediatR; si se necesitan, se agregan como decoradores explícitos sobre `IDispatcher` o sobre los handlers, en una iteración futura.
- Superficie de la interfaz (`ICommand`, `IQuery`, `ICommandHandler`, `IQueryHandler`, `IDispatcher`) queda bajo control total del proyecto — cambiarla no depende de una librería externa.
- Si en el futuro se decide adoptar MediatR igual (o cualquier otra librería), el cambio queda contenido a `Fluxo.BuildingBlocks.Application`, ya que ningún módulo llama a MediatR directamente — todos pasan por `IDispatcher`.
