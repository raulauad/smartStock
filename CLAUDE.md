# Instrucciones
language: spanish

Responde siempre en español.


# SmartStock — Contexto del Proyecto

## ¿Qué es este proyecto?
API REST de gestión de inventario (stock) desarrollada en **ASP.NET Core (.NET 10)**.
Permite registrar productos, categorías(solo admin), proveedores(solo admin), sesiones de venta/compra, movimientos de stock y cierre de caja.

Permite al admin, gestion de empleados, movimientos realizados por los empleados, e iremos implementando funcionalidades futuras.
---

## Stack técnico
- **Framework:** ASP.NET Core Web API (.NET 10)
- **ORM:** Entity Framework Core 10 + SQL Server
- **Identity:** ASP.NET Core Identity con `IdentityUser<Guid>` y roles (`IdentityRole<Guid>`)
- **CQRS + Mediator:** MediatR 12
- **Validación:** FluentValidation 12 (integrada en pipeline MediatR via `ValidationBehavior`)
- **Autenticación:** JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- **Documentación API:** OpenAPI + Scalar (`/scalar/v1` en desarrollo)
- **Nullable references:** habilitado
- **Implicit usings:** habilitado

---

## Estructura de la Solución

La solución está organizada para soportar múltiples proyectos futuros:

```
smartStock/                              ← raíz del repositorio
├── smartStock.sln                       ← solución en la raíz
└── src/
    ├── smartStock.Api/                  ← proyecto principal (actual)
    │   ├── smartStock.Api.csproj
    │   ├── Program.cs
    │   ├── appsettings.json
    │   ├── appsettings.Development.json
    │   ├── Properties/
    │   ├── Domain/
    │   ├── Application/
    │   ├── Infrastructure/
    │   └── Presentation/
    │
    ├── smartStock.Shared/               ← proyecto de DTOs compartidos (activo)
    │   ├── smartStock.Shared.csproj
    │   ├── Dtos/
    │   │   ├── Admin/                   ← DTOs de features del actor Admin
    │   │   │   ├── AltaEmpleado/
    │   │   │   ├── AltaProveedor/
    │   │   │   ├── CambiarEstadoEmpleado/
    │   │   │   ├── CambiarEstadoProveedor/
    │   │   │   ├── EditarProveedor/
    │   │   │   ├── RegistrarAdmin/
    │   │   │   ├── ObtenerDetalleEmpleado/
    │   │   │   ├── ObtenerDetalleProveedor/
    │   │   │   ├── ObtenerListaEmpleados/
    │   │   │   ├── ObtenerListaProveedores/
    │   │   │   └── ObtenerPerfilAdmin/
    │   │   ├── Auth/                    ← DTOs de features de Auth
    │   │   │   └── IniciarSesion/
    │   │   ├── Empleados/               ← DTOs de features del actor Empleado
    │   │   │   ├── CambiarContrasena/
    │   │   │   └── EditarPerfilEmpleado/
    │   │   └── Shared/                  ← DTOs transversales (DireccionDto)
    │   └── Contracts/                   ← (FUTURO) interfaces y contratos públicos
    │
    ├── smartStock.UI/                   ← (FUTURO)
    │   ├── smartStock.UI.csproj         ← Razor Class Library
    │   ├── Components/                  ← componentes .razor reutilizables
    │   ├── Services/                    ← servicios HTTP que consumen la API
    │   └── wwwroot/                     ← CSS y assets compartidos
    │
    └── smartStock.Web/                  ← (FUTURO)
        ├── smartStock.Web.csproj        ← Blazor WebAssembly
        ├── Pages/                       ← páginas y rutas navegables
        ├── Layout/                      ← shell, navbar, sidebar
        └── wwwroot/                     ← index.html, favicon
```

**Dependencias entre proyectos:**
- `smartStock.Api`  → referencia `smartStock.Shared` (ya activo)
- `smartStock.UI`   → referenciará `smartStock.Shared` (FUTURO)
- `smartStock.Web`  → referenciará `smartStock.UI` y `smartStock.Shared` (FUTURO)

---

## Arquitectura de smartStock.Api

