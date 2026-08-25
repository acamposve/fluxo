# Spec — Fase 5: Accounting

**Referencia:** `plan.md` Sección 6 (Fase 5) · `constitution.md` Artículos I, II, IV, V, VIII, XIII.3-4.

## 1. Objetivo

Construir facturación, cuentas por cobrar/pagar, conciliación y reportería financiera.

## 2. Bounded Context involucrado

`Finance & Accounting`.

## 3. Alcance funcional

- Generación de facturas a partir de eventos de Shipping (`ShipmentCreated`, `BillOfLadingIssued`) y Warehousing (`PackageConsolidated` para recálculo de peso volumétrico, Art. XIII.3).
- Cuentas por cobrar (CxC) y por pagar (CxP).
- Conciliación de pagos.
- Reportes financieros (read models pesados, Art. II.2 y Art. II.5).

## 4. Modelo de dominio candidato

- **Aggregate** `Invoice`.
- **Aggregate** `Payment`.
- **Entity** `LedgerEntry` / `AccountReceivable` / `AccountPayable`.

## 5. Commands principales

- `CreateInvoiceCommand`
- `RecordPaymentCommand`
- `ReconcileAccountCommand`
- `RecalculateBillableWeightCommand` (disparado al consumir `PackageConsolidated`)

## 6. Queries principales

- `GetFinancialReportQuery`
- `GetAgingReportQuery` (CxC/CxP)
- `GetInvoiceDetailsQuery`

## 7. Eventos de integración

- Consume: `ShipmentCreated`, `BillOfLadingIssued` (Shipping); `PackageConsolidated` (Warehousing, Fase 3).
- Publica: `InvoiceIssued`, `PaymentRecorded` — relevantes para Customs (Fase 6) si la documentación aduanera referencia el valor facturado.

## 8. Requisitos no funcionales / artículos aplicables

- Todo dato fiscal/financiero cifrado en tránsito y en reposo (Art. VIII.1).
- Modificar una factura queda en audit log inmutable (Art. VIII.3).
- El recálculo de peso volumétrico tras un repack nunca se asume: siempre usa la medición recapturada (Art. XIII.3).
- Ningún cargo puede "perderse" tras un repack (Art. XIII.4) — la trazabilidad de `Package` debe ser suficiente para sostener esta regla.

## 9. Fuera de alcance de esta fase

- Documentación aduanera con valor declarado (Fase 6, Customs, aunque consuma estos eventos).

## 10. Criterio de aceptación / entregable

Facturación, CxC/CxP y reportes financieros operativos, con tests y demo aceptada, según `plan.md`.
