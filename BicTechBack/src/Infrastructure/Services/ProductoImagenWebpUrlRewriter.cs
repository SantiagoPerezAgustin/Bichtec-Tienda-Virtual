using Application.Interfaces;
using Application.Options;
using BicTechBack.src.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public sealed class ProductoImagenWebpUrlRewriter : IProductoImagenWebpUrlRewriter
{
    private readonly IOptions<ProductoImagenWebpOptions> _options;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductoImagenWebpUrlRewriter(
        IOptions<ProductoImagenWebpOptions> options,
        IHttpContextAccessor httpContextAccessor)
    {
        _options = options;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Apply(ProductoDTO dto)
    {
        if (!_options.Value.RewriteRasterUrlsToProxy)
            return;

        if (string.IsNullOrWhiteSpace(dto.ImagenUrl))
            return;

        if (!ShouldProxyAsWebp(dto.ImagenUrl))
            return;

        var baseUrl = ResolvePublicBaseUrl();
        if (string.IsNullOrEmpty(baseUrl))
            return;

        dto.ImagenUrl = $"{baseUrl}/imagenes/producto/{dto.Id}/webp";
    }

    private string? ResolvePublicBaseUrl()
    {
        var configured = _options.Value.PublicApiBaseUrl?.Trim().TrimEnd('/');
        if (!string.IsNullOrEmpty(configured))
            return configured;

        var ctx = _httpContextAccessor.HttpContext;
        if (ctx?.Request is not { } req)
            return null;

        return $"{req.Scheme}://{req.Host.Value}";
    }

    private static bool ShouldProxyAsWebp(string url)
    {
        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var u))
            return false;

        if (u.Scheme != Uri.UriSchemeHttps && u.Scheme != Uri.UriSchemeHttp)
            return false;

        var path = u.AbsolutePath;
        if (path.Contains("/imagenes/producto/", StringComparison.OrdinalIgnoreCase)
            && path.EndsWith("/webp", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}
