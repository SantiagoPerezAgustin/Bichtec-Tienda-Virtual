using BicTechBack.src.Core.Entities;
using BicTechBack.src.Core.Interfaces;
using BicTechBack.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Repositories;

namespace BicTechBack.src.Infrastructure.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario.Id;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;
            var normalized = email.Trim().ToLowerInvariant();
            return await _context.Usuarios
                .Include(u => u.Pedidos)
                .Include(u => u.Carritos)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalized);
        }

        public override async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Pedidos)
                .Include(u => u.Carritos)
                .ToListAsync();
        }

        public override async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Pedidos)
                .Include(u => u.Carritos)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}