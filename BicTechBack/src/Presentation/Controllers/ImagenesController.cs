using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BicTechBack.src.API.Controllers;

/// <summary>
/// Utilidades de imagen: conversión a WebP (ImageSharp). Solo administradores.
/// </summary>
[ApiController]
[Route("imagenes")]
[Authorize(Roles = "Admin")]
public class ImagenesController : ControllerBase
{
    private const long MaxUploadBytes = 20 * 1024 * 1024; // 20 MB
    private readonly IImageWebpEncoder _webpEncoder;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ImagenesController> _logger;

    public ImagenesController(
        IImageWebpEncoder webpEncoder,
        IHttpClientFactory httpClientFactory,
        ILogger<ImagenesController> logger)
    {
        _webpEncoder = webpEncoder;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Sube un archivo de imagen y devuelve el mismo contenido codificado como WebP (descarga).
    /// </summary>
    /// <param name="archivo">JPEG, PNG, WebP, etc.</param>
    /// <param name="calidad">1–100 (por defecto 80).</param>
    [HttpPost("a-webp")]
    [RequestSizeLimit(MaxUploadBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadBytes)]
    [Consumes("multipart/form-data")]
    [Produces("image/webp")]
    public async Task<IActionResult> ArchivoAWebp(
        IFormFile archivo,
        [FromQuery] int calidad = 80,
        CancellationToken cancellationToken = default)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest(new { message = "Enviá un archivo en el campo \"archivo\"." });

        if (archivo.Length > MaxUploadBytes)
            return BadRequest(new { message = $"El archivo supera el máximo de {MaxUploadBytes / (1024 * 1024)} MB." });

        if (!EsContentTypeImagen(archivo.ContentType))
            return BadRequest(new { message = "El content-type debe ser una imagen (image/jpeg, image/png, etc.)." });

        await using var input = archivo.OpenReadStream();
        byte[] webp;
        try
        {
            webp = await _webpEncoder.EncodeToWebpAsync(input, calidad, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo convertir el archivo a WebP.");
            return BadRequest(new { message = "No se pudo leer o convertir la imagen (formato no soportado o archivo dañado)." });
        }

        var nombreBase = Path.GetFileNameWithoutExtension(archivo.FileName);
        if (string.IsNullOrWhiteSpace(nombreBase))
            nombreBase = "imagen";

        return File(webp, "image/webp", $"{nombreBase}.webp");
    }

    /// <summary>
    /// Descarga una imagen desde una URL pública (https) y devuelve WebP.
    /// </summary>
    [HttpPost("url-a-webp")]
    [RequestSizeLimit(MaxUploadBytes)]
    public async Task<IActionResult> UrlAWebp(
        [FromBody] ConvertirUrlImagenRequest body,
        [FromQuery] int calidad = 80,
        CancellationToken cancellationToken = default)
    {
        if (body?.Url is not { Length: > 0 } urlRaw)
            return BadRequest(new { message = "Enviá { \"url\": \"https://...\" } en el cuerpo JSON." });

        if (!Uri.TryCreate(urlRaw.Trim(), UriKind.Absolute, out var uri) ||
            uri.Scheme != Uri.UriSchemeHttps)
            return BadRequest(new { message = "La URL debe ser absoluta y usar https." });

        if (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "No se permiten URLs a localhost." });

        using var client = _httpClientFactory.CreateClient(nameof(ImagenesController));
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "BicTechBack-ImageWebp/1.0");

        using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            return BadRequest(new { message = "No se pudo descargar la imagen.", status = (int)response.StatusCode });

        var contentType = response.Content.Headers.ContentType?.MediaType;
        if (!string.IsNullOrEmpty(contentType) && !EsContentTypeImagen(contentType) && contentType != "application/octet-stream")
            return BadRequest(new { message = "La URL no devolvió un tipo image/* reconocible." });

        await using var remote = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        byte[] webp;
        try
        {
            webp = await _webpEncoder.EncodeToWebpAsync(remote, calidad, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo convertir la imagen de la URL a WebP.");
            return BadRequest(new { message = "La URL no devolvió una imagen válida para convertir." });
        }

        var nombre = uri.Segments.Length > 0 ? Path.GetFileNameWithoutExtension(uri.Segments[^1]) : "imagen";
        if (string.IsNullOrWhiteSpace(nombre))
            nombre = "imagen";

        return File(webp, "image/webp", $"{nombre}.webp");
    }

    private static bool EsContentTypeImagen(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return false;
        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
}

public sealed class ConvertirUrlImagenRequest
{
    public string? Url { get; set; }
}
