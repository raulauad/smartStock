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
    │   │   │   ├── CambiarEstadoEmpleado/
    │   │   │   ├── RegistrarAdmin/
    │   │   │   ├── ObtenerDetalleEmpleado/
    │   │   │   ├── ObtenerListaEmpleados/
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
│       │   ├── Admin/   → AdminYaExisteException
│       │   ├── Auth/    → AccesoNoPermitidoException, CuentaInactivaException, CredencialesInvalidasException
│       │   └── Usuarios/→ DniDuplicadoException, EmailDuplicadoException, UsuarioNoEncontradoException
│       ├── Interfaces/  → IJwtTokenService, IExceptionHandler
│       └── Middleware/  → GlobalExceptionHandler, VerificarUsuarioActivoMiddleware
├── Infrastructure/
│   ├── Persistence/     → AppDbContext, EF Configurations, Migrations
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
| `Usuario` | Extiende `IdentityUser<Guid>`. Campos extra: Nombre, Dni, Telefono, Direccion (owned), FechaAlta, EstaActivo |
| `Direccion` | Owned Entity de Usuario. Campos: Calle, Numero, Piso (nullable), Departamento (nullable), Ciudad, Provincia, CodigoPostal |
| `Producto` | Tiene PrecioCosto, PrecioVenta, CategoriaId, UsuarioAltaId. Relacionado 1:1 con StockActual |
| `StockActual` | PK = ProductoId (1:1 con Producto). Cantidad decimal |
| `MovimientoStock` | Tipo enum (Compra/Venta/Ajuste). FK a Producto, Usuario, y opcionales a ItemVenta/ItemCompra |
| `SesionVenta` / `SesionCompra` | Agrupan transacciones. Estado: Abierto/Cerrado |
| `TransaccionVenta` / `TransaccionCompra` | Pertenecen a una sesión. Tienen items |
| `ItemTransaccionVenta` | Snapshot de PrecioVenta, PrecioCosto, Subtotal, GananciaTotal |
| `ItemTransaccionCompra` | Snapshot de PrecioCompra y Subtotal |
| `CierreCaja` | Relacionado 1:1 con SesionVenta y SesionCompra |
| `Categoria` | Nombre único (max 50), Descripcion (max 300) |
| `Proveedor` | Cuit único (max 11), Nombre, Telefono, Direccion |

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
- `{Nombre}CommandValidator.cs` (si aplica)
- `Hubs/` → (reservado para SignalR futuro)

> **Los `*Response.cs` ya NO van dentro del feature en `smartStock.Api`.** Se ubican en `smartStock.Shared/Dtos/{Actor}/{Feature}/{Nombre}Response.cs`, namespace `smartStock.Shared.Dtos.{Actor}.{Feature}`. Igual para `DireccionDto` → `smartStock.Shared.Dtos.Shared`.

**Features implementadas:**
***Cu01: Gestion De Usuarios**
*COMMANDS:*
`RegistrarAdministrador` — CU01-W1: registra el administrador del sistema (operación única, lanza `AdminYaExisteException` si ya existe uno). Endpoint: `POST api/administrador/registrar-administrador`.
`IniciarSesion` — CU01-W2: inicio de sesión para Administrador y Empleado con email y contraseña. Retorna JWT. Lanza `CredencialesInvalidasException` (401) o `CuentaInactivaException` (403). Endpoint: `POST api/auth/iniciar-sesion`.
`AltaEmpleado` — CU01-W3: el administrador da de alta un usuario (empleado) dentro del sistema. Requiere `[Authorize(Roles = "Administrador")]`. Endpoint: `POST api/administrador/alta-empleado`.
`EditarPerfilEmpleado` — CU01-W4 (empleado): el empleado autenticado edita sus propios datos (Nombre, Email, Teléfono, DNI, Dirección). `UsuarioId` extraído del claim `sub` en el controller — nunca del body. Mismas validaciones que CU01-W3 (sin contraseña). Lanza `DniDuplicadoException` o `EmailDuplicadoException` si el dato ya lo usa otro usuario. Endpoint: `PUT api/empleado/editar-perfil-empleado`.
`CambiarEstadoEmpleado` — CU01-W4/W5 (admin): el administrador activa o suspende un empleado (`EstaActivo`). `EmpleadoId` viene de la ruta — nunca del body. Solo aplica a usuarios con rol `Empleado` (lanza `AccesoNoPermitidoException` si se intenta sobre un admin). Lanza `EstadoUsuarioSinCambioException` (409) si el estado ya es el solicitado. Endpoint único: `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`.
`EliminarEmpleado` — CU01-W6: el administrador elimina permanentemente la cuenta de un empleado. `EmpleadoId` viene de la ruta — nunca del body. Solo aplica a usuarios con rol `Empleado` (lanza `AccesoNoPermitidoException` si se intenta sobre un admin). Sin body ni response (204 No Content) → el Command implementa `IRequest` sin tipo genérico y no tiene Validator. Endpoint: `DELETE api/administrador/eliminar-empleado/{id:guid}`.
`CambiarContrasena` — CU01-W7: el empleado autenticado cambia su propia contraseña. Requiere `[Authorize(Roles = "Empleado")]`. `UsuarioId` extraído del claim `sub` en el controller — nunca del body. Valida que la contraseña actual sea correcta (lanza `CredencialesInvalidasException` 401 si no lo es) y que la nueva cumpla las reglas de Identity (mín. 8 caracteres, mayúscula, dígito, carácter especial). FA3: confirmación debe coincidir con la nueva (validado en FluentValidation con `.Equal(...)`). Endpoint: `PATCH api/empleado/cambiar-contrasena`.

