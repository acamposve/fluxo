# Spec — Fase 0: Discovery, Arquitectura Base, Identity & Access

**Referencia:** `plan.md` Sección 6 (Fase 0) · `constitution.md` Artículos I, II, III, IV, V, VIII, X.

## 1. Objetivo

Dejar sentadas las bases de dominio, arquitectura y seguridad sobre las que se construyen todas las fases siguientes, sin construir funcionalidad de negocio real (más allá de un walking skeleton).

## 2. Bounded Contexts involucrados

- Todos: a nivel de **definición** (glosario, límites, context map).
- `IdentityAccess`: a nivel de **implementación** (es el único contexto con código de negocio real en esta fase).

## 3. Alcance funcional

- Glosario de Ubiquitous Language y Context Map de los 6 Bounded Contexts (`constitution.md` Art. I.2).
- Autenticación y autorización (RBAC) básica: usuarios, roles, permisos.
- Audit log inmutable (mecanismo transversal, `constitution.md` Art. VIII.3), sin reglas de negocio de auditoría específicas de otros módulos todavía.
- Walking skeleton: un caso mínimo que atraviesa API → Command Handler → Domain → Repositorio Dapper → PostgreSQL, y su contraparte de lectura.

## 4. Modelo de dominio candidato (`IdentityAccess`)

- **Aggregate** `User` (Value Objects: `Email`, `PasswordHash`).
- **Aggregate** `Role` (contiene `Permission[]`).
- **Entity/Read model** `AuditLogEntry` (inmutable, solo insert).

## 5. Commands principales

- `CreateUserCommand`
- `AssignRoleCommand`
- `RecordAuditEntryCommand` (o mecanismo transversal equivalente, ej. interceptor/decorator de Command Handlers en vez de un command explícito — a decidir en diseño)

## 6. Queries principales

- `GetUserPermissionsQuery`
- `GetAuditLogQuery` (filtrable por entidad/usuario/fecha)

## 7. Eventos de integración

- `UserCreated` — publicado por `IdentityAccess`, consumido potencialmente por otros contextos para sus propios read models de "quién hizo qué" sin acceso directo a la tabla de usuarios (`constitution.md` Art. I.3).

## 8. Requisitos no funcionales / artículos aplicables

- Estructura de capas y regla de dependencias del Art. IV (Domain sin referencias a Dapper/ASP.NET).
- CQRS puro desde el día 1 (Art. II).
- Gate de cobertura 70% en Domain/Application (Art. V.3).
- RBAC desde el día 1, no "para después" (Art. VIII.2).
- Preparar el artefacto Docker para que sea desplegable en modo Cloud u On-Premise más adelante (Art. X.1), aunque la validación completa de ambos modos es Fase 3.

## 9. Fuera de alcance de esta fase

- Cualquier Aggregate de negocio de Shipping/Warehousing/CRM/Finance/Customs.
- Frontend más allá de lo necesario para probar login/RBAC del walking skeleton.
- Validación real de despliegue On-Premise (eso es Fase 3).

## 10. Criterio de aceptación / entregable

Documento de bounded contexts + ambiente base + CI/CD funcionando + walking skeleton desplegado en staging, según `plan.md`.
