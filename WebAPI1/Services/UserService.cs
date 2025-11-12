using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebAPI1.Data;
using WebAPI1.Models;
using WebAPI1.Models.DTOs;

namespace WebAPI1.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        public UserService(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }
        public async Task<List<User>> GetAllUserAsync()
        {
            var user = _appDbContext.Users.ToListAsync();
            return await user;
        }
        public async Task<User> RegisterUserAsync(UserDto dto)
        {
            if (await _appDbContext.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email ya registrado");
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Password = HashPassword(dto.Password),
                Rol = dto.Rol
            };

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public string HashPassword(string password) {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<User> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _appDbContext.Users.FindAsync(dto.Id);
            if (user == null)
                throw new Exception("Usuario no encontrado");
            if (!string.IsNullOrEmpty(dto.UserName))
                user.UserName = dto.UserName;
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = HashPassword(dto.Password);
            if (!string.IsNullOrEmpty(dto.Rol))
                user.Rol = dto.Rol;
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateEmailAsync(UpdateEmailUserDto dto)
        {
            var user = await _appDbContext.Users.FindAsync(dto.Id);
            if (user == null)
                throw new Exception("Usuario no encontrado");
            user.Email = dto.NuevoEmail;
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(DeleteUserDto dto)
        {
            var user = await _appDbContext.Users.FindAsync(dto.Id);
            if (user == null)
                throw new Exception("Usuario no encontrado");
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