*QUERIES:*
`ObtenerPerfilAdmin` — CU01-R1: obtiene el perfil del administrador autenticado (datos de Usuario). Requiere `[Authorize(Roles = "Administrador")]`. Extrae el `adminId` desde el claim `"sub"`. Endpoint: `GET api/administrador/obtener-perfil-admin`.
`ObtenerListaEmpleados` — CU01-R2: el ADMINISTRADOR obtiene el listado de empleados con estado activo/inactivo. Endpoint: `GET api/administrador/obtener-lista-empleados`.
`ObtenerDetalleEmpleado` — CU01-R2: el ADMINISTRADOR consulta todos los datos de un empleado por su `Id`. Endpoint: `GET api/administrador/obtener-detalle-empleado/{id:guid}`.

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
- Los **Validators** son `sealed class : AbstractValidator<TCommand>`
- Los **Responses** son `sealed record` ubicados en `smartStock.Shared/Dtos/{Actor}/{Feature}/`, namespace `smartStock.Shared.Dtos.{Actor}.{Feature}`
- Las **excepciones de dominio** implementan `IExceptionHandler` (expone `CodigoHttp` y `Titulo`)
- Los **campos calculados** en el dominio llevan comentario `// calculado en app:`
- Los **snapshots de precio** en items llevan comentario `// snapshot`
- Las **configuraciones de EF** van en `src/smartStock.Api/Infrastructure/Persistence/Configurations/` como `IEntityTypeConfiguration<T>`, una clase por entidad
- Los **IDs sensibles** (userId, empleadoId) que provienen de JWT o de la ruta se declaran como `{ get; init; }` fuera del constructor del record y son sobreescritos en el controller con `command with { ... }` — nunca se leen del body

### Validaciones
- Siempre usar **FluentValidation** en el Validator del Command (nunca data annotations en los Commands)
- Las reglas de Identity (email, password) se integran llamando a `_userManager.UserValidators` / `_userManager.PasswordValidators` dentro de validaciones custom de FluentValidation
- La validación de dominio DNS del email se hace con `Dns.GetHostAddressesAsync` con timeout de 3 segundos via `CancellationTokenSource.CreateLinkedTokenSource` + `CancelAfter(TimeSpan.FromSeconds(3))`

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

# Migración pendiente: índice único de Administrador en UsuarioRoles
dotnet ef migrations add UniqueAdminRole --project src/smartStock.Api --output-dir Infrastructure/Persistence/Migrations
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

---

## Estado actual del proyecto

### Implementado ✅
- Dominio completo (todos los modelos definidos; `Direccion` como Owned Entity)
- EF configurado: `CategoriaConfiguration`, `UsuarioConfiguration`, `StockActualConfiguration`, `UsuarioRolConfiguration`; 2 migraciones (`InitialCreate`, `DireccionComoOwned`) + **pendiente migrar** `UniqueAdminRole`
- `IJwtTokenService` (interface) + `JwtTokenService` (implementación en Infrastructure/Services)
- **CU01-W1:** `RegistrarAdministrador` → `POST api/administrador/registrar-administrador`
- **CU01-W2:** `IniciarSesion` → `POST api/auth/iniciar-sesion` (retorna JWT con claims: sub, email, nombre, roles)
- **CU01-W3:** `AltaEmpleado` → `POST api/administrador/alta-empleado`
- **CU01-W4 (empleado):** `EditarPerfilEmpleado` → `PUT api/empleado/editar-perfil-empleado`
- **CU01-W4 (admin):** `CambiarEstadoEmpleado` → `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`
- **CU01-W5:** unificado con CU01-W4 → mismo endpoint `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`
- **CU01-W6:** `EliminarEmpleado` → `DELETE api/administrador/eliminar-empleado/{id:guid}` (204 No Content)
- **CU01-W7:** `CambiarContrasena` → `PATCH api/empleado/cambiar-contrasena`
- **CU01-R1:** `ObtenerPerfilAdmin` → `GET api/administrador/obtener-perfil-admin`
- **CU01-R2:** `ObtenerListaEmpleados` → `GET api/administrador/obtener-lista-empleados`
- **CU01-R2:** `ObtenerDetalleEmpleado` → `GET api/administrador/obtener-detalle-empleado/{id:guid}`
- Roles creados: `"Administrador"`, `"Empleado"`
- **Seguridad:** `CuentaInactivaException` → 403, rate limiting en login (5 req/min por IP), `VerificarUsuarioActivoMiddleware` (valida `EstaActivo` en cada request autenticado), validación JWT key al arrancar, timeout DNS 3s en validators, índice único filtrado en `UsuarioRoles` para Administrador

### Pendiente ⏳
- CRUD de Productos, Categorías, Proveedores
- Sesiones de venta/compra, TransaccionVenta/Compra, MovimientoStock, CierreCaja
- DbSets faltantes en `AppDbContext` (actualmente solo `Categorias`)
- Configuraciones EF faltantes para Producto, Proveedor, Sesiones, etc.

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
- Registro en pipeline: `app.UseAuthentication()` → `app.UseMiddleware<VerificarUsuarioActivoMiddleware>()` → `app.UseAuthorization()`

### Índice único de Administrador
- `UsuarioRolConfiguration` tiene un índice único filtrado `[Rol] = 'Administrador'` (`UX_UsuarioRoles_Administrador`)
- Garantiza a nivel de base de datos que solo puede existir un administrador, protegiendo contra race conditions
- **Requiere migración:** `dotnet ef migrations add UniqueAdminRole --project src/smartStock.Api --output-dir Infrastructure/Persistence/Migrations`