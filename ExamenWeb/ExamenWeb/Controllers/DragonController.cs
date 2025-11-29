using Microsoft.AspNetCore.Mvc;
using ExamenWeb.Models.DTOs;
using ExamenWeb.Services;

namespace ExamenWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DragonController : ControllerBase
    {
        private readonly DragonService _dragonService;
        public DragonController(DragonService dragonService)
        {
            _dragonService = dragonService;
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> Obtener()
        {
            try
            {
                var dragon = await _dragonService.GetAllDragonAsync();
                return Ok(dragon);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(DragonDto dto)
        {
            try
            {
                var dragon = await _dragonService.RegisterDragonAsync(dto);
                return Ok(new { message = "Dragon creado", dragon.IdDragon, dragon.NombreDragon, dragon.Descripcion, dragon.MadreDeDragones, dragon.Color });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateDragonDto dto)
        {
            try
            {
                var dragon = await _dragonService.UpdateDragonAsync(dto);
                return Ok(new { message = "Dragon actualizado", dragon.IdDragon, dragon.NombreDragon, dragon.Descripcion, dragon.MadreDeDragones, dragon.Color });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(DeleteDragonDto dto)
        {
            try
            {
                await _dragonService.DeleteDragonAsync(dto);
                return Ok(new { message = "Dragon eliminado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
