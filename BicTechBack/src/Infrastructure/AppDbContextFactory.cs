using BicTechBack.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

/// <summary>
/// Solo para <c>dotnet ef</c>. Lee la cadena desde <c>Presentation/appsettings.json</c>
/// (misma que en runtime). Podés sobreescribir con <c>--connection</c> en la CLI.
/// </summary>
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var presentationBasePath = ResolvePresentationBasePath();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(presentationBasePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSQLConnection")
            ?? throw new InvalidOperationException(
                "Falta ConnectionStrings:PostgreSQLConnection. Revisá Presentation/appsettings.json " +
                "o definí la variable de entorno ConnectionStrings__PostgreSQLConnection.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        return new AppDbContext(optionsBuilder.Options);
    }

    /// <summary>
    /// Encuentra la carpeta de Presentation según desde dónde ejecutes <c>dotnet ef</c>
    /// (raíz del repo, carpeta Presentation, Infrastructure, etc.).
    /// </summary>
    private static string ResolvePresentationBasePath()
    {
        var cwd = Directory.GetCurrentDirectory();
        var candidates = new[]
        {
            Path.Combine(cwd, "appsettings.json"),
            Path.Combine(cwd, "Presentation", "appsettings.json"),
            Path.Combine(cwd, "src", "Presentation", "appsettings.json"),
            Path.Combine(cwd, "..", "Presentation", "appsettings.json"),
        };

        foreach (var relative in candidates)
        {
            var full = Path.GetFullPath(relative);
            if (File.Exists(full))
                return Path.GetDirectoryName(full)!;
        }

        throw new InvalidOperationException(
            "No se encontró Presentation/appsettings.json. Ejecutá dotnet ef desde la carpeta " +
            "BicTechBack o indicá la cadena con --connection \"Host=...;...\".");
    }
}
