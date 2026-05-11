using BicTechBack.src.Core.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Opcionalmente reescribe <see cref="ProductoDTO.ImagenUrl"/> hacia el endpoint proxy WebP.
/// </summary>
public interface IProductoImagenWebpUrlRewriter
{
    void Apply(ProductoDTO dto);
}
