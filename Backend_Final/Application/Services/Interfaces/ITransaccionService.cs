using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Transaccion;

namespace Backend_Final.Application.Services.Interfaces
{
    public interface ITransaccionService
    {
        Task<Result<ResponseTransaccionDto>> CrearTransaccionAsync(CrearTransaccionDto dto);
        Task<Result<List<ResponseTransaccionDto>>> ObtenerTransaccionesAsync();
        Task<Result<ResponseTransaccionDto>> ObtenerTransaccionIdAsync(int id);
        Task<Result<ResponseTransaccionDto>> EditarTransaccionAsync(int id, EditarTransaccionDto dto);
        Task<Result<bool>> EliminarTransaccionAsync(int id);
    }
}
