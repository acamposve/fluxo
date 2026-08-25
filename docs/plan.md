# Plan de Proyecto — Plataforma Digital de Freight Forwarding
### (Clon funcional de Magaya Digital Freight Platform)

**Versión:** 1.0
**Fecha:** Agosto 2026
**Autor:** Equipo de Arquitectura de Software

---

## 1. Resumen Ejecutivo

Este documento define el plan de construcción de una plataforma integral de gestión logística y freight forwarding, equivalente funcional a **Magaya Digital Freight Platform**, cubriendo los cinco pilares del negocio: **Freight Forwarding, Warehouse Management (WMS), Customs (Aduanas), CRM y Accounting (Contabilidad)**.

El proyecto se ejecuta con un **desarrollador/arquitecto senior, apoyado en herramientas de IA para acelerar la implementación**, bajo una arquitectura moderna, mantenible y escalable (**C# / .NET, PostgreSQL, Domain-Driven Design, CQRS**), priorizando la entrega de valor incremental por fases en lugar de un "big bang" al final del proyecto.

**Premisa central del plan:** dado que se trata de un suite completo tipo ERP logístico, la estrategia no es "todo o nada". Se entrega en fases independientes, cada una facturable y demostrable, de forma que el cliente vea resultados tangibles y pueda validar el ROI antes de invertir en la siguiente fase. Esto reduce el riesgo percibido y financiero del cliente, y le da a nuestro equipo flexibilidad para ajustar prioridades sobre la marcha.

---

## 2. Objetivo del Proyecto

Construir una plataforma propia (white-label, sin licenciamiento de terceros ni costos recurrentes por usuario) que permita a una empresa de freight forwarding:

- Cotizar, reservar y trackear cargas (marítima, aérea, terrestre).
- Gestionar almacenes y inventario de carga en tránsito (WMS).
- Generar documentación aduanera y de comercio exterior (Customs).
- Administrar relación comercial con clientes y agentes (CRM).
- Facturar, cobrar y llevar contabilidad operativa (Accounting).

**Diferenciador clave frente a Magaya:** costo total de propiedad (TCO) menor a mediano/largo plazo, cero dependencia de licencias por usuario, y total control/customización del código fuente.

---

## 3. Alcance del Proyecto

### 3.1 Módulos incluidos (Suite completo)

| Módulo | Descripción funcional | Complejidad relativa |
|---|---|---|
| **Freight Forwarding (Core)** | Cotizaciones, órdenes de embarque (Shipments), Bill of Lading / Airway Bill, tracking, consolidación de carga | Alta |
| **Warehouse Management (WMS)** | Recepción, almacenaje, picking/packing, control de inventario, cross-docking | Alta |
| **Customs (Aduanas)** | Generación de documentación aduanera, integración con entidades regulatorias, gestión de clasificación arancelaria | Muy alta (regulatorio) |
| **CRM** | Gestión de clientes, agentes, cotizaciones comerciales, pipeline de ventas, comunicaciones | Media |
| **Accounting** | Facturación, cuentas por cobrar/pagar, conciliación, reportes financieros | Alta |

### 3.2 Fuera de alcance (para esta fase del proyecto)

- Integraciones con carriers específicos vía EDI (se contempla como fase posterior/adicional).
- Aplicación móvil nativa (se contempla PWA/responsive en el core).
- Módulo de Business Intelligence avanzado (dashboards ejecutivos se dejan para fase de expansión).

> Nota para el cliente: dejar estos puntos explícitamente fuera de alcance en el contrato inicial es lo que permite mantener el presupuesto controlado. Se pueden incorporar como fases adicionales cotizadas por separado.

---

## 4. Arquitectura y Stack Tecnológico

| Capa | Tecnología | Justificación |
|---|---|---|
| Backend | **C# / .NET 8+** | Ecosistema maduro, tipado fuerte, alto rendimiento, gran disponibilidad de talento |
| Base de datos | **PostgreSQL** | Open source, robusto para cargas transaccionales, sin costo de licenciamiento |
| Patrón de diseño | **Domain-Driven Design (DDD)** | Permite modelar un dominio complejo (logística + aduanas + finanzas) en Bounded Contexts independientes y mantenibles |
| Patrón de aplicación | **CQRS** | Separa lecturas de escrituras; crítico en un sistema con reportería pesada (contabilidad, tracking) y alta concurrencia operativa |
| Mensajería entre contextos | Event-driven (outbox pattern) | Desacopla los módulos (ej. una recepción en WMS dispara evento que actualiza tracking en Forwarding) |
| Frontend | SPA (React + Vite, TypeScript) | Interfaz moderna, responsive; Vite acelera el ciclo de build/dev frente a CRA, y React tiene el ecosistema y la disponibilidad de talento/soporte IA más amplios para un proyecto de este tamaño |
| Infraestructura | Contenedores (Docker) + CI/CD | Portabilidad y despliegues consistentes |

### 4.1 Bounded Contexts propuestos (DDD)

1. **Shipping** (Freight Forwarding core)
2. **Warehousing** (WMS)
3. **Customs & Compliance**
4. **CRM & Sales**
5. **Finance & Accounting**
6. **Identity & Access** (transversal)

Cada Bounded Context es un módulo con su propio modelo de datos, su propio conjunto de comandos/queries (CQRS), y se comunica con los demás mediante eventos de dominio — esto es clave para poder **construir y entregar módulos de forma independiente**, que es la base de la estrategia de fases de este plan.

Ver `constitution.md` para las reglas de arquitectura no negociables.

---

## 5. Modelo de Ejecución

Con un suite completo tipo ERP, la construcción exige foco absoluto en el dominio antes que en features accesorias. El proyecto se ejecuta con un **desarrollador/arquitecto senior**, apoyado en herramientas de asistencia por IA para acelerar la implementación (generación de código boilerplate, tests, documentación y revisión continua), lo que permite sostener el ritmo de entrega de las 7 fases sin diluir la calidad ni las prácticas de arquitectura definidas en `constitution.md`.

| Responsabilidad | Cómo se cubre |
|---|---|
| Arquitectura y modelado DDD | Diseño y decisión humana, senior, desde la Fase 0 |
| Implementación de dominio y CQRS | Desarrollo asistido por IA bajo supervisión y revisión humana constante |
| Frontend | Desarrollo asistido por IA, con foco en UX de flujos operativos |
| QA / Testing | Suite de tests automatizados como gate de cada entrega (ver `constitution.md`, Artículo V) |
| Revisión de calidad | Checklist de calidad + revisión exhaustiva antes de cada demo/entrega |

> Ventaja de este modelo frente a un equipo tradicional grande: cero costos de coordinación entre personas, una sola persona con contexto completo del dominio de punta a punta, y un ritmo de entrega sostenido gracias a la aceleración que da la IA en tareas repetitivas. La contrapartida (a comunicar con transparencia si el cliente pregunta por el equipo) es que se trata de una dedicación unipersonal senior, no de una célula de varias personas — el cronograma y el buffer de riesgo de este plan ya contemplan esa realidad.

---

## 6. Fases y Cronograma

Dado que el horizonte de tiempo aún no está definido con el cliente, se propone un roadmap de referencia de **~20 meses**, estructurado en fases independientes y priorizadas por valor de negocio (lo que genera ingresos/operación primero).

| Fase | Módulo | Duración estimada | Entregable |
|---|---|---|---|
| **Fase 0** | Discovery, modelado DDD, arquitectura base, Identity & Access | 4-6 semanas | Documento de bounded contexts, ambiente base, CI/CD funcionando |
| **Fase 1** | Freight Forwarding (Core) | 4-5 meses | Cotización, shipments, BL/AWB, tracking operativo |
| **Fase 2** | Warehouse Management (WMS) | 3-4 meses | Recepción, almacenaje, picking/packing, inventario |
| **Fase 3** | CRM | 2-3 meses | Gestión de clientes/agentes, pipeline comercial |
| **Fase 4** | Accounting | 3-4 meses | Facturación, CxC/CxP, reportes financieros |
| **Fase 5** | Customs | 3-4 meses | Documentación aduanera, clasificación arancelaria |
| **Fase 6** | Integración final, hardening, UAT y go-live | 4-6 semanas | Sistema integrado, pruebas de aceptación, capacitación |

**Duración total estimada: 18-22 meses** con el modelo de ejecución propuesto (desarrollador senior + asistencia de IA).

> El orden de las fases (Forwarding → WMS → CRM → Accounting → Customs) puede reordenarse según qué área genera más impacto inmediato en la operación del cliente. Esto es una decisión conjunta con el cliente en la Fase 0.

Ver `presupuesto.xlsx` para el detalle de costos y tiempos por fase, incluyendo el modelo de pago vinculado a hitos (no todo por adelantado).

---

## 7. Riesgos y Mitigaciones

| Riesgo | Probabilidad | Impacto | Mitigación |
|---|---|---|---|
| Alcance tipo "suite completo" con dedicación unipersonal genera expectativas de plazo poco realistas | Alta | Alto | Comunicar roadmap por fases desde el día 1; contrato por hitos, no "todo incluido en X meses" |
| Complejidad regulatoria del módulo de Customs (varía por país/aduana) | Alta | Alto | Fase de Customs al final, con discovery legal/regulatorio dedicado antes de codificar |
| Ausencia o imprevisto del único desarrollador — *bus factor* de 1 | Media | Alto | Documentación de dominio (DDD) como activo, no dependiente de la memoria de una persona; buffer de riesgo del 15% en el cronograma; comunicación temprana al cliente ante cualquier demora |
| Cliente solicita cambios de alcance a mitad de fase | Alta | Medio | Change requests formales, presupuestados aparte; no se re-negocia el precio de una fase ya cerrada |
| Subestimación de esfuerzo en integraciones (bancos, carriers, aduanas) | Media | Alto | Buffer del 15% en cada fase (ya incluido en el presupuesto) |

---

## 8. Metodología de Trabajo

- **Scrum** con sprints de 2 semanas.
- Demo funcional al cliente al cierre de cada sprint (visibilidad constante = confianza, especialmente valioso con clientes sensibles al costo).
- **Definition of Done** estricta por historia de usuario (ver `constitution.md`).
- Backlog priorizado conjuntamente con el cliente al inicio de cada fase.

---

## 9. Criterios de Éxito

- Cada fase se considera exitosa cuando el módulo correspondiente está en producción, con UAT firmado por el cliente.
- KPIs técnicos: cobertura de tests >70% en dominio, cero incidentes críticos en producción durante los primeros 30 días post go-live de cada fase.
- KPI de negocio: el cliente puede operar el módulo entregado de forma autónoma (sin depender del sistema legado/Magaya) al cierre de cada fase.

---

## 10. Argumento de Venta del Enfoque por Fases (clave para el cierre comercial)

Este punto es central para la conversación comercial con un cliente sensible al precio:

1. **No se le pide pagar todo el proyecto por adelantado.** Se paga fase por fase, y cada fase entrega un módulo operativo real.
2. **El cliente puede frenar o re-priorizar después de cada fase** sin perder lo invertido, porque cada módulo es funcional por sí mismo.
3. **Comparación de costo total de propiedad (TCO):** Magaya cobra licenciamiento recurrente por usuario/mes de forma indefinida. Este desarrollo es una inversión única que se vuelve un activo propio, sin costo de licencia a perpetuidad.
4. **Sin vendor lock-in:** el cliente es dueño del código, de los datos y de la infraestructura.
5. **Roadmap flexible:** si el cliente tiene más urgencia en un módulo que en otro, se reordena sin penalidad (siempre que no se haya iniciado la fase).

> Ver la presentación `presentacion_ventas.pptx` para la versión resumida y visual de este argumento, orientada a la conversación comercial.
