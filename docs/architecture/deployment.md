# Docker y Despliegue Híbrido (tarea 2.7)

Responde a `docs/tasks/fase-0-tasks.md` 2.7: cómo el artefacto Docker soporta, desde su diseño, el despliegue híbrido de Fase 3 (`constitution.md` Artículo X.1) sin necesidad de reescritura. La validación completa de ambos modos (Cloud y On-Premise reales) es explícitamente Fase 3 — acá solo se sientan las bases para que esa validación no requiera cambiar código.

## Una sola imagen, dos formas de correrla

`src/Host/Fluxo.Api/Dockerfile` construye una única imagen (multi-stage: SDK para build/publish, ASP.NET runtime para ejecutar). No hay ramas de build ni Dockerfiles distintos para Cloud vs. On-Premise.

- **Cloud (Azure):** la misma imagen se publica a un registro (Azure Container Registry) y corre en el servicio de contenedores que se decida en Fase 3 (Azure Container Apps o AKS — decisión pendiente de ADR en esa fase). Configuración multi-tenant vía variables de entorno.
- **On-Premise:** la misma imagen corre con `docker compose` en la infraestructura del cliente, sin necesidad de Azure ni de conectividad saliente a nuestros servidores.

## Cómo se logra sin reescritura

1. **Toda configuración por variable de entorno** (connection string de PostgreSQL, secretos, flags de feature) — nunca hardcodeada ni leída de un servicio propietario de Azure dentro de `Domain`/`Application`. Si en el futuro Cloud usa Azure Key Vault para secretos, eso se resuelve en `Infrastructure`/al nivel de orquestación (variables inyectadas al contenedor), no en el código de la aplicación.
2. **Sin SDKs de Azure en el código de la aplicación** (Artículo X.4). La única dependencia externa de infraestructura en esta fase es Npgsql (PostgreSQL), que es igual en Cloud y On-Premise.
3. **Puerto y host-binding genéricos** (`ASPNETCORE_URLS=http://+:8080`): no asume estar detrás de un servicio específico de Azure; cualquier reverse proxy (Azure o el del cliente) puede exponerlo.
4. **Build de contexto único**: `docker build -f src/Host/Fluxo.Api/Dockerfile .` se ejecuta desde la raíz del repo (el Dockerfile copia `Fluxo.slnx` y `src/`), igual en el pipeline de CI/CD hacia Azure que en la máquina del cliente on-premise.

## Pendiente para Fase 3 (no Fase 0)

- Docker Compose de referencia para on-premise (`docs/tasks/fase-3-tasks.md`).
- Estrategia de aislamiento multi-tenant para Cloud (esquema vs. base de datos por tenant).
- Validación real en ambos entornos con un cliente/infraestructura de referencia.

## Estado de validación en Fase 0

El Dockerfile no se validó con un build real en este entorno (Docker Desktop no estaba corriendo). Pendiente de correr `docker build` antes de dar por cerrado el Workstream 4 (CI/CD), donde además se agrega el `docker-compose.yml` de desarrollo local.
