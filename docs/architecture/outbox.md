# Outbox Pattern

Implementa la mensajería entre Bounded Contexts que exige `constitution.md` Artículo I.3 (nunca joins cruzados ni entidades compartidas) sin perder eventos si el proceso muere justo después de confirmar la transacción.

## Esquema

Una tabla `OutboxMessages` por esquema de módulo (misma base de datos, un esquema PostgreSQL por Bounded Context — Fase 0 no separa en bases de datos físicas distintas; eso queda abierto para el ADR de multi-tenancy de Fase 3, `constitution.md` Artículo X.2):

| Columna | Tipo | Nota |
|---|---|---|
| `Id` | uuid | PK |
| `Type` | text | nombre calificado del evento (ej. `IdentityAccess.UserCreated`) |
| `Content` | jsonb | payload serializado del `IDomainEvent` |
| `OccurredOn` | timestamptz | cuándo se generó el evento en el dominio |
| `ProcessedOn` | timestamptz nullable | null = pendiente de publicar |
| `Error` | text nullable | último error de publicación, si lo hubo |

Mapea a `Fluxo.BuildingBlocks.Infrastructure.OutboxMessage`.

## Escritura

`IOutboxWriter.AppendAsync` recibe los `AggregateRoot.DomainEvents` acumulados y los inserta usando la misma `IDbConnection`/`IDbTransaction` que el repositorio de escritura usó para persistir el Aggregate. Se llama justo antes del commit. Si la transacción falla, ni el estado ni los eventos quedan escritos — atomicidad garantizada por la base de datos, no por código de aplicación.

## Publicación

Un `BackgroundService` (uno por módulo, o uno genérico parametrizado por conexión) hace polling periódico de filas con `ProcessedOn IS NULL`, deserializa el `Content` y las publica al mecanismo de integración elegido. Para Fase 0-3, ese mecanismo es **in-process** (un dispatcher de eventos de integración dentro del mismo binario, ya que la arquitectura es un monolito modular, no microservicios) — no hay broker externo (Kafka/RabbitMQ/Azure Service Bus) todavía. Si en una fase futura hace falta desacoplar entre procesos, el cambio es exclusivamente de `Infrastructure` (el publicador), sin tocar `Domain`/`Application` (mismo principio que Artículo X.4).

## Por qué no publicar directo en memoria sin outbox

Publicar el evento de dominio in-memory inmediatamente después del `Handle` del Command, sin pasar por una tabla, pierde el evento si el proceso crashea entre el commit de la transacción y la publicación. El Outbox convierte "publicar" en "hay una fila pendiente", que sobrevive un crash y se reintenta.

## Estado en Fase 0 / Workstream 2

Diseño y contrato (`IOutboxWriter`, `OutboxMessage`) definidos. La implementación concreta (script de tabla, `BackgroundService` de publicación) se construye en Workstream 3 junto con el primer evento real, `IdentityAccess.UserCreated` (`docs/specs/fase-0-spec.md`, sección 7).
