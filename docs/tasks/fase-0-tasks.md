# Fase 0 — Tareas: Discovery, Arquitectura Base, Identity & Access

**Spec:** [`/docs/specs/fase-0-spec.md`](../specs/fase-0-spec.md)

## 1. Discovery del Dominio
1.1. Relevar el proceso de negocio end-to-end (cotización → shipment → BL/AWB → tracking → facturación) con el cliente.
1.2. Documentar el glosario de Ubiquitous Language inicial.
1.3. Validar/ajustar los 6 Bounded Contexts propuestos en `plan.md`.
1.4. Mapear el Context Map (relaciones y eventos de integración entre contextos).
1.5. Documentar todo en `/docs/domain/`.

## 2. Arquitectura Base
2.1. Definir estructura de carpetas/proyectos por capa (Domain/Application/Infrastructure/API) y por Bounded Context.
2.2. Definir cómo se va a verificar la regla "Domain no depende de infraestructura" (análisis estático / arquitectura tests, a nivel de decisión).
2.3. Definir convención de Commands/Queries/Handlers (CQRS).
2.4. Definir convención de acceso a datos con Dapper (write-side y read-side).
2.5. Definir diseño del Outbox Pattern.
2.6. Definir estrategia de migraciones de base de datos (sin EF Core).
2.7. Definir cómo el artefacto Docker soportará, desde su diseño, el despliegue híbrido de Fase 3 (Art. X.1) sin necesidad de reescritura.
2.8. Documentar en `/docs/architecture/`.

## 3. Identity & Access
3.1. Definir modelo de autenticación.
3.2. Definir modelo de RBAC (catálogo inicial de roles/permisos).
3.3. Definir propagación de identidad hacia Command/Query Handlers.
3.4. Diseñar el audit log inmutable.
3.5. Implementar `IdentityAccess` (User, Role, Permission, AuditLogEntry) según spec.
3.6. Documentar en `/docs/domain/identity-access.md`.

## 4. Infraestructura y CI/CD
4.1. Definir/armar docker-compose para ambiente local (backend, PostgreSQL, frontend).
4.2. Definir ambientes (local, staging, producción).
4.3. Armar pipeline de CI: build + tests + gate de cobertura 70% en Domain/Application.
4.4. Armar pipeline de CD hacia staging.
4.5. Definir estrategia de gestión de secretos/configuración por ambiente.
4.6. Definir logging/observabilidad mínima.
4.7. Documentar en `/docs/infrastructure/README.md`.

## 5. Frontend — Bootstrap
5.1. Definir estructura de carpetas del frontend (React + Vite + TypeScript), alineada a Bounded Contexts.
5.2. Definir librería de estado/data-fetching y librería de UI/componentes base.
5.3. Definir convención de comunicación con el backend (cliente HTTP, auth, errores).
5.4. Definir librería de formularios y estrategia de theming base.
5.5. Documentar en `/docs/architecture/frontend.md`.

## 6. Walking Skeleton
6.1. Implementar un caso mínimo end-to-end (API → Command → Domain → Dapper → PostgreSQL) usando `IdentityAccess` (ej. `CreateUserCommand`).
6.2. Validar que respeta capas y CQRS.
6.3. Validar que se despliega correctamente vía CI/CD a staging.
6.4. Retirar o marcar explícitamente como descartable antes de iniciar Fase 1.

## 7. Gobernanza y Cierre
7.1. Redactar ADRs de toda decisión relevante y difícil de revertir tomada en esta fase.
7.2. Verificar que ninguna decisión contradiga `constitution.md` sin ADR aprobado.
7.3. Preparar y realizar la demo de cierre de Fase 0 con el cliente.
7.4. Registrar deuda técnica pendiente, si la hay.

---

## Checklist de Cierre — Fase 0

- [ ] Glosario de Ubiquitous Language documentado y validado con el cliente.
- [ ] Los 6 Bounded Contexts y su Context Map están documentados.
- [ ] Estructura de solución (Domain/Application/Infrastructure/API) creada por Bounded Context.
- [ ] Regla "Domain no depende de infraestructura" validada.
- [ ] Convenciones de CQRS y de acceso a datos con Dapper documentadas.
- [ ] Diseño del Outbox Pattern y de migraciones de BD definidos.
- [ ] `IdentityAccess` (auth, RBAC, audit log) implementado y funcionando.
- [ ] Ambiente local (docker-compose) reproducible.
- [ ] Pipeline de CI con gate de cobertura 70% funcionando.
- [ ] Pipeline de CD a staging funcionando.
- [ ] Estructura base de frontend (React + Vite + TS) creada.
- [ ] Walking skeleton corriendo end-to-end en local y en staging.
- [ ] Código del walking skeleton retirado/marcado antes de Fase 1.
- [ ] ADRs de decisiones relevantes registrados en `/docs/adr/`.
- [ ] Demo de cierre de Fase 0 aceptada por el cliente.
