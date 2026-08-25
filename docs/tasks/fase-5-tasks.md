# Fase 5 — Tareas: Accounting

**Spec:** [`/docs/specs/fase-5-spec.md`](../specs/fase-5-spec.md)

## 1. Discovery y Modelado de Dominio
1.1. Relevar el proceso de facturación, CxC/CxP y conciliación con el cliente.
1.2. Modelar los Aggregates `Invoice`, `Payment` y las entidades `LedgerEntry`/`AccountReceivable`/`AccountPayable`.
1.3. Definir el cálculo de peso volumétrico facturable y su dependencia de mediciones recapturadas (Art. XIII.3).

## 2. Backend — Application y Domain
2.1. Implementar Aggregates `Invoice`, `Payment` y entidades de ledger.
2.2. Implementar consumidores de eventos `ShipmentCreated`, `BillOfLadingIssued` (Shipping) y `PackageConsolidated` (Warehousing) para generación/recalculo de facturas.
2.3. Implementar Command Handlers: `CreateInvoiceCommand`, `RecordPaymentCommand`, `ReconcileAccountCommand`, `RecalculateBillableWeightCommand`.
2.4. Implementar Query Handlers: `GetFinancialReportQuery`, `GetAgingReportQuery`, `GetInvoiceDetailsQuery` (read models pesados).
2.5. Implementar cifrado de datos fiscales/financieros sensibles en tránsito y reposo.
2.6. Registrar modificaciones de facturas en el audit log inmutable.
2.7. Implementar publicación de eventos `InvoiceIssued`, `PaymentRecorded`.

## 3. Frontend
3.1. Construir flujo de facturación.
3.2. Construir gestión de CxC/CxP.
3.3. Construir reportes financieros.

## 4. Testing y Calidad
4.1. Unit tests de Aggregates `Invoice`, `Payment`.
4.2. Tests de integración de cada Command Handler, incluyendo el recalculo tras `PackageConsolidated`.
4.3. Verificar cobertura ≥70% en Domain/Application de `Finance & Accounting`.

## 5. Cierre de Fase
5.1. Checklist de calidad contra `constitution.md` (especial atención a Art. VIII.1 y XIII.3-4).
5.2. Demo funcional al cliente y aceptación.
5.3. Despliegue en staging.
5.4. Registrar deuda técnica pendiente, si la hay.

---

## Checklist de Cierre — Fase 5

- [ ] Aggregates `Invoice`, `Payment` implementados.
- [ ] Facturación, CxC/CxP y conciliación funcionando end-to-end.
- [ ] Recalculo de peso volumétrico tras repack basado siempre en medición recapturada, nunca asumida.
- [ ] Datos fiscales/financieros sensibles cifrados en tránsito y en reposo.
- [ ] Modificaciones de facturas registradas en audit log inmutable.
- [ ] Reportes financieros (read model) funcionando de forma desacoplada del modelo transaccional.
- [ ] Cobertura ≥70% en Domain/Application de `Finance & Accounting`.
- [ ] Desplegado en staging y demostrado/aceptado por el cliente.
