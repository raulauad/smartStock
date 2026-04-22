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
- **CQRS + Mediator:** MediatR 12
- **Validación:** FluentValidation 12 (integrada en pipeline MediatR via `ValidationBehavior`)
- **Autenticación:** JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) + bcrypt para passwords
- **Documentación API:** OpenAPI + Scalar (`/scalar/v1` en desarrollo)
- **Nullable references:** habilitado
- **Implicit usings:** habilitado

---

## Estructura de la Solución

La solución está organizada para soportar múltiples proyectos futuros:

```
smartStock/                              <- raiz del repositorio
+-- smartStock.sln                       <- solucion en la raiz
+-- src/
    +-- smartStock.Api/                  <- proyecto principal (actual)
    |   +-- smartStock.Api.csproj
    |   +-- Program.cs
    |   +-- appsettings.json
    |   +-- appsettings.Development.json
    |   +-- Properties/
    |   +-- Domain/
    |   +-- Application/
    |   +-- Infrastructure/
    |   +-- Presentation/
    |
    +-- smartStock.Shared/               <- proyecto de DTOs compartidos (activo)
    |   +-- smartStock.Shared.csproj
    |   +-- Dtos/
    |   |   +-- Admin/                   <- DTOs de features del actor Admin
    |   |   |   +-- AltaEmpleado/
    |   |   |   +-- AltaProveedor/
    |   |   |   +-- AltaCategoria/
    |   |   |   +-- CambiarEstadoEmpleado/
    |   |   |   +-- CambiarEstadoProveedor/
    |   |   |   +-- CambiarEstadoCategoria/
    |   |   |   +-- EditarProveedor/
    |   |   |   +-- EditarCategoria/
    |   |   |   +-- RegistrarAdmin/
    |   |   |   +-- ObtenerDetalleEmpleado/
    |   |   |   +-- ObtenerDetalleProveedor/
    |   |   |   +-- ObtenerDetalleCategoria/
    |   |   |   +-- ObtenerListaEmpleados/
    |   |   |   +-- ObtenerListaProveedores/
    |   |   |   +-- ObtenerListaCategorias/
    |   |   |   +-- ObtenerPerfilAdmin/
    |   |   +-- Auth/                    <- DTOs de features de Auth
    |   |   |   +-- IniciarSesion/
    |   |   +-- Empleados/               <- DTOs de features del actor Empleado
    |   |   |   +-- CambiarContrasena/
    |   |   |   +-- EditarPerfilEmpleado/
    |   |   +-- Shared/                  <- DTOs transversales (DireccionDto)
    |   +-- Contracts/                   <- (FUTURO) interfaces y contratos publicos
    |
    +-- smartStock.UI/                   <- (FUTURO) Razor Class Library
    +-- smartStock.Web/                  <- (FUTURO) Blazor WebAssembly
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
+-- Domain/              -> Modelos de negocio, enums (sin dependencias externas)
+-- Application/         -> CQRS (Commands/Queries/Handlers), Validators, Middleware
|   +-- Common/
|       +-- Behaviors/   -> ValidationBehavior (pipeline MediatR)
|       +-- Exceptions/  -> Excepciones de dominio que implementan IExceptionHandler
|       |   +-- Admin/       -> AdminYaExisteException
|       |   +-- Auth/        -> AccesoNoPermitidoException, CuentaInactivaException, CredencialesInvalidasException
|       |   +-- Proveedores/ -> CuitDuplicadoException, EstadoProveedorSinCambioException,
|       |   |                   NombreDuplicadoException, EmailProveedorDuplicadoException,
|       |   |                   TelefonoDuplicadoException, ProveedorNoEncontradoException
|       |   +-- Categorias/  -> CategoriaNoEncontradaException, CategoriaNombreDuplicadoException,
|       |   |                   EstadoCategoriaSinCambioException, SinCategoriasActivasAlternativasException,
|       |   |                   CategoriaDestinoInvalidaException, CategoriaReasignacionRequiereDestinoException
|       |   +-- Usuarios/    -> DniDuplicadoException, EmailDuplicadoException,
|       |                       EstadoUsuarioSinCambioException, UsuarioNoEncontradoException
|       +-- Interfaces/  -> IJwtTokenService, IExceptionHandler, IPasswordHasher, IUsuarioRepository,
|       |                   ITokenRevocadoRepository, IProveedorRepository, ICategoriaRepository
|       +-- Middleware/  -> GlobalExceptionHandler, VerificarUsuarioActivoMiddleware
|       +-- Validators/  -> EmailDomainValidator (DNS check compartido)
+-- Infrastructure/
|   +-- Persistence/     -> AppDbContext, EF Configurations, Migrations
|   |   +-- Repositories/ -> UsuarioRepository, TokenRevocadoRepository, ProveedorRepository, CategoriaRepository
|   +-- Services/        -> JwtTokenService, BcryptPasswordHasher
+-- Presentation/
    +-- Controllers/
        +-- Auth/        -> AuthController (api/auth)
        +-- Usuarios/
            +-- Admin/    -> AdministradorController (api/administrador)
            +-- Empleado/ -> EmpleadoController (api/empleado)
```

