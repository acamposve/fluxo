# Arquitectura Base

Resultado de la Fase 0 / Workstream 2 (`docs/tasks/fase-0-tasks.md`, sección 2). Documenta las convenciones sobre las que se construye cualquier módulo de negocio a partir de Fase 1, según `constitution.md` Artículos I, II, III y IV.

## Solución y capas

```
Fluxo.slnx
src/
  BuildingBlocks/                              # Compartido por todos los módulos. Sin lógica de negocio.
    Fluxo.BuildingBlocks.Domain/                # Entity, AggregateRoot, ValueObject, IDomainEvent
    Fluxo.BuildingBlocks.Application/           # ICommand, IQuery, handlers, IDispatcher, Result
    Fluxo.BuildingBlocks.Infrastructure/        # IDbConnectionFactory (Npgsql), Outbox
  Modules/
    IdentityAccess/                             # Primer módulo real (Workstream 3)
      Fluxo.Modules.IdentityAccess.Domain/
      Fluxo.Modules.IdentityAccess.Application/
      Fluxo.Modules.IdentityAccess.Infrastructure/
    <Shipping|Warehousing|Customs|Crm|Finance>/  # Mismo patrón, uno por Bounded Context, a medida que cada fase los necesita
  Host/
    Fluxo.Api/                                  # Composition root — único proyecto que conoce todos los módulos a la vez
tests/
  Fluxo.BuildingBlocks.Application.Tests/
```

No se crearon proyectos vacíos para Shipping/Warehousing/Customs/CRM/Finance todavía: se replican desde `Modules/IdentityAccess` cuando la fase correspondiente los necesita (Fase 1 en adelante), para no mantener esqueleto sin uso durante meses.

## Regla de dependencias (Clean Architecture, Artículo IV)

```
Domain          → no referencia nada (verificado: cero PackageReference/ProjectReference salvo BuildingBlocks.Domain)
Application     → referencia solo su propio Domain + BuildingBlocks.Application
Infrastructure  → referencia Application + Domain del mismo módulo + BuildingBlocks.Infrastructure
Fluxo.Api (Host)→ referencia Application + Infrastructure de cada módulo que compone
```

**Verificación de la regla "Domain no depende de infraestructura" (tarea 2.2):** decidido para nivel de diseño, no implementado todavía. Se hará con **NetArchTest.Rules** en un proyecto `tests/Fluxo.ArchitectureTests`, con una regla por capa (`Domain` no puede referenciar Dapper/Npgsql/ASP.NET/`*.Infrastructure`). Se implementa en Fase 1, cuando exista el primer módulo con lógica de dominio real que la regla deba proteger.

## Convenciones por tema

- [`cqrs.md`](cqrs.md) — Commands, Queries, handlers, dispatcher.
- [`data-access.md`](data-access.md) — convención Dapper (write-side/read-side) y migraciones de esquema.
- [`outbox.md`](outbox.md) — diseño del Outbox Pattern para mensajería entre contextos.
- [`deployment.md`](deployment.md) — cómo el artefacto Docker soporta despliegue híbrido (Artículo X) sin reescritura.

## Decisiones registradas como ADR

- [`/docs/adr/0001-in-process-dispatcher-instead-of-mediatr.md`](../adr/0001-in-process-dispatcher-instead-of-mediatr.md)
- [`/docs/adr/0002-dbup-for-sql-migrations.md`](../adr/0002-dbup-for-sql-migrations.md)