El proyecto sigue una arquitectura **en capas**. Root namespace: `smartStock.Api`.

```
src/smartStock.Api/
├── Domain/              → Modelos de negocio, enums, interfaces (sin dependencias externas)
├── Application/         → CQRS (Commands/Queries/Handlers), Validators, DTOs, Middleware
│   └── Common/
│       ├── Behaviors/   → ValidationBehavior (pipeline MediatR)
│       ├── Exceptions/  → Excepciones de dominio que implementan IExceptionHandler
│       │   ├── Admin/       → AdminYaExisteException
│       │   ├── Auth/        → AccesoNoPermitidoException, CuentaInactivaException, CredencialesInvalidasException
│       │   ├── Proveedores/ → CuitDuplicadoException, EstadoProveedorSinCambioException,
│       │   │                   NombreEmailDuplicadoException, NombreTelefonoDuplicadoException,
│       │   │                   ProveedorNoEncontradoException
│       │   └── Usuarios/    → DniDuplicadoException, EmailDuplicadoException,
│       │                       EstadoUsuarioSinCambioException, UsuarioNoEncontradoException
│       ├── Interfaces/  → IJwtTokenService, IExceptionHandler, IUsuarioRepository,
│       │                   ITokenRevocadoRepository, IProveedorRepository
│       ├── Middleware/  → GlobalExceptionHandler, VerificarUsuarioActivoMiddleware
│       └── Validators/  → EmailDomainValidator (DNS check compartido)
├── Infrastructure/
│   ├── Persistence/     → AppDbContext, EF Configurations, Migrations
│   │   └── Repositories/→ UsuarioRepository, TokenRevocadoRepository, ProveedorRepository
│   └── Services/        → JwtTokenService (implementa IJwtTokenService)
└── Presentation/
    └── Controllers/
        ├── Auth/        → AuthController (api/auth)
        └── Usuarios/
            ├── Admin/    → AdministradorController (api/administrador)
            └── Empleado/ → EmpleadoController (api/empleado)
```

---

## Modelos de Dominio
Todos en `Domain/Models/`. Las relaciones clave son:

| Entidad | Descripción |
|---|---|
| `Usuario` | Extiende `IdentityUser<Guid>`. Campos extra: Nombre, Dni, Telefono, Direccion (owned), FechaAlta, FechaBaja (nullable), EstaActivo |
| `Direccion` | Owned Entity de Usuario. Campos: Calle, Numero, Piso (nullable), Departamento (nullable), Ciudad, Provincia, CodigoPostal |
| `Producto` | PK Guid. Tiene PrecioCosto, PrecioVenta, CategoriaId (Guid), UsuarioAltaId. Relacionado 1:1 con StockActual |
| `StockActual` | PK = ProductoId (1:1 con Producto). Cantidad decimal |
| `MovimientoStock` | Tipo enum (Compra/Venta/Ajuste). FK a Producto, Usuario, y opcionales a ItemDetalleVenta/ItemDetalleCompra |
| `VentaDia` / `CompraDia` | Agrupan detalles de una jornada. Tienen Total acumulado y Estado: Abierto/Cerrado |
| `DetalleVenta` / `DetalleCompra` | Una transacción individual (ticket) dentro de un VentaDia/CompraDia. Tienen items |
| `ItemDetalleVenta` | Snapshot de PrecioVenta, PrecioCosto, Subtotal, GananciaTotal |
| `ItemDetalleCompra` | Snapshot de PrecioCompra y Subtotal |
| `CierreCaja` | Relacionado 1:1 con VentaDia y CompraDia |
| `Categoria` | PK Guid. Nombre único (max 50), Descripcion (max 300) |
| `Proveedor` | PK Guid. Campos: Nombre, Cuit (nullable, CHAR 11), Telefono, Email, Direccion (owned), Observaciones (nullable), EstaActivo, FechaAlta. FK a UsuarioAlta |

**Enums:** `TipoMovimiento` (Compra, Venta, Ajuste), `EstadoCierre` (Abierto, Cerrado) y `EstadoUsuario` (Suspendido, Activo, Conectado — `Conectado` reservado para SignalR).

---

