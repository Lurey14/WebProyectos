using ExamenWeb.Data;
using Microsoft.EntityFrameworkCore;
using ExamenWeb.Models;
using ExamenWeb.Models.DTOs;

namespace ExamenWeb.Services
{
    public class DragonService
    {
        private readonly AppDbContext _appDbContext;
        public DragonService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<Dragon>> GetAllDragonAsync()
        {
            var dragon = _appDbContext.Dragons.ToListAsync();
            return await dragon;
        }
        public async Task<Dragon> RegisterDragonAsync(DragonDto dto)
        {
            if (await _appDbContext.Dragons.AnyAsync(u => u.NombreDragon == dto.NombreDragon))
                throw new Exception("Nombre ya registrado");
            var dragon = new Dragon
            {
                NombreDragon = dto.NombreDragon,
                Descripcion = dto.Descripcion,
                MadreDeDragones = dto.MadreDeDragones,
                Color = dto.Color
            };

            _appDbContext.Dragons.Add(dragon);
            await _appDbContext.SaveChangesAsync();
            return dragon;
        }
        public async Task<Dragon> UpdateDragonAsync(UpdateDragonDto dto)
        {
            var dragon = await _appDbContext.Dragons.FindAsync(dto.IdDragon);
            if (dragon == null)
                throw new Exception("Dragon no encontrado");
            if (!string.IsNullOrEmpty(dto.NombreDragon))
                dragon.NombreDragon = dto.NombreDragon;
            if (!string.IsNullOrEmpty(dto.Descripcion))
                dragon.Descripcion = dto.Descripcion;
            if (!string.IsNullOrEmpty(dto.MadreDeDragones))
                dragon.MadreDeDragones = dto.MadreDeDragones;
            if (!string.IsNullOrEmpty(dto.Color))
                dragon.Color = dto.Color;
            await _appDbContext.SaveChangesAsync();
            return dragon;
        }

        public async Task DeleteDragonAsync(DeleteDragonDto dto)
        {
            var dragon = await _appDbContext.Dragons.FindAsync(dto.IdDragon);
            if (dragon == null)
                throw new Exception("Dragon no encontrado");
            _appDbContext.Dragons.Remove(dragon);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
