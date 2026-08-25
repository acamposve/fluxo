# Fase 3 — Plataforma Híbrida, App Móvil, Dimensioning y Repack & Consolidation
### Plan de Tareas y Checklist de Cierre

**Referencia:** `plan.md` (Sección 6, Fase 3) y `constitution.md` (Artículos X, XI, XII y XIII).

**Duración estimada:** 6-8 semanas.

**Entregable de fase (según plan.md):** Despliegue on-premise validado además del de nube, app móvil mínima en producción, integración con al menos un modelo de báscula/dimensionador, flujo de repack/consolidation operativo en WMS.

**Definición de éxito de la fase:** al cerrar Fase 3, el mismo artefacto Docker corre tanto en Cloud como on-premise sin cambios de código; un operario puede pesar/medir un paquete con hardware real y verlo reflejado en el sistema; dos paquetes pueden consolidarse en uno nuevo sin perder trazabilidad hacia su shipment/factura original; y un usuario puede seguir el estado de sus operaciones y aprobar lo esencial desde el celular.

Esta fase se apoya en `Shipping` (Fase 1) y `Warehousing` (Fase 2) ya construidos; no repite ni modifica su alcance, solo extiende Warehousing con Dimensioning y Repack & Consolidation, y agrega un canal de acceso (móvil) y una topología de despliegue (híbrida) sobre lo ya construido.

---

## 1. Workstream: Despliegue Híbrido (Cloud + On-Premise)

Objetivo: validar que el mismo artefacto Docker corre en ambos modos, respetando `constitution.md` Artículo X.

1.1. Auditar `Domain`/`Application` en busca de dependencias directas a servicios propietarios de un proveedor cloud; extraerlas detrás de interfaces en `Application` si existieran.
1.2. Definir la estrategia de aislamiento de datos multi-tenant para Cloud (esquema por tenant vs. base de datos por tenant) y documentarla como ADR.
1.3. Empaquetar un despliegue On-Premise de referencia vía Docker Compose (mínimo soportado), sin dependencia de conectividad permanente a servidores propios.
1.4. Definir estrategia de configuración por ambiente (variables de entorno/secretos) que permita pasar de Cloud a On-Premise sin recompilar.
1.5. Validar en un entorno on-premise real o simulado (VM aislada, sin salida a internet salvo lo estrictamente necesario) que el sistema opera de punta a punta.
1.6. Documentar el modelo de despliegue híbrido en `/docs/infrastructure/hybrid-deployment.md`.

## 2. Workstream: App Móvil Mínima

Objetivo: dejar en producción una app móvil delgada, según el alcance del `constitution.md` Artículo XI.

2.1. Definir el alcance cerrado de la v1 (tracking/estado de shipments, aprobaciones simples, notificaciones, captura de dimensioning y evidencia de repack) y dejar explícitamente fuera todo lo demás.
2.2. Bootstrap del proyecto React Native (Expo), reusando tipos/contratos de API generados para la SPA.
2.3. Implementar autenticación/autorización reusando `IdentityAccess` (mismo modelo de la SPA, sin lógica de permisos duplicada en el cliente).
2.4. Implementar las pantallas de captura (dimensioning, recepción, evidencia de repack) con cola local y sincronización ante conectividad intermitente.
2.5. Validar publicación/distribución mínima (build interno o tienda, según lo que el cliente necesite para probarla).
2.6. Documentar la arquitectura de la app móvil en `/docs/architecture/mobile.md`.

## 3. Workstream: Integración de Hardware — Dimensioning

Objetivo: capturar peso y medidas automáticamente desde una báscula/dimensionador, según `constitution.md` Artículo XII.

3.1. Seleccionar al menos un dispositivo de referencia para la integración inicial (báscula o dimensionador; priorizar el protocolo más simple disponible: serial/USB/Bluetooth) y documentar la decisión como ADR.
3.2. Definir la interfaz `IDimensioningDeviceAdapter` en `Application` y su contrato (peso, largo, ancho, alto, unidad, origen del dato).
3.3. Implementar el adaptador del dispositivo elegido en `Infrastructure`, aislado de `Domain`.
3.4. Implementar `RecordPackageMeasurementCommand`, compartido entre captura manual y captura por dispositivo, con las mismas validaciones de dominio para ambos orígenes.
3.5. Validar el comportamiento de fallback: si el dispositivo no responde o no está presente, la captura manual sigue disponible sin fricción adicional.
3.6. Probar la integración end-to-end con hardware real (no solo mocks) al menos una vez antes del cierre de fase.
3.7. Documentar el proceso para sumar un nuevo fabricante/dispositivo en `/docs/architecture/dimensioning-adapters.md`.

## 4. Workstream: Repack & Consolidation (WMS)

