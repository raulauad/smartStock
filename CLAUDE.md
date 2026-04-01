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

## Arquitectura
El proyecto sigue una arquitectura **en capas dentro de un único proyecto**:

```
smartStock/
├── Domain/              → Modelos de negocio, enums, interfaces (sin dependencias externas)
├── Application/         → CQRS (Commands/Queries/Handlers), Validators, DTOs, Middleware
│   └── Common/
│       ├── Behaviors/   → ValidationBehavior (pipeline MediatR)
│       ├── Exceptions/  → Excepciones de dominio que implementan IExceptionHandler
│       └── Middleware/  → GlobalExceptionHandler
├── Infrastructure/      → AppDbContext, EF Configurations, Migrations
└── Presentation/        → Controllers (organizados por entidad/rol)
```

---

## Modelos de Dominio
Todos en `Domain/Models/`. Las relaciones clave son:

| Entidad | Descripción |
|---|---|
| `Usuario` | Extiende `IdentityUser<Guid>`. Campos extra: Nombre, Dni, Telefono, Direccion, FechaAlta, EstaActivo |
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

**Enums:** `TipoMovimiento` (Compra, Venta, Ajuste) y `EstadoCierre` (Abierto, Cerrado).

---

## Estructura de Features (CQRS)
Cada feature vive en `Application/Features/Commands o Queries/{Entidad}/{NombreFeature}/"Dentro iria el command, el handler, commandvalidator y dto"/` y contiene:
- `Commands/` → `{Nombre}Command.cs` (record), `{Nombre}CommandHandler.cs`, `{Nombre}CommandValidator.cs`, `{Nombre}Dto.cs` 
- `Queries/` → Similar para queries
- `DTOs/` → Response records
- `Hubs/` → (reservado para SignalR futuro)

**Feature existente:**
*COMMANDS:* 
`RegistrarAdministrador` — registra el administrador del sistema (operación única, lanza `AdminYaExisteException` si ya existe uno).
`IniciarSesion` -aplica para ambos roles ya que ambos inician sesion con su email y contraseña.
`Alta Empleado` -el administrador da de alta un usuario(empleado) dentro del sistema.

*QUERIES:*
`ObtenerPerfilAdmin` -obtiene del perfil admin tanto el usuario como el administrador mismo.
`ObtenerListaEmpleados` -el ADMINISTRADOR obtiene el listado de los empleados dados de alta dentro del sistema.
`ObtenerDetalleEmpleados` -el ADMINISTRADOR selecciona un usuario dentro de esa lista de empleados y puede ver TODOS LOS DATOS del mismo.

---

## Convenciones de código

### Naming
- **Clases, records, interfaces:** PascalCase
- **Campos privados:** `_camelCase`
- **Propiedades:** PascalCase
- **Métodos:** PascalCase
- **Variables locales:** camelCase
- **Idioma del código:** castellano para nombres de dominio y variables (Nombre, Precio, CierreCaja...), inglés para infraestructura técnica

### Patrones obligatorios
- Los **Commands** son `sealed record` que implementan `IRequest<TResponse>`
- Los **Handlers** son `sealed class` que implementan `IRequestHandler<TCommand, TResponse>`
- Los **Validators** son `sealed class : AbstractValidator<TCommand>`
- Los **Responses** son `sealed record`
- Las **excepciones de dominio** implementan `IExceptionHandler` (expone `CodigoHttp` y `Titulo`)
- Los **campos calculados** en el dominio llevan comentario `// calculado en app:`
- Los **snapshots de precio** en items llevan comentario `// snapshot`
- Las **configuraciones de EF** van en `Infrastructure/Persistence/Configurations/` como `IEntityTypeConfiguration<T>`, una clase por entidad

### Validaciones
- Siempre usar **FluentValidation** en el Validator del Command (nunca data annotations en los Commands)
- Las reglas de Identity (email, password) se integran llamando a `_userManager.UserValidators` / `_userManager.PasswordValidators` dentro de validaciones custom de FluentValidation
- La validación de dominio DNS del email se hace con `Dns.GetHostAddressesAsync`

### Controllers
- Heredan de `ControllerBase`
- Decorados con `[ApiController]` y `[Route("api/{entidad}")]`
- Solo inyectan `IMediator` y delegan todo al mediator
- Documentan casos de uso en `<summary>` con formato `CU0X-WX: descripción`

---

## Comandos útiles
```bash
# Compilar
dotnet build

# Migrations
dotnet ef migrations add {NombreMigracion} --output-dir Infrastructure/Persistence/Migrations
dotnet ef database update

# Actualizar herramienta EF
dotnet tool update --global dotnet-ef --version 10.0.3

# Ejecutar
dotnet run
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
| Excepciones que implementan `IExceptionHandler` | Según `CodigoHttp` | Errores de dominio (ej: 409 AdminYaExiste) |
| `IdentityException` | 422 | Fallos de Identity al crear usuario |
| Cualquier otra | 500 | Error inesperado |

---

## Estado actual del proyecto
- ✅ Dominio completo (todos los modelos definidos)
- ✅ EF configurado (Categoria, Usuario configurados; migración inicial creada)
- ✅ CU01-W1: Registro de administrador (operación única)
- ⏳ Pendiente: autenticación JWT (login), CRUD de Productos, Categorías, Proveedores, Sesiones de venta/compra, MovimientoStock, CierreCaja
- ⏳ Pendiente: roles adicionales (Empleado, etc.)
- ⏳ Pendiente: DbSets faltantes en AppDbContext (solo está Categorias; faltan Productos, Proveedores, etc.)

---

## Lo que NO hacer
- No usar Data Annotations para validación en Commands → siempre FluentValidation
- No poner lógica de negocio en los Controllers → solo delegar a MediatR
- No hardcodear strings de rol → usar constantes privadas en los Handlers (ej: `const string RolAdministrador = "Administrador"`)
- No calcular campos (Subtotal, Ganancia) en la base de datos → calcular en la capa de aplicación antes de persistir
- No agregar migraciones sin tener las configuraciones EF correspondientes en `Configurations/`