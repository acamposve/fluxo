# Fase 2 — Tareas: Warehouse Management (WMS)

**Spec:** [`/docs/specs/fase-2-spec.md`](../specs/fase-2-spec.md)

## 1. Discovery y Modelado de Dominio
1.1. Relevar el proceso operativo de almacén (recepción, almacenaje, picking, packing, cross-docking) con el cliente.
1.2. Modelar los Aggregates `Warehouse`, `InventoryItem`, `Package` y sus invariantes.
1.3. Definir el ciclo de vida/estados de `Package` dejando espacio para la extensión de repack de Fase 3 (Art. XIII).

## 2. Backend — Application y Domain
2.1. Implementar Aggregates `Warehouse` (con `Location`/`Bin`), `InventoryItem`, `Package`.
2.2. Implementar consumidor del evento `ShipmentCreated` (Shipping) para disparar recepción.
2.3. Implementar Command Handlers: `ReceiveGoodsCommand`, `CreatePackageCommand`, `MoveInventoryCommand`, `PickCommand`, `PackCommand`, `AdjustInventoryCommand`.
2.4. Implementar Query Handlers: `GetInventoryLevelsQuery`, `GetWarehouseDashboardQuery`, `GetPackageDetailsQuery`.
2.5. Implementar publicación de eventos `GoodsReceived`, `InventoryAdjusted` (Outbox Pattern).
2.6. Registrar recepción y ajustes de inventario relevantes en el audit log.

## 3. Frontend
3.1. Construir flujo de recepción de mercancía.
3.2. Construir gestión de almacenaje/ubicaciones.
3.3. Construir flujo de picking/packing.
3.4. Construir dashboard de inventario.

## 4. Testing y Calidad
4.1. Unit tests de Aggregates `Warehouse`, `InventoryItem`, `Package`.
4.2. Tests de integración de cada Command Handler, incluyendo el consumo del evento `ShipmentCreated`.
4.3. Verificar cobertura ≥70% en Domain/Application de `Warehousing`.

## 5. Cierre de Fase
5.1. Checklist de calidad contra `constitution.md`.
5.2. Demo funcional al cliente y aceptación.
5.3. Despliegue en staging.
5.4. Registrar deuda técnica pendiente, si la hay.

---

## Checklist de Cierre — Fase 2

- [ ] Aggregates `Warehouse`, `InventoryItem`, `Package` implementados.
- [ ] Recepción vinculada al evento `ShipmentCreated` de Shipping.
- [ ] Almacenaje, picking/packing e inventario operativos end-to-end.
- [ ] Eventos `GoodsReceived`, `InventoryAdjusted` publicados vía Outbox.
- [ ] `Package` diseñado de forma extensible para la operación de repack de Fase 3.
- [ ] Cobertura ≥70% en Domain/Application de `Warehousing`.
- [ ] Desplegado en staging y demostrado/aceptado por el cliente.