---

## Modelos de Dominio
Todos en `Domain/Models/`. Las relaciones clave son:

| Entidad | Descripción |
|---|---|
| `Usuario` | Modelo propio (no extiende IdentityUser). Campos: Nombre, Dni, Telefono, Email, ContrasenaHash, Direccion (owned), FechaAlta, FechaBaja (nullable), EstaActivo. Colecciones: Roles, Productos, Movimientos, VentasDia, ComprasDia |
| `UsuarioRol` | Tabla puente usuario-rol (custom, no Identity). Índice único filtrado para Administrador |
| `Direccion` | Owned Entity compartida por Usuario y Proveedor. Campos: Pais, Provincia, Localidad, CodigoPostal, Calle, Numero |
| `Categoria` | PK Guid. Nombre único (max 50), Descripcion nullable (max 250), EstaActivo, FechaAlta, UsuarioAltaId (FK a Usuario) |
| `Producto` | PK Guid. Nombre, Descripcion, PrecioCosto, PrecioVenta, CategoriaId (FK), UsuarioAltaId (FK). 1:1 con StockActual |
| `StockActual` | PK = ProductoId (1:1 con Producto). Cantidad decimal |
| `MovimientoStock` | Tipo enum (Compra/Venta/Ajuste). FK a Producto, Usuario, opcionales a ItemDetalleVenta/ItemDetalleCompra |
| `VentaDia` / `CompraDia` | Agrupan detalles de una jornada. Total acumulado, Estado: Abierto/Cerrado |
| `DetalleVenta` / `DetalleCompra` | Transacción individual (ticket) dentro de VentaDia/CompraDia |
| `ItemDetalleVenta` | Snapshot: PrecioVenta, PrecioCosto, Subtotal, GananciaTotal |
| `ItemDetalleCompra` | Snapshot: PrecioCompra, Subtotal |
| `CierreCaja` | 1:1 con VentaDia y CompraDia |
| `Proveedor` | PK Guid. Nombre, Cuit (nullable, CHAR 11), Telefono, Email, Direccion (owned), Observaciones (nullable), EstaActivo, FechaAlta. FK a UsuarioAlta |
| `TokenRevocado` | Almacena JTIs de tokens revocados (logout) |

**Enums:** `TipoMovimiento` (Compra, Venta, Ajuste), `EstadoCierre` (Abierto, Cerrado) y `EstadoUsuario` (Suspendido, Activo, Conectado — `Conectado` reservado para SignalR).

---

## Estructura de Features (CQRS)
Los features se organizan por **dominio de actor** en `src/smartStock.Api/Application/Features/{Actor}/{Commands|Queries}/{NombreFeature}/`.

