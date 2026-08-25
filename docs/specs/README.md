# Specs por Fase

Este directorio contiene la especificación funcional/técnica de cada fase de `plan.md`, derivada de las reglas de `constitution.md`. Cada spec traduce el "qué" del plan y el "cómo" de la constitution en: Bounded Context(s) involucrados, modelo de dominio candidato (Aggregates/Entities/Value Objects), Commands, Queries, eventos de integración, requisitos no funcionales y criterios de aceptación de la fase.

Las specs son la entrada para generar los archivos de tareas en `/docs/tasks/`. No contienen código ni diseño detallado de clases — eso se decide durante la implementación de cada fase, respetando `constitution.md`.

| Fase | Archivo | Bounded Context(s) |
|---|---|---|
| Fase 0 | [fase-0-spec.md](./fase-0-spec.md) | Todos (definición) / IdentityAccess (implementación) |
| Fase 1 | [fase-1-spec.md](./fase-1-spec.md) | Shipping |
| Fase 2 | [fase-2-spec.md](./fase-2-spec.md) | Warehousing |
| Fase 3 | [fase-3-spec.md](./fase-3-spec.md) | Warehousing (extensión) + canales (móvil/escritorio) + despliegue |
| Fase 4 | [fase-4-spec.md](./fase-4-spec.md) | CRM & Sales |
| Fase 5 | [fase-5-spec.md](./fase-5-spec.md) | Finance & Accounting |
| Fase 6 | [fase-6-spec.md](./fase-6-spec.md) | Customs & Compliance |
| Fase 7 | [fase-7-spec.md](./fase-7-spec.md) | Todos (integración) |
