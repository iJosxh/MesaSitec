# MesaSitec

Sistema de gestion mesa ayuda desarrollado como parte de una prueba tecnica de desarrollo. 
Permite la administracion de solicitudes entre usuarios para su seguimiento y resolucion.

---

# Tecnologías utilizadas

## Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger (OpenAPI)

## Frontend

- Vue 3
- TypeScript
- Vite
- Vue Router
- Pinia
- Axios

---

# Requisitos Previos

Antes de ejecutar el proyecto asegurese de tener instalado:

- .NET SDK 8 o superior
- Node.js 20 LTS
- SQL Lite
- Git

---

# Variables de entorno

Dentro del repositorio se subio un archivo `.env.example` para dar a conocer variables de entorno que se usan
en el proyecto. Su uso es para su personalización, mas adelante se describe como se implementaran

---

# Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/iJosxh/MesaSitec.git
cd MesaSitec
```

### 2. Instalar las dependencias

#### Backend

```bash
cd MesaSitec.API
dotnet restore
```

#### Frontend

```bash
cd ../mesasitec-web
npm install
```

### 3. Configurar la variable de entorno

Antes de ejecutar el backend, configure la variable `JWT_SECRET` en MesaSitec.API.

**PowerShell:**

```powershell
$env:JWT_SECRET="TuClaveJWT"
```

### 4. Ejecutar la aplicación

#### Backend

```bash
cd ../MesaSitec.API
dotnet run
```

#### Frontend

```bash
cd ../mesasitec-web
npm run dev
```

---

# Credenciales de prueba

## Administrador

Usuario:

```
admin@norte.test
```

Contraseña:

```
Sitec.2026
```

---

## Agente

Usuario:

```
agente1@norte.test
```

Contraseña:

```
Sitec.2026
```

---

## Solicitante

Usuario:

```
user1@norte.test
```

Contraseña:

```
Sitec.2026
```

---

# Pruebas Unitarias

```bash
cd MesaSitec.API.Tests
dotnet test
```


# Funcionalidades implementadas

- El backend valida todas las reglas de negocio, la maquina de estados y el calculo del SLA.
- Autenticación mediante JWT.
- Mostrar el listado de las solicitudes.
- Opcion para mostrar el detalle de cada solicitud.
- Filtros de busqueda.
- Paginacion segun el formato solicitado.

---

# Funcionalidades pendientes

- Validar que las solicitudes semilla no se creen con el estado vencida por la variable de entorno SEED_FECHA_BASE descrita en la prueba.
- El frontend no tiene implementado en un 100% todo las funcionalidades del backend.
- El consumo de todas las APIs en el frontend.
- Validaciones en las vistas como por ejemplo solo mostrar solicitudes de la organizacion del usuario logueado.

---

# Autor

**Josué David Pú López**

Prueba Técnica - MesaSitec