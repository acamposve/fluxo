# Fase 1 — Tareas: Freight Forwarding (Core / Shipping)

**Spec:** [`/docs/specs/fase-1-spec.md`](../specs/fase-1-spec.md)

## 1. Discovery y Modelado de Dominio
1.1. Profundizar el Ubiquitous Language de `Shipping` (Quote, Shipment, BL/AWB, Consolidation, hitos de tracking).
1.2. Modelar los Aggregates `Quote`, `Shipment`, `Consolidation` y sus invariantes con el cliente/Product Owner.
1.3. Definir el ciclo de vida y estados de `Shipment` (máquina de estados de tracking).

## 2. Backend — Application y Domain
2.1. Implementar Aggregates y Value Objects (`Quote`, `Shipment`, `BillOfLading`/`AirwayBill`, `Consolidation`).
2.2. Implementar Command Handlers: `CreateQuoteCommand`, `ConvertQuoteToShipmentCommand`, `IssueBillOfLadingCommand`, `ConsolidateShipmentsCommand`, `UpdateShipmentTrackingStatusCommand`.
2.3. Implementar Query Handlers: `GetQuoteQuery`, `GetShipmentDetailsQuery`, `GetShipmentTrackingQuery`, `ListShipmentsQuery`.
2.4. Implementar repositorios Dapper (write-side) y proyecciones de lectura (read-side) para tracking.
2.5. Implementar publicación de eventos de integración: `ShipmentCreated`, `BillOfLadingIssued`, `ShipmentStatusChanged` (Outbox Pattern).
2.6. Integrar emisión de BL/AWB con el audit log inmutable.

## 3. Frontend
3.1. Construir flujo de cotización.
3.2. Construir flujo de creación de shipment y emisión de BL/AWB.
3.3. Construir vista de tracking operativo.
3.4. Construir flujo de consolidación de shipments (Master BL/AWB).

## 4. Testing y Calidad
4.1. Unit tests de Aggregates (`Quote`, `Shipment`, `Consolidation`) sin dependencias externas.
4.2. Tests de integración de cada Command Handler.
4.3. Verificar cobertura ≥70% en Domain/Application del contexto `Shipping`.

## 5. Cierre de Fase
5.1. Checklist de calidad contra `constitution.md`.
5.2. Demo funcional al cliente y aceptación.
5.3. Despliegue en staging.
5.4. Registrar deuda técnica pendiente, si la hay.

---

## Checklist de Cierre — Fase 1

- [ ] Aggregates `Quote`, `Shipment`, `Consolidation` implementados con sus invariantes.
- [ ] Cotización, creación de shipment y emisión de BL/AWB funcionando end-to-end.
- [ ] Consolidación de shipments (MBL/HBL) funcionando.
- [ ] Tracking operativo con read model desacoplado del modelo transaccional.
- [ ] Eventos `ShipmentCreated`, `BillOfLadingIssued`, `ShipmentStatusChanged` publicados vía Outbox.
- [ ] Emisión de BL/AWB registrada en audit log inmutable.
- [ ] Cobertura ≥70% en Domain/Application de `Shipping`.
- [ ] Tests de integración de todos los Command Handlers pasando en CI.
- [ ] Desplegado en staging y demostrado/aceptado por el cliente.
