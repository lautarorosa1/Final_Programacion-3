using Backend_Final.Domain.Models;
using Backend_Final.Infrastructure.Data;
using Backend_Final.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Final.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CrearClienteAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteEmailAsync(string email, int? excludeId = null)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Email == email && (!excludeId.HasValue || c.Id != excludeId));
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public async Task<Cliente?> ObtenerClienteAsync(int id)
        {
            return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> EditarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
