# Spec — Fase 6: Customs

**Referencia:** `plan.md` Sección 6 (Fase 6) · `constitution.md` Artículos I, II, IV, V, VIII.4, XIII.4.

## 1. Objetivo

Construir la generación de documentación aduanera y de comercio exterior, incluyendo clasificación arancelaria, como el módulo de mayor complejidad regulatoria del proyecto.

## 2. Bounded Context involucrado

`Customs & Compliance`.

## 3. Alcance funcional

- Discovery legal/regulatorio dedicado antes de codificar (según riesgo identificado en `plan.md` Sección 7).
- Generación de documentación aduanera a partir de shipments/facturas existentes.
- Clasificación arancelaria de mercancía.
- Reglas regulatorias versionadas por país y en el tiempo (Art. VIII.4).

## 4. Modelo de dominio candidato

- **Aggregate** `CustomsDeclaration`.
- **Entity** `TariffClassification`.
- **Entity/config versionada** `RegulatoryRuleSet` (por país, con vigencia temporal) — Art. VIII.4: nunca hardcodeada en el dominio.

## 5. Commands principales

- `CreateCustomsDeclarationCommand`
- `ClassifyTariffCommand`
- `ApproveCustomsDeclarationCommand`
- `PublishRegulatoryRuleSetCommand` (versión nueva de reglas por país)

## 6. Queries principales

- `GetDeclarationStatusQuery`
- `GetApplicableTariffRulesQuery` (por país y fecha)

## 7. Eventos de integración

- Consume: `ShipmentCreated`/`BillOfLadingIssued` (Shipping), `InvoiceIssued` (Finance), `PackageConsolidated` (Warehousing, para mantener trazabilidad de mercancía consolidada, Art. XIII.4).
- Publica: `CustomsDeclarationApproved`.

## 8. Requisitos no funcionales / artículos aplicables

- Reglas arancelarias como configuración versionada, no hardcodeadas (Art. VIII.4).
- Aprobación de documentación aduanera queda en audit log inmutable (Art. VIII.3).
- Ningún documento aduanero puede perder referencia a la mercancía original tras un repack (Art. XIII.4).

## 9. Fuera de alcance de esta fase

- Integración EDI directa con aduanas/entidades regulatorias (fuera de alcance del proyecto, `plan.md` Sección 3).

## 10. Criterio de aceptación / entregable

Documentación aduanera y clasificación arancelaria operativas, con tests y demo aceptada, según `plan.md`.
