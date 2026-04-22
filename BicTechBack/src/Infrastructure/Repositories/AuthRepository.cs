using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Repositories;

namespace BicTechBack.src.Infrastructure.Repositories
{
    public class AuthRepository : Repository<Usuario>, IAuthRepository
    {
        public AuthRepository(AppDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            // Sin Include: login/registro solo necesitan columnas de usuarios; incluir Carritos/Pedidos
            // rompe si esas tablas tienen esquema distinto (p. ej. columnas legacy).
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public override async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken == refreshToken &&
                    u.RefreshTokenExpiryTime != null &&
                    u.RefreshTokenExpiryTime > DateTime.UtcNow);
        }

        public async Task SaveRefreshTokenAsync(int id, string refreshToken, DateTime dateTime)
        {
            // Un solo UPDATE (mejor con Supabase Session pooler que Find + SaveChanges, que a veces cuelga).
            await _context.Usuarios
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.RefreshToken, refreshToken)
                    .SetProperty(u => u.RefreshTokenExpiryTime, dateTime));
        }

        public async Task<bool> UpdatePasswordAsync(int id, string newPassword)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }
            usuario.Password = newPassword;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}