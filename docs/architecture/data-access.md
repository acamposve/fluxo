# Acceso a Datos: Dapper y Migraciones

## Write-side (repositorios)

- Interfaz del repositorio (ej. `IUserRepository`) definida en `{Modulo}.Application`; implementación con Dapper en `{Modulo}.Infrastructure`.
- El repositorio mapea explícitamente el Aggregate ↔ filas SQL: sin change tracker, sin ORM. Carga vía un método de reconstitución (constructor privado o factory interno) que no viola invariantes del Aggregate; guarda vía SQL explícito dentro de una transacción.
- Esa misma transacción es la que usa `IOutboxWriter` (`Fluxo.BuildingBlocks.Infrastructure`) para insertar los eventos de dominio pendientes — ver [`outbox.md`](outbox.md). Guardar el Aggregate y encolar sus eventos es una única operación atómica.
- La conexión sale siempre de `IDbConnectionFactory` (implementado por `NpgsqlDbConnectionFactory`); nunca se instancia `NpgsqlConnection` directamente fuera de Infrastructure.

## Read-side (Query Handlers)

- Los `IQueryHandler<TQuery, TResponse>` usan Dapper directo contra SQL crudo (o vistas/proyecciones desnormalizadas) para armar el modelo de lectura. No pasan por el repositorio de escritura ni por el shape del Aggregate (`constitution.md` Artículo II.2 y IV, nota sobre Dapper y CQRS).

## Migraciones de esquema (sin EF Core)

Ver ADR-0002. Herramienta elegida: **DbUp**.

- Un script SQL versionado por cambio (`NNN_descripcion.sql`), en `src/Modules/{Modulo}/Fluxo.Modules.{Modulo}.Infrastructure/Migrations/`. Cada módulo versiona su propio esquema — consistente con que ningún Bounded Context accede a la base de otro (`constitution.md` Artículo I.3).
- DbUp aplica los scripts pendientes en orden, registrando los ya ejecutados en una tabla de control por base de datos.
- Aplicación: en local/desarrollo, al levantar `Fluxo.Api` (Fase 0, Workstream 4 define el docker-compose que lo dispara); en staging/producción, como paso explícito del pipeline de CD, antes de desplegar el nuevo binario.
- No hay scripts todavía: el primero se escribe en Workstream 3, junto con el esquema de `IdentityAccess` (`User`, `Role`, `Permission`, outbox).
