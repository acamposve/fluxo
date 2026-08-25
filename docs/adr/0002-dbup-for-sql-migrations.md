# ADR-0002: DbUp para migraciones SQL

**Estado:** Propuesto (pendiente de aprobación del Tech Lead — ver `constitution.md` Artículo VII.3).
**Fecha:** 2026-08-25
**Fase:** 0 (Workstream 2 — Arquitectura Base)

## Contexto

`constitution.md` Artículo III fija Dapper (SQL explícito) como acceso a datos, explícitamente sin EF Core y por lo tanto sin su motor de migraciones. `docs/tasks/fase-0-tasks.md` (tarea 2.6) pide definir cómo se versiona el esquema de PostgreSQL sin esa herramienta.

Opciones consideradas: **DbUp**, **FluentMigrator**, **Flyway** (JVM, dependencia externa al stack .NET), scripts SQL aplicados a mano.

## Decisión

Usar **DbUp**: aplica scripts `.sql` versionados en orden, registrando en una tabla de control (`SchemaVersions`) cuáles ya corrieron. Un script es simplemente SQL — no requiere aprender la DSL de una librería (a diferencia de FluentMigrator, que expresa migraciones en C#), lo cual encaja con la filosofía de "SQL explícito" que ya rige el acceso a datos con Dapper (Artículo III).

Convención: un directorio `Migrations/` por módulo, dentro de su proyecto `Infrastructure`, con scripts `NNN_descripcion.sql` — ver `docs/architecture/data-access.md`.

## Consecuencias

- Los scripts de un módulo solo tocan el esquema de ese módulo — refuerza que ningún Bounded Context accede a la base de otro (Artículo I.3).
- DbUp corre tanto embebido al arrancar la app (útil en local/desarrollo) como en un paso explícito de un pipeline de CI/CD (recomendado para staging/producción, para no acoplar el arranque del servicio a permisos de DDL en la base).
- Sin migraciones automáticas "hacia atrás" (down-migrations) — DbUp es forward-only por diseño; un cambio que haya que revertir se resuelve con un nuevo script correctivo, nunca reescribiendo uno ya aplicado en algún ambiente.
- No se escribió ningún script todavía: el primero corresponde al esquema de `IdentityAccess` en Workstream 3.