Cada carpeta de feature contiene:
- `{Nombre}Command.cs` / `{Nombre}Query.cs` (record)
- `{Nombre}CommandHandler.cs` / `{Nombre}QueryHandler.cs`
- `{Nombre}CommandValidator.cs` / `{Nombre}QueryValidator.cs` (si aplica) — co-ubicado con su command/query

> **Patrón CQRS read-side (intencional):** Los `*QueryHandler` pueden inyectar `AppDbContext` directamente para proyecciones optimizadas (EF select sin tracking). Los `*CommandHandler` usan siempre el repositorio correspondiente. No mezclar dentro del mismo handler.

> **Los `*Response.cs` ya NO van dentro del feature en `smartStock.Api`.** Se ubican en `smartStock.Shared/Dtos/{Actor}/{Feature}/{Nombre}Response.cs`, namespace `smartStock.Shared.Dtos.{Actor}.{Feature}`. Igual para `DireccionDto` → `smartStock.Shared.Dtos.Shared`.

**Features implementadas:**
***CU01: Gestión De Usuarios**
*COMMANDS:*
`RegistrarAdministrador` — CU01-W1: registra el administrador del sistema (operación única, lanza `AdminYaExisteException` si ya existe uno). Endpoint: `POST api/administrador/registrar-administrador`.
`IniciarSesion` — CU01-W2: inicio de sesión para Administrador y Empleado con email y contraseña. Retorna JWT. Lanza `CredencialesInvalidasException` (401) o `CuentaInactivaException` (403). Endpoint: `POST api/auth/iniciar-sesion`.
`AltaEmpleado` — CU01-W3: el administrador da de alta un empleado. Requiere `[Authorize(Roles = "Administrador")]`. Endpoint: `POST api/administrador/alta-empleado`.
`EditarPerfilEmpleado` — CU01-W4 (empleado): el empleado autenticado edita sus propios datos (Nombre, Email, Teléfono, DNI, Dirección). `UsuarioId` extraído del claim `sub` — nunca del body. Lanza `DniDuplicadoException` o `EmailDuplicadoException` si el dato ya lo usa otro. Endpoint: `PUT api/empleado/editar-perfil-empleado`.
`CambiarEstadoEmpleado` — CU01-W4/W5 (admin): activa o suspende un empleado. `EmpleadoId` de la ruta. Solo aplica a Empleados (lanza `AccesoNoPermitidoException` si es admin). Lanza `EstadoUsuarioSinCambioException` (409) si el estado ya es el solicitado. Endpoint: `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`.
`EliminarEmpleado` — CU01-W6: borrado lógico (`EstaActivo = false`, `FechaBaja = UtcNow`). `EmpleadoId` de la ruta. Solo aplica a Empleados. Sin body ni response (204). El Command implementa `IRequest` sin tipo genérico y no tiene Validator. Endpoint: `DELETE api/administrador/eliminar-empleado/{id:guid}`.
`CambiarContrasena` — CU01-W7: el empleado cambia su propia contraseña. `UsuarioId` del claim `sub`. Valida contraseña actual (lanza `CredencialesInvalidasException` 401 si falla). FA3: confirmación coincide con nueva. Endpoint: `PATCH api/empleado/cambiar-contrasena`.

*QUERIES:*
`ObtenerPerfilAdmin` — CU01-R1. Endpoint: `GET api/administrador/obtener-perfil-admin`.
`ObtenerListaEmpleados` — CU01-R2. Endpoint: `GET api/administrador/obtener-lista-empleados`.
`ObtenerDetalleEmpleado` — CU01-R2. Endpoint: `GET api/administrador/obtener-detalle-empleado/{id:guid}`.

---

