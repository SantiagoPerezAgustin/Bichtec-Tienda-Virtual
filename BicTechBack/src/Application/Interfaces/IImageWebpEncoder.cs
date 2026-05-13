namespace Application.Interfaces;

/// <summary>
/// Codifica imágenes (JPEG, PNG, GIF, BMP, WebP, etc.) a formato WebP usando SixLabors.ImageSharp.
/// </summary>
public interface IImageWebpEncoder
{
    /// <param name="imageStream">Stream posicionado al inicio; puede ser no seekable (se copia a memoria).</param>
    /// <param name="quality">Calidad WebP lossy 1–100 (por defecto 80).</param>
    /// <param name="maxEdgePixels">Si es mayor que 0, reduce la imagen para que el lado mayor no supere este valor (ahorra RAM en el servidor).</param>
    Task<byte[]> EncodeToWebpAsync(
        Stream imageStream,
        int quality = 80,
        CancellationToken cancellationToken = default,
        int maxEdgePixels = 0);
}
