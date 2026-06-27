using Backend_Final.Application.DTOs.Cliente;
using Backend_Final.Domain.Models;

namespace Backend_Final.Infrastructure.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task CrearClienteAsync(Cliente cliente);
        Task<bool> ExisteEmailAsync(string email, int? excludeId = null);
        Task<List<Cliente>> ObtenerClientesAsync();
        Task<Cliente?> ObtenerClienteAsync(int id);
    }
}
