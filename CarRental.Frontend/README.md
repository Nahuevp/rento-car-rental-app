# CarRental Frontend - Angular

Frontend de la aplicación de alquiler de autos desarrollado con Angular y Bootstrap.

## Requisitos Previos

- Node.js (versión 18 o superior)
- npm o yarn
- Angular CLI instalado globalmente: `npm install -g @angular/cli`

## Instalación

1. Instalar dependencias:
```bash
npm install
```

## Configuración

Asegúrate de que el backend esté ejecutándose en `https://localhost:7287` (o ajusta la URL en los servicios si es diferente).

## Ejecutar la aplicación

```bash
npm start
```

La aplicación estará disponible en `http://localhost:4200`

## Estructura del Proyecto

```
src/
├── app/
│   ├── components/
│   │   ├── car-list/          # Componente para listar autos
│   │   └── rental-create/     # Componente para crear alquiler
│   ├── models/                # Interfaces TypeScript
│   ├── services/              # Servicios para comunicación con API
│   ├── app.ts                 # Componente principal
│   └── app.routes.ts          # Configuración de rutas
└── index.html                 # HTML principal con Bootstrap
```

## Características

- ✅ Listado de autos disponibles
- ✅ Formulario para crear alquileres
- ✅ Validación de fechas
- ✅ Cálculo automático de precio total
- ✅ Diseño responsive con Bootstrap 5
- ✅ Manejo de errores

## Notas

- El userId está hardcodeado a 1 por ahora. Puedes implementar autenticación en el futuro.
- Asegúrate de que el backend tenga CORS configurado para permitir peticiones desde `http://localhost:4200`.
