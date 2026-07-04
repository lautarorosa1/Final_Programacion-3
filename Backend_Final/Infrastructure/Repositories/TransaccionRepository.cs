using Backend_Final.Domain.Models;
using Backend_Final.Infrastructure.Data;
using Backend_Final.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Final.Infrastructure.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly AppDbContext _context;

        public TransaccionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CrearAsync(Transaccion transaccion)
        {
            await _context.Transacciones.AddAsync(transaccion);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> ObtenerTotalComprado(int clientId, string crypto)
        {
            var total = await _context.Transacciones
                .Where(t => t.ClienteId == clientId &&
                            t.CodigoCripto == crypto &&
                            t.TipoTransaccion == "purchase")
                .Select(t => (decimal?)t.CantidadCripto)
                .SumAsync();

            return total ?? 0;
        }

        public async Task<decimal> ObtenerTotalVendido(int clientId, string crypto)
        {
            var total = await _context.Transacciones
                .Where(t => t.ClienteId == clientId &&
                            t.CodigoCripto == crypto &&
                            t.TipoTransaccion == "sale")
                .Select(t => (decimal?)t.CantidadCripto)
                .SumAsync();

            return total ?? 0;
        }

        public async Task<List<Transaccion>> ObtenerTransaccionesAsync()
        {
            return await _context.Transacciones.AsNoTracking().ToListAsync();

        }

        public async Task<Transaccion?> ObtenerTransaccionIdAsync(int id)
        {
            return await _context.Transacciones.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<bool> EditarTransaccionAsync(Transaccion transaccion)
        {
            _context.Transacciones.Update(transaccion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarTransaccionAsync(Transaccion transaccion)
        {
            _context.Transacciones.Remove(transaccion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
