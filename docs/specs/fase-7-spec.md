# Spec — Fase 7: Integración Final, Hardening, UAT y Go-Live

**Referencia:** `plan.md` Sección 6 (Fase 7) · `constitution.md` (todos los artículos, como validación final de cumplimiento).

## 1. Objetivo

Validar el sistema integrado (los 6 Bounded Contexts + canales móvil/escritorio + ambos modos de despliegue) end-to-end, endurecer seguridad y desempeño, y llevarlo a producción con aceptación formal del cliente.

## 2. Bounded Contexts involucrados

Todos (validación de integración cruzada vía eventos, nunca vía acceso directo entre bases — Art. I.3).

## 3. Alcance funcional

- Pruebas de integración cross-context sobre los flujos completos (cotización → shipment → recepción/WMS → repack → facturación → documentación aduanera).
- Pruebas de carga/desempeño sobre los read models más pesados (tracking, reportería contable — Art. II.5).
- Hardening de seguridad: revisión completa contra Art. VIII (cifrado, RBAC, audit log).
- Validación de despliegue híbrido con datos/carga representativos en infraestructura real del cliente (On-Premise) y en Cloud.
- UAT formal con el cliente y capacitación de usuarios finales.

## 4. Alcance técnico

- Revisión de la regla de dependencias de capas en los 6 Bounded Contexts (Art. IV).
- Verificación del gate de cobertura 70% en Domain/Application de todos los contextos (Art. V.3).
- Runbook de despliegue y soporte para ambos modos (Cloud/On-Premise).
- Plan de rollback por si el go-live de algún módulo falla.

## 5. Eventos de integración

No se introducen eventos nuevos; se valida el conjunto completo definido en fases anteriores (`ShipmentCreated`, `BillOfLadingIssued`, `GoodsReceived`, `PackageConsolidated`, `CustomerCreated`, `InvoiceIssued`, `CustomsDeclarationApproved`, etc.).

## 6. Requisitos no funcionales / artículos aplicables

- Checklist de calidad contra la constitution completa antes del cierre (Art. V.4).
- Cero incidentes críticos en producción durante los primeros 30 días post go-live (`plan.md` Sección 9).

## 7. Fuera de alcance de esta fase

- Nuevas features de negocio (esta fase es de integración/hardening, no de desarrollo de funcionalidad nueva).

## 8. Criterio de aceptación / entregable

Sistema integrado, ambos modos de despliegue validados, pruebas de aceptación firmadas por el cliente y capacitación completada, según `plan.md`.