## Estructura de Features (CQRS)
Los features se organizan por **dominio de actor** en `src/smartStock.Api/Application/Features/{Actor}/{Commands|Queries}/{NombreFeature}/`.

```
Application/Features/
├── Admin/              → features que ejecuta el administrador
│   ├── Commands/
│   └── Queries/
├── Empleados/          → features que ejecuta el empleado autenticado
│   └── Commands/
├── Auth/               → features de autenticación (login, sin rol requerido)
│   └── Commands/
└── {FuturoActor}/      → misma estructura al agregar nuevos actores
```

Cada carpeta de feature contiene:
- `{Nombre}Command.cs` / `{Nombre}Query.cs` (record)
- `{Nombre}CommandHandler.cs` / `{Nombre}QueryHandler.cs`
- `{Nombre}CommandValidator.cs` (si aplica) — co-ubicado con su command
- `Hubs/` → (reservado para SignalR futuro)

> **Los `*Response.cs` ya NO van dentro del feature en `smartStock.Api`.** Se ubican en `smartStock.Shared/Dtos/{Actor}/{Feature}/{Nombre}Response.cs`, namespace `smartStock.Shared.Dtos.{Actor}.{Feature}`. Igual para `DireccionDto` → `smartStock.Shared.Dtos.Shared`.

**Features implementadas:**
***CU01: Gestión De Usuarios**
*COMMANDS:*
`RegistrarAdministrador` — CU01-W1: registra el administrador del sistema (operación única, lanza `AdminYaExisteException` si ya existe uno). Endpoint: `POST api/administrador/registrar-administrador`.
`IniciarSesion` — CU01-W2: inicio de sesión para Administrador y Empleado con email y contraseña. Retorna JWT. Lanza `CredencialesInvalidasException` (401) o `CuentaInactivaException` (403). Endpoint: `POST api/auth/iniciar-sesion`.
`AltaEmpleado` — CU01-W3: el administrador da de alta un usuario (empleado) dentro del sistema. Requiere `[Authorize(Roles = "Administrador")]`. Endpoint: `POST api/administrador/alta-empleado`.
`EditarPerfilEmpleado` — CU01-W4 (empleado): el empleado autenticado edita sus propios datos (Nombre, Email, Teléfono, DNI, Dirección). `UsuarioId` extraído del claim `sub` en el controller — nunca del body. Mismas validaciones que CU01-W3 (sin contraseña). Lanza `DniDuplicadoException` o `EmailDuplicadoException` si el dato ya lo usa otro usuario. Endpoint: `PUT api/empleado/editar-perfil-empleado`.
`CambiarEstadoEmpleado` — CU01-W4/W5 (admin): el administrador activa o suspende un empleado (`EstaActivo`). `EmpleadoId` viene de la ruta — nunca del body. Solo aplica a usuarios con rol `Empleado` (lanza `AccesoNoPermitidoException` si se intenta sobre un admin). Lanza `EstadoUsuarioSinCambioException` (409) si el estado ya es el solicitado. Endpoint único: `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`.
`EliminarEmpleado` — CU01-W6: el administrador realiza un **borrado lógico** de un empleado (`EstaActivo = false`, `FechaBaja = UtcNow`). No elimina la fila — preserva el historial de transacciones. `EmpleadoId` viene de la ruta — nunca del body. Solo aplica a usuarios con rol `Empleado` (lanza `AccesoNoPermitidoException` si se intenta sobre un admin). Sin body ni response (204 No Content) → el Command implementa `IRequest` sin tipo genérico y no tiene Validator. El `VerificarUsuarioActivoMiddleware` bloquea automáticamente cualquier JWT del empleado eliminado. Endpoint: `DELETE api/administrador/eliminar-empleado/{id:guid}`.
`CambiarContrasena` — CU01-W7: el empleado autenticado cambia su propia contraseña. Requiere `[Authorize(Roles = "Empleado")]`. `UsuarioId` extraído del claim `sub` en el controller — nunca del body. Valida que la contraseña actual sea correcta (lanza `CredencialesInvalidasException` 401 si no lo es) y que la nueva cumpla las reglas de Identity (mín. 8 caracteres, mayúscula, dígito, carácter especial). FA3: confirmación debe coincidir con la nueva (validado en FluentValidation con `.Equal(...)`). Endpoint: `PATCH api/empleado/cambiar-contrasena`.

