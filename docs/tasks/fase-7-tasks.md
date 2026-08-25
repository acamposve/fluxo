# Fase 7 — Tareas: Integración Final, Hardening, UAT y Go-Live

**Spec:** [`/docs/specs/fase-7-spec.md`](../specs/fase-7-spec.md)

## 1. Integración Cross-Context
1.1. Ejecutar pruebas de integración del flujo completo: cotización → shipment → recepción/WMS → repack → facturación → documentación aduanera.
1.2. Validar que toda comunicación entre Bounded Contexts sigue siendo exclusivamente vía eventos/APIs internas explícitas (Art. I.3), sin joins ni accesos directos introducidos "para cerrar rápido".

## 2. Desempeño y Carga
2.1. Pruebas de carga sobre los read models más pesados: tracking (Shipping), reportería contable (Finance), dashboards (Warehousing).
2.2. Ajustar índices/proyecciones de lectura donde el desempeño no cumpla los SLA acordados con el cliente.

## 3. Seguridad y Compliance (Hardening)
3.1. Revisión completa contra `constitution.md` Artículo VIII: cifrado en tránsito/reposo, RBAC, audit log, versionado de reglas regulatorias.
3.2. Pruebas de penetración / revisión de vulnerabilidades básicas (OWASP Top 10) sobre API y frontend.
3.3. Revisión de gestión de secretos en todos los ambientes.

## 4. Despliegue Híbrido — Validación Final
4.1. Ejecutar el despliegue completo (los 6 Bounded Contexts + app móvil + app de escritorio) en modo Cloud.
4.2. Ejecutar el despliegue completo en modo On-Premise, en infraestructura real del cliente si es posible.
4.3. Documentar runbook de despliegue y soporte definitivo para ambos modos.
4.4. Definir y probar plan de rollback.

## 5. UAT y Capacitación
5.1. Preparar y ejecutar el plan de UAT con el cliente sobre los 6 módulos integrados.
5.2. Capacitar a los usuarios finales del cliente.
5.3. Obtener firma de aceptación de UAT.

## 6. Cierre de Proyecto (esta fase)
6.1. Checklist de calidad final contra `constitution.md` completa.
6.2. Verificar cobertura ≥70% en Domain/Application en todos los Bounded Contexts.
6.3. Go-live y monitoreo reforzado durante los primeros 30 días (KPI de cero incidentes críticos, `plan.md` Sección 9).
6.4. Cerrar cualquier deuda técnica pendiente registrada en fases anteriores o documentarla como backlog post-go-live.

---

## Checklist de Cierre — Fase 7 (Go-Live)

- [ ] Flujo end-to-end cross-context validado (Shipping → Warehousing → Finance → Customs).
- [ ] Pruebas de carga sobre read models pesados aprobadas contra SLA acordado.
- [ ] Revisión de seguridad (Art. VIII + OWASP Top 10) completada sin hallazgos críticos abiertos.
- [ ] Despliegue Cloud y despliegue On-Premise ambos validados con el mismo artefacto Docker.
- [ ] Runbook de despliegue/soporte y plan de rollback documentados.
- [ ] UAT firmado por el cliente.
- [ ] Usuarios finales capacitados.
- [ ] Cobertura ≥70% en Domain/Application verificada en los 6 Bounded Contexts.
- [ ] Go-live realizado; monitoreo de los primeros 30 días sin incidentes críticos.
