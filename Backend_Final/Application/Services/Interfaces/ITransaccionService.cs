using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Transaccion;
using Backend_Final.Domain.Models;

namespace Backend_Final.Application.Services.Interfaces
{
    public interface ITransaccionService
    {
        Task<Result<ResponseTransaccionDto>> CrearTransaccionAsync(CrearTransaccionDto dto);
        Task<Result<List<ResponseTransaccionDto>>> ObtenerTransaccionesAsync();
        Task<Result<ResponseTransaccionDto>> ObtenerTransaccionIdAsync(int id);
    }
}
