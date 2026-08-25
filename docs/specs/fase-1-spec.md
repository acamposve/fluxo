# Spec — Fase 1: Freight Forwarding (Core / Shipping)

**Referencia:** `plan.md` Sección 6 (Fase 1) · `constitution.md` Artículos I, II, III, IV, V, VIII.

## 1. Objetivo

Construir el núcleo de Freight Forwarding: cotización, creación de shipments, emisión de BL/AWB y tracking operativo, como primer Bounded Context de negocio completo.

## 2. Bounded Context involucrado

`Shipping`.

## 3. Alcance funcional

- Cotización de carga (marítima, aérea, terrestre).
- Conversión de cotización aceptada en Shipment (orden de embarque).
- Emisión de Bill of Lading (BL) / Airway Bill (AWB).
- Consolidación de carga a nivel de Shipping (agrupar varios shipments/clientes en un embarque master — MBL/HBL), distinto del repack de paquetes físicos de Warehousing (Fase 3, Art. XIII).
- Tracking operativo del shipment (cambios de estado/hitos).

## 4. Modelo de dominio candidato

- **Aggregate** `Quote` (Value Objects: `Route`, `CargoDetails`, `Price`).
- **Aggregate** `Shipment` (Aggregate Root; referencia `BillOfLading`, hitos de tracking).
- **Entity** `BillOfLading` / `AirwayBill` (dentro del Aggregate `Shipment` o Aggregate propio si su ciclo de vida lo justifica — decisión de diseño en la fase).
- **Aggregate** `Consolidation` (agrupa varios `Shipment` bajo un Master BL/AWB).

## 5. Commands principales

- `CreateQuoteCommand`
- `ConvertQuoteToShipmentCommand`
- `IssueBillOfLadingCommand`
- `ConsolidateShipmentsCommand`
- `UpdateShipmentTrackingStatusCommand`

## 6. Queries principales

- `GetQuoteQuery`
- `GetShipmentDetailsQuery`
- `GetShipmentTrackingQuery` (read model optimizado, Art. II.2)
- `ListShipmentsQuery`

## 7. Eventos de integración

- `ShipmentCreated` — consumido más adelante por Warehousing (Fase 2) y Finance (Fase 5) para iniciar recepción/facturación.
- `BillOfLadingIssued` — relevante para Customs (Fase 6).
- `ShipmentStatusChanged` — alimenta dashboards/tracking de otros contextos.

## 8. Requisitos no funcionales / artículos aplicables

- Emisión de BL/AWB queda en audit log inmutable (Art. VIII.3, "acción de negocio relevante").
- Ningún dato se comparte por join directo con otros contextos; todo vía los eventos anteriores (Art. I.3).
- Read model de tracking desacoplado del modelo transaccional (Art. II.2).

## 9. Fuera de alcance de esta fase

- Recepción/almacenaje físico de la carga (Fase 2, Warehousing).
- Documentación aduanera real (Fase 6, Customs).
- Facturación (Fase 5, Accounting) — Shipping solo emite el evento que la dispara.

## 10. Criterio de aceptación / entregable

SPA web con cotización, shipments, BL/AWB y tracking operativo, con tests y demo aceptada, según `plan.md`.
