using ExamenWeb.Data;
using Microsoft.EntityFrameworkCore;
using ExamenWeb.Models;
using ExamenWeb.Models.DTOs;

namespace ExamenWeb.Services
{
    public class ProductoService
    {
        private readonly AppDbContext _appDbContext;
        public ProductoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<Producto>> GetAllProductoAsync()
        {
            var producto = _appDbContext.Productos.ToListAsync();
            return await producto;
        }
        public async Task<Producto> RegisterProductoAsync(ProductoDto dto)
        {
            if (await _appDbContext.Productos.AnyAsync(u => u.NombreProducto == dto.NombreProducto))
                throw new Exception("Nombre ya registrado");
            var producto = new Producto
            {
                NombreProducto = dto.NombreProducto,
                DescripcionCorta = dto.DescripcionCorta,
                Precio = dto.Precio,
                Stock = dto.Stock,
                IdCategoria = dto.IdCategoria
            };

            _appDbContext.Productos.Add(producto);
            await _appDbContext.SaveChangesAsync();
            return producto;
        }
        public async Task<Producto> UpdateProductoAsync(UpdateProductoDto dto)
        {
            var producto = await _appDbContext.Productos.FindAsync(dto.IdProducto);
            if (producto == null)
                throw new Exception("Producto no encontrado");
            if (!string.IsNullOrEmpty(dto.NombreProducto))
                producto.NombreProducto = dto.NombreProducto;
            if (!string.IsNullOrEmpty(dto.DescripcionCorta))
                producto.DescripcionCorta = dto.DescripcionCorta;
            if (!string.IsNullOrEmpty(dto.Precio))
                producto.Precio = dto.Precio;
            if (!string.IsNullOrEmpty(dto.Stock))
                producto.Stock = dto.Stock;
            /*if (!string.IsNullOrEmpty(dto.IdCategoria))
                producto.IdCategoria = dto.IdCategoria;*/
            await _appDbContext.SaveChangesAsync();
            return producto;
        }

        public async Task DeleteProductoAsync(DeleteProductoDto dto)
        {
            var producto = await _appDbContext.Productos.FindAsync(dto.IdProducto);
            if (producto == null)
                throw new Exception("Producto no encontrado");
            _appDbContext.Productos.Remove(producto);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
