# Fluxo

Plataforma digital de **Freight Forwarding** — clon funcional de Magaya Digital Freight Platform, cubriendo los cinco pilares del negocio logístico: **Freight Forwarding, Warehouse Management (WMS), Customs (Aduanas), CRM y Accounting**.

Producto propio (white-label), sin licenciamiento de terceros ni costos recurrentes por usuario, construido con **Domain-Driven Design** y **CQRS** para modelar un dominio complejo y regulado en Bounded Contexts independientes.

## Módulos

| Módulo | Descripción |
|---|---|
| **Freight Forwarding (Core)** | Cotizaciones, órdenes de embarque, Bill of Lading / Airway Bill, tracking, consolidación de carga |
| **Warehouse Management (WMS)** | Recepción, almacenaje, picking/packing, control de inventario, cross-docking |
| **Customs (Aduanas)** | Documentación aduanera, clasificación arancelaria, integración con entidades regulatorias |
| **CRM** | Gestión de clientes, agentes, cotizaciones comerciales, pipeline de ventas |
| **Accounting** | Facturación, cuentas por cobrar/pagar, conciliación, reportes financieros |

## Bounded Contexts (DDD)

1. `Shipping` (Freight Forwarding)
2. `Warehousing` (WMS)
3. `Customs` (Aduanas)
4. `CRM`
5. `Finance` (Accounting)
6. `IdentityAccess` (transversal)

Ningún Bounded Context accede directamente a la base de datos de otro: la comunicación entre contextos es vía eventos de dominio o APIs internas explícitas.

## Stack tecnológico

| Decisión | Tecnología |
|---|---|
| Backend | C# / .NET 8+ |
| Base de datos | PostgreSQL |
| Arquitectura | Clean Architecture + DDD |
| Patrón de aplicación | CQRS |
| Mensajería interna | Outbox Pattern + eventos de dominio |
| Acceso a datos | Dapper (micro-ORM, solo en capa de infraestructura) |
| Frontend | React + Vite + TypeScript |
| Infraestructura | Docker + CI/CD |

Cambiar cualquier decisión de esta tabla requiere un ADR aprobado (ver [`docs/constitution.md`](docs/constitution.md), Artículo III).

## Arquitectura (Clean Architecture)

```
Domain          → no depende de nada (ni de Dapper, ni de ASP.NET)
Application     → depende solo de Domain (Commands, Queries, Handlers, interfaces)
Infrastructure  → depende de Application y Domain (implementa interfaces: repos con Dapper, mensajería)
API/Presentation→ depende de Application (controllers finos, sin lógica de negocio)
```

## Calidad

- Todo Aggregate y Domain Service tiene unit tests sin dependencias externas.
- Todo Command Handler tiene al menos un test de integración.
- Cobertura mínima de dominio: **70%**, gate de CI.
- Ningún Pull Request se mergea sin pasar el checklist de calidad de la constitution.

## Documentación

- [`docs/constitution.md`](docs/constitution.md) — principios de arquitectura e ingeniería no negociables (DDD, CQRS, capas, testing, seguridad, ADRs). Es la fuente de verdad de *cómo* se construye el sistema.
- [`docs/plan.md`](docs/plan.md) — plan de proyecto: alcance, fases, cronograma y modelo de ejecución. Define *qué* y *cuándo*.

## Flujo de contribución

La rama `main` está protegida mediante un [Repository Ruleset](.github/rulesets/protect-main.json) de GitHub: todo cambio se integra mediante **Pull Request** (bloquea force-push y borrado de la rama), sin excepción salvo para el rol admin del repositorio (el propietario).

```bash
git checkout -b feature/mi-cambio
# ... cambios ...
git push -u origin feature/mi-cambio
gh pr create --base main
```
