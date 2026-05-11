using Application.Interfaces;
using BicTechBack.src.Core.DTOs;

namespace Application.Services;

/// <summary>Implementación nula para tests o cuando no se usa reescritura.</summary>
public sealed class NoOpProductoImagenWebpUrlRewriter : IProductoImagenWebpUrlRewriter
{
    public void Apply(ProductoDTO dto)
    {
    }
}
