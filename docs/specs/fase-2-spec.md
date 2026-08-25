# Spec — Fase 2: Warehouse Management (WMS)

**Referencia:** `plan.md` Sección 6 (Fase 2) · `constitution.md` Artículos I, II, III, IV, V, VIII, XIII (base del Aggregate `Package`).

## 1. Objetivo

Construir la gestión de almacenes: recepción, almacenaje, picking/packing, inventario y cross-docking. Introduce el Aggregate `Package` como entidad de primera clase (Art. XIII.1), aunque la operación de **repack/consolidation** en sí se implementa en Fase 3.

## 2. Bounded Context involucrado

`Warehousing`.

## 3. Alcance funcional

- Recepción de mercancía (vinculada al `ShipmentCreated` de Fase 1).
- Almacenaje: ubicación en ambientes/racks/bins.
- Picking y packing para salida/despacho.
- Control de inventario (niveles, ajustes, cross-docking).
- Modelado base de `Package` (bulto/caja) como Aggregate, sin operación de repack todavía.

## 4. Modelo de dominio candidato

- **Aggregate** `Warehouse` (contiene `Location`/`Bin`).
- **Aggregate** `InventoryItem` (cantidad, ubicación, estado).
- **Aggregate** `Package` (Art. XIII.1): peso, dimensiones, contenido, estado (`Received`, `Stored`, `Picked`, `Packed`, `Dispatched`). Sin operación de consolidación aún.

## 5. Commands principales

- `ReceiveGoodsCommand` (consume evento `ShipmentCreated` de Shipping o se dispara manualmente)
- `CreatePackageCommand`
- `MoveInventoryCommand`
- `PickCommand` / `PackCommand`
- `AdjustInventoryCommand`

## 6. Queries principales

- `GetInventoryLevelsQuery`
- `GetWarehouseDashboardQuery` (read model operativo, Art. II.2)
- `GetPackageDetailsQuery`

## 7. Eventos de integración

- Consume: `ShipmentCreated` (Shipping).
- Publica: `GoodsReceived`, `InventoryAdjusted` — consumidos por Shipping (actualizar tracking) y, más adelante, Finance/Customs.

## 8. Requisitos no funcionales / artículos aplicables

- `Package` se modela ya pensando en que Fase 3 le agregará la operación de repack con trazabilidad (Art. XIII.2) — no diseñar el Aggregate de forma que bloquee esa extensión.
- Recepción y ajustes de inventario relevantes quedan en audit log (Art. VIII.3).

## 9. Fuera de alcance de esta fase

- Operación de repack/consolidación de paquetes (Fase 3, Art. XIII).
- Integración de hardware de dimensioning (Fase 3, Art. XII) — la captura de peso/medidas en esta fase es manual.
- App móvil/escritorio (Fase 3).

## 10. Criterio de aceptación / entregable

Recepción, almacenaje, picking/packing e inventario operativos en la SPA web, con tests y demo aceptada, según `plan.md`.
