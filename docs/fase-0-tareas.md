# Fase 0 — Discovery, Modelado DDD, Arquitectura Base e Identity & Access
### Plan de Tareas y Checklist de Cierre

**Referencia:** `plan.md` (Sección 6, Fase 0) y `constitution.md` (todos los artículos, ya que Fase 0 es la que instala las reglas que gobiernan el resto del proyecto).

**Duración estimada:** 4-6 semanas.

**Entregable de fase (según plan.md):** Documento de bounded contexts, ambiente base, CI/CD funcionando.

**Definición de éxito de la fase:** al cerrar Fase 0, cualquier módulo de negocio (Fase 1 en adelante) puede empezar a construirse el día 1 sin decisiones de arquitectura pendientes, sobre un esqueleto de Clean Architecture + DDD + CQRS + Dapper ya validado end-to-end con Identity & Access funcionando.

No se genera código de negocio en esta fase más allá de lo estrictamente necesario para validar el esqueleto técnico (un "walking skeleton").

---

## 1. Workstream: Discovery del Dominio

Objetivo: entender el negocio lo suficiente para modelar los Bounded Contexts, antes de tocar una línea de infraestructura.

1.1. Relevar el proceso de negocio end-to-end de freight forwarding con el cliente (cotización → shipment → BL/AWB → tracking → facturación), usando entrevistas o material existente (Magaya como referencia funcional).
1.2. Documentar el **Ubiquitous Language** inicial: glosario de términos de negocio (Shipment, Booking, BL/AWB, Consolidación, HBL/MBL, etc.) consensuado con el cliente.
1.3. Identificar los **Bounded Contexts** definitivos y sus límites, validando los 6 propuestos en `plan.md` (Shipping, Warehousing, Customs & Compliance, CRM & Sales, Finance & Accounting, Identity & Access).
1.4. Mapear las relaciones entre contextos (Context Map): quién es upstream/downstream, qué eventos de integración cruzan de un contexto a otro.
1.5. Identificar los primeros Aggregates candidatos dentro de `Shipping` (el contexto de Fase 1), con sus invariantes de negocio principales.
1.6. Documentar decisiones de discovery y Context Map en `/docs/domain/` (glosario, bounded contexts, context map).

## 2. Workstream: Arquitectura Base y Estructura de Solución

Objetivo: dejar el esqueleto de Clean Architecture funcionando, respetando `constitution.md` Artículos I, III y IV.

2.1. Definir estructura de carpetas/proyectos por Bounded Context, respetando capas: `Domain`, `Application`, `Infrastructure`, `API/Presentation`.
2.2. Configurar reglas de dependencia entre capas (verificación de que `Domain` no referencia infraestructura ni Dapper ni ASP.NET) — decidir cómo se va a enforcer (análisis estático / arquitectura tests) sin escribir el código todavía, solo dejar la tarea planteada para Fase 1 si corresponde.
2.3. Definir convención de organización de Commands/Queries/Handlers (CQRS) por contexto.
2.4. Definir el enfoque de acceso a datos con Dapper: convención de repositorios de escritura (mapeo explícito Aggregate ↔ filas SQL) y de Query Handlers (SQL crudo / proyecciones de lectura).
2.5. Definir el approach de Outbox Pattern para mensajería entre contextos (esquema de tabla outbox, proceso de publicación, a nivel de diseño).
2.6. Definir convenciones de versionado de esquema de base de datos (estrategia de migraciones sin EF Core, ej. scripts SQL versionados / herramienta de migración compatible con Dapper).
2.7. Documentar la arquitectura base resultante en `/docs/architecture/` (diagrama de capas, diagrama de dependencias, convenciones).

## 3. Workstream: Identity & Access (transversal)

Objetivo: tener el control de acceso funcionando desde el día 1 (constitution Artículo VIII.2).

3.1. Definir el modelo de autenticación (ej. JWT, OpenID Connect / proveedor externo vs. propio) — decisión a nivel de diseño.
3.2. Definir el modelo de autorización basado en roles (RBAC): catálogo inicial de roles y permisos transversales a los módulos.
3.3. Definir cómo se propaga la identidad del usuario autenticado hacia los Command/Query Handlers (contexto de ejecución, claims necesarios).
3.4. Definir el diseño del **audit log** inmutable (constitution Artículo VIII.3): qué se audita, estructura del registro, dónde se almacena.
3.5. Documentar el diseño de `IdentityAccess` en `/docs/domain/identity-access.md`.

## 4. Workstream: Infraestructura, DevOps y CI/CD

Objetivo: ambiente base reproducible y pipeline de CI/CD funcionando (entregable explícito de la fase).

4.1. Definir estrategia de contenedores (Docker) para backend, base de datos PostgreSQL y frontend.
4.2. Definir ambientes: local (docker-compose), staging, y (si aplica) producción.
4.3. Definir pipeline de CI: build, ejecución de tests, gate de cobertura mínima del 70% en Domain/Application (constitution Artículo V.3).
4.4. Definir pipeline de CD hacia staging (despliegue automático o semi-automático tras merge a rama principal).
4.5. Definir estrategia de gestión de secretos/configuración por ambiente.
4.6. Definir estrategia de logging y observabilidad mínima (logs estructurados, correlación de requests) para poder debuggear el walking skeleton.
4.7. Documentar el setup de infraestructura en `/docs/infrastructure/README.md`.