***CU02: Gestión De Proveedores**
*COMMANDS:*
`AltaProveedor` — CU02-W1: alta de proveedor en estado activo. `UsuarioAltaId` del claim `sub`. Valida unicidad de CUIT (opcional, 11 dígitos), Nombre, Email y Teléfono. Lanza `CuitDuplicadoException`, `NombreDuplicadoException`, `EmailProveedorDuplicadoException` o `TelefonoDuplicadoException` (409). Endpoint: `POST api/administrador/alta-proveedor`.
`EditarProveedor` — CU02-W2: edita datos de proveedor existente. `ProveedorId` de la ruta. Mismas validaciones excluyendo el propio registro. Lanza `ProveedorNoEncontradoException` (404). Endpoint: `PUT api/administrador/editar-proveedor/{id:guid}`.
`CambiarEstadoProveedor` — CU02-W3: activa o desactiva proveedor. `ProveedorId` de la ruta. Lanza `EstadoProveedorSinCambioException` (409). Sin validator. Endpoint: `PATCH api/administrador/cambiar-estado-proveedor/{id:guid}`.

*QUERIES:*
`ObtenerListaProveedores` — CU02-R1: filtro por estado (`activo`/`inactivo`) y búsqueda por Nombre, CUIT o Email. Endpoint: `GET api/administrador/obtener-lista-proveedores?filtroEstado=&busqueda=`.
`ObtenerDetalleProveedor` — CU02-R2: detalle con nombre del admin que lo creó (EF projection). Lanza `ProveedorNoEncontradoException` (404). Endpoint: `GET api/administrador/obtener-detalle-proveedor/{id:guid}`.

---

***CU03: Gestión De Categorías**
*COMMANDS:*
`AltaCategoria` — CU03-W1: alta de categoría en estado activo. `UsuarioAltaId` del claim `sub`. Nombre: 2-50 chars, debe comenzar con letra mayúscula Unicode (`^[\p{Lu}]`). Descripcion: opcional, max 250 chars. Lanza `CategoriaNombreDuplicadoException` (409) — comparación case-insensitive con trim (`.ToLower()` en LINQ). Endpoint: `POST api/administrador/alta-categoria`.
`EditarCategoria` — CU03-W2: edita nombre y descripción. `CategoriaId` de la ruta. Mismas validaciones excluyendo la propia categoría. Lanza `CategoriaNoEncontradaException` (404). Endpoint: `PUT api/administrador/editar-categoria/{id:guid}`.
`CambiarEstadoCategoria` — CU03-W3: activa o desactiva categoría. `CategoriaId` de la ruta. Body: `{ estaActivo: bool, categoriaDestinoId: guid? }`. Lanza `EstadoCategoriaSinCambioException` (409). Si desactiva y tiene productos: verifica alternativas activas (→ 409 `SinCategoriasActivasAlternativasException` si no hay), requiere `categoriaDestinoId` (→ 422 `CategoriaReasignacionRequiereDestinoException` si null), valida destino (→ 422 `CategoriaDestinoInvalidaException` si no existe/inactivo/mismo). Reasignación usa `ExecuteUpdateAsync` (bulk SQL). Respuesta incluye `productosReasignados`. Sin validator. Endpoint: `PATCH api/administrador/cambiar-estado-categoria/{id:guid}`.

*QUERIES:*
`ObtenerListaCategorias` — CU03-R1: filtro por estado y búsqueda por Nombre. Retorna `CantidadProductos` (via `c.Productos.Count` en proyección EF). Endpoint: `GET api/administrador/obtener-lista-categorias?filtroEstado=&busqueda=`.
`ObtenerDetalleCategoria` — CU03-R2: detalle con nombre del admin creador y cantidad de productos (EF projection). Lanza `CategoriaNoEncontradaException` (404). Endpoint: `GET api/administrador/obtener-detalle-categoria/{id:guid}`.

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
- Los **IDs sensibles** (userId, empleadoId, proveedorId, categoriaId) que provienen de JWT o de la ruta se declaran como `{ get; init; }` fuera del constructor del record y son sobreescritos en el controller con `command with { ... }` — nunca se leen del body

