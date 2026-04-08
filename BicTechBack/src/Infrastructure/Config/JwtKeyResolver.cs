using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Config
{
    /// <summary>
    /// Si JWT_KEY en el entorno existe pero es demasiado corta para HS256, usa Jwt:Key de configuración.
    /// Una variable JWT_KEY vacía o corta en Windows suele romper el login con 500 (IDX10720).
    /// </summary>
    public static class JwtKeyResolver
    {
        public static string Resolve(IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("JWT_KEY");
            var cfg = configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(env))
                return cfg ?? "";
            if (Encoding.UTF8.GetByteCount(env) < 32 && !string.IsNullOrEmpty(cfg))
                return cfg;
            return env;
        }
    }
}