*QUERIES:*
`ObtenerPerfilAdmin` — CU01-R1: obtiene el perfil del administrador autenticado (datos de Usuario). Requiere `[Authorize(Roles = "Administrador")]`. Extrae el `adminId` desde el claim `"sub"`. Endpoint: `GET api/administrador/obtener-perfil-admin`.
`ObtenerListaEmpleados` — CU01-R2: el ADMINISTRADOR obtiene el listado de empleados con estado activo/inactivo. Endpoint: `GET api/administrador/obtener-lista-empleados`.
`ObtenerDetalleEmpleado` — CU01-R2: el ADMINISTRADOR consulta todos los datos de un empleado por su `Id`. Endpoint: `GET api/administrador/obtener-detalle-empleado/{id:guid}`.

---

***CU02: Gestión De Proveedores**
*COMMANDS:*
`AltaProveedor` — CU02-W1: el administrador da de alta un proveedor en estado activo. `UsuarioAltaId` extraído del claim `sub` en el controller — nunca del body. Valida unicidad de CUIT (opcional, exactamente 11 dígitos), Nombre+Email y Nombre+Teléfono. Lanza `CuitDuplicadoException`, `NombreEmailDuplicadoException` o `NombreTelefonoDuplicadoException` (409). Endpoint: `POST api/administrador/alta-proveedor`.
`EditarProveedor` — CU02-W2: edita datos de un proveedor existente. `ProveedorId` viene de la ruta — nunca del body. Mismas validaciones de unicidad excluyendo el propio registro. Lanza `ProveedorNoEncontradoException` (404). Endpoint: `PUT api/administrador/editar-proveedor/{id:guid}`.
`CambiarEstadoProveedor` — CU02-W3: activa o desactiva un proveedor. `ProveedorId` viene de la ruta. Lanza `EstadoProveedorSinCambioException` (409) si el estado ya es el solicitado. Sin validator (solo un bool en el body). Endpoint: `PATCH api/administrador/cambiar-estado-proveedor/{id:guid}`.

*QUERIES:*
`ObtenerListaProveedores` — CU02-R1: lista proveedores con filtro opcional por estado (`activo`/`inactivo`) y búsqueda por Nombre, CUIT o Email (`[FromQuery]`). Endpoint: `GET api/administrador/obtener-lista-proveedores?filtroEstado=&busqueda=`.
`ObtenerDetalleProveedor` — CU02-R2: detalle completo de un proveedor incluyendo el nombre del admin que lo dio de alta (JOIN via EF projection). Lanza `ProveedorNoEncontradoException` (404). Endpoint: `GET api/administrador/obtener-detalle-proveedor/{id:guid}`.

---

## Convenciones de código

### Naming
- **Clases, records, interfaces:** PascalCase
- **Campos privados:** `_camelCase`
- **Propiedades:** PascalCase
- **Métodos:** PascalCase
- **Variables locales:** camelCase.
- **Idioma del código:** castellano para nombres de dominio y variables (Nombre, Precio, CierreCaja...), inglés para infraestructura técnica.
- **Endpoints (rutas URL):** el segmento de ruta es el nombre exacto del feature en kebab-case. Ej: `AltaEmpleado` → `alta-empleado`, `CambiarEstadoEmpleado` → `cambiar-estado-empleado/{id:guid}`, `ObtenerListaEmpleados` → `obtener-lista-empleados`.

