using System.Text.Json;
using Application.Interfaces;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Core.Services;
using BicTechBack.src.Infrastructure.Data;
using BicTechBack.src.Infrastructure.Logging;
using BicTechBack.src.Infrastructure.Repositories;
using BicTechBack.src.Infrastructure.Security;
using Infrastructure.Config;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Render (y otros PaaS) inyectan PORT; el contenedor debe escuchar en 0.0.0.0 para que el proxy no devuelva 502.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BicTechBack API",
        Version = "v1",
        Description = "API para gestión de productos, categorías, marcas, carritos y pedidos.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Soporte BicTech",
            Email = "soporte@bictech.com"
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    // Http + bearer: Swagger UI muestra bien "Authorize" y el candado (ApiKey + Bearer suele fallar).
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT: pegá solo el token (Swagger agrega \"Bearer\"). Obtenelo con POST /auth/login.",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.OperationFilter<BicTechBack.src.Presentation.Swagger.AuthorizeOperationFilter>();
});

// ==========================================
// Repositorios y servicios
// ==========================================
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPedidoDetalleRepository, PedidoDetalleRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaMarcaRepository, CategoriaMarcaRepository>();
builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IPaisRepository, PaisRepository>();

builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ICategoriaMarcaService, CategoriaMarcaService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));


// ==========================================
// CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ==========================================
// Base de datos
// ==========================================

var postgresConnectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
if (string.IsNullOrWhiteSpace(postgresConnectionString))
{
    throw new InvalidOperationException(
        "Falta ConnectionStrings:PostgreSQLConnection. En Render define ConnectionStrings__PostgreSQLConnection (Host=...;Database=...;Username=...;SSL Mode=Require). Opcional: POSTGRES_PASSWORD con la clave sola para evitar errores al pegar.");
}

// Si POSTGRES_PASSWORD está definida, reemplaza la contraseña (útil en Render: clave larga o caracteres que rompen el parsing).
var passwordOverride = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")?.Trim();
if (!string.IsNullOrEmpty(passwordOverride))
{
    var csb = new NpgsqlConnectionStringBuilder(postgresConnectionString) { Password = passwordOverride };
    postgresConnectionString = csb.ConnectionString;
}

LogPostgresTarget(postgresConnectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(postgresConnectionString)
           .UseSnakeCaseNamingConvention());


// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ==========================================
// 🔐 Configuración JWT desde variables de entorno
// ==========================================
var jwtKey = JwtKeyResolver.Resolve(builder.Configuration);
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? builder.Configuration["Jwt:Issuer"];
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                  ?? builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey))
    throw new Exception("Jwt:Key no está definida (appsettings o JWT_KEY válida).");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});

// ==========================================
// 🔐 Configuración Stripe desde variables de entorno
// ==========================================
builder.Services.Configure<StripeOptions>(opts =>
{
    // Si la variable existe pero está vacía en Render, no pisar appsettings (?? solo cubre null).
    var stripeBase = Environment.GetEnvironmentVariable("STRIPE_BASEURL");
    opts.BaseUrl = string.IsNullOrWhiteSpace(stripeBase)
        ? builder.Configuration["Stripe:BaseUrl"]
        : stripeBase;
    var stripeSecret = Environment.GetEnvironmentVariable("STRIPE_SECRETKEY");
    opts.SecretKey = string.IsNullOrWhiteSpace(stripeSecret)
        ? builder.Configuration["Stripe:SecretKey"]
        : stripeSecret;

    if (string.IsNullOrWhiteSpace(opts.SecretKey))
        throw new Exception("Stripe:SecretKey vacía. Definí STRIPE_SECRETKEY o Stripe:SecretKey en configuración.");
});

// Servicio Stripe
builder.Services.AddStripeHttpClient();
builder.Services.AddScoped<IStripeService, StripeService>();

// ==========================================
// Construir app
// ==========================================
static void LogPostgresTarget(string cs)
{
    try
    {
        var trimmed = cs.Trim();
        if (trimmed.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
        {
            var uri = new Uri(trimmed);
            var userInfo = uri.UserInfo.Split(':', 2);
            var user = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : "?";
            var db = uri.AbsolutePath.TrimStart('/');
            Console.WriteLine($"[DB] Host={uri.Host} Port={uri.Port} Database={db} Username={user}");
            return;
        }

        string? Get(string key)
        {
            foreach (var part in trimmed.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var idx = part.IndexOf('=');
                if (idx <= 0) continue;
                var k = part[..idx].Trim();
                var v = part[(idx + 1)..].Trim();
                if (k.Equals(key, StringComparison.OrdinalIgnoreCase))
                    return v;
            }
            return null;
        }

        var host = Get("Host") ?? Get("Server");
        var port = Get("Port");
        var database = Get("Database");
        var username = Get("Username") ?? Get("User ID") ?? Get("User");
        Console.WriteLine($"[DB] Host={host} Port={port} Database={database} Username={username}");
    }
    catch
    {
        Console.WriteLine("[DB] No se pudo interpretar la cadena de conexión para el log (formato inválido).");
    }
}

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BicTech API v1");
    c.ConfigObject.PersistAuthorization = true;
});

// CORS
app.UseCors("AllowAll");

// Middleware de excepciones personalizado
app.UseMiddleware<BicTechBack.src.API.Extensions.ExceptionMiddleware>();

// Sin UseHttpsRedirection: en Docker/Render el TLS lo termina el proxy; el contenedor solo escucha HTTP
// y evita "Failed to determine the https port for redirect".

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Redirigir raíz a Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();