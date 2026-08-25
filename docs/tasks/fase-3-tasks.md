# Fase 3 — Tareas: Plataforma Híbrida, App Móvil, App de Escritorio, Dimensioning y Repack & Consolidation

**Spec:** [`/docs/specs/fase-3-spec.md`](../specs/fase-3-spec.md)

## 1. Despliegue Híbrido (Art. X)
1.1. Definir/validar el mecanismo de aislamiento de datos multi-tenant para Cloud.
1.2. Preparar el artefacto Docker Compose (mínimo soportado) y, si aplica, manifiestos Kubernetes para On-Premise.
1.3. Ejecutar un despliegue On-Premise real o representativo (infraestructura del cliente o equivalente) y validar operación sin conectividad permanente a servidores propios.
1.4. Documentar runbook de despliegue para ambos modos.

## 2. Dimensioning (Art. XII)
2.1. Definir la interfaz `IDimensioningDeviceAdapter` en `Application`.
2.2. Implementar un adaptador concreto en `Infrastructure` para al menos un modelo de báscula/dimensionador.
2.3. Implementar `RecordPackageMeasurementCommand` (soporta origen manual y por dispositivo).
2.4. Validar que la ausencia/falla del dispositivo nunca bloquea la operación (fallback manual).

## 3. Repack & Consolidation (Art. XIII)
3.1. Extender el Aggregate `Package` con la operación de consolidación (`RepackConsolidateCommand`).
3.2. Implementar la regla de trazabilidad: paquetes de origen nunca se eliminan, quedan marcados como consolidados.
3.3. Forzar la recaptura de peso/dimensiones del paquete resultante (vía Dimensioning o manual), nunca sumando los de origen.
3.4. Implementar publicación del evento `PackageConsolidated`.
3.5. Registrar la operación de repack en el audit log inmutable, incluyendo evidencia fotográfica si viene de la app móvil.
3.6. Implementar `GetPackageTraceabilityQuery`.

## 4. App Móvil (Art. XI)
4.1. Bootstrap del proyecto React Native (Expo), reusando tipos/contratos con la SPA web.
4.2. Implementar pantallas: tracking, aprobaciones simples, notificaciones.
4.3. Implementar captura de dimensioning/recepción con cola offline (encolar y sincronizar sin bloquear al operario).
4.4. Implementar captura de evidencia fotográfica para repack.

## 5. App de Escritorio (Art. XIV)
5.1. Bootstrap del proyecto Electron empaquetando la SPA web.
5.2. Implementar drivers/adaptadores de báscula/dimensionador vía serial/USB en el proceso Electron (nunca en la SPA compartida).
5.3. Implementar impresión térmica de etiquetas y documentación (BL/AWB, código de barra).
5.4. Implementar cola local de sincronización ante pérdida de conectividad con el backend.

## 6. Testing y Calidad
6.1. Unit tests de la operación de consolidación de `Package` y sus invariantes de trazabilidad.
6.2. Tests de integración del adaptador de dimensioning (con dispositivo real o simulado) y del fallback manual.
6.3. Pruebas manuales/E2E de la app móvil y de escritorio contra el backend en modo Cloud y On-Premise.
6.4. Verificar cobertura ≥70% en Domain/Application de los cambios en `Warehousing`.

## 7. Cierre de Fase
7.1. Checklist de calidad contra `constitution.md` (Art. X-XIV en particular).
7.2. Demo funcional al cliente: repack con trazabilidad, dimensioning, app móvil, app de escritorio, ambos modos de despliegue.
7.3. Registrar deuda técnica pendiente, si la hay.

---

## Checklist de Cierre — Fase 3

- [ ] Despliegue On-Premide validado además del de Cloud, mismo artefacto Docker.
- [ ] Al menos un adaptador de báscula/dimensionador funcionando, con fallback manual siempre disponible.
- [ ] Operación de repack/consolidation implementada con trazabilidad completa (paquetes de origen nunca eliminados).
- [ ] Peso/dimensiones del paquete resultante siempre recapturados, nunca sumados.
- [ ] Evento `PackageConsolidated` publicado y consumible por otros contextos.
- [ ] Operaciones de repack registradas en audit log inmutable, con evidencia fotográfica cuando aplica.
- [ ] App móvil en producción: tracking, aprobaciones, notificaciones, captura offline-first.
- [ ] App de escritorio en producción: báscula/dimensionador cableado + impresión térmica funcionando.
- [ ] Drivers de hardware de escritorio aislados del código compartido con la SPA web.
- [ ] Desplegado en staging/producción y demostrado/aceptado por el cliente.
