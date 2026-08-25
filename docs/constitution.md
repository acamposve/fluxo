# Constitution del Proyecto
### Plataforma Digital de Freight Forwarding — Principios de Arquitectura e Ingeniería

**Versión:** 1.2
**Última actualización:** 2026-08-25 — v1.1 incorpora despliegue híbrido Cloud + On-Premise (Artículo X), app móvil mínima (Artículo XI), integración de hardware de dimensioning (Artículo XII) y Repack & Consolidation (Artículo XIII). v1.2 agrega la app de escritorio (Artículo XIV) para puestos fijos con hardware cableado.
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
| Frontend móvil | React Native (Expo) | Fijo |
| Frontend escritorio | Electron + React (empaqueta la SPA web) | Fijo |
| Modelo de despliegue | Contenedores Docker; mismo artefacto para Cloud (SaaS multi-tenant) y On-Premise (single-tenant) | Fijo |
| Integración de hardware | Adaptadores en `Infrastructure` (patrón Adapter) por dispositivo; sin SDKs de hardware en `Domain`/`Application` | Fijo |

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

## Artículo X — Modelo de Despliegue Híbrido (Cloud + On-Premise)

1. El sistema se distribuye como un único conjunto de contenedores Docker (Artículo III); no existen dos versiones de código para Cloud y On-Premise, solo dos formas de desplegar el mismo artefacto.
2. **Cloud**: despliegue SaaS multi-tenant, con aislamiento de datos por tenant (a nivel de esquema o base de datos, decisión que se fija por ADR al implementarse), para clientes que no quieren operar infraestructura propia.
3. **On-Premise**: despliegue single-tenant en la infraestructura del cliente (servidor propio o su nube privada), vía Docker Compose como mínimo soportado, o Kubernetes si el cliente ya lo opera. No requiere conectividad permanente a servidores nuestros para operar.
4. Ninguna capa de `Domain` o `Application` puede depender de un servicio propietario de un proveedor cloud específico (colas, storage, IAM). Toda dependencia de infraestructura externa se aísla detrás de una interfaz definida en `Application` e implementada en `Infrastructure` (mismo principio de capas del Artículo IV), de forma que pasar de Cloud a On-Premise, o cambiar de proveedor, sea un cambio de configuración/infraestructura, nunca de dominio.
5. Justificación: clientes de freight forwarding suelen operar en zonas con conectividad poco confiable, o tienen políticas internas que exigen que los datos de aduanas/facturación vivan en su propia infraestructura. Soportar ambos modos sin mantener dos bases de código es lo que hace esto viable con el modelo de un solo desarrollador (`plan.md`, Sección 5).

## Artículo XI — Aplicación Móvil

1. La app móvil es un **cliente delgado**: consume los mismos Commands/Queries que expone `Application` a la SPA web (Artículo II). No implementa lógica de negocio propia ni duplica reglas de dominio.
2. Alcance deliberadamente mínimo: consulta de estado/tracking de shipments, aprobaciones simples (cotizaciones, documentación), notificaciones, y captura de datos en el piso de operación (dimensioning, evidencia fotográfica de repack, escaneo de código de barra). Ampliar este alcance a un ERP móvil completo requiere un ADR.
3. Tecnología: React Native (Expo), para reusar tipos y contratos de API con la SPA (TypeScript) y minimizar el costo de mantener un segundo frontend con un equipo de una persona.
4. Las pantallas de captura en el piso de operación (dimensioning, recepción de mercancía) deben tolerar conectividad intermitente: encolan la operación localmente y sincronizan cuando hay señal; nunca bloquean al operario porque "no hay internet".

## Artículo XII — Integración de Hardware (Dimensioning)

1. Toda báscula o dimensionador (equipo que mide peso y volumen de una carga) se integra mediante un **adaptador** en `Infrastructure` que implementa una interfaz común (`IDimensioningDeviceAdapter`) definida en `Application`. `Domain` y `Application` no conocen protocolos de hardware (serial, USB, Bluetooth, SDK del fabricante).
2. La captura automática por hardware y la captura manual llegan al sistema por el **mismo Command** (ej. `RecordPackageMeasurementCommand`). El origen del dato (dispositivo vs. manual) es un atributo, no un camino distinto que se salte las validaciones del dominio.
3. La ausencia o falla de un dispositivo nunca bloquea la operación: la captura manual es el fallback por defecto y debe estar siempre disponible, sin excepción.
4. Cada integración de un fabricante/dispositivo nuevo es un adaptador aislado y aditivo; no debe requerir modificar los adaptadores existentes (Open/Closed).

## Artículo XIII — Repack & Consolidation

1. `Package` (bulto/caja) es un **Aggregate** de primera clase dentro de `Warehousing`, no un campo suelto de `Shipment`.
2. Una operación de repack/consolidación (combinar N paquetes origen en uno o más paquetes destino) es una operación de dominio atómica que debe preservar **trazabilidad completa**: el paquete resultante mantiene referencia a todos sus paquetes de origen, y estos quedan marcados como consolidados, nunca eliminados. Esta trazabilidad es un invariante de dominio, no un detalle de UI.
3. El peso y las dimensiones del paquete resultante de un repack se **vuelven a capturar** (idealmente vía integración de dimensioning, Artículo XII), nunca se asumen sumando los paquetes de origen: consolidar reduce espacio muerto y cambia el peso volumétrico real, que es la base de la facturación de flete.
4. Finance y Customs siguen referenciando los paquetes/shipments originales a través de la trazabilidad del punto 2, incluso después de un repack — ningún cargo ni documento aduanero puede "perderse" porque la mercancía cambió de caja físicamente.
5. Toda operación de repack queda registrada en el audit log inmutable (Artículo VIII.3), incluyendo evidencia fotográfica antes/después cuando la captura se hace desde la app móvil.
6. Justificación de negocio: Magaya no ofrece consolidación de paquetes con generación de un tercero manteniendo trazabilidad — es un diferenciador competitivo explícito de este proyecto (ver `plan.md`, Diferenciador clave).

## Artículo XIV — Aplicación de Escritorio

1. La app de escritorio es otro **cliente delgado**: empaqueta la misma SPA web mediante Electron y consume los mismos Commands/Queries de `Application`. No duplica lógica de negocio ni mantiene un modelo de datos propio.
2. Su razón de ser es el acceso nativo que un navegador no da de forma confiable: puertos serial/USB para básculas y dimensionadores (Artículo XII), e impresión directa en impresoras térmicas de etiquetas y documentación (BL/AWB, código de barra).
3. Se usa en puestos fijos de operación (recepción, empaque, dimensioning) donde el hardware está cableado a una PC — a diferencia de la app móvil (Artículo XI), que cubre el caso de operario o gestión sin atarse a un puesto fijo.
4. Los adaptadores de hardware específicos de escritorio (drivers de báscula/dimensionador, drivers de impresión) viven en el proceso de Electron, nunca en el código de la SPA compartido con la web — así la SPA web sigue funcionando igual sin Electron cuando no hay hardware local que atender.
5. Igual que la app móvil, debe tolerar operar sin conectividad hacia el backend y encolar localmente lo capturado hasta poder sincronizar.

## Artículo XV — Enmiendas

Esta constitution puede modificarse, pero:
- Cualquier cambio se propone por escrito.
- Se discute en equipo.
- Se versiona (este documento lleva número de versión y fecha).
- Un cambio de artículo no aplica retroactivamente a código ya escrito bajo la versión anterior, salvo que se decida explícitamente una migración.
