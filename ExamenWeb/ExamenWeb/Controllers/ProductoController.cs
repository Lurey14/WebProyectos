using Microsoft.AspNetCore.Mvc;
using ExamenWeb.Models.DTOs;
using ExamenWeb.Services;

namespace ExamenWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;
        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> Obtener()
        {
            try
            {
                var producto = await _productoService.GetAllProductoAsync();
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(ProductoDto dto)
        {
            try
            {
                var producto = await _productoService.RegisterProductoAsync(dto);
                return Ok(new { message = "Producto creado", producto.IdProducto, producto.NombreProducto, producto.DescripcionCorta, producto.Precio, producto.Stock, producto.IdCategoria });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductoDto dto)
        {
            try
            {
                var producto = await _productoService.UpdateProductoAsync(dto);
                return Ok(new { message = "Producto actualizado", producto.IdProducto, producto.NombreProducto, producto.DescripcionCorta, producto.Precio, producto.Stock, producto.IdCategoria });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(DeleteProductoDto dto)
        {
            try
            {
                await _productoService.DeleteProductoAsync(dto);
                return Ok(new { message = "Producto eliminado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
