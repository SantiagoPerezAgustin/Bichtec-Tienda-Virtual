using Application.Interfaces;
using AutoMapper;
using BicTechBack.src.Core.Common;
using BicTechBack.src.Core.DTOs;
using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;

namespace BicTechBack.src.Core.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMarcaRepository _marcaRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<ProductoService> _logger;
        private readonly IProductoImagenWebpUrlRewriter _imagenWebpUrlRewriter;

        public ProductoService(
            IProductoRepository repository,
            IMarcaRepository marcaRepository,
            ICategoriaRepository categoriaRepository,
            IMapper mapper,
            IAppLogger<ProductoService> logger,
            IProductoImagenWebpUrlRewriter imagenWebpUrlRewriter)
        {
            _repository = repository;
            _categoriaRepository = categoriaRepository;
            _marcaRepository = marcaRepository;
            _mapper = mapper;
            _logger = logger;
            _imagenWebpUrlRewriter = imagenWebpUrlRewriter;
        }

        private void RewriteImagenUrl(ProductoDTO dto) => _imagenWebpUrlRewriter.Apply(dto);

        private void RewriteImagenUrl(IEnumerable<ProductoDTO> dtos)
        {
            foreach (var dto in dtos)
                _imagenWebpUrlRewriter.Apply(dto);
        }
        public async Task<ProductoDTO> CreateProductoAsync(CrearProductoDTO dto)
        {
            _logger.LogInformation("Intentando crear producto: {Nombre}", dto.Nombre);
            var productos = await _repository.GetAllAsync();
            if (productos.Any(p => p.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning("Intento de crear producto con nombre duplicado: {Nombre}", dto.Nombre);
                throw new InvalidOperationException("Ya existe un producto con ese nombre.");
            }

            if (!await _categoriaRepository.ExistsAsync(dto.CategoriaId))
            {
                _logger.LogWarning("Categoría no encontrada al crear producto: {CategoriaId}", dto.CategoriaId);
                throw new InvalidOperationException("La categoría especificada no existe.");
            }

            if (!await _marcaRepository.ExistsAsync(dto.MarcaId))
            {
                _logger.LogWarning("Marca no encontrada al crear producto: {MarcaId}", dto.MarcaId);
                throw new InvalidOperationException("La marca especificada no existe.");
            }

            NormalizarYValidarMaterialFunda(dto);

            var producto = _mapper.Map<Producto>(dto);
            var productoCreado = await _repository.AddAsync(producto);
            _logger.LogInformation("Producto creado correctamente. Id: {Id}, Nombre: {Nombre}", productoCreado.Id, productoCreado.Nombre);
            var dtoCreado = _mapper.Map<ProductoDTO>(productoCreado);
            RewriteImagenUrl(dtoCreado);
            return dtoCreado;
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            _logger.LogInformation("Intentando eliminar producto. Id: {Id}", id);
            var eliminado = await _repository.DeleteAsync(id);

            if (eliminado)
                _logger.LogInformation("Producto eliminado correctamente. Id: {Id}", id);
            else
                _logger.LogWarning("No se pudo eliminar el producto. Id: {Id}", id);

            return eliminado;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllProductosAsync()
        {
            _logger.LogInformation("Obteniendo todos los productos.");
            var productos = await _repository.GetAllAsync();
            if (productos == null || !productos.Any())
            {
                _logger.LogInformation("No se encontraron productos en la base de datos.");
                return Enumerable.Empty<ProductoDTO>();
            }
            var lista = _mapper.Map<IEnumerable<ProductoDTO>>(productos);
            RewriteImagenUrl(lista);
            return lista;
        }

        public async Task<ProductoDTO> GetProductoByIdAsync(int id)
        {
            _logger.LogInformation("Buscando producto por Id: {Id}", id);
            var producto = await _repository.GetByIdAsync(id);
            if (producto == null)
            {
                _logger.LogWarning("Producto no encontrado. Id: {Id}", id);
                throw new KeyNotFoundException("Producto no encontrado.");
            }

            var dto = _mapper.Map<ProductoDTO>(producto);
            RewriteImagenUrl(dto);
            return dto;
        }

        public async Task<(IEnumerable<ProductoDTO> Productos, int Total)> GetProductosAsync(int page, int pageSize, string? filtro)
        {
            _logger.LogInformation("Obteniendo productos paginados. Página: {Page}, Tamaño: {PageSize}, Filtro: {Filtro}", page, pageSize, filtro);
            var productos = await _repository.GetAllAsync();

            var total = productos.Count();

            var productosPaginados = productos
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var productosDTO = _mapper.Map<IEnumerable<ProductoDTO>>(productosPaginados);
            RewriteImagenUrl(productosDTO);

            return (productosDTO, total);
        }

        public async Task<ProductoDTO> UpdateProductoAsync(int id, CrearProductoDTO dto)
        {
            _logger.LogInformation("Intentando actualizar producto. Id: {Id}, Nombre: {Nombre}", id, dto.Nombre);

            var productoExistente = await _repository.GetByIdAsync(id);
            if (productoExistente == null)
            {
                _logger.LogWarning("Producto no encontrado al intentar actualizar. Id: {Id}", id);
                throw new KeyNotFoundException("Producto no encontrado.");
            }

            var productos = await _repository.GetAllAsync();
            if (productos.Any(p => p.Nombre.Equals(dto.Nombre, StringComparison.OrdinalIgnoreCase) && p.Id != id))
            {
                _logger.LogWarning("Intento de actualizar producto con nombre duplicado: {Nombre}", dto.Nombre);
                throw new InvalidOperationException("Ya existe un producto con ese nombre.");
            }

            if (!await _categoriaRepository.ExistsAsync(dto.CategoriaId))
            {
                _logger.LogWarning("Categoría no encontrada al actualizar producto: {CategoriaId}", dto.CategoriaId);
                throw new InvalidOperationException("La categoría especificada no existe.");
            }

            if (!await _marcaRepository.ExistsAsync(dto.MarcaId))
            {
                _logger.LogWarning("Marca no encontrada al actualizar producto: {MarcaId}", dto.MarcaId);
                throw new InvalidOperationException("La marca especificada no existe.");
            }

            NormalizarYValidarMaterialFunda(dto);

            _mapper.Map(dto, productoExistente);

            var productoActualizado = await _repository.UpdateAsync(productoExistente);

            _logger.LogInformation("Producto actualizado correctamente. Id: {Id}, Nombre: {Nombre}", productoActualizado.Id, productoActualizado.Nombre);

            var dtoActualizado = _mapper.Map<ProductoDTO>(productoActualizado);
            RewriteImagenUrl(dtoActualizado);
            return dtoActualizado;
        }

        /// <summary>
        /// Fundas de silicona exigen color; otras fundas y el resto de productos no.
        /// </summary>
        private static void NormalizarYValidarMaterialFunda(CrearProductoDTO dto)
        {
            var mat = MaterialFundaValores.Normalizar(dto.MaterialFunda);
            dto.MaterialFunda = mat;

            var color = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim();
            dto.Color = color;

            if (mat is null)
            {
                dto.Color = null;
                return;
            }

            if (!MaterialFundaValores.EsValido(mat))
                throw new InvalidOperationException("Material de funda no válido. Use Silicona u Otro.");

            if (mat == MaterialFundaValores.Silicona)
            {
                if (string.IsNullOrWhiteSpace(color))
                    throw new InvalidOperationException("Las fundas de silicona deben incluir un color.");
                return;
            }

            // Otro: color no requerido
        }
    }
}