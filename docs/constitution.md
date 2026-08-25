# Constitution del Proyecto
### Plataforma Digital de Freight Forwarding — Principios de Arquitectura e Ingeniería

**Versión:** 1.0
**Alcance:** Aplica a todo el código, decisiones de diseño y procesos de este proyecto, para todos los módulos (Forwarding, WMS, Customs, CRM, Accounting) y para todo miembro del equipo, presente o futuro.

**Propósito:** Este documento es la fuente de verdad de "cómo construimos" el sistema. A diferencia del `plan.md` (que dice *qué* y *cuándo*), esta constitution dice *cómo* y *por qué*, y sus reglas no se rompen sin un ADR (Architecture Decision Record) que lo justifique y sea aprobado por el Tech Lead.

---

## Artículo I — Domain-Driven Design es la ley del dominio

1. El negocio (freight forwarding, aduanas, contabilidad, etc.) es **complejo y regulado**. El código debe reflejar el lenguaje del negocio (**Ubiquitous Language**), no el lenguaje de la base de datos ni el de un framework.
2. El sistema se divide en **Bounded Contexts** independientes:
   - `Shipping` (Freight Forwarding)
   - `Warehousing` (WMS)
   - `Customs` (Aduanas)
   - `CRM`
   - `Finance` (Accounting)
   - `IdentityAccess` (transversal)
3. **Ningún Bounded Context accede directamente a la base de datos de otro.** La comunicación entre contextos es exclusivamente vía **eventos de dominio** (integration events) o **APIs internas explícitas**. Nunca vía joins de SQL cruzados ni entidades compartidas.
4. Toda entidad de negocio central se modela como **Aggregate** con un **Aggregate Root** claro, que es el único punto de entrada para modificar su estado y garantizar sus invariantes.
5. La lógica de negocio vive en el dominio (entidades, value objects, domain services), **nunca en controllers, nunca en la capa de infraestructura.**

## Artículo II — CQRS es el patrón de aplicación estándar

1. Toda operación de escritura se modela como un **Command** (ej. `CreateShipmentCommand`, `ReceiveGoodsCommand`) manejado por un único `CommandHandler`.
2. Toda operación de lectura se modela como una **Query**, y **puede** usar un modelo de datos optimizado para lectura (read model), separado del modelo transaccional, especialmente en:
   - Reportería contable (Accounting)
   - Tracking de carga (Shipping)
   - Dashboards operativos (WMS)
3. Los Commands **no devuelven datos de negocio**, solo confirmación/identificador. Si la UI necesita ver el resultado, hace una Query separada.
4. No se permite lógica de negocio dentro de un Query Handler: las queries **solo leen**.
5. La justificación de este patrón: el módulo de Accounting y el de tracking tienen patrones de lectura muy pesados y distintos a los de escritura operativa; separar ambos caminos es lo que permite escalar cada uno de forma independiente sin sobre-ingeniería prematura en el resto del sistema.

## Artículo III — Stack Tecnológico (no se cambia sin ADR)

| Decisión | Tecnología | Estado |
|---|---|---|
| Lenguaje backend | C# / .NET 8+ | Fijo |
| Base de datos | PostgreSQL | Fijo |
| Arquitectura | Clean Architecture + DDD | Fijo |
| Patrón de aplicación | CQRS (con o sin Event Sourcing según contexto) | Fijo |
| Mensajería interna | Outbox Pattern + eventos de dominio | Fijo |
| Acceso a datos | Dapper (micro-ORM, SQL explícito, solo en capa de infraestructura) | Fijo |

Cambiar cualquier fila de esta tabla requiere un **ADR** aprobado, no una decisión unilateral de un desarrollador en un sprint.

## Artículo IV — Capas y dependencias (Clean Architecture)

```
Domain          → no depende de nada (ni de Dapper, ni de ASP.NET)
Application     → depende solo de Domain (Commands, Queries, Handlers, interfaces)
Infrastructure  → depende de Application y Domain (implementa interfaces: repos con Dapper, mensajería)
API/Presentation→ depende de Application (controllers finos, sin lógica de negocio)
```

**Regla dura:** el proyecto `Domain` no puede tener ninguna referencia a `Dapper`, ni a ningún paquete de infraestructura (ni de acceso a datos en general). Si un desarrollador necesita hacerlo "para que funcione rápido", es señal de un problema de diseño, no una excepción válida.