Objetivo: permitir consolidar N paquetes en uno nuevo sin perder trazabilidad, según `constitution.md` Artículo XIII.

4.1. Modelar `Package` como Aggregate de primera clase dentro de `Warehousing` (si no existe ya con ese nivel de detalle desde Fase 2).
4.2. Diseñar la operación de dominio de repack/consolidación: invariante de trazabilidad completa (paquetes origen referenciados desde el paquete destino, nunca eliminados).
4.3. Definir cómo el repack dispara una nueva captura de peso/dimensiones (integración con el Workstream 3) en vez de sumar valores de los paquetes origen.
4.4. Definir cómo Finance y Customs siguen resolviendo cargos y documentación hacia los paquetes/shipments originales después de un repack (read model de trazabilidad para reportería).
4.5. Implementar el registro en audit log inmutable de toda operación de repack, incluyendo evidencia fotográfica cuando la captura viene de la app móvil.
4.6. Test de integración que cubra el caso completo: dos paquetes origen → repack → paquete destino con nuevo peso/dimensiones → trazabilidad verificable hacia ambos orígenes.
4.7. Documentar el flujo de repack/consolidation en `/docs/domain/warehousing-repack.md`.

## 5. Workstream: Gobernanza, ADRs y Cierre de Fase

Objetivo: dejar registrada la toma de decisiones y preparar el cierre formal de Fase 3 frente al cliente.

5.1. Redactar como **ADR** (`/docs/adr/`) las decisiones difíciles de revertir de esta fase (dispositivo de dimensioning elegido, estrategia multi-tenant, alcance cerrado de la app móvil).
5.2. Revisar que ninguna decisión de esta fase contradiga `constitution.md`; si alguna lo hace, documentarla como ADR y obtener aprobación del Tech Lead antes de avanzar.
5.3. Preparar la demo de cierre de Fase 3 para el cliente (mostrar: despliegue on-premise corriendo, app móvil en un dispositivo real, captura por hardware de dimensioning, y un repack/consolidation completo con trazabilidad visible).
5.4. Registrar explícitamente cualquier deuda técnica tomada durante Fase 3 como ticket, según `constitution.md` Artículo VI.
5.5. Confirmar con el cliente que el orden de fases siguiente (CRM → Accounting → Customs) sigue vigente o si esta fase cambió prioridades.

---

## Checklist de Cierre de Fase 3 (Definition of Done de la fase)

Usar este checklist como gate formal antes de dar la fase por cerrada y pasar a Fase 4 (CRM).

### Despliegue Híbrido
- [ ] Estrategia de aislamiento multi-tenant para Cloud definida y documentada (ADR).
- [ ] Despliegue On-Premise de referencia (Docker Compose) funcionando de punta a punta.
- [ ] `Domain`/`Application` verificados sin dependencias directas a servicios propietarios de un proveedor cloud.
- [ ] Configuración por ambiente permite pasar de Cloud a On-Premise sin recompilar.

### App Móvil
- [ ] Alcance cerrado de la v1 documentado y respetado (sin scope creep hacia un ERP móvil completo).
- [ ] Autenticación/autorización de la app móvil reusa `IdentityAccess` sin lógica de permisos duplicada.
- [ ] Pantallas de captura funcionan con conectividad intermitente (cola local + sincronización).
- [ ] App móvil probada en al menos un dispositivo real por el cliente/Product Owner.

### Dimensioning
- [ ] `IDimensioningDeviceAdapter` definido en `Application` e implementado para al menos un dispositivo real.
- [ ] Captura manual y captura por dispositivo comparten el mismo Command y las mismas validaciones.
- [ ] Fallback a captura manual validado ante ausencia/falla del dispositivo.
- [ ] Integración probada con hardware real, no solo con mocks.

### Repack & Consolidation
- [ ] `Package` modelado como Aggregate de primera clase en `Warehousing`.
- [ ] Operación de repack preserva trazabilidad completa (paquetes origen nunca eliminados, siempre referenciados).
- [ ] Peso/dimensiones del paquete resultante se recapturan, no se calculan por suma.
- [ ] Finance y Customs pueden resolver cargos/documentación hacia los paquetes originales después de un repack.
- [ ] Toda operación de repack queda en el audit log inmutable.

### Gobernanza y Cierre
- [ ] Todas las decisiones relevantes de Fase 3 registradas como ADR en `/docs/adr/`.
- [ ] Ninguna decisión de Fase 3 contradice `constitution.md` sin ADR aprobado.
- [ ] Deuda técnica de Fase 3 (si existe) registrada explícitamente como ticket.
- [ ] Demo de cierre de Fase 3 realizada y aceptada por el cliente/Product Owner.
- [ ] `plan.md` actualizado si hubo cambios de alcance u orden de las fases siguientes.