### Validaciones
- Siempre usar **FluentValidation** en el Validator del Command (nunca data annotations en los Commands)
- La validación DNS del email se centraliza en `Application/Common/Validators/EmailDomainValidator.DominioExisteAsync` (static, timeout 3s). Llamar con `.MustAsync(EmailDomainValidator.DominioExisteAsync)` — no duplicar el método en cada validator
- **DNI:** 7 u 8 dígitos numéricos (`^\d{7,8}$`). Regla uniforme para Administrador y Empleado
- **Nombre de categoría:** debe comenzar con letra mayúscula Unicode (`^[\p{Lu}]`), entre 2 y 50 caracteres

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
- `AppDbContext` extiende `DbContext` (NO IdentityDbContext — la gestión de usuarios y roles es completamente custom)
- DbSets activos: `Usuarios`, `UsuarioRoles`, `Categorias`, `Productos`, `Proveedores`, `TokensRevocados`
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
| `NombreDuplicadoException` | 409 | Ya existe un proveedor con ese nombre |
| `EmailProveedorDuplicadoException` | 409 | Ya existe un proveedor con ese email |
| `TelefonoDuplicadoException` | 409 | Ya existe un proveedor con ese teléfono |
| `EstadoProveedorSinCambioException` | 409 | Se intenta activar/desactivar un proveedor que ya tiene ese estado |
| `CategoriaNoEncontradaException` | 404 | No se encontró la categoría solicitada |
| `CategoriaNombreDuplicadoException` | 409 | Ya existe una categoría con ese nombre |
| `EstadoCategoriaSinCambioException` | 409 | Se intenta activar/desactivar una categoría que ya tiene ese estado |
| `SinCategoriasActivasAlternativasException` | 409 | No hay otras categorías activas para reasignar productos (FA3 de CU03-W3) |
| `CategoriaDestinoInvalidaException` | 422 | La categoría destino no existe, está inactiva o es la misma fuente |
| `CategoriaReasignacionRequiereDestinoException` | 422 | La categoría tiene productos pero no se indicó destino de reasignación |

---

## Estado actual del proyecto

### Implementado ✅
- Dominio completo (todos los modelos definidos; `Direccion` como Owned Entity compartida por Usuario y Proveedor)
- EF configurado: `CategoriaConfiguration`, `UsuarioConfiguration`, `StockActualConfiguration`, `UsuarioRolConfiguration`, `ProductoConfiguration`, `MovimientoStockConfiguration`, `CierreCajaConfiguration`, `CompraDiaConfiguration`, `VentaDiaConfiguration`, `ItemDetalleCompraConfiguration`, `ItemDetalleVentaConfiguration`, `ProveedorConfiguration`
- Migraciones aplicadas: `InitialCreate`, `DireccionComoOwned`, `UniqueAdminRole`, `BorradoLogicoEmpleado`, `CU03_GestionCategorias` (incluye renombrado tabla `Producto` → `Productos` y nuevos campos en `Categorias`)
- `IJwtTokenService` + `JwtTokenService`, `IPasswordHasher` + `BcryptPasswordHasher`
- **Repositorios:** `IUsuarioRepository` / `UsuarioRepository`, `ITokenRevocadoRepository` / `TokenRevocadoRepository`, `IProveedorRepository` / `ProveedorRepository`, `ICategoriaRepository` / `CategoriaRepository`
- **CU01-W1:** `RegistrarAdministrador` → `POST api/administrador/registrar-administrador`
- **CU01-W2:** `IniciarSesion` → `POST api/auth/iniciar-sesion` (retorna JWT con claims: sub, email, nombre, roles)
- **CU01-W3:** `AltaEmpleado` → `POST api/administrador/alta-empleado`
- **CU01-W4 (empleado):** `EditarPerfilEmpleado` → `PUT api/empleado/editar-perfil-empleado`
- **CU01-W4/W5 (admin):** `CambiarEstadoEmpleado` → `PATCH api/administrador/cambiar-estado-empleado/{id:guid}`
- **CU01-W6:** `EliminarEmpleado` → `DELETE api/administrador/eliminar-empleado/{id:guid}` (204 No Content)
- **CU01-W7:** `CambiarContrasena` → `PATCH api/empleado/cambiar-contrasena`
- **CU01-R1:** `ObtenerPerfilAdmin` → `GET api/administrador/obtener-perfil-admin`
- **CU01-R2:** `ObtenerListaEmpleados` → `GET api/administrador/obtener-lista-empleados`
- **CU01-R2:** `ObtenerDetalleEmpleado` → `GET api/administrador/obtener-detalle-empleado/{id:guid}`
- **CU02-W1:** `AltaProveedor` → `POST api/administrador/alta-proveedor`
- **CU02-W2:** `EditarProveedor` → `PUT api/administrador/editar-proveedor/{id:guid}`
- **CU02-W3:** `CambiarEstadoProveedor` → `PATCH api/administrador/cambiar-estado-proveedor/{id:guid}`
- **CU02-R1:** `ObtenerListaProveedores` → `GET api/administrador/obtener-lista-proveedores?filtroEstado=&busqueda=`
- **CU02-R2:** `ObtenerDetalleProveedor` → `GET api/administrador/obtener-detalle-proveedor/{id:guid}`
- **CU03-W1:** `AltaCategoria` → `POST api/administrador/alta-categoria`
- **CU03-W2:** `EditarCategoria` → `PUT api/administrador/editar-categoria/{id:guid}`
- **CU03-W3:** `CambiarEstadoCategoria` → `PATCH api/administrador/cambiar-estado-categoria/{id:guid}` (con reasignación automática de productos si `estaActivo=false` y hay productos; requiere `categoriaDestinoId`)
- **CU03-R1:** `ObtenerListaCategorias` → `GET api/administrador/obtener-lista-categorias?filtroEstado=&busqueda=`
- **CU03-R2:** `ObtenerDetalleCategoria` → `GET api/administrador/obtener-detalle-categoria/{id:guid}`
- Roles: `"Administrador"`, `"Empleado"`
- **Seguridad:** rate limiting con políticas `"login"` (5 req/min) y `"admin-escritura"` (30 req/min), `VerificarUsuarioActivoMiddleware`, validación JWT key al arrancar, DNS timeout 3s centralizado, índice único filtrado Administrador, índice único filtrado CUIT Proveedor