### Patrones obligatorios
- Los **Commands** son `sealed record` que implementan `IRequest<TResponse>`, o `IRequest` (sin tipo) si el endpoint retorna 204 No Content
- Los **Handlers** son `sealed class` que implementan `IRequestHandler<TCommand, TResponse>`, o `IRequestHandler<TCommand>` si no hay respuesta
- Los **Validators** son `sealed class : AbstractValidator<TCommand>`, co-ubicados con su command en la carpeta del feature
- Los **Responses** son `sealed record` ubicados en `smartStock.Shared/Dtos/{Actor}/{Feature}/`, namespace `smartStock.Shared.Dtos.{Actor}.{Feature}`
- Las **excepciones de dominio** implementan `IExceptionHandler` (expone `CodigoHttp` y `Titulo`)
- Los **campos calculados** en el dominio llevan comentario `// calculado en app:`
- Los **snapshots de precio** en items llevan comentario `// snapshot`
- Las **configuraciones de EF** van en `src/smartStock.Api/Infrastructure/Persistence/Configurations/` como `IEntityTypeConfiguration<T>`, una clase por entidad
- Los **IDs sensibles** (userId, empleadoId, proveedorId) que provienen de JWT o de la ruta se declaran como `{ get; init; }` fuera del constructor del record y son sobreescritos en el controller con `command with { ... }` — nunca se leen del body

### Validaciones
- Siempre usar **FluentValidation** en el Validator del Command (nunca data annotations en los Commands)
- La validación DNS del email se centraliza en `Application/Common/Validators/EmailDomainValidator.DominioExisteAsync` (static, timeout 3s). Llamar con `.MustAsync(EmailDomainValidator.DominioExisteAsync)` — no duplicar el método en cada validator
- **DNI:** 7 u 8 dígitos numéricos (`^\d{7,8}$`). Regla uniforme para Administrador y Empleado
- Las reglas de Identity (email, password) se integran llamando a `_userManager.UserValidators` / `_userManager.PasswordValidators` dentro de validaciones custom de FluentValidation

### Controllers
- Heredan de `ControllerBase`
- Decorados con `[ApiController]` y `[Route("api/{entidad}")]`
- Solo inyectan `IMediator` y delegan todo al mediator
- Documentan casos de uso en `<summary>` con formato `CU0X-WX: descripción`

---

## Comandos útiles
```bash
# Compilar (desde la raíz del repositorio)
dotnet build

# Migrations (desde la raíz del repositorio)
dotnet ef migrations add {NombreMigracion} --project src/smartStock.Api --output-dir Infrastructure/Persistence/Migrations
dotnet ef database update --project src/smartStock.Api

# Migración pendiente: renombrado de modelos compra/venta + Guid PKs
dotnet ef migrations add RenombrarModelosYGuidPks --project src/smartStock.Api --output-dir Infrastructure/Persistence/Migrations
dotnet ef database update --project src/smartStock.Api

# Actualizar herramienta EF
dotnet tool update --global dotnet-ef --version 10.0.3

# Ejecutar (desde la raíz)
dotnet run --project src/smartStock.Api

# Seguridad: desrastrear appsettings.Development.json del repositorio git
git rm --cached src/smartStock.Api/appsettings.Development.json
```

---

## Base de datos
- Motor: **SQL Server**
- Connection string en `appsettings.json` → clave `DefaultConnection`
- `AppDbContext` extiende `IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>`
- Configuraciones EF aplicadas con `builder.ApplyConfigurationsFromAssembly(...)`

---

## Manejo de errores
El `GlobalExceptionHandler` (middleware) centraliza todas las respuestas de error como `ProblemDetails`:

| Excepción | HTTP | Cuándo |
|---|---|---|
| `ValidationException` (FluentValidation) | 400 | Fallos de validación de campos |
| Excepciones que implementan `IExceptionHandler` | Según `CodigoHttp` | Errores de dominio |
| Cualquier otra | 500 | Error inesperado |

**Excepciones de dominio implementadas:**

| Excepción | HTTP | Cuándo |
|---|---|---|
| `AdminYaExisteException` | 409 | Se intenta registrar un segundo administrador |
| `CredencialesInvalidasException` | 401 | Email o contraseña incorrectos en login |
| `CuentaInactivaException` | 403 | El usuario existe pero `EstaActivo = false` |
| `AccesoNoPermitidoException` | 403 | El usuario no tiene permisos para la acción |
| `DniDuplicadoException` | 409 | Ya existe un usuario con ese DNI |
| `EmailDuplicadoException` | 409 | Ya existe un usuario con ese email |
| `UsuarioNoEncontradoException` | 404 | No se encontró el usuario solicitado |
| `EstadoUsuarioSinCambioException` | 409 | Se intenta desactivar a un usuario ya suspendido, o reactivar a uno ya activo |
| `ProveedorNoEncontradoException` | 404 | No se encontró el proveedor solicitado |
| `CuitDuplicadoException` | 409 | Ya existe un proveedor con ese CUIT |
| `NombreEmailDuplicadoException` | 409 | Ya existe un proveedor con la misma combinación Nombre+Email |
| `NombreTelefonoDuplicadoException` | 409 | Ya existe un proveedor con la misma combinación Nombre+Teléfono |
| `EstadoProveedorSinCambioException` | 409 | Se intenta activar/desactivar un proveedor que ya tiene ese estado |

