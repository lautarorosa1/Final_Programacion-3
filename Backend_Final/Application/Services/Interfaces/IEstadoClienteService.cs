using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Cliente;

namespace Backend_Final.Application.Services.Interfaces
{
    public interface IEstadoClienteService
    {
        Task<Result<ResponseEstadoClienteDto>> ObtenerEstadoAsync(int clientId);
    }
}
