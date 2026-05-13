namespace Application.Options;

/// <summary>
/// Si está activo, las respuestas JSON de productos reemplazan <c>imagenUrl</c> por la URL del proxy
/// <c>GET /imagenes/producto/{id}/webp</c>, que descarga la imagen remota (p. ej. JPG) y la sirve como WebP.
/// </summary>
public sealed class ProductoImagenWebpOptions
{
    public const string SectionName = "ProductoImagenWebp";

    /// <summary>Reescribir imagenUrl en ProductoDTO hacia el proxy WebP (por defecto true).</summary>
    public bool RewriteRasterUrlsToProxy { get; set; } = true;

    /// <summary>
    /// Base pública de la API sin barra final (ej. https://bichtech-api-zv6u.onrender.com).
    /// En Render/Vercel conviene definirla por variable de entorno si el host interno no coincide.
    /// Si está vacío, se usa el host del request HTTP actual.
    /// </summary>
    public string? PublicApiBaseUrl { get; set; }

    public int WebpQuality { get; set; } = 80;

    /// <summary>Tamaño máximo de la imagen origen descargada (bytes).</summary>
    public int MaxSourceBytes { get; set; } = 5 * 1024 * 1024;

    /// <summary>
    /// Lado mayor máximo en píxeles antes de codificar WebP en el proxy público (reduce memoria y CPU en el servidor).
    /// </summary>
    public int MaxProxyImageEdgePixels { get; set; } = 1600;

    /// <summary>
    /// Máximo de transcodificaciones WebP en paralelo (evita picos de RAM cuando muchas imágenes cargan a la vez).
    /// </summary>
    public int MaxConcurrentWebpTranscodes { get; set; } = 2;

    /// <summary>Entradas en caché de bytes WebP por producto.</summary>
    public int CacheSeconds { get; set; } = 3600;
}