**Nota sobre Dapper y CQRS:** al no haber ORM con tracking de cambios ni change tracker, los repositorios de escritura (write side) mapean explícitamente el Aggregate desde/hacia filas SQL, y los Query Handlers (read side) pueden usar SQL crudo optimizado (incluso vistas o proyecciones desnormalizadas) sin pasar por el modelo de agregados. Esto encaja naturalmente con la separación CQRS del Artículo II: el read model no tiene por qué respetar el shape del aggregate.

## Artículo V — Testing y calidad

1. Todo Aggregate y Domain Service tiene **unit tests** sin dependencias externas (sin base de datos, sin mocks de infraestructura pesados).
2. Todo Command Handler tiene al menos un test de integración.
3. Cobertura mínima de dominio: **70%**. No es una meta aspiracional, es un gate de CI: el build falla por debajo de ese umbral en el código de `Domain` y `Application`.
4. Ningún Pull Request se mergea sin pasar por un checklist de calidad explícito (contra esta constitution) y, cuando el equipo lo permita, revisión de otra persona. En un modelo de desarrollador único apoyado por IA, la revisión asistida por IA contra este documento reemplaza al segundo revisor humano, pero nunca reemplaza los tests automatizados ni el checklist.

## Artículo VI — Definition of Done (DoD)

Una historia de usuario está "Done" solo si:

- [ ] El código sigue las reglas de esta constitution (capas, DDD, CQRS).
- [ ] Tiene tests automatizados (unitarios + integración donde aplique).
- [ ] Pasó code review.
- [ ] Está desplegado en ambiente de staging.
- [ ] Fue demostrado y aceptado por el Product Owner / cliente en la demo de sprint.
- [ ] No introduce deuda técnica sin documentar (si se toma un atajo, se registra como ticket de deuda técnica explícito).

## Artículo VII — Decisiones Arquitectónicas (ADR)

1. Toda decisión que se desvíe de esta constitution, o que sea significativa y difícil de revertir (ej. elegir Event Sourcing para un contexto específico, cambiar de mensajería in-process a un broker externo), se documenta como **ADR** (Architecture Decision Record) en `/docs/adr/`.
2. Un ADR incluye: contexto, opciones consideradas, decisión tomada, consecuencias.
3. Un ADR lo propone cualquier miembro del equipo, pero lo aprueba el Tech Lead.

## Artículo VIII — Seguridad y Compliance (crítico por el dominio)

Dado que el sistema maneja datos de comercio exterior, aduanas y facturación:

1. Todo dato sensible (información fiscal, datos de clientes, documentación aduanera) se transmite y almacena cifrado (TLS en tránsito, cifrado en reposo para campos sensibles).
2. El módulo `IdentityAccess` implementa control de acceso basado en roles (RBAC) desde el día 1 — no se agrega "después".
3. Toda acción de negocio relevante (crear shipment, modificar factura, aprobar documentación aduanera) queda registrada en un **audit log** inmutable, por requisito regulatorio del dominio aduanero/contable.
4. El módulo de Customs se diseña asumiendo que las reglas regulatorias **cambian por país y en el tiempo** — no se hardcodean reglas arancelarias en el dominio; se modelan como configuración versionada.

## Artículo IX — Gestión del alcance frente al cliente

1. Cada fase del `plan.md` tiene un alcance cerrado y firmado antes de iniciar.
2. Todo pedido de cambio durante una fase en curso se documenta como **Change Request**, se estima aparte, y se prioriza para la fase actual o una siguiente — nunca se absorbe silenciosamente "porque es rápido", ya que eso es lo que erosiona presupuestos y cronogramas en proyectos con clientes sensibles al costo.
3. La transparencia sobre el avance (demos cada 2 semanas) es una obligación del equipo, no una gentileza: es lo que sostiene la confianza del cliente en un proyecto de 18-22 meses.

## Artículo X — Enmiendas

Esta constitution puede modificarse, pero:
- Cualquier cambio se propone por escrito.
- Se discute en equipo.
- Se versiona (este documento lleva número de versión y fecha).
- Un cambio de artículo no aplica retroactivamente a código ya escrito bajo la versión anterior, salvo que se decida explícitamente una migración.
