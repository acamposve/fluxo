# Fase 6 — Tareas: Customs

**Spec:** [`/docs/specs/fase-6-spec.md`](../specs/fase-6-spec.md)

## 1. Discovery Legal/Regulatorio
1.1. Discovery dedicado de las reglas regulatorias/arancelarias del/los país(es) objetivo, antes de codificar (mitigación de riesgo de `plan.md` Sección 7).
1.2. Modelar `RegulatoryRuleSet` como configuración versionada por país y vigencia temporal (Art. VIII.4), nunca hardcodeada.

## 2. Backend — Application y Domain
2.1. Implementar Aggregate `CustomsDeclaration` y entidad `TariffClassification`.
2.2. Implementar consumidores de eventos `ShipmentCreated`/`BillOfLadingIssued` (Shipping), `InvoiceIssued` (Finance), `PackageConsolidated` (Warehousing) para mantener trazabilidad de mercancía consolidada (Art. XIII.4).
2.3. Implementar Command Handlers: `CreateCustomsDeclarationCommand`, `ClassifyTariffCommand`, `ApproveCustomsDeclarationCommand`, `PublishRegulatoryRuleSetCommand`.
2.4. Implementar Query Handlers: `GetDeclarationStatusQuery`, `GetApplicableTariffRulesQuery`.
2.5. Registrar aprobación de documentación aduanera en el audit log inmutable.
2.6. Implementar publicación del evento `CustomsDeclarationApproved`.

## 3. Frontend
3.1. Construir flujo de generación de documentación aduanera.
3.2. Construir flujo de clasificación arancelaria.
3.3. Construir gestión de reglas regulatorias versionadas (administración).

## 4. Testing y Calidad
4.1. Unit tests de `CustomsDeclaration` y de la resolución de reglas regulatorias por país/fecha.
4.2. Tests de integración de cada Command Handler, incluyendo consumo de `PackageConsolidated`.
4.3. Verificar cobertura ≥70% en Domain/Application de `Customs & Compliance`.

## 5. Cierre de Fase
5.1. Checklist de calidad contra `constitution.md` (especial atención a Art. VIII.4 y XIII.4).
5.2. Demo funcional al cliente y aceptación.
5.3. Despliegue en staging.
5.4. Registrar deuda técnica pendiente, si la hay.

---

## Checklist de Cierre — Fase 6

- [ ] `RegulatoryRuleSet` modelado como configuración versionada por país/tiempo, sin reglas hardcodeadas en el dominio.
- [ ] `CustomsDeclaration` y `TariffClassification` implementados y funcionando end-to-end.
- [ ] Trazabilidad de mercancía consolidada preservada en la documentación aduanera tras un repack.
- [ ] Aprobación de documentación registrada en audit log inmutable.
- [ ] Cobertura ≥70% en Domain/Application de `Customs & Compliance`.
- [ ] Desplegado en staging y demostrado/aceptado por el cliente.
