# Spec — Fase 4: CRM

**Referencia:** `plan.md` Sección 6 (Fase 4) · `constitution.md` Artículos I, II, IV, V, VIII.

## 1. Objetivo

Construir la gestión comercial: clientes, agentes, pipeline de ventas y comunicaciones.

## 2. Bounded Context involucrado

`CRM & Sales`.

## 3. Alcance funcional

- Alta y gestión de clientes y agentes.
- Cotizaciones comerciales (distintas de las `Quote` operativas de Shipping — a diferenciar explícitamente en el discovery de esta fase, evitando duplicar el término en el Ubiquitous Language).
- Pipeline de ventas (oportunidades, etapas).
- Registro de comunicaciones con clientes/agentes.

## 4. Modelo de dominio candidato

- **Aggregate** `Customer` (dentro de CRM; Shipping/Finance mantienen su propia referencia liviana vía eventos, no acceso directo — Art. I.3).
- **Aggregate** `Agent`.
- **Aggregate** `Opportunity` (pipeline comercial, etapas de venta).
- **Entity** `CommunicationLog`.

## 5. Commands principales

- `CreateCustomerCommand`
- `CreateAgentCommand`
- `CreateOpportunityCommand` / `AdvanceOpportunityStageCommand`
- `LogCommunicationCommand`

## 6. Queries principales

- `GetPipelineQuery` (read model de ventas)
- `GetCustomerProfileQuery`

## 7. Eventos de integración

- `CustomerCreated` / `CustomerUpdated` — consumidos por Shipping y Finance para mantener su propio read model de referencia del cliente, sin acceso directo a la base de CRM (Art. I.3).

## 8. Requisitos no funcionales / artículos aplicables

- Alta/baja de clientes y cambios comerciales relevantes en audit log si están dentro del criterio de "acción de negocio relevante" (Art. VIII.3) — a confirmar alcance exacto en discovery de la fase.

## 9. Fuera de alcance de esta fase

- Facturación real (Fase 5, Finance & Accounting) — CRM solo gestiona la relación comercial, no cobra.

## 10. Criterio de aceptación / entregable

Gestión de clientes/agentes y pipeline comercial operativo en la SPA web, con tests y demo aceptada, según `plan.md`.
