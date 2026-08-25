# Spec — Fase 3: Plataforma Híbrida, App Móvil, App de Escritorio, Dimensioning y Repack & Consolidation

**Referencia:** `plan.md` Sección 6 (Fase 3) · `constitution.md` Artículos X, XI, XII, XIII, XIV.

## 1. Objetivo

Entregar las capacidades diferenciales frente a Magaya: despliegue híbrido validado, canales de acceso adicionales (móvil, escritorio) y las capacidades de piso de operación de Warehousing (dimensioning, repack & consolidation).

## 2. Bounded Context involucrado

`Warehousing` (extensión de `Package`); sin Bounded Context nuevo — móvil/escritorio/despliegue son canales y topología, no dominio (`plan.md` Sección 4.1).

## 3. Alcance funcional

### 3.1 Despliegue híbrido (Art. X)
- Validar el mismo artefacto Docker desplegado en modo Cloud (multi-tenant) y On-Premise (single-tenant), en infraestructura real o representativa del cliente.
- Definir y validar el mecanismo de aislamiento de datos por tenant en Cloud.

### 3.2 Dimensioning (Art. XII)
- Adaptador (`IDimensioningDeviceAdapter`) para al menos un modelo de báscula/dimensionador.
- Comando único (`RecordPackageMeasurementCommand`) para captura automática y manual.
- Fallback manual siempre disponible.

### 3.3 Repack & Consolidation (Art. XIII)
- Operación de dominio para consolidar N `Package` origen en uno o más `Package` destino, con trazabilidad completa (origen nunca se elimina, queda marcado como consolidado).
- Recaptura obligatoria de peso/dimensiones del paquete resultante (vía 3.2 o manual).
- Evidencia fotográfica opcional desde la app móvil.

### 3.4 App móvil (Art. XI)
- Cliente delgado (React Native/Expo): tracking, aprobaciones simples, notificaciones, captura de dimensioning/recepción con cola offline.

### 3.5 App de escritorio (Art. XIV)
- Cliente Electron para puestos fijos: acceso a báscula/dimensionador vía serial/USB e impresión térmica de etiquetas/BL/AWB/código de barra.

## 4. Modelo de dominio candidato (extensión de `Package`)

- `Package.Consolidate(sourcePackageIds, capturedMeasurement)` como operación de Aggregate, no como simple update de campos.
- Nuevo estado `Consolidated` para los paquetes de origen (nunca `Deleted`).
- Value Object `MeasurementSource` (`Manual` | `Device:<adapterId>`).

## 5. Commands principales

- `RecordPackageMeasurementCommand`
- `RepackConsolidateCommand`

## 6. Queries principales

- `GetPackageTraceabilityQuery` (paquete resultante → paquetes de origen y viceversa)
- `GetDeviceStatusQuery` (estado de conexión de báscula/dimensionador, para UI de escritorio/móvil)

## 7. Eventos de integración

- `PackageConsolidated` — consumido por Finance (Fase 5, recalculo de peso volumétrico para facturación, Art. XIII.3) y Customs (Fase 6, trazabilidad, Art. XIII.4).
- `PackageMeasurementRecorded`.

## 8. Requisitos no funcionales / artículos aplicables

- Ningún adaptador de hardware ni SDK propietario en `Domain`/`Application` (Art. X.4, Art. XII.1).
- Toda operación de repack queda en audit log inmutable, con evidencia fotográfica si aplica (Art. XIII.5).
- Apps móvil/escritorio toleran conectividad intermitente y encolan localmente (Art. XI.4, Art. XIV.5).
- Drivers de hardware de escritorio viven en el proceso Electron, nunca en la SPA compartida (Art. XIV.4).

## 9. Fuera de alcance de esta fase

- Ampliar la app móvil a un ERP móvil completo (requiere ADR, Art. XI.2).
- Integrar más de un modelo de báscula/dimensionador (se agrega de forma aditiva después, Art. XII.4).

## 10. Criterio de aceptación / entregable

On-premise validado además del de nube, app móvil y app de escritorio en producción, al menos una integración de báscula/dimensionador y una impresora funcionando, flujo de repack/consolidation operativo en WMS, según `plan.md`.
