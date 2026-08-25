# Fase 4 — Tareas: CRM

**Spec:** [`/docs/specs/fase-4-spec.md`](../specs/fase-4-spec.md)

## 1. Discovery y Modelado de Dominio
1.1. Relevar el proceso comercial (alta de clientes/agentes, cotización comercial, pipeline de ventas) con el cliente.
1.2. Diferenciar explícitamente "cotización comercial" (CRM) de `Quote` operativa (Shipping) en el Ubiquitous Language.
1.3. Modelar los Aggregates `Customer`, `Agent`, `Opportunity`.

## 2. Backend — Application y Domain
2.1. Implementar Aggregates `Customer`, `Agent`, `Opportunity`, entidad `CommunicationLog`.
2.2. Implementar Command Handlers: `CreateCustomerCommand`, `CreateAgentCommand`, `CreateOpportunityCommand`, `AdvanceOpportunityStageCommand`, `LogCommunicationCommand`.
2.3. Implementar Query Handlers: `GetPipelineQuery`, `GetCustomerProfileQuery`.
2.4. Implementar publicación de eventos `CustomerCreated`/`CustomerUpdated` (Outbox Pattern).

## 3. Frontend
3.1. Construir gestión de clientes/agentes.
3.2. Construir pipeline de ventas (kanban/etapas).
3.3. Construir registro de comunicaciones.

## 4. Testing y Calidad
4.1. Unit tests de Aggregates `Customer`, `Agent`, `Opportunity`.
4.2. Tests de integración de cada Command Handler.
4.3. Verificar cobertura ≥70% en Domain/Application de `CRM & Sales`.

## 5. Cierre de Fase
5.1. Checklist de calidad contra `constitution.md`.
5.2. Demo funcional al cliente y aceptación.
5.3. Despliegue en staging.
5.4. Registrar deuda técnica pendiente, si la hay.

---

## Checklist de Cierre — Fase 4

- [ ] Aggregates `Customer`, `Agent`, `Opportunity` implementados.
- [ ] Gestión de clientes/agentes y pipeline comercial funcionando end-to-end.
- [ ] Evento `CustomerCreated`/`CustomerUpdated` publicado vía Outbox y consumible por Shipping/Finance.
- [ ] Cobertura ≥70% en Domain/Application de `CRM & Sales`.
- [ ] Desplegado en staging y demostrado/aceptado por el cliente.
