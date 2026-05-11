using Application.Interfaces;
using Application.Options;
using BicTechBack.src.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BicTechBack.src.API.Controllers;

/// <summary>
/// Sirve la imagen del producto como WebP (descarga la URL guardada en BD y transcodifica).
/// Público para que el navegador pueda cargar &lt;img src&gt; sin JWT.
/// </summary>
[ApiController]
[Route("imagenes")]
[AllowAnonymous]
public class ProductoImagenWebpPublicController : ControllerBase
{
    private const string HttpClientName = "ProductoWebpProxy";
    private readonly IProductoRepository _productos;
    private readonly IImageWebpEncoder _webpEncoder;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly IOptions<ProductoImagenWebpOptions> _options;
    private readonly ILogger<ProductoImagenWebpPublicController> _logger;

    public ProductoImagenWebpPublicController(
        IProductoRepository productos,
        IImageWebpEncoder webpEncoder,
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IOptions<ProductoImagenWebpOptions> options,
        ILogger<ProductoImagenWebpPublicController> logger)
    {
        _productos = productos;
        _webpEncoder = webpEncoder;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _options = options;
        _logger = logger;
    }

    /// <summary>Obtiene la imagen del producto en formato WebP (origen JPG/PNG/etc. desde la URL almacenada).</summary>
    [HttpGet("producto/{id:int}/webp")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 3600)]
    [Produces("image/webp")]
    public async Task<IActionResult> GetProductoWebp(int id, CancellationToken cancellationToken)
    {
        var opt = _options.Value;
        var cacheKey = $"webp:producto:{id}";
        if (_cache.TryGetValue(cacheKey, out byte[]? cached) && cached is { Length: > 0 })
        {
            Response.Headers.CacheControl = $"public,max-age={opt.CacheSeconds}";
            return File(cached, "image/webp");
        }

        var producto = await _productos.GetByIdAsync(id).ConfigureAwait(false);
        if (producto == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(producto.ImagenUrl))
            return NotFound();

        if (!Uri.TryCreate(producto.ImagenUrl.Trim(), UriKind.Absolute, out var sourceUri))
            return BadRequest();

        if (sourceUri.Scheme != Uri.UriSchemeHttps && sourceUri.Scheme != Uri.UriSchemeHttp)
            return BadRequest();

        using var client = _httpClientFactory.CreateClient(HttpClientName);
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "BicTechBack-ProductoWebp/1.0");

        using var response = await client
            .GetAsync(sourceUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "Fallo al descargar imagen del producto {Id}. Status {Status}",
                id,
                (int)response.StatusCode);
            return StatusCode(502);
        }

        await using var remote = await response.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        await using var capped = await CopyStreamWithLimitAsync(remote, opt.MaxSourceBytes, cancellationToken)
            .ConfigureAwait(false);

        byte[] webp;
        try
        {
            webp = await _webpEncoder
                .EncodeToWebpAsync(capped, opt.WebpQuality, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo transcodificar imagen del producto {Id}", id);
            return StatusCode(502);
        }

        _cache.Set(
            cacheKey,
            webp,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Math.Max(60, opt.CacheSeconds)),
            });

        Response.Headers.CacheControl = $"public,max-age={opt.CacheSeconds}";
        return File(webp, "image/webp");
    }

    private static async Task<MemoryStream> CopyStreamWithLimitAsync(
        Stream source,
        int maxBytes,
        CancellationToken cancellationToken)
    {
        var ms = new MemoryStream();
        var buffer = new byte[8192];
        long total = 0;
        while (total < maxBytes)
        {
            var toRead = (int)Math.Min(buffer.Length, maxBytes - total);
            var read = await source.ReadAsync(buffer.AsMemory(0, toRead), cancellationToken).ConfigureAwait(false);
            if (read == 0)
                break;
            await ms.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(false);
            total += read;
        }

        ms.Position = 0;
        return ms;
    }
}
