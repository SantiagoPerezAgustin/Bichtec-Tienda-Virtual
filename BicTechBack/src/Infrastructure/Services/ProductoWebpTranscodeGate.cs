using Application.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

/// <summary>
/// Limita cuántas transcodificaciones WebP del proxy de producto corren a la vez (evita OOM en hosts con poca RAM).
/// </summary>
public sealed class ProductoWebpTranscodeGate : IDisposable
{
    public SemaphoreSlim Semaphore { get; }

    public ProductoWebpTranscodeGate(IOptions<ProductoImagenWebpOptions> options)
    {
        var n = Math.Clamp(options.Value.MaxConcurrentWebpTranscodes, 1, 8);
        Semaphore = new SemaphoreSlim(n, n);
    }

    public void Dispose() => Semaphore.Dispose();
}