---

## Estado actual del proyecto

### Implementado ✅
- Dominio completo (todos los modelos definidos; `Direccion` como Owned Entity)
- EF configurado: `CategoriaConfiguration`, `UsuarioConfiguration`, `StockActualConfiguration`, `UsuarioRolConfiguration`, `ProductoConfiguration`, `MovimientoStockConfiguration`, `CierreCajaConfiguration`, `CompraDiaConfiguration`, `VentaDiaConfiguration`, `ItemDetalleCompraConfiguration`, `ItemDetalleVentaConfiguration`, `ProveedorConfiguration`
- Migraciones aplicadas: `InitialCreate`, `DireccionComoOwned`, `UniqueAdminRole`, `BorradoLogicoEmpleado` — **pendiente migrar:** renombrado de modelos compra/venta + Guid PKs (`RenombrarModelosYGuidPks`)
- `IJwtTokenService` (interface) + `JwtTokenService` (implementación en Infrastructure/Services)
- **Repositorios:** `IUsuarioRepository` / `UsuarioRepository`, `ITokenRevocadoRepository` / `TokenRevocadoRepository`, `IProveedorRepository` / `ProveedorRepository`
- **CU01-W1:** `RegistrarAdministrador` → `POST api/administrador/registrar-administrador`
- **CU01-W2:** `IniciarSesion` → `POST api/auth/iniciar-sesion` (retorna JWT con claims: sub, email, nombre, roles)
- **CU01-W3:** `AltaEmpleado` → `POST api/administrador/alta-empleado`
- **CU01-W4 (empleado):** `EditarPerfilEmpleado` → `PUT api/empleado/editar-perfil-empleado`
- **CU01-W4 (admin):** `CambiarEstadoEmpleado` → `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`
- **CU01-W5:** unificado con CU01-W4 → mismo endpoint `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`
- **CU01-W6:** `EliminarEmpleado` → `DELETE api/administrador/eliminar-empleado/{id:guid}` (204 No Content) — borrado lógico (`EstaActivo = false`, `FechaBaja = UtcNow`)
- **CU01-W7:** `CambiarContrasena` → `PATCH api/empleado/cambiar-contrasena`
- **CU01-R1:** `ObtenerPerfilAdmin` → `GET api/administrador/obtener-perfil-admin`
- **CU01-R2:** `ObtenerListaEmpleados` → `GET api/administrador/obtener-lista-empleados`
- **CU01-R2:** `ObtenerDetalleEmpleado` → `GET api/administrador/obtener-detalle-empleado/{id:guid}`
- **CU02-W1:** `AltaProveedor` → `POST api/administrador/alta-proveedor`
- **CU02-W2:** `EditarProveedor` → `PUT api/administrador/editar-proveedor/{id:guid}`
- **CU02-W3:** `CambiarEstadoProveedor` → `PATCH api/administrador/cambiar-estado-proveedor/{id:guid}`
- **CU02-R1:** `ObtenerListaProveedores` → `GET api/administrador/obtener-lista-proveedores?filtroEstado=&busqueda=`
- **CU02-R2:** `ObtenerDetalleProveedor` → `GET api/administrador/obtener-detalle-proveedor/{id:guid}`
- Roles creados: `"Administrador"`, `"Empleado"`
- **Seguridad:** `CuentaInactivaException` → 403, rate limiting en login (5 req/min por IP), `VerificarUsuarioActivoMiddleware` (valida `EstaActivo` en cada request autenticado), validación JWT key al arrancar, timeout DNS 3s centralizado en `EmailDomainValidator`, índice único filtrado en `UsuarioRoles` para Administrador, índice único filtrado en CUIT de Proveedor