## 5. Workstream: Frontend — Bootstrap

Objetivo: dejar decidido el andamiaje de la SPA (React + Vite + TypeScript), sin construir features de negocio todavía.

5.1. Definir estructura de carpetas del frontend (por feature/módulo, alineado a los Bounded Contexts).
5.2. Definir librería de manejo de estado/data-fetching (ej. React Query/TanStack Query u otra) y librería de UI/componentes base.
5.3. Definir convención de comunicación con el backend (cliente HTTP, manejo de autenticación/token, manejo de errores).
5.4. Definir estrategia de theming/UX base y librería de formularios (relevante por la cantidad de formularios operativos del dominio).
5.5. Documentar las decisiones de frontend en `/docs/architecture/frontend.md`.

## 6. Workstream: Walking Skeleton (validación end-to-end del esqueleto)

Objetivo: probar que la arquitectura definida funciona de punta a punta con el mínimo caso posible, **sin** construir funcionalidad real de negocio.

6.1. Definir un caso trivial de prueba (ej. un health-check o un caso mínimo tipo "crear entidad de ejemplo") que atraviese: API → Command Handler → Dominio → Repositorio Dapper → PostgreSQL, y su contraparte de lectura vía Query Handler.
6.2. Validar que el caso de prueba respeta la separación de capas y CQRS definida.
6.3. Validar que el pipeline de CI/CD despliega ese caso de prueba en staging correctamente.
6.4. Retirar o marcar explícitamente como descartable el código del walking skeleton antes de iniciar Fase 1 (no se hereda como deuda técnica silenciosa).

## 7. Workstream: Gobernanza, ADRs y Cierre de Fase

Objetivo: dejar registrada la toma de decisiones y preparar el cierre formal de Fase 0 frente al cliente.

7.1. Redactar como **ADR** (`/docs/adr/`) toda decisión relevante y difícil de revertir tomada durante Fase 0 (ej. elección de proveedor de identidad, estrategia de migraciones SQL, librería de estado en frontend).
7.2. Revisar que ninguna decisión de Fase 0 contradiga `constitution.md`; si alguna lo hace, documentarla como ADR y obtener aprobación del Tech Lead antes de avanzar.
7.3. Preparar la demo de cierre de Fase 0 para el cliente (mostrar: bounded contexts documentados, ambiente corriendo, CI/CD ejecutando, walking skeleton funcionando).
7.4. Actualizar `plan.md` si el discovery cambió el orden o alcance de fases siguientes (requiere acuerdo con el cliente, Artículo IX de la constitution).
7.5. Registrar explícitamente cualquier deuda técnica tomada durante Fase 0 como ticket, según constitution Artículo VI.

---

## Checklist de Cierre de Fase 0 (Definition of Done de la fase)

Usar este checklist como gate formal antes de dar la fase por cerrada y pasar a Fase 1.

### Discovery y Dominio
- [ ] Glosario de Ubiquitous Language documentado y validado con el cliente.
- [ ] Los 6 Bounded Contexts están definidos y documentados con sus límites y responsabilidades.
- [ ] Context Map (relaciones entre contextos y eventos de integración) documentado.
- [ ] Aggregates candidatos de `Shipping` identificados para arrancar Fase 1.

### Arquitectura y Backend
- [ ] Estructura de solución (Domain/Application/Infrastructure/API) creada y respetada por Bounded Context.
- [ ] Regla de "Domain no depende de infraestructura" validada (manual o con herramienta).
- [ ] Convención de Commands/Queries/Handlers (CQRS) definida y documentada.
- [ ] Convención de acceso a datos con Dapper (write-side y read-side) definida y documentada.
- [ ] Diseño del Outbox Pattern definido.
- [ ] Estrategia de migraciones de base de datos (sin EF Core) definida.

### Identity & Access
- [ ] Modelo de autenticación definido.
- [ ] Modelo de RBAC (roles y permisos iniciales) definido.
- [ ] Diseño de audit log inmutable definido.

### Infraestructura y CI/CD
- [ ] Ambiente local reproducible (docker-compose) funcionando.
- [ ] Pipeline de CI corriendo build + tests + gate de cobertura 70% en Domain/Application.
- [ ] Pipeline de CD desplegando a staging.
- [ ] Estrategia de logging/observabilidad mínima implementada.

### Frontend
- [ ] Estructura base del proyecto React + Vite + TypeScript creada.
- [ ] Librerías base (estado/data-fetching, UI, formularios) seleccionadas y documentadas.
- [ ] Estrategia de autenticación/manejo de token en frontend definida.

### Walking Skeleton
- [ ] Caso end-to-end (API → Command/Query → Dominio → Dapper → PostgreSQL) corriendo en local.
- [ ] Caso end-to-end desplegado y validado en staging vía el pipeline de CI/CD.
- [ ] Código del walking skeleton retirado o marcado explícitamente antes de iniciar Fase 1.

### Gobernanza y Cierre
- [ ] Todas las decisiones relevantes de Fase 0 registradas como ADR en `/docs/adr/`.
- [ ] Ninguna decisión de Fase 0 contradice `constitution.md` sin ADR aprobado.
- [ ] Deuda técnica de Fase 0 (si existe) registrada explícitamente como ticket.
- [ ] Demo de cierre de Fase 0 realizada y aceptada por el cliente/Product Owner.
- [ ] `plan.md` actualizado si hubo cambios de alcance u orden de fases siguientes.
