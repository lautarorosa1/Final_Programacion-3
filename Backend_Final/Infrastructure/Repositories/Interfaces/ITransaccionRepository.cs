using Backend_Final.Domain.Models;

namespace Backend_Final.Infrastructure.Repositories.Interfaces
{
    public interface ITransaccionRepository
    {
        Task CrearAsync(Transaccion transaccion);
        Task<decimal> ObtenerTotalComprado(int clientId, string crypto);
        Task<decimal> ObtenerTotalVendido(int clientId, string crypto);

        Task<List<Transaccion>> ObtenerTransaccionesAsync();
        Task<Transaccion?> ObtenerTransaccionIdAsync(int id);
        Task<bool> EditarTransaccionAsync(Transaccion transaccion);
        Task<bool> EliminarTransaccionAsync(Transaccion transaccion);
    }
}
