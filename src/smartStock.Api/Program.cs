using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using smartStock.Api.Application.Common.Behaviors;
using smartStock.Api.Application.Common.Interfaces.Auth;
using smartStock.Api.Application.Common.Interfaces.Repositories;
using smartStock.Api.Application.Common.Middleware;
using smartStock.Api.Application.Features.Admin.Commands.RegistrarAdmin;
using FluentValidation;
using smartStock.Api.Infrastructure.Persistence;
using smartStock.Api.Infrastructure.Persistence.Repositories;
using smartStock.Api.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Validar clave JWT al arrancar (falla rápido si no está configurada) ---
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
    throw new InvalidOperationException(
        "Jwt:Key debe configurarse con al menos 32 caracteres. " +
        "Usá variables de entorno (Jwt__Key) o appsettings.Development.json en local.");

// --- Base de datos ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Auth: repositorio y hasher ---
builder.Services.AddScoped<IUsuarioRepository,       UsuarioRepository>();
builder.Services.AddScoped<IPasswordHasher,          BcryptPasswordHasher>();
builder.Services.AddScoped<ITokenRevocadoRepository, TokenRevocadoRepository>();

// --- JWT ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwt = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwt["Issuer"],
        ValidAudience            = jwt["Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(jwt["Key"]!))
    };
});

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// --- Rate limiting: 5 intentos/minuto por IP en el endpoint de login ---
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anon",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit          = 5,
                Window               = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit           = 0
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// --- MediatR ---
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegistrarAdministradorCommand).Assembly));

// --- FluentValidation ---
builder.Services.AddValidatorsFromAssemblyContaining<RegistrarAdministradorCommandValidator>();

// --- Pipeline CQRS: ValidationBehavior ---
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

// --- Manejo global de excepciones ---
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// --- Controllers + OpenAPI ---
builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, _, _) =>
    {
        doc.Info.Title       = "SmartStock API";
        doc.Info.Version     = "v1";
        doc.Info.Description = "API de gestión de inventario SmartStock";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title             = "SmartStock API";
        options.Theme             = ScalarTheme.DeepSpace;
        options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.HttpClient);
        options.AddHttpAuthentication("Bearer", bearer => { });
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<VerificarUsuarioActivoMiddleware>();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
