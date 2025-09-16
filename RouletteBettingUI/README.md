Este proyecto es una implementación de una **ruleta de apuestas** como parte de la prueba técnica de frontend.  
El objetivo fue demostrar habilidades en **Vue 3 + TypeScript**, manejo de **estado de la UI**, integración con **API REST**, y buenas prácticas de código/documentación.

## Tecnologías utilizadas

- [Vue 3] con **Composition API** y `<script setup>`
- [TypeScript] para tipado fuerte y DTOs
- [Vite] para bundling y desarrollo rápido
- **CSS puro + Flex/Grid** para el diseño responsivo
- Arquitectura basada en **modelos de UI** y **DTOs de API**

## Funcionalidades principales

- **Apuestas soportadas**:
  - Por color (`Rojo`, `Negro`).
  - Paridad + color (`Par/Impar + color`).
  - Número + color (`número específico + color`).
- **Traducciones dinámicas** (colores/paridad en español en la UI).
- **Estados del juego manejados en la UI**:
  - Usuario actual y balance.
  - Último resultado (`numero`, `color`, `paridad`).
  - Apuesta en curso.
  - Indicador de si la ruleta está girando.
- **Consumo de API**:
  - `POST /api/roulette/spin` → obtiene el resultado del spin.
  - `POST /api/roulette/prize` → calcula premio según la apuesta.

## Cómo ejecutar el proyecto

## 1. Clonar el repositorio
```bash
git clone <url-del-repo>
cd roulette-game
``` 

## 2. Instalar dependencias
```bash
npm install
``` 

## 3. Ejecutar en modo desarrollo
```bash
npm run dev
```

## 4. Abrir en el navegador