### Pendiente ⏳
- CRUD de Productos (CU04)
- Features de VentaDia/CompraDia, DetalleVenta/DetalleCompra, MovimientoStock, CierreCaja

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
- No leer IDs sensibles (userId, proveedorId, empleadoId, categoriaId) del body → siempre de JWT claim `sub` o de la ruta, inyectados con `command with { ... }` en el controller
- No usar `Guid.Parse(...)!` para extraer el claim `sub` → usar `Guid.TryParse(...)` y retornar 401 si falla
- No guardar emails de proveedores con capitalización original → normalizar con `.ToLowerInvariant()` en el handler antes de persistir y comparar
- No desactivar una categoría con productos sin proveer `categoriaDestinoId` → el handler lo valida con `CategoriaReasignacionRequiereDestinoException` (422)

---

## Seguridad

### Clave JWT
- `appsettings.json` tiene `"Key": ""` — vacío a propósito
- En desarrollo: la clave está en `appsettings.Development.json` (gitignoreado). Para desrastrearlo: `git rm --cached src/smartStock.Api/appsettings.Development.json`
- En producción: usar variable de entorno `Jwt__Key` (doble guión bajo = jerarquía de configuración en .NET)
- El app valida que `Jwt:Key` tenga al menos 32 caracteres al arrancar; falla rápido si no

### Rate limiting
- Política `"login"`: ventana fija de 1 minuto, máximo 5 requests por IP. Aplicado con `[EnableRateLimiting("login")]` en `POST api/auth/iniciar-sesion`
- Política `"admin-escritura"`: ventana fija de 1 minuto, máximo 30 requests por IP. Aplicado con `[EnableRateLimiting("admin-escritura")]` en endpoints de mutación del administrador (alta/editar/cambiar-estado de proveedores y categorías)
- Retorna `429 Too Many Requests` al superarse
- Registrado en `Program.cs` con `AddRateLimiter` + `AddPolicy`

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
