using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Cliente;

namespace Backend_Final.Application.Services.Interfaces
{
    public interface IClienteService
    {
        Task<Result<ResponseClienteDto>> CrearClienteAsync(CrearClienteDto dto);
        Task<Result<List<ResponseClienteDto>>> ObtenerClientesAsync();
        Task<Result<ResponseClienteDto>> ObtenerClienteAsync(int id);
    }
}