### Pendiente ⏳
- Migración EF: renombrado de modelos compra/venta y cambio de PK a Guid en `Categoria` y `Producto` (combinar en `RenombrarModelosYGuidPks`)
- CRUD de Productos y Categorías
- Features de VentaDia/CompraDia, DetalleVenta/DetalleCompra, MovimientoStock, CierreCaja
- DbSets faltantes en `AppDbContext` (actualmente solo `Categorias` y `Proveedores`)

---

## Lo que NO hacer
- No usar Data Annotations para validación en Commands → siempre FluentValidation
- No poner lógica de negocio en los Controllers → solo delegar a MediatR
- No hardcodear strings de rol → usar constantes privadas en los Handlers (ej: `const string RolAdministrador = "Administrador"`)
- No calcular campos (Subtotal, Ganancia) en la base de datos → calcular en la capa de aplicación antes de persistir
- No agregar migraciones sin tener las configuraciones EF correspondientes en `Configurations/`
- No crear `*Response.cs` dentro de `smartStock.Api` → siempre en `smartStock.Shared/Dtos/{Actor}/{Feature}/`
- No crear `DireccionDto` ni DTOs de entrada/salida en `smartStock.Api` → van en `smartStock.Shared`
- No hardcodear `Jwt:Key` en `appsettings.json` → la clave va en `appsettings.Development.json` (local) o en variable de entorno `Jwt__Key` (producción). El app arranca con excepción si la clave está vacía o tiene menos de 32 caracteres
- No duplicar `DominioExisteAsync` en los validators → usar `EmailDomainValidator.DominioExisteAsync` de `Application/Common/Validators/`
- No leer IDs sensibles (userId, proveedorId, empleadoId) del body → siempre de JWT claim `sub` o de la ruta, inyectados con `command with { ... }` en el controller

---

## Seguridad

### Clave JWT
- `appsettings.json` tiene `"Key": ""` — vacío a propósito
- En desarrollo: la clave está en `appsettings.Development.json` (gitignoreado). Para desrastrearlo: `git rm --cached src/smartStock.Api/appsettings.Development.json`
- En producción: usar variable de entorno `Jwt__Key` (doble guión bajo = jerarquía de configuración en .NET)
- El app valida que `Jwt:Key` tenga al menos 32 caracteres al arrancar; falla rápido si no

### Rate limiting
- Política `"login"`: ventana fija de 1 minuto, máximo 5 requests por IP en `POST api/auth/iniciar-sesion`
- Retorna `429 Too Many Requests` al superarse
- Registrado en `Program.cs` con `AddRateLimiter` + `AddPolicy`; aplicado con `[EnableRateLimiting("login")]` en `AuthController`

### Verificación de cuenta activa por request
- `VerificarUsuarioActivoMiddleware` se ejecuta después de `UseAuthentication` en cada request autenticado
- Llama a `IUsuarioRepository.EstaActivoAsync(userId)` (query liviana: `AnyAsync(u => u.Id == id && u.EstaActivo)`)
- Si el usuario fue suspendido, su JWT sigue siendo válido sintácticamente pero el middleware retorna `403` inmediatamente
- Solo actúa cuando `context.User.Identity.IsAuthenticated == true` → rutas públicas (login, registrar-admin) se saltan el check automáticamente
- Registro en pipeline: `app.UseAuthentication()` → `app.UseMiddleware<VerificarUsuarioActivoMiddleware>()` → `app.UseAuthorization()`

### Índice único de Administrador
- `UsuarioRolConfiguration` tiene un índice único filtrado `[Rol] = 'Administrador'` (`UX_UsuarioRoles_Administrador`)
- Garantiza a nivel de base de datos que solo puede existir un administrador, protegiendo contra race conditions

### Índice único de CUIT en Proveedor
- `ProveedorConfiguration` tiene un índice único filtrado donde CUIT no es null (`UX_Proveedores_Cuit`)
- Permite múltiples proveedores sin CUIT, pero garantiza unicidad cuando se ingresa uno